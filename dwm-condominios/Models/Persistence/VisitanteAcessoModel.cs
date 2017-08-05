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
            return DWMSessaoLocal.GetSessaoLocal().Unidades == null;
        }

        #region Métodos da classe CrudContext
        public override VisitanteAcesso MapToEntity(VisitanteAcessoViewModel value)
        {
            VisitanteAcesso entity = Find(value);

            if (entity == null)
            {
                entity = new VisitanteAcesso();
                entity.VisitanteAcessoUnidade = new List<VisitanteAcessoUnidade>();
            }
            else
                entity.VisitanteAcessoUnidade.Clear();

            entity.CondominioID = value.CondominioID;
            entity.VisitanteID = value.VisitanteID;
            entity.DataInclusao = Funcoes.Brasilia();
            entity.DataAutorizacao = value.DataAutorizacao;
            entity.HoraInicio = value.HoraInicio;
            entity.HoraLimite = value.HoraLimite;
            entity.DataAcesso = value.DataAcesso;
            entity.Interfona = value.Interfona;
            entity.Observacao = value.Observacao;
            entity.AluguelID = value.AluguelID;

            #region VisitanteAcessoUnidadeViewModel
            if (IsUserAdm())
            {
                entity.VisitanteAcessoUnidade.Add(new VisitanteAcessoUnidade()
                {
                    CondominioID = value.CondominioID,
                    EdificacaoID = value.EdificacaoID.Value,
                    UnidadeID = value.UnidadeID.Value,
                    CondominoID = value.VisitanteAcessoUnidadeViewModel.FirstOrDefault().CondominoID,
                    VisitanteAcesso = entity
                });
            }
            else
            {
                entity.VisitanteAcessoUnidade.Add(new VisitanteAcessoUnidade()
                {
                    CondominioID = value.CondominioID,
                    EdificacaoID = value.EdificacaoID.Value,
                    UnidadeID = value.UnidadeID.Value,
                    CondominoID = DWMSessaoLocal.GetSessaoLocal(sessaoCorrente, this.db).CondominoID,
                    CredenciadoID = DWMSessaoLocal.GetSessaoLocal(sessaoCorrente, this.db).CredenciadoID,
                    VisitanteAcesso = entity
                });
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
                VisitanteAcessoUnidadeViewModel = new List<VisitanteAcessoUnidadeViewModel>(),
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };

            foreach (VisitanteAcessoUnidade und in entity.VisitanteAcessoUnidade)
            {
                v.EdificacaoID = und.EdificacaoID;
                v.UnidadeID = und.UnidadeID;

                VisitanteAcessoUnidadeViewModel item = new VisitanteAcessoUnidadeViewModel()
                {
                    AcessoID = und.AcessoID,
                    CondominioID = und.CondominioID,
                    empresaId = und.CondominioID,
                    EdificacaoID = und.EdificacaoID,
                    UnidadeID = und.UnidadeID,
                    CondominoID = und.CondominoID,
                    CredenciadoID = und.CredenciadoID,
                    VisitanteAcessoViewModel = v,
                    mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
                };

                ((List<VisitanteAcessoUnidadeViewModel>)v.VisitanteAcessoUnidadeViewModel).Add(item);
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

            if (value.EdificacaoID == 0 || value.UnidadeID == 0)
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
            IEnumerable<AluguelEspacoViewModel> alugueis = (from a in db.AluguelEspacos join e in db.EspacoComums on a.EspacoID equals e.EspacoID
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

            return value.mensagem;
        }

        public override VisitanteAcessoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            VisitanteAcessoViewModel acesso = base.CreateRepository(Request);
            SessaoLocal SessaoLocal = DWMSessaoLocal.GetSessaoLocal();
            acesso.CondominioID = SessaoLocal.empresaId;
            acesso.empresaId = acesso.CondominioID;

            if (SessaoLocal.Unidades == null)
            {
                if (Request != null && Request ["EdificacaoID"] != null && Request["EdificacaoID"] != "")
                {
                    acesso.EdificacaoID = int.Parse(Request["EdificacaoID"]);
                    acesso.UnidadeID = int.Parse(Request["UnidadeID"]);
                    VisitanteModel model = new VisitanteModel();
                }

                //acesso.Visitantes = 
            }
            else
            {
                acesso.EdificacaoID = SessaoLocal.Unidades.FirstOrDefault().EdificacaoID;
                acesso.UnidadeID = SessaoLocal.Unidades.FirstOrDefault().UnidadeID;
                //acesso.Visitantes = 
            }
            acesso.DataAcesso = null;

            return acesso;
        }
        #endregion
    }
}