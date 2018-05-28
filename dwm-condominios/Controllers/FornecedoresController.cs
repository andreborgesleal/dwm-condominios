using App_Dominio.Contratos;
using App_Dominio.Controllers;
using App_Dominio.Security;
using DWM.Models.Enumeracoes;
using DWM.Models.Persistence;
using DWM.Models.Repositories;
using System;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;

namespace DWM.Controllers
{
    public class FornecedoresController : RootController<CredorViewModel, CredorModel>
    {
        #region Receita Federal
        private CookieContainer _cookies;
        private readonly string urlBaseReceitaFederal;
        private readonly string paginaValidacao;
        private readonly string paginaPrincipal;
        private readonly string paginaCaptcha;

        public FornecedoresController()
        {
            _cookies = new CookieContainer();
            urlBaseReceitaFederal = "http://www.receita.fazenda.gov.br/pessoajuridica/cnpj/cnpjreva/";
            paginaValidacao = "valida.asp";
            paginaPrincipal = "cnpjreva_solicitacao2.asp";
            paginaCaptcha = "captcha/gerarCaptcha.asp";
        }

        public JsonResult GetCaptcha()
        {

            var htmlResult = string.Empty;

            using (var wc = new App_Dominio.ConsultaCNPJ.Infra.CookieAwareWebClient(_cookies))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                wc.Headers[HttpRequestHeader.KeepAlive] = "300";
                htmlResult = wc.DownloadString(urlBaseReceitaFederal + paginaPrincipal);
            }

            if (htmlResult.Length > 0)
            {
                var wc2 = new App_Dominio.ConsultaCNPJ.Infra.CookieAwareWebClient(_cookies);
                wc2.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                wc2.Headers[HttpRequestHeader.KeepAlive] = "300";
                byte[] data = wc2.DownloadData(urlBaseReceitaFederal + paginaCaptcha);

                Session["cookies"] = _cookies;

                return Json("data:image/jpeg;base64," + Convert.ToBase64String(data, 0, data.Length), JsonRequestBehavior.AllowGet);
            }

            return null;

        }

        public ActionResult ConsultarDados(string cnpj, string captcha)
        {
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            var msg = string.Empty;
            var resp = ObterDados(cnpj, captcha);

            if (resp.Contains("Verifique se o mesmo foi digitado corretamente"))
                msg = "O número do CNPJ não foi digitado corretamente";

            if (resp.Contains("Erro na Consulta"))
                msg += "Os caracteres não conferem com a imagem";


            return Json(
                new
                {
                    erro = msg,
                    dados = resp.Length > 0 ? App_Dominio.ConsultaCNPJ.Infra.FormatarDados.MontarObjEmpresa(cnpj, resp) : null
                },
                JsonRequestBehavior.DenyGet);
        }

        private string ObterDados(string aCNPJ, string aCaptcha)
        {
            _cookies = (CookieContainer)Session["cookies"];

            var request = (HttpWebRequest)WebRequest.Create(urlBaseReceitaFederal + paginaValidacao);
            request.ProtocolVersion = HttpVersion.Version10;
            request.CookieContainer = _cookies;
            request.Method = "POST";

            var postData = string.Empty;
            postData += "origem=comprovante&";
            postData += "cnpj=" + new Regex(@"[^\d]").Replace(aCNPJ, string.Empty) + "&";
            postData += "txtTexto_captcha_serpro_gov_br=" + aCaptcha + "&";
            postData += "submit1=Consultar&";
            postData += "search_type=cnpj";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            var stHtml = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            return stHtml.ReadToEnd();
        }
        #endregion

        public override int _sistema_id() { return (int)Sistema.DWMCONDOMINIOS; }
        public override string getListName()
        {
            return "Listar Fornecedores";
        }

        #region List
        [AuthorizeFilter]
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                ListViewCredor l = new ListViewCredor();
                return this._List(index, pageSize, "Browse", l, descricao);
            }
            else
                return View();
        }

        [AuthorizeFilter]
        public ActionResult _ListCredorModal(int? index, int? pageSize = 50, string descricao = null)
        {
            if (ViewBag.ValidateRequest)
            {
                LookupCredorFiltroModel l = new LookupCredorFiltroModel();
                return this.ListModal(index, pageSize, l, "Descrição", descricao);
            }
            else
                return View();
        }
        #endregion

        #region CreateError
        //public override void OnCreateError(ref CredorViewModel value, ICrudContext<CredorViewModel> model, FormCollection collection)
        //{
        //    value.nome_correio = collection["nome_correio1"] ?? "";
        //}
        #endregion

        #region BeforeCreate
        public override void BeforeCreate(ref CredorViewModel value, ICrudContext<CredorViewModel> model, FormCollection collection)
        {
            if (value.ind_tipo_pessoa == "PF")
            {
                if (collection["dt_nascimento"] != "")
                    value.dt_nascimento = DateTime.Parse(collection["dt_nascimento"].Substring(6, 4) + "-" + collection["dt_nascimento"].Substring(3, 2) + "-" + collection["dt_nascimento"].Substring(0, 2));
            }
            else
            {
                value.sexo = null;
                value.dt_nascimento = null;
            }
        }
        #endregion

        #region edit
        [AuthorizeFilter]
        public ActionResult Edit(int credorId)
        {
            return _Edit(new CredorViewModel() { credorId = credorId });
        }

        #region BeforeEdit
        public override void BeforeEdit(ref CredorViewModel value, ICrudContext<CredorViewModel> model, FormCollection collection)
        {
            BeforeCreate(ref value, model, collection);
        }
        #endregion

        #endregion

        #region Delete
        [AuthorizeFilter]
        public ActionResult Delete(int credorId)
        {
            return Edit(credorId);
        }
        #endregion

        public JsonResult getNames()
        {
            return JSonTypeahead(null, new ListViewCredor());
        }
    }
}