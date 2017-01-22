using App_Dominio.Controllers;
using App_Dominio.Entidades;
using App_Dominio.Negocio;
using App_Dominio.Repositories;
using App_Dominio.Security;
using System;
using System.Web.Mvc;

namespace TimMaia.Controllers
{
    public class AlterarSenhaController : RootController<UsuarioRepository, AlterarSenhaModel>
    {
        #region Inheritance
        public override int _sistema_id() { return (int)DWM.Models.Enumeracoes.Sistema.DWMCONDOMINIOS; }
        public override string getListName()
        {
            return "Alterar Senha";
        }

        #region List
        public override ActionResult List(int? index, int? pageSize = 50, string descricao = null)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region edit
        [AuthorizeFilter]
        public ActionResult Edit()
        {
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            int _usuarioId = security.getSessaoCorrente().usuarioId;
            if (_usuarioId > 0)
                return _Edit(new AlterarSenhaRepository() { usuarioId = _usuarioId });
            else
                return View();
        }

        [HttpPost]
        [AuthorizeFilter]
        public override ActionResult Edit(UsuarioRepository value, FormCollection collection)
        {
            if (ViewBag.ValidateRequest)
            {
                AlterarSenhaRepository newValue = new AlterarSenhaRepository()
                {
                    usuarioId = value.usuarioId,
                    login = value.login,
                    nome = value.nome,
                    situacao = value.situacao,
                    isAdmin = value.isAdmin,
                    senha = value.senha,
                    confirmacaoSenha = value.confirmacaoSenha,
                    senhaAtual = collection["senhaAtual"]
                };

                UsuarioRepository ret = SetEdit(newValue, getModel(), collection);

                if (ret.mensagem.Code == 0)
                {
                    return RedirectToAction("Default", "Home");
                }
                else
                    return View(ret);
            }
            else
                return null;
        }

        #endregion
    }
}
