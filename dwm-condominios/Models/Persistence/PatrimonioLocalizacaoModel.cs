using App_Dominio.Component;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DWM.Models.Persistence
{
    public class PatrimonioLocalizacaoModel : CrudModelLocal<PatrimonioLocalizacao, PatrimonioLocalizacaoViewModel>
    {
        #region Constructor
        public PatrimonioLocalizacaoModel()
        {
        }

        public PatrimonioLocalizacaoModel(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override PatrimonioLocalizacao MapToEntity(PatrimonioLocalizacaoViewModel value)
        {
            PatrimonioLocalizacao entity = Find(value);

            if (entity == null)
                entity = new PatrimonioLocalizacao();

            entity.Descricao = value.Descricao;
            entity.CondominioID = value.CondominioID;
            entity.PatrimonioLocalizacaoID = value.PatrimonioLocalizacaoID;

            return entity;
        }

        public override PatrimonioLocalizacaoViewModel MapToRepository(PatrimonioLocalizacao entity)
        {
            PatrimonioLocalizacaoViewModel v = new PatrimonioLocalizacaoViewModel()
            {
                PatrimonioLocalizacaoID = entity.PatrimonioLocalizacaoID,
                Descricao = entity.Descricao,
                CondominioID = entity.CondominioID,
                sessionId = SessaoLocal.sessaoId,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };

            return v;
        }

        public override PatrimonioLocalizacao Find(PatrimonioLocalizacaoViewModel key)
        {
            return db.PatrimonioLocalizacaos.Find(key.PatrimonioLocalizacaoID);
        }

        public override Validate Validate(PatrimonioLocalizacaoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

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
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "O condomínio deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (string.IsNullOrEmpty(value.Descricao))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(4, "Descrição").ToString();
                value.mensagem.MessageBase = "Descrição deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.ALTERAR || operation == Crud.EXCLUIR)
            {
                if (value.PatrimonioLocalizacaoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "PatrimonioLocalizacao").ToString();
                    value.mensagem.MessageBase = "Requisito de limpeza deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            return value.mensagem;
        }

        public override PatrimonioLocalizacaoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            PatrimonioLocalizacaoViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }
        #endregion
    }

    public class ListViewPatrimonioLocalizacao : ListViewModelLocal<PatrimonioLocalizacaoViewModel>
    {
        #region Constructor
        public ListViewPatrimonioLocalizacao() { }
        public ListViewPatrimonioLocalizacao(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<PatrimonioLocalizacaoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            var q = (from value in db.PatrimonioLocalizacaos
                     orderby value.Descricao
                     select new PatrimonioLocalizacaoViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         Descricao = value.Descricao,
                         PatrimonioLocalizacaoID = value.PatrimonioLocalizacaoID,
                         CondominioID = value.CondominioID,
                         sessionId = sessaoCorrente.sessaoId,
                     }).ToList();

            return q;
        }

        public override Repository getRepository(Object id)
        {
            return new PatrimonioLocalizacaoModel().getObject((PatrimonioLocalizacaoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-visitantes";
        }
    }
}