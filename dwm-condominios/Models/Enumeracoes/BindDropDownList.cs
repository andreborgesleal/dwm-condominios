using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using System.Data.Objects.SqlClient;
using DWM.Models.Entidades;
using App_Dominio.Security;
using App_Dominio.Entidades;
using static DWM.Models.Enumeracoes.Enumeradores;

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

        public IEnumerable<SelectListItem> EmailTipos(params object[] param)
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

                q = q.Union(from t in db.EmailTipos.AsEnumerable()
                            where t.CondominioID == _CondominioID
                            orderby t.Descricao
                            select new SelectListItem()
                            {
                                Value = t.EmailTipoID.ToString(),
                                Text = t.Descricao,
                                Selected = (selectedValue != "" ? t.EmailTipoID.ToString() == selectedValue : false)
                            }).ToList();

                return q;
            }
        }

        public IEnumerable<SelectListItem> EmailTemplates(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            // params[2] -> Condominio
            // params[3] -> EmailTipo (Informativo, Cadastro, etc.)
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();
            int _CondominioID = 0;
            int _EmailTipoID = 0;

            if (param.Count() > 2)
                _CondominioID = (int)param[2];
            else
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                _CondominioID = security.getSessaoCorrente().empresaId;
            }

            if (param.Count() > 3)
                _EmailTipoID = (int)param[3];

            using (ApplicationContext db = new ApplicationContext())
            {
                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from t in db.EmailTemplates.AsEnumerable()
                            where t.CondominioID == _CondominioID && (_EmailTipoID == 0 || t.EmailTipoID == _EmailTipoID)
                            orderby t.Nome
                            select new SelectListItem()
                            {
                                Value = t.EmailTemplateID.ToString(),
                                Text = t.Nome,
                                Selected = (selectedValue != "" ? t.EmailTemplateID.ToString() == selectedValue : false)
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

        public IEnumerable<SelectListItem> Usuarios(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            EmpresaSecurity<SecurityContext> Security = new EmpresaSecurity<SecurityContext>();
            

            using (ApplicationContext db = new ApplicationContext())
            {
                using (SecurityContext seguranca_db = new SecurityContext())
                {
                    IList<SelectListItem> q = new List<SelectListItem>();

                    if (cabecalho != "")
                        q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                    Sessao sessaoCorrente = Security._getSessaoCorrente(seguranca_db);

                    int GRUPO_USUARIO = int.Parse(db.Parametros.Find(sessaoCorrente.empresaId, (int)Param.GRUPO_USUARIO).Valor);
                    int GRUPO_CREDENCIADO = int.Parse(db.Parametros.Find(sessaoCorrente.empresaId, (int)Param.GRUPO_CREDENCIADO).Valor);

                    q = q.Union(from u in seguranca_db.Usuarios.AsEnumerable()
                                join g in seguranca_db.UsuarioGrupos.AsEnumerable() on u.usuarioId equals g.usuarioId
                                where u.empresaId == sessaoCorrente.empresaId && g.grupoId != GRUPO_USUARIO && g.grupoId != GRUPO_CREDENCIADO
                                orderby u.nome
                                select new SelectListItem()
                                {
                                    Value = u.usuarioId.ToString(),
                                    Text = u.nome,
                                    Selected = (selectedValue != "" ? u.nome.Equals(selectedValue) : false)
                                }).GroupBy(info => info.Value).Select(m => m.First()).ToList();

                    return q;
                }
            }
        }
    }
}