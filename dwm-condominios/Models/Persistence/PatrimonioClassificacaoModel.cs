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
    public class PatrimonioClassificacaoModel : CrudModelLocal<PatrimonioClassificacao, PatrimonioClassificacaoViewModel>
    {
        #region Constructor
        public PatrimonioClassificacaoModel()
        {
        }

        public PatrimonioClassificacaoModel(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override PatrimonioClassificacao MapToEntity(PatrimonioClassificacaoViewModel value)
        {
            PatrimonioClassificacao entity = Find(value);

            if (entity == null)
                entity = new PatrimonioClassificacao();

            entity.Descricao = value.Descricao;
            entity.CondominioID = value.empresaId;
            entity.PatrimonioClassificacaoID = value.PatrimonioClassificacaoID;

            return entity;
        }

        public override PatrimonioClassificacaoViewModel MapToRepository(PatrimonioClassificacao entity)
        {
            PatrimonioClassificacaoViewModel v = new PatrimonioClassificacaoViewModel()
            {
                PatrimonioClassificacaoID = entity.PatrimonioClassificacaoID,
                Descricao = entity.Descricao,
                CondominioID = entity.CondominioID,
                sessionId = SessaoLocal.sessaoId,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };

            return v;
        }

        public override PatrimonioClassificacao Find(PatrimonioClassificacaoViewModel key)
        {
            return db.PatrimonioClassificacaos.Find(key.PatrimonioClassificacaoID);
        }

        public override Validate Validate(PatrimonioClassificacaoViewModel value, Crud operation)
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
                if (value.PatrimonioClassificacaoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "PatrimonioClassificacao").ToString();
                    value.mensagem.MessageBase = "Requisito de limpeza deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            return value.mensagem;
        }

        public override PatrimonioClassificacaoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            PatrimonioClassificacaoViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }
        #endregion
    }

    public class ListViewPatrimonioClassificacao : ListViewModelLocal<PatrimonioClassificacaoViewModel>
    {
        #region Constructor
        public ListViewPatrimonioClassificacao() { }
        public ListViewPatrimonioClassificacao(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<PatrimonioClassificacaoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            var q = (from value in db.PatrimonioClassificacaos
                     orderby value.Descricao
                     select new PatrimonioClassificacaoViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         Descricao = value.Descricao,
                         PatrimonioClassificacaoID = value.PatrimonioClassificacaoID,
                         CondominioID = value.CondominioID,
                         sessionId = sessaoCorrente.sessaoId,
                     }).ToList();

            return q;
        }

        public override Repository getRepository(Object id)
        {
            return new PatrimonioClassificacaoModel().getObject((PatrimonioClassificacaoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-visitantes";
        }
    }
}