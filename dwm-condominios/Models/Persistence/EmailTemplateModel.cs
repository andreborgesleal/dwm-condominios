using App_Dominio.Contratos;
using App_Dominio.Entidades;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using App_Dominio.Enumeracoes;
using System.Web;
using App_Dominio.Security;
using App_Dominio.Component;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DWM.Models.Persistence
{
    public class EmailTemplateModel : CrudModel<EmailTemplate, EmailTemplateViewModel, ApplicationContext>
    {
        #region Constructor
        public EmailTemplateModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override EmailTemplate MapToEntity(EmailTemplateViewModel value)
        {
            EmailTemplate entity = Find(value);

            if (entity == null)
                entity = new EmailTemplate();

            entity.EmailTemplateID = value.EmailTemplateID;
            entity.EmailTipoID = value.EmailTipoID;
            entity.CondominioID = value.CondominioID;
            entity.Nome = value.Nome;
            entity.EmailMensagem = value.EmailMensagem;

            return entity;
        }

        public override EmailTemplateViewModel MapToRepository(EmailTemplate entity)
        {
            return new EmailTemplateViewModel()
            {
                EmailTemplateID = entity.EmailTemplateID,
                EmailTipoID = entity.EmailTipoID,
                CondominioID = entity.CondominioID,
                Nome = entity.Nome,
                EmailMensagem = entity.EmailMensagem,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override EmailTemplate Find(EmailTemplateViewModel key)
        {
            return db.EmailTemplates.Find(key.EmailTemplateID);
        }

        public override Validate Validate(EmailTemplateViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.EmailTemplateID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Template ID").ToString();
                value.mensagem.MessageBase = "Código identificador do modelo de e-mail deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            if (operation != Crud.EXCLUIR)
            {
                if (value.CondominioID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                    value.mensagem.MessageBase = "Condomínio deve ser informado";
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

                if (value.Nome == null || value.Nome.Trim().Length <= 3)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Nome").ToString();
                    value.mensagem.MessageBase = "Nome do modelo deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.EmailMensagem == null || value.EmailMensagem.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Corpo da Mensagem").ToString();
                    value.mensagem.MessageBase = "Corpo da mensagem deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        public override EmailTemplateViewModel CreateRepository(HttpRequestBase Request = null)
        {
            EmailTemplateViewModel value = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            value.CondominioID = security.getSessaoCorrente().empresaId;
            value.empresaId = value.CondominioID;
            return value;
        }
        #endregion
    }

    public class ListViewEmailTemplates : ListViewModel<EmailTemplateViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewEmailTemplates() { }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EmailTemplateViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? (string)param[0] : null;

            return (from ema in db.EmailTemplates
                    join tip in db.EmailTipos on ema.EmailTipoID equals tip.EmailTipoID
                    where ema.CondominioID == sessaoCorrente.empresaId
                    && (_nome == null || ema.Nome == _nome)
                    orderby ema.Nome
                    select new EmailTemplateViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        EmailTemplateID = ema.EmailTemplateID,
                        CondominioID = ema.CondominioID,
                        Descricao = tip.Descricao,
                        Nome = ema.Nome,
                        EmailMensagem = ema.EmailMensagem,
                        PageSize = pageSize,
                        TotalCount = ((from ema1 in db.EmailTemplates
                                       join tip1 in db.EmailTipos on ema1.EmailTipoID equals tip1.EmailTipoID
                                       where ema1.CondominioID == sessaoCorrente.empresaId
                                       && (_nome == null || ema1.Nome == _nome)
                                       orderby ema1.Nome
                                       select ema1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new EmailTemplateModel().getObject((EmailTemplateViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-email-template";
        }
    }
}