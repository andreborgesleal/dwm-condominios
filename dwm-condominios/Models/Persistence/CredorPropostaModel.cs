using App_Dominio.Entidades;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App_Dominio.Contratos;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using App_Dominio.Component;
using System.IO;
using App_Dominio.Security;
using System.Data.Entity.SqlServer;

namespace DWM.Models.Persistence
{
    public class CredorPropostaModel : CrudModelLocal<CredorProposta, CredorPropostaViewModel>
    {
        #region Constructor
        public CredorPropostaModel() { }
        public CredorPropostaModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        public CredorPropostaModel(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }

        #endregion

        #region Métodos da classe CrudModel

        public override CredorProposta MapToEntity(CredorPropostaViewModel value)
        {
            CredorProposta entity = Find(value);

            if (entity == null)
                entity = new CredorProposta();

            entity.CredorPropostaID = value.CredorPropostaID;
            entity.LicitacaoID = value.LicitacaoID;
            entity.credorId = value.credorId;
            entity.DataProposta = value.DataProposta;
            entity.Valor = value.Valor;
            entity.ArquivoProposta = value.ArquivoProposta;

            return entity;
        }

        public override CredorPropostaViewModel MapToRepository(CredorProposta entity)
        {
            return new CredorPropostaViewModel()
            {
                CredorPropostaID = entity.CredorPropostaID,
                LicitacaoID = entity.LicitacaoID,
                credorId = entity.credorId,
                DataProposta = entity.DataProposta,
                Valor = entity.Valor,
                ArquivoProposta = entity.ArquivoProposta,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override CredorProposta Find(CredorPropostaViewModel key)
        {
            return db.CredorPropostas.Find(key.CredorPropostaID);
        }

        public override Validate Validate(CredorPropostaViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.LicitacaoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Proposta").ToString();
                value.mensagem.MessageBase = "Código identificador da Proposta deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (value.credorId == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Código identificador do credor deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (value.LicitacaoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Licitação").ToString();
                value.mensagem.MessageBase = "Código identificador Licitação deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (value.Valor == decimal.MinValue)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Código identificador Tipo de Serviço deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            else if (DateTime.MinValue == value.DataProposta)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "DataEdital").ToString();
                value.mensagem.MessageBase = "Data do edital deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation != Crud.EXCLUIR)
            {
                if (value.empresaId == 0)
                {
                    value.mensagem.Code = 35;
                    value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                    value.mensagem.MessageBase = "Sua sessão expirou. Faça um novo login no sistema";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        public override CredorPropostaViewModel CreateRepository(HttpRequestBase Request = null)
        {
            CredorPropostaViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            return u;
        }

        #endregion
    }

    public class ListViewCredorProposta : ListViewModelLocal<CredorPropostaViewModel>
    {
        public ListViewCredorProposta()
        {
        }

        public ListViewCredorProposta(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #region Métodos da classe ListViewRepository
        public override IEnumerable<CredorPropostaViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            var q = (from value in db.CredorPropostas
                     select new CredorPropostaViewModel
                     {
                         CredorPropostaID = value.CredorPropostaID,
                         empresaId = sessaoCorrente.empresaId,
                         LicitacaoID = value.LicitacaoID,
                         credorId = value.credorId,
                         DataProposta = value.DataProposta,
                         Valor = value.Valor,
                         ArquivoProposta = value.ArquivoProposta,
                         sessionId = sessaoCorrente.sessaoId,
                     }).ToList();

            return q;
        }

        public override string action()
        {
            return "../Home/ListCredorProposta";
        }

        public override string DivId()
        {
            return "div-CredorProposta";
        }

        public override Repository getRepository(Object id)
        {
            return new CredorPropostaModel().getObject((CredorPropostaViewModel)id);
        }
        #endregion
    }
}