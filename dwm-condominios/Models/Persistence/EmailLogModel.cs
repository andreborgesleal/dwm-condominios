using App_Dominio.Entidades;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using App_Dominio.Contratos;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using System.Web;

namespace DWM.Models.Persistence
{
    public class EmailLogModel : CrudModelLocal<EmailLog, EmailLogViewModel>
    {
        #region Constructor
        public EmailLogModel() { }
        public EmailLogModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override EmailLog MapToEntity(EmailLogViewModel value)
        {
            EmailLog entity = Find(value);

            if (entity == null)
                entity = new EmailLog();

            entity.EmailLogID = value.EmailLogID;
            entity.EmailTipoID = value.EmailTipoID;
            entity.CondominioID = value.CondominioID;
            entity.EdificacaoID = value.EdificacaoID;
            entity.UnidadeID = value.UnidadeID;
            entity.GrupoCondominoID = value.GrupoCondominoID;
            entity.DataEmail = Funcoes.Brasilia();
            entity.Assunto = value.Assunto;
            entity.EmailMensagem = value.EmailMensagem;
            return entity;
        }

        public override EmailLogViewModel MapToRepository(EmailLog entity)
        {
            return new EmailLogViewModel()
            {
                empresaId = entity.CondominioID,
                EmailLogID = entity.EmailLogID,
                EmailTipoID = entity.EmailTipoID,
                CondominioID = entity.CondominioID,
                EdificacaoID = entity.EdificacaoID,
                UnidadeID = entity.UnidadeID,
                GrupoCondominoID = entity.GrupoCondominoID,
                DataEmail = entity.DataEmail,
                Assunto = entity.Assunto,
                EmailMensagem = entity.EmailMensagem,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override EmailLog Find(EmailLogViewModel key)
        {
            return db.EmailLogs.Find(key.EmailLogID);
        }

        public override Validate Validate(EmailLogViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.EmailLogID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID").ToString();
                value.mensagem.MessageBase = "Código identificador do LOG de auditoria do E-mail deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (value.CondominioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Código identificador do condomínio deve ser informado";
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

                if (value.EmailTipoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Tipo do E-mail").ToString();
                    value.mensagem.MessageBase = "Tipo do e-mail deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Assunto == null || value.Assunto.Trim().Length < 5)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Assunto").ToString();
                    value.mensagem.MessageBase = "Assunto deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.EmailMensagem == null || value.EmailMensagem.Trim().Length <= 20)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Resumo").ToString();
                    value.mensagem.MessageBase = "Mensagem do e-mail deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        public override EmailLogViewModel CreateRepository(HttpRequestBase Request = null)
        {
            EmailLogViewModel log = base.CreateRepository(Request);
            log.CondominioID = SessaoLocal.empresaId;
            return log;
        }
        #endregion
    }
}