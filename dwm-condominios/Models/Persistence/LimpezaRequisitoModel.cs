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
    public class LimpezaRequisitoModel : CrudModelLocal<LimpezaRequisito, LimpezaRequisitoViewModel>
    {
        #region Constructor
        public LimpezaRequisitoModel()
        {
        }

        public LimpezaRequisitoModel(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override LimpezaRequisito MapToEntity(LimpezaRequisitoViewModel value)
        {
            LimpezaRequisito entity = Find(value);

            if (entity == null)
                entity = new LimpezaRequisito();

            entity.Descricao = value.Descricao;
            entity.EspacoID = value.EspacoID;
            entity.LimpezaRequisitoID = value.LimpezaRequisitoID;
            entity.CondominioID = value.CondominioID;
            entity.Situacao = value.Situacao;


            return entity;
        }

        public override LimpezaRequisitoViewModel MapToRepository(LimpezaRequisito entity)
        {
            LimpezaRequisitoViewModel v = new LimpezaRequisitoViewModel()
            {
                LimpezaRequisitoID = entity.LimpezaRequisitoID,
                Descricao = entity.Descricao,
                EspacoID = entity.EspacoID,
                CondominioID = entity.CondominioID,
                Situacao = entity.Situacao,
                sessionId = SessaoLocal.sessaoId,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };

            return v;
        }

        public override LimpezaRequisito Find(LimpezaRequisitoViewModel key)
        {
            return db.LimpezaRequisitos.Find(key.LimpezaRequisitoID);
        }

        public override Validate Validate(LimpezaRequisitoViewModel value, Crud operation)
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

            if (value.EspacoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "A área deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.CondominioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "O Condomínio deve ser informado";
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

            if (string.IsNullOrEmpty(value.Situacao))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(4, "Descrição").ToString();
                value.mensagem.MessageBase = "Situação deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.ALTERAR || operation == Crud.EXCLUIR)
            {
                if (value.LimpezaRequisitoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "LimpezaRequisito").ToString();
                    value.mensagem.MessageBase = "Requisito de limpeza deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            return value.mensagem;
        }

        public override LimpezaRequisitoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            LimpezaRequisitoViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }
        #endregion
    }

    public class ListViewLimpezaRequisito : ListViewModelLocal<LimpezaRequisitoViewModel>
    {
        #region Constructor
        public ListViewLimpezaRequisito() { }
        public ListViewLimpezaRequisito(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<LimpezaRequisitoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            var q = (from value in db.LimpezaRequisitos
                     orderby value.Descricao
                     select new LimpezaRequisitoViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         Descricao = value.Descricao,
                         LimpezaRequisitoID = value.LimpezaRequisitoID,
                         CondominioID = value.CondominioID,
                         Situacao = value.Situacao,
                         EspacoID = value.EspacoID,
                         EspacoComum = value.EspacoComum,
                         sessionId = sessaoCorrente.sessaoId,
                     }).ToList();

            return q;
        }

        public override Repository getRepository(Object id)
        {
            return new LimpezaRequisitoModel().getObject((LimpezaRequisitoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-visitantes";
        }
    }
}