﻿using System.Collections.Generic;
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
        public IEnumerable<SelectListItem> Espacos(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();

            using (ApplicationContext db = new ApplicationContext())
            {
                Sessao sessao = security.getSessaoCorrente();

                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from e in db.EspacoComums.AsEnumerable()
                            where e.CondominioID == sessao.empresaId
                            orderby e.Descricao
                            select new SelectListItem()
                            {
                                Value = e.EspacoID.ToString(),
                                Text = e.Descricao,
                                Selected = (selectedValue != "" ? e.Descricao.Equals(selectedValue) : false)
                            }).ToList();

                return q;
            }
        }

        public IEnumerable<SelectListItem> Notas(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();

            using (ApplicationContext db = new ApplicationContext())
            {
                Sessao sessao = security.getSessaoCorrente();

                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q.Add(new SelectListItem() { Value = "0", Text = "Sem rendimento" });
                q.Add(new SelectListItem() { Value = "1", Text = "Insuficiente" });
                q.Add(new SelectListItem() { Value = "2", Text = "Ruim" });
                q.Add(new SelectListItem() { Value = "3", Text = "Regular" });
                q.Add(new SelectListItem() { Value = "4", Text = "Ótimo" });

                return q;
            }
        }

        public IEnumerable<SelectListItem> GrupoFornecedores(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();

            using (ApplicationContext db = new ApplicationContext())
            {
                Sessao sessao = security.getSessaoCorrente();

                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from e in db.GrupoCredores.AsEnumerable()
                            where e.CondominioID == sessao.empresaId
                            orderby e.nome
                            select new SelectListItem()
                            {
                                Value = e.grupoCredorId.ToString(),
                                Text = e.nome,
                                Selected = (selectedValue != "" ? e.nome.Equals(selectedValue) : false)
                            }).ToList();

                return q;
            }
        }

        public IEnumerable<SelectListItem> PatrimonioLocalizacao(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();

            using (ApplicationContext db = new ApplicationContext())
            {
                Sessao sessao = security.getSessaoCorrente();

                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from e in db.PatrimonioLocalizacaos.AsEnumerable()
                            orderby e.Descricao
                            select new SelectListItem()
                            {
                                Value = e.PatrimonioLocalizacaoID.ToString(),
                                Text = e.Descricao,
                                Selected = (selectedValue != "" ? e.Descricao.Equals(selectedValue) : false)
                            }).ToList();

                return q;
            }
        }

        public IEnumerable<SelectListItem> PatrimonioClassificacao(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();

            using (ApplicationContext db = new ApplicationContext())
            {
                Sessao sessao = security.getSessaoCorrente();

                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from e in db.PatrimonioClassificacaos.AsEnumerable()
                            orderby e.Descricao
                            select new SelectListItem()
                            {
                                Value = e.PatrimonioClassificacaoID.ToString(),
                                Text = e.Descricao,
                                Selected = (selectedValue != "" ? e.Descricao.Equals(selectedValue) : false)
                            }).ToList();

                return q;
            }
        }

        public IEnumerable<SelectListItem> CredorID(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();

            using (ApplicationContext db = new ApplicationContext())
            {
                Sessao sessao = security.getSessaoCorrente();

                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from e in db.Credores.AsEnumerable()
                            orderby e.nome
                            select new SelectListItem()
                            {
                                Value = e.credorId.ToString(),
                                Text = e.nome,
                                Selected = (selectedValue != "" ? e.nome.Equals(selectedValue) : false)
                            }).ToList();

                return q;
            }
        }

        public IEnumerable<SelectListItem> TiposPrestadores(params object[] param)
        {
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            using (ApplicationContext db = new ApplicationContext())
            {
                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != null)
                    q.Add(new SelectListItem() { Text = cabecalho, Value = "" });

                q = q.Union(from tp in db.PrestadorTipos.AsEnumerable()
                            orderby tp.Descricao
                            select new SelectListItem()
                            {
                                Value = tp.PrestadorTipoID.ToString(),
                                Text = tp.Descricao.ToString()
                            }).ToList();
                return q;
            }
        }

        public IEnumerable<SelectListItem> EspacosComuns(params object[] param)
        {
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
                    q.Add(new SelectListItem() { Text = cabecalho, Value = "" });

                q = q.Union(from ec in db.EspacoComums.AsEnumerable()
                            where ec.CondominioID == _CondominioID
                            orderby ec.Descricao
                            select new SelectListItem()
                            {
                                Value = ec.EspacoID.ToString(),
                                Text = ec.Descricao.ToString()
                            }).ToList();
                return q;
            }
        }


        public IEnumerable<SelectListItem> Cargos(params object[] param)
        {
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

                if (cabecalho != null)
                    q.Add(new SelectListItem() { Text = cabecalho, Value = "" });

                q = q.Union(from c in db.Cargos.AsEnumerable()
                            where c.CondominioID == _CondominioID && c.Situacao == "A"
                            orderby c.Descricao
                            select new SelectListItem()
                            {
                                Value = c.CargoID.ToString(),
                                Text = c.Descricao.ToString()
                            }).ToList();
                return q;
            }
        }

        public IEnumerable<SelectListItem> Unidades2(params object[] param)
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
                IEnumerable<Unidade> Unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;

                if (Unidades == null)
                {
                    if (cabecalho != "")
                        q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                    q = q.Union(from u in db.Unidades.AsEnumerable()
                                where u.CondominioID.Equals(_CondominioID) && u.EdificacaoID.Equals(_EdificacaoID)
                                orderby u.Codigo
                                select new SelectListItem()
                                {
                                    Value = u.UnidadeID.ToString(),
                                    Text = u.Codigo,
                                    Selected = (selectedValue != "" ? u.UnidadeID.ToString() == selectedValue : false)
                                }).ToList();

                    return q;
                }
                else
                {
                    q = q.Union(from u in Unidades
                                orderby u.UnidadeID
                                select new SelectListItem()
                                {
                                    Value = u.UnidadeID.ToString(),
                                    Text = u.Codigo,
                                    Selected = (selectedValue != "" ? u.UnidadeID.ToString() == selectedValue : false)
                                }).ToList();

                    return q;
                }
            }
        }

        public IEnumerable<SelectListItem> Edificacoes2(params object[] param)
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

                IEnumerable<Unidade> Unidades = DWMSessaoLocal.GetSessaoLocal().Unidades;

                if (Unidades == null)
                {
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
                else
                {
                    q = q.Union(from e in Unidades
                                orderby e.EdificacaoID
                                select new SelectListItem()
                                {
                                    Value = e.EdificacaoID.ToString(),
                                    Text = db.Edificacaos.Find(e.EdificacaoID).Descricao,
                                    Selected = (selectedValue != "" ? e.EdificacaoID.ToString() == selectedValue : false)
                                }).ToList();

                    return q;
                }
            }
        }

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
                            orderby u.Codigo
                            select new SelectListItem()
                            {
                                Value = u.UnidadeID.ToString(),
                                Text = u.Codigo,
                                Selected = (selectedValue != "" ? u.UnidadeID.ToString() == selectedValue : false),
                            }).ToList();

                return q.AsEnumerable().ToList();
            }
        }

        public IEnumerable<SelectListItem> UnidadesDesocupadas(params object[] param)
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

                q = q.Union(from u in db.Unidades
                            where u.CondominioID.Equals(_CondominioID) 
                                  && u.EdificacaoID.Equals(_EdificacaoID)
                                  && !(from cunid in db.CondominoUnidades
                                       where cunid.CondominioID == _CondominioID 
                                             && cunid.EdificacaoID == _EdificacaoID
                                             && cunid.DataFim == null
                                       select new { cunid.CondominioID, cunid.EdificacaoID, cunid.UnidadeID }).Contains(new { u.CondominioID, u.EdificacaoID, u.UnidadeID })
                            orderby u.Codigo
                            select new SelectListItem()
                            {
                                Value = u.UnidadeID.ToString(),
                                Text = u.Codigo,
                                Selected = (selectedValue != "" ? u.UnidadeID.ToString() == selectedValue : false),
                            }).ToList();

                return q.AsEnumerable().ToList();
            }
        }

        public IEnumerable<SelectListItem> UnidadesSemProprietario(params object[] param)
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

                q = q.Union(from u in db.Unidades
                            where u.CondominioID.Equals(_CondominioID)
                                  && u.EdificacaoID.Equals(_EdificacaoID)
                                  && !(from punid in db.ProprietarioUnidades
                                       where punid.CondominioID == _CondominioID
                                             && punid.EdificacaoID == _EdificacaoID
                                             && punid.DataFim == null
                                       select new { punid.CondominioID, punid.EdificacaoID, punid.UnidadeID }).Contains(new { u.CondominioID, u.EdificacaoID, u.UnidadeID })
                            orderby u.Codigo
                            select new SelectListItem()
                            {
                                Value = u.UnidadeID.ToString(),
                                Text = u.Codigo,
                                Selected = (selectedValue != "" ? u.UnidadeID.ToString() == selectedValue : false),
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

        public IEnumerable<SelectListItem> RamoAtividade(params object[] param)
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

                q = q.Union(from e in db.RamoAtividades.AsEnumerable()
                            orderby e.Descricao
                            select new SelectListItem()
                            {
                                Value = e.RamoAtividadeID.ToString(),
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

        public IEnumerable<SelectListItem> ChamadoMotivos(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            int _CondominioID = security.getSessaoCorrente().empresaId;

            using (ApplicationContext db = new ApplicationContext())
            {
                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from m in db.ChamadoMotivos.AsEnumerable()
                            where m.CondominioID == _CondominioID
                            orderby m.Descricao
                            select new SelectListItem()
                            {
                                Value = m.ChamadoMotivoID.ToString(),
                                Text = m.Descricao,
                                Selected = (selectedValue != "" ? m.ChamadoMotivoID.ToString() == selectedValue : false)
                            }).ToList();

                if ((selectedValue == "" || selectedValue == null || selectedValue == "0") && q.Count > 0)
                    q[0].Selected = true;

                return q;
            }
        }

        public IEnumerable<SelectListItem> ChamadoStatus(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            int _CondominioID = security.getSessaoCorrente().empresaId;

            using (ApplicationContext db = new ApplicationContext())
            {
                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from m in db.ChamadoStatuss.AsEnumerable()
                            where m.CondominioID == _CondominioID
                            orderby m.Descricao
                            select new SelectListItem()
                            {
                                Value = m.ChamadoStatusID.ToString(),
                                Text = m.Descricao,
                                Selected = (selectedValue != "" ? m.ChamadoStatusID.ToString() == selectedValue : false)
                            }).ToList();

                if ((selectedValue == "" || selectedValue == null || selectedValue == "0") && q.Count > 0)
                    q[0].Selected = true;

                return q;
            }
        }

        public IEnumerable<SelectListItem> Filas(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();
            IList<SelectListItem> q = new List<SelectListItem>();

            if (cabecalho != "")
                q.Add(new SelectListItem() { Value = "", Text = cabecalho });

            using (ApplicationContext db = new ApplicationContext())
            {
                using (SecurityContext seguranca_db = new SecurityContext())
                {
                    EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                    Sessao sessaoCorrente = security._getSessaoCorrente(seguranca_db);
                    SessaoLocal SessaoLocal = DWMSessaoLocal.GetSessaoLocal(sessaoCorrente, db);
                    if (SessaoLocal.CondominoID > 0) // usuário logado é um condômino
                    {
                        q = q.Union(from f in db.FilaAtendimentos.AsEnumerable()
                                    where f.CondominioID == SessaoLocal.empresaId &&
                                            f.VisibilidadeCondomino == "S" &&
                                            f.IsFornecedor != "S"
                                    orderby f.Descricao
                                    select new SelectListItem()
                                    {
                                        Value = f.FilaAtendimentoID.ToString(),
                                        Text = f.Descricao,
                                        Selected = (selectedValue != "" ? f.FilaAtendimentoID.ToString() == selectedValue : false)
                                    }).ToList();
                    }
                    else if (SessaoLocal.FilaFornecedorID.HasValue && SessaoLocal.FilaFornecedorID.Value > 0) // É um fornecedor
                    {
                        int FilaCondominoID = DWMSessaoLocal.FilaCondominoID(sessaoCorrente, db);
                        q = q.Union(from f in db.FilaAtendimentos.AsEnumerable()
                                    where f.CondominioID == SessaoLocal.empresaId &&
                                            f.IsFornecedor == "N" &&
                                            f.FilaAtendimentoID != FilaCondominoID
                                    orderby f.Descricao
                                    select new SelectListItem()
                                    {
                                        Value = f.FilaAtendimentoID.ToString(),
                                        Text = f.Descricao,
                                        Selected = (selectedValue != "" ? f.FilaAtendimentoID.ToString() == selectedValue : false)
                                    }).ToList();
                    }
                    else
                    {
                        q = q.Union(from f in db.FilaAtendimentos.AsEnumerable()
                                    where f.CondominioID == SessaoLocal.empresaId
                                    orderby f.Descricao
                                    select new SelectListItem()
                                    {
                                        Value = f.FilaAtendimentoID.ToString(),
                                        Text = f.Descricao,
                                        Selected = (selectedValue != "" ? f.FilaAtendimentoID.ToString() == selectedValue : false)
                                    }).ToList();
                    }
                }
            }

            if ((selectedValue == "" || selectedValue == null || selectedValue == "0") && q.Count > 0)
                q[0].Selected = true;

            return q;
        }

        public IEnumerable<SelectListItem> FilaSolicitante(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();
            IList<SelectListItem> q = new List<SelectListItem>();

            if (cabecalho != "")
                q.Add(new SelectListItem() { Value = "", Text = cabecalho });

            using (ApplicationContext db = new ApplicationContext())
            {
                using (SecurityContext seguranca_db = new SecurityContext())
                {
                    EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                    Sessao sessaoCorrente = security._getSessaoCorrente(seguranca_db);
                    SessaoLocal SessaoLocal = DWMSessaoLocal.GetSessaoLocal(sessaoCorrente, db);
                    if (SessaoLocal.CondominoID > 0) // usuário logado é um condômino
                    {
                        q = q.Union(from f in db.FilaAtendimentos.AsEnumerable()
                                    where f.CondominioID == SessaoLocal.empresaId &&
                                            f.Descricao == "Condôminos"
                                    orderby f.Descricao
                                    select new SelectListItem()
                                    {
                                        Value = f.FilaAtendimentoID.ToString(),
                                        Text = f.Descricao,
                                        Selected = (selectedValue != "" ? f.FilaAtendimentoID.ToString() == selectedValue : false)
                                    }).ToList();
                    }
                    else
                        q = q.Union(from f in db.FilaAtendimentos.AsEnumerable()
                                    join u in db.FilaAtendimentoUsuarios.AsEnumerable() on f.FilaAtendimentoID equals u.FilaAtendimentoID
                                    where f.CondominioID == SessaoLocal.empresaId &&
                                            u.UsuarioID == SessaoLocal.usuarioId &&
                                            u.Situacao == "A"
                                    orderby f.Descricao
                                    select new SelectListItem()
                                    {
                                        Value = f.FilaAtendimentoID.ToString(),
                                        Text = f.Descricao,
                                        Selected = (selectedValue != "" ? f.FilaAtendimentoID.ToString() == selectedValue : false)
                                    }).ToList();
                }
            }
            if ((selectedValue == "" || selectedValue == null || selectedValue == "0") && q.Count > 0)
                q[0].Selected = true;
            return q;
        }

        public IEnumerable<SelectListItem> UsuariosFila(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();
            int FilaAtendimentoID = (int)param[2];
            IList<SelectListItem> q = new List<SelectListItem>();

            if (cabecalho != "")
                q.Add(new SelectListItem() { Value = "", Text = cabecalho });

            using (ApplicationContext db = new ApplicationContext())
            {
                using (SecurityContext seguranca_db = new SecurityContext())
                {
                    EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                    Sessao sessaoCorrente = security._getSessaoCorrente(seguranca_db);
                    q = q.Union(from f in db.FilaAtendimentos.AsEnumerable()
                                join u in db.FilaAtendimentoUsuarios.AsEnumerable() on f.FilaAtendimentoID equals u.FilaAtendimentoID
                                where f.CondominioID == sessaoCorrente.empresaId &&
                                      f.FilaAtendimentoID == FilaAtendimentoID &&
                                      u.Situacao == "A"
                                orderby u.Nome
                                select new SelectListItem()
                                {
                                    Value = u.UsuarioID.ToString(),
                                    Text = u.Nome,
                                    Selected = (selectedValue != "" ? u.UsuarioID.ToString() == selectedValue : false)
                                }).ToList();
                }
            }
            if ((selectedValue == "" || selectedValue == null || selectedValue == "0") && q.Count > 0)
                q[0].Selected = true;
            return q;
        }

        public IEnumerable<SelectListItem> PrestadorCondominio()
        {
            IList<SelectListItem> q = new List<SelectListItem>();
            q.Add(new SelectListItem() { Value = "N", Text = "Não" });
            q.Add(new SelectListItem() { Value = "S", Text = "Sim" });
            return q;
        }

        public IEnumerable<SelectListItem> Sexo()
        {
            IList<SelectListItem> q = new List<SelectListItem>();
            q.Add(new SelectListItem() { Value = "M", Text = "Masculino" });
            q.Add(new SelectListItem() { Value = "F", Text = "Feminino" });
            return q;
        }

        public IEnumerable<SelectListItem> SituacaoVisitante(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();

            using (ApplicationContext db = new ApplicationContext())
            {
                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q.Add(new SelectListItem() { Value = "A", Text = "Ativo" });
                q.Add(new SelectListItem() { Value = "D", Text = "Desativado" });

                return q;
            }
        }

        public IEnumerable<SelectListItem> TipoUnidade(string SelectedValue)
        {
            IList<SelectListItem> q = new List<SelectListItem>();
            q.Add(new SelectListItem() { Value = "R", Text = "Residencial", Selected = (SelectedValue != null && SelectedValue != "" ? SelectedValue == "R" : false) });
            q.Add(new SelectListItem() { Value = "C", Text = "Comercial", Selected = (SelectedValue != null && SelectedValue != "" ? SelectedValue == "C" : false) });
            return q;
        }

        public IEnumerable<SelectListItem> TipoCondomino(string SelectedValue)
        {
            IList<SelectListItem> q = new List<SelectListItem>();
            q.Add(new SelectListItem() { Value = "F", Text = "Pessoa Física", Selected = (SelectedValue != null && SelectedValue != "" ? SelectedValue == "F" : false) });
            q.Add(new SelectListItem() { Value = "J", Text = "Pessoa Jurídica", Selected = (SelectedValue != null && SelectedValue != "" ? SelectedValue == "J" : false) });
            return q;
        }

        public IEnumerable<SelectListItem> Cidades(params object[] param)
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

                q = q.Union(from e in db.Cidades.AsEnumerable()
                            orderby e.Nome
                            select new SelectListItem()
                            {
                                Value = e.CidadeID.ToString(),
                                Text = e.Nome,
                                Selected = (selectedValue != "" ? e.Nome.Equals(selectedValue) : false)
                            }).ToList();

                return q;
            }
        }

        public IEnumerable<SelectListItem> SituacaoAluguel()
        {
            IList<SelectListItem> q = new List<SelectListItem>();
            q.Add(new SelectListItem() { Value = "A", Text = "Autorizado" });
            q.Add(new SelectListItem() { Value = "R", Text = "Revogado" });
            return q;
        }
    }
}