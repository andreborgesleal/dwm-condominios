using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using System.Data.Objects.SqlClient;
using DWM.Models.Entidades;
using App_Dominio.Security;
using App_Dominio.Entidades;

namespace DWM.Models.Enumeracoes
{
    public class BindDropDownList
    {
        public IEnumerable<SelectListItem> Unidades(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();
            int _EdificacaoID = int.Parse(param[2].ToString());
            int _CondominioID = 0;

            if (param.Count() > 3)
                _CondominioID = (int)param[3];
            else
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                _CondominioID = security.getSessaoCorrente().empresaId;
            }

            using (ApplicationContext db = new ApplicationContext())
            {
                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from u in db.Unidades.AsEnumerable()
                            where u.CondominioID.Equals(_CondominioID) && u.EdificacaoID.Equals(_EdificacaoID)
                            orderby u.UnidadeID
                            select new SelectListItem()
                            {
                                Value = u.UnidadeID.ToString(),
                                Text = u.UnidadeID.ToString(),
                                Selected = (selectedValue != "" ? u.UnidadeID.ToString() == selectedValue : false)
                            }).ToList();

                return q.AsEnumerable().ToList();
            }
        }

        public IEnumerable<SelectListItem> Edificacoes(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();
            int _CondominioID = 0;

            if (param.Count() > 2)
                _CondominioID = (int)param[2];
            else
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                _CondominioID = security.getSessaoCorrente().empresaId;
            }

            using (ApplicationContext db = new ApplicationContext())
            {
                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from e in db.Edificacaos.AsEnumerable()
                            where e.CondominioID == _CondominioID
                            orderby e.Descricao
                            select new SelectListItem()
                            {
                                Value = e.EdificacaoID.ToString(),
                                Text = e.Descricao,
                                Selected = (selectedValue != "" ? e.EdificacaoID.ToString() == selectedValue : false)
                            }).ToList();

                return q;
            }
        }

        public IEnumerable<SelectListItem> GrupoCondominos(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();
            int _CondominioID = 0;

            if (param.Count() > 2)
                _CondominioID = (int)param[2];
            else
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                _CondominioID = security.getSessaoCorrente().empresaId;
            }

            using (ApplicationContext db = new ApplicationContext())
            {
                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from g in db.GrupoCondominos.AsEnumerable()
                            where g.CondominioID == _CondominioID
                            orderby g.Descricao
                            select new SelectListItem()
                            {
                                Value = g.GrupoCondominoID.ToString(),
                                Text = g.Descricao,
                                Selected = (selectedValue != "" ? g.GrupoCondominoID.ToString() == selectedValue : false)
                            }).ToList();

                return q;
            }
        }

        public IEnumerable<SelectListItem> Profissoes(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            using (ApplicationContext db = new ApplicationContext())
            {
                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from e in db.Profissaos.AsEnumerable()
                            orderby e.Descricao
                            select new SelectListItem()
                            {
                                Value = e.ProfissaoID.ToString(),
                                Text = e.Descricao,
                                Selected = (selectedValue != "" ? e.Descricao.Equals(selectedValue) : false)
                            }).ToList();

                return q;
            }
        }

        public IEnumerable<SelectListItem> TipoCredenciados(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            using (ApplicationContext db = new ApplicationContext())
            {
                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from e in db.TipoCredenciados.AsEnumerable()
                            orderby e.Descricao
                            select new SelectListItem()
                            {
                                Value = e.TipoCredenciadoID.ToString(),
                                Text = e.Descricao,
                                Selected = (selectedValue != "" ? e.Descricao.Equals(selectedValue) : false)
                            }).ToList();

                return q;
            }
        }
    }
}