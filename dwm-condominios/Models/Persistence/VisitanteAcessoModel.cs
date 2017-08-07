using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using System.Web;
using App_Dominio.Security;
using dwm_condominios.Models.Persistence;
using System.Collections.Generic;
using System;
using App_Dominio.Component;
using App_Dominio.Repositories;

namespace DWM.Models.Persistence
{
    public class VisitanteAcessoModel : CrudModelLocal<VisitanteAcesso, VisitanteAcessoViewModel>
    {
        #region Constructor
        public VisitanteAcessoModel()
        {
        }

        public VisitanteAcessoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        private bool IsUserAdm()
        {
            return SessaoLocal.Unidades == null; // DWMSessaoLocal.GetSessaoLocal().Unidades == null;
        }

        private bool IsPortaria()
        {
            string grupo_portaria = db.Parametros.Find(SessaoLocal.empresaId, (int)Enumeracoes.Enumeradores.Param.GRUPO_PORTARIA).Valor;
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            IEnumerable<GrupoRepository> grp = security.getGrupoUsuario(SessaoLocal.usuarioId).AsEnumerable();

            return (from g in grp where g.grupoId == int.Parse(grupo_portaria) select g).Count() > 0;
        }

        #region Métodos da classe CrudContext
        public override VisitanteAcessoViewModel BeforeInsert(VisitanteAcessoViewModel value)
        {
            value.mensagem = new App_Dominio.Contratos.Validate() { Code = 0, Message = "Registro processado com sucesso !" };



            
            if (IsPortaria())
                value.DataAcesso = Funcoes.Brasilia();
            return base.BeforeInsert(value);
        }

        public override VisitanteAcessoViewModel BeforeUpdate(VisitanteAcessoViewModel value)
        {
            return BeforeInsert(value);
        }

        public override VisitanteAcessoViewModel BeforeDelete(VisitanteAcessoViewModel value)
        {
            value.mensagem = new App_Dominio.Contratos.Validate() { Code = 0, Message = "Registro processado com sucesso !" };

            #region Exclui o VisitanteAcessoUnidade
            VisitanteAcessoUnidade entity = db.VisitanteAcessoUnidades.Find(value.AcessoID);
            if (entity != null)
                this.db.Set<VisitanteAcessoUnidade>().Remove(entity);
            #endregion

            return base.BeforeDelete(value);
        }

        public override VisitanteAcesso MapToEntity(VisitanteAcessoViewModel value)
        {
            VisitanteAcesso entity = Find(value);

            if (entity == null)
            {
                entity = new VisitanteAcesso();
                value.DataInclusao = Funcoes.Brasilia();
            }

            entity.CondominioID = value.CondominioID;
            entity.VisitanteID = value.VisitanteID;
            entity.DataInclusao = value.DataInclusao;
            entity.DataAutorizacao = value.DataAutorizacao;
            entity.HoraInicio = value.HoraInicio;
            entity.HoraLimite = value.HoraLimite;
            entity.DataAcesso = value.DataAcesso;
            entity.Interfona = value.Interfona;
            entity.Observacao = value.Observacao;
            entity.AluguelID = value.AluguelID == 0 ? null : value.AluguelID;

            #region VisitanteAcessoUnidadeViewModel
            if (value.EdificacaoID != null && entity.VisitanteAcessoUnidade == null)
            {
                entity.VisitanteAcessoUnidade = new VisitanteAcessoUnidade();
                if (IsUserAdm())
                {
                    entity.VisitanteAcessoUnidade.AcessoID = value.AcessoID;
                    entity.VisitanteAcessoUnidade.CondominioID = value.CondominioID;
                    entity.VisitanteAcessoUnidade.EdificacaoID = value.EdificacaoID.Value;
                    entity.VisitanteAcessoUnidade.UnidadeID = value.UnidadeID.Value;
                    if (value.VisitanteAcessoUnidadeViewModel == null)
                        entity.VisitanteAcessoUnidade.CondominoID = db.CondominoUnidades.Where(info => info.CondominioID == value.CondominioID && info.EdificacaoID == value.EdificacaoID && info.UnidadeID == value.UnidadeID && info.DataFim == null).FirstOrDefault().CondominoID;
                    else
                        entity.VisitanteAcessoUnidade.CondominoID = value.VisitanteAcessoUnidadeViewModel.CondominoID;
                    entity.VisitanteAcessoUnidade.CredenciadoID = null;
                }
                else
                {
                    entity.VisitanteAcessoUnidade.AcessoID = value.AcessoID;
                    entity.VisitanteAcessoUnidade.CondominioID = value.CondominioID;
                    entity.VisitanteAcessoUnidade.EdificacaoID = value.EdificacaoID.Value;
                    entity.VisitanteAcessoUnidade.UnidadeID = value.UnidadeID.Value;
                    entity.VisitanteAcessoUnidade.CondominoID = DWMSessaoLocal.GetSessaoLocal(sessaoCorrente, this.db).CondominoID;
                    entity.VisitanteAcessoUnidade.CredenciadoID = DWMSessaoLocal.GetSessaoLocal(sessaoCorrente, this.db).CredenciadoID == 0 ? null : DWMSessaoLocal.GetSessaoLocal(sessaoCorrente, this.db).CredenciadoID;
                }
            }
            #endregion

            return entity;
        }

        public override VisitanteAcessoViewModel MapToRepository(VisitanteAcesso entity)
        {
            VisitanteAcessoViewModel v = new VisitanteAcessoViewModel()
            {
                AcessoID = entity.AcessoID,
                CondominioID = entity.CondominioID,
                empresaId = entity.CondominioID,
                VisitanteID = entity.VisitanteID,
                DataInclusao = entity.DataInclusao,
                DataAutorizacao = entity.DataAutorizacao,
                HoraInicio = entity.HoraInicio,
                HoraLimite = entity.HoraLimite,
                DataAcesso = entity.DataAcesso,
                Interfona = entity.Interfona,
                Observacao = entity.Observacao,
                AluguelID = entity.AluguelID,
                VisitanteAcessoUnidadeViewModel = new VisitanteAcessoUnidadeViewModel(),
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };

            VisitanteModel model = new VisitanteModel(this.db, this.seguranca_db);
            v.Visitante = model.getObject(new VisitanteViewModel() { VisitanteID = entity.VisitanteID });

            if (entity.VisitanteAcessoUnidade != null)
            {
                v.EdificacaoID = entity.VisitanteAcessoUnidade.EdificacaoID;
                v.UnidadeID = entity.VisitanteAcessoUnidade.UnidadeID;
                v.DescricaoEdificacao = db.Edificacaos.Where(info => info.EdificacaoID == v.EdificacaoID && info.CondominioID == v.CondominioID).FirstOrDefault().Descricao;

                v.VisitanteAcessoUnidadeViewModel = new VisitanteAcessoUnidadeViewModel()
                {
                    AcessoID = entity.VisitanteAcessoUnidade.AcessoID,
                    CondominioID = entity.VisitanteAcessoUnidade.CondominioID,
                    empresaId = entity.VisitanteAcessoUnidade.CondominioID,
                    EdificacaoID = entity.VisitanteAcessoUnidade.EdificacaoID,
                    UnidadeID = entity.VisitanteAcessoUnidade.UnidadeID,
                    CondominoID = entity.VisitanteAcessoUnidade.CondominoID,
                    CredenciadoID = entity.VisitanteAcessoUnidade.CredenciadoID,
                    VisitanteAcessoViewModel = v,
                    mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
                };
            }

            if (SessaoLocal.Unidades != null)
            {
                ListViewVisitante list = new ListViewVisitante(db, seguranca_db);
                v.Visitantes = list.Bind(0, 25, v.EdificacaoID, v.UnidadeID);
            }

            return v;
        }

        public override VisitanteAcesso Find(VisitanteAcessoViewModel key)
        {
            return db.VisitanteAcessos.Find(key.AcessoID);
        }

        public override Validate Validate(VisitanteAcessoViewModel value, Crud operation)
        {
            if (value.mensagem.Code != 0)
                return value.mensagem;

            if (operation == Crud.ALTERAR || operation == Crud.EXCLUIR)
                if (value.AcessoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Acesso ID").ToString();
                    value.mensagem.MessageBase = "Identificador de acesso deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

            if (value.empresaId == 0)
            {
                value.mensagem.Code = 35;
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "Sua sessão expirou. Faça um novo login no sistema";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.CondominioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Condomínio deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Valida exclusão => Não permitir excluir se o visitante já tiver acessado o condomínio
            if (operation == Crud.EXCLUIR)
            {
                if (db.VisitanteAcessos.Find(value.AcessoID).DataAcesso != null)
                {
                    value.mensagem.Code = 56;
                    value.mensagem.Message = MensagemPadrao.Message(56).ToString();
                    value.mensagem.MessageBase = "Registro não pode ser excluído porque o visitante/prestador já acessou o condomínio";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            #endregion

            if (operation != Crud.EXCLUIR)
            {
                if (SessaoLocal.Unidades != null && (value.EdificacaoID == 0 || value.UnidadeID == 0))
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Unidade").ToString();
                    value.mensagem.MessageBase = "Unidade deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.VisitanteID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Visitante/Prestador").ToString();
                    value.mensagem.MessageBase = "Visitante/Prestador deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.DataAutorizacao < Funcoes.Brasilia().Date)
                {
                    value.mensagem.Code = 7;
                    value.mensagem.Message = MensagemPadrao.Message(8, "Data da Autorização").ToString();
                    value.mensagem.MessageBase = "Data de Autorização inválida";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                #region valida hoa início e hora fim
                if (value.HoraInicio != null && value.HoraInicio.Trim().Length > 0)
                {
                    if (value.HoraInicio.Trim().Length < 5)
                    {
                        value.mensagem.Code = 4;
                        value.mensagem.Message = MensagemPadrao.Message(4, "Hora Início", value.HoraInicio).ToString();
                        value.mensagem.MessageBase = "Hora de Inicio inválida";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                    if (!Funcoes.ValidaHora(value.HoraInicio))
                    {
                        value.mensagem.Code = 4;
                        value.mensagem.Message = MensagemPadrao.Message(4, "Hora Início", value.HoraInicio).ToString();
                        value.mensagem.MessageBase = "Hora de Inicio inválida";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                }

                if (value.HoraLimite != null && value.HoraLimite.Trim().Length > 0)
                {
                    if (value.HoraLimite.Trim().Length < 5)
                    {
                        value.mensagem.Code = 4;
                        value.mensagem.Message = MensagemPadrao.Message(4, "Hora Limite", value.HoraLimite).ToString();
                        value.mensagem.MessageBase = "Hora Limite inválida";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }

                    if (!Funcoes.ValidaHora(value.HoraLimite))
                    {
                        value.mensagem.Code = 4;
                        value.mensagem.Message = MensagemPadrao.Message(4, "Hora Limite", value.HoraInicio).ToString();
                        value.mensagem.MessageBase = "Hora Limite inválida";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                }

                #endregion

                #region valida quantidade de visitantes => aluguel de espaço
                var _DataAtual = Funcoes.Brasilia();
                var _qte_acessos = 0;
                IEnumerable<AluguelEspacoViewModel> alugueis = (from a in db.AluguelEspacos
                                                                join e in db.EspacoComums on a.EspacoID equals e.EspacoID
                                                                where a.CondominioID == value.CondominioID
                                                                      && a.EdificacaoID == value.EdificacaoID
                                                                      && a.UnidadeID == value.UnidadeID
                                                                      && a.DataEvento >= _DataAtual
                                                                      && a.DataAutorizacao.HasValue
                                                                select new AluguelEspacoViewModel()
                                                                {
                                                                    DataEvento = a.DataEvento,
                                                                    LimitePessoas = e.LimitePessoas,
                                                                    DescricaoEspaco = e.Descricao
                                                                }).ToList();

                foreach (AluguelEspacoViewModel aluguel in alugueis)
                {
                    _qte_acessos = (from v in db.VisitanteAcessos
                                    join u in db.VisitanteAcessoUnidades on v.AcessoID equals u.AcessoID
                                    join prest in db.Visitantes on v.VisitanteID equals prest.VisitanteID
                                    where u.CondominioID == value.CondominioID
                                          && u.EdificacaoID == value.EdificacaoID
                                          && u.UnidadeID == value.UnidadeID
                                          && v.DataAutorizacao == aluguel.DataEvento
                                          && !prest.PrestadorTipoID.HasValue
                                    select v).Count();

                    if (_qte_acessos > aluguel.LimitePessoas)
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(53, aluguel.LimitePessoas.ToString()).ToString();
                        value.mensagem.MessageBase = "Quantidade de visitantes excede o limite permitido para acesso a(o) " + aluguel.DescricaoEspaco;
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                }
                #endregion

                #region valida se o visitante preencheu a RG ou CPF (somente se usuário for do grupo Portaria)
                if (IsPortaria())
                {
                    Visitante v = db.Visitantes.Find(value.VisitanteID);
                    if (String.IsNullOrEmpty(v.RG) && String.IsNullOrEmpty(v.CPF))
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(5, "RG ou CPF do Visitante/Prestador").ToString();
                        value.mensagem.MessageBase = "RG ou CPF do visitante/prestador deve ser informado para registrar o seu acesso ao condomínio";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                }
                #endregion
            }

            return value.mensagem;
        }

        public override VisitanteAcessoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            VisitanteAcessoViewModel acesso = base.CreateRepository(Request);
            this.SessaoLocal = DWMSessaoLocal.GetSessaoLocal();
            acesso.mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso !" };
            acesso.CondominioID = SessaoLocal.empresaId;
            acesso.empresaId = acesso.CondominioID;
            acesso.IsPortaria = IsPortaria();

            if (SessaoLocal.Unidades == null)
            {
                if (Request != null && Request ["EdificacaoID"] != null && Request["EdificacaoID"] != "")
                {
                    acesso.EdificacaoID = int.Parse(Request["EdificacaoID"]);
                    acesso.UnidadeID = int.Parse(Request["UnidadeID"]);
                }

                using (ApplicationContext db = new ApplicationContext())
                {
                    using (SecurityContext seguranca_db = new SecurityContext())
                    {
                        ListViewVisitante list = new ListViewVisitante(db, seguranca_db);
                        acesso.Visitantes = list.Bind(0, 25, 0, 0);
                    }
                }
            }
            else
            {
                acesso.EdificacaoID = SessaoLocal.Unidades.FirstOrDefault().EdificacaoID;
                acesso.UnidadeID = SessaoLocal.Unidades.FirstOrDefault().UnidadeID;
                using (ApplicationContext db = new ApplicationContext())
                {
                    using (SecurityContext seguranca_db = new SecurityContext())
                    {
                        ListViewVisitante list = new ListViewVisitante(db, seguranca_db);
                        acesso.Visitantes = list.Bind(0, 25, acesso.EdificacaoID, acesso.UnidadeID);
                    }
                }
            }
            acesso.DataAcesso = null;

            return acesso;
        }
        #endregion
    }

    public class ListViewVisitanteAcesso : ListViewModelLocal<VisitanteAcessoViewModel>
    {
        #region Constructor
        public ListViewVisitanteAcesso() { }
        public ListViewVisitanteAcesso(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        private bool IsPortaria()
        {
            string grupo_portaria = db.Parametros.Find(SessaoLocal.empresaId, (int)Enumeracoes.Enumeradores.Param.GRUPO_PORTARIA).Valor;
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            IEnumerable<GrupoRepository> grp = security.getGrupoUsuario(SessaoLocal.usuarioId).AsEnumerable();

            return (from g in grp where g.grupoId == int.Parse(grupo_portaria) select g).Count() > 0;
        }


        #region Métodos da classe ListViewRepository
        public override IEnumerable<VisitanteAcessoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;
            int _EdificacaoID;
            int _UnidadeID;
            bool IsCondomino = false;
            DateTime dataHoje = Funcoes.Brasilia().Date;

            if (SessaoLocal.CondominoID > 0)
            {
                _EdificacaoID = db.Edificacaos.Where(x => x.CondominioID == _CondominioID).FirstOrDefault().EdificacaoID;
                _UnidadeID = SessaoLocal.Unidades.FirstOrDefault().UnidadeID;
                IsCondomino = true;
            }
            else
            {
                _EdificacaoID = param != null && param.Count() > 1 && param[1] != null ? int.Parse(param[1].ToString()) : 0;
                _UnidadeID = param != null && param.Count() > 2 && param[2] != null ? int.Parse(param[2].ToString()) : 0;
            }

            bool _IsPortaria = IsPortaria();

            var q = (from v in db.Visitantes
                     join vu in db.VisitanteUnidades on v.VisitanteID equals vu.VisitanteID into vleft
                     from vu in vleft.DefaultIfEmpty()
                     join ed in db.Edificacaos on vu.EdificacaoID equals ed.EdificacaoID into vuleft
                     from ed in vuleft.DefaultIfEmpty()
                     join vac in db.VisitanteAcessos on v.VisitanteID equals vac.VisitanteID
                     join con in db.Condominos on vu.CondominoID equals con.CondominoID into conleft
                     from con in conleft.DefaultIfEmpty()
                     join tp in db.PrestadorTipos on v.PrestadorTipoID equals tp.PrestadorTipoID into tpleft
                     from tp in tpleft.DefaultIfEmpty()
                     where v.CondominioID == _CondominioID
                           && (_EdificacaoID == 0 || vu.EdificacaoID == _EdificacaoID)
                           && (_UnidadeID == 0 || vu.UnidadeID == _UnidadeID)
                           && v.Situacao == "A"
                           && ((!IsCondomino && vac.DataAutorizacao == dataHoje) || (IsCondomino && vac.DataAutorizacao >= dataHoje))
                     orderby v.Nome
                     select new VisitanteAcessoViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         CondominioID = v.CondominioID,
                         AcessoID = vac.AcessoID,
                         HoraInicio = vac.HoraInicio,
                         HoraLimite = vac.HoraLimite,
                         DataInclusao = vac.DataInclusao,
                         DataAcesso = vac.DataAcesso,
                         DataAutorizacao = vac.DataAutorizacao,
                         Interfona = vac.Interfona,
                         IsPortaria = _IsPortaria,
                         Visitante = new VisitanteViewModel()
                         {
                             Nome = v.Nome,
                             Sexo = v.Sexo == "M" ? "Masculino" : "Feminino",
                             RG = v.RG,
                             CPF = v.CPF,
                             DataInclusao = v.DataInclusao,
                             Fotografia = v.Fotografia,
                             OrgaoEmissor = v.OrgaoEmissor,
                             VisitanteID = v.VisitanteID,
                             PrestadorCondominio = v.PrestadorCondominio,
                             PrestadorTipoID = v.PrestadorTipoID,
                             Situacao = v.Situacao,
                             Telefone = v.Telefone,
                             UnidadeID = vu.UnidadeID,
                             DescricaoEdificacao = ed.Descricao,
                             NomeCondomino = con.Nome,
                             DescricaoTipoPrestador = tp.Descricao,
                             Placa = v.Placa,
                             Cor = v.Cor,
                             Descricao = v.Descricao,
                             Marca = v.Marca,
                         }
                     }).ToList();

            return q;
        }

        public override Repository getRepository(Object id)
        {
            return new FuncionarioModel().getObject((FuncionarioViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-visitantes";
        }
    }
}