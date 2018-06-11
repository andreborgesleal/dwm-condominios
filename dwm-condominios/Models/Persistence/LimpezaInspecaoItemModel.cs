using App_Dominio.Component;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dwm_condominios.Models.Persistence
{
    public class LimpezaInspecaoItemModel : CrudModelLocal<LimpezaInspecaoItem, LimpezaInspecaoItemViewModel>
    {
        #region Constructor
        public LimpezaInspecaoItemModel()
        {
        }

        public LimpezaInspecaoItemModel(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override LimpezaInspecaoItem MapToEntity(LimpezaInspecaoItemViewModel value)
        {
            LimpezaInspecaoItem entity = Find(value);

            if (entity == null)
                entity = new LimpezaInspecaoItem();

            entity.LimpezaInspecaoID = value.LimpezaInspecaoID;
            entity.LimpezaRequisitoID = value.LimpezaRequisitoID;
            entity.Justificativa = value.Justificativa;
            entity.Nota = value.Nota;

            return entity;
        }

        public override LimpezaInspecaoItemViewModel MapToRepository(LimpezaInspecaoItem entity)
        {
            LimpezaInspecaoItemViewModel v = new LimpezaInspecaoItemViewModel()
            {
                // Dados Externos
                DataInspecao = (from inspecao in db.LimpezaInspecaos where inspecao.LimpezaInspecaoID == entity.LimpezaInspecaoID select inspecao).FirstOrDefault().DataInspecao,
                EspacoDescricao = (from espaco in db.EspacoComums
                                   join inspecao in db.LimpezaInspecaos on espaco.EspacoID equals inspecao.EspacoID
                                   select espaco).FirstOrDefault().Descricao,
                FornecedorDescricao = (from credor in db.Credores
                                       join inspecao in db.LimpezaInspecaos on credor.credorId equals inspecao.credorId
                                       select credor).FirstOrDefault().nome,
                ItemDescricao = (from requisito in db.LimpezaRequisitos where requisito.LimpezaRequisitoID == entity.LimpezaRequisitoID select requisito).FirstOrDefault().Descricao,

                // Dados da table
                Justificativa = entity.Justificativa,
                LimpezaInspecaoID = entity.LimpezaInspecaoID,
                LimpezaRequisitoID = entity.LimpezaRequisitoID,
                Nota = entity.Nota,
                sessionId = SessaoLocal.sessaoId,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };

            return v;
        }

        public override LimpezaInspecaoItem Find(LimpezaInspecaoItemViewModel key)
        {
            return db.LimpezaInspecaoItem.Find(key.LimpezaInspecaoID, key.LimpezaRequisitoID);
        }

        public override Validate Validate(LimpezaInspecaoItemViewModel value, Crud operation)
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

            if (value.LimpezaInspecaoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Inspeção").ToString();
                value.mensagem.MessageBase = "Inspeção deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.LimpezaRequisitoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Requisito").ToString();
                value.mensagem.MessageBase = "Requisito deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            return value.mensagem;
        }

        public override LimpezaInspecaoItemViewModel CreateRepository(HttpRequestBase Request = null)
        {
            LimpezaInspecaoItemViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.empresaId = security.getSessaoCorrente().empresaId;
            return u;
        }
        #endregion
    }

    public class ListViewLimpezaInspecaoItem : ListViewModelLocal<LimpezaInspecaoItemViewModel>
    {
        #region Constructor
        public ListViewLimpezaInspecaoItem() { }
        public ListViewLimpezaInspecaoItem(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<LimpezaInspecaoItemViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            var q = (from value in db.LimpezaInspecaoItem
                     select new LimpezaInspecaoItemViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         
                         sessionId = sessaoCorrente.sessaoId,
                     }).ToList();

            return q;
        }

        public override Repository getRepository(Object id)
        {
            return new LimpezaInspecaoItemModel().getObject((LimpezaInspecaoItemViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-visitantes";
        }
    }
}