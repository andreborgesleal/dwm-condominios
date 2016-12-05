using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using App_Dominio.Enumeracoes;
using System.Web;
using App_Dominio.Security;

namespace DWM.Models.Persistence
{
    public class ChamadoStatusModel : CrudModelLocal<ChamadoStatus, ChamadoStatusViewModel>
    {
        #region Constructor
        public ChamadoStatusModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override ChamadoStatusViewModel BeforeInsert(ChamadoStatusViewModel value)
        {
            if (value.ChamadoStatusID == 0)
                value.ChamadoStatusID = GetID();

            return value;
        }

        public override ChamadoStatus MapToEntity(ChamadoStatusViewModel value)
        {
            ChamadoStatus entity = Find(value);

            if (entity == null)
                entity = new ChamadoStatus();

            entity.ChamadoStatusID = value.ChamadoStatusID;
            entity.CondominioID = SessaoLocal.empresaId;
            entity.Descricao = value.Descricao;
            entity.IndFixo = "N";

            return entity;
        }

        public override ChamadoStatusViewModel MapToRepository(ChamadoStatus entity)
        {
            return new ChamadoStatusViewModel()
            {
                ChamadoStatusID = entity.ChamadoStatusID,
                CondominioID = entity.CondominioID,
                Descricao = entity.Descricao,
                IndFixo = entity.IndFixo,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override ChamadoStatus Find(ChamadoStatusViewModel key)
        {
            return db.ChamadoStatuss.Find(key.ChamadoStatusID, key.CondominioID);
        }

        public override Validate Validate(ChamadoStatusViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.ChamadoStatusID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Identificador do Motivo").ToString();
                value.mensagem.MessageBase = "Código identificador da situação do chamado deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            if (value.CondominioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Condomínio deve ser informado";
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

                if (value.Descricao == null || value.Descricao.Trim().Length <= 3)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Descrição").ToString();
                    value.mensagem.MessageBase = "Descrição do motivo deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        public override ChamadoStatusViewModel CreateRepository(HttpRequestBase Request = null)
        {
            ChamadoStatusViewModel value = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            value.CondominioID = security.getSessaoCorrente().empresaId;
            value.IndFixo = "N";
            return value;
        }
        #endregion

        #region Métodos Customizados
        private int GetID()
        {
            return (from sta in db.ChamadoStatuss where sta.CondominioID == SessaoLocal.empresaId select sta.ChamadoStatusID).Max() + 1;
        }
        #endregion
    }

    public class ListViewChamadoStatus : ListViewModelLocal<ChamadoStatusViewModel>
    {
        #region Constructor
        public ListViewChamadoStatus() { }

        public ListViewChamadoStatus(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ChamadoStatusViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? (string)param[0] : null;

            return (from sta in db.ChamadoStatuss
                    where sta.CondominioID == SessaoLocal.empresaId
                    && (_nome == null || sta.Descricao == _nome)
                    orderby sta.Descricao
                    select new ChamadoStatusViewModel
                    {
                        empresaId = SessaoLocal.empresaId,
                        ChamadoStatusID = sta.ChamadoStatusID,
                        CondominioID = sta.CondominioID,
                        Descricao = sta.Descricao,
                        IndFixo = sta.IndFixo,
                        PageSize = pageSize,
                        TotalCount = ((from sta1 in db.ChamadoStatuss
                                       where sta1.CondominioID == SessaoLocal.empresaId
                                       && (_nome == null || sta1.Descricao == _nome)
                                       orderby sta1.Descricao
                                       select sta1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new ChamadoStatusModel().getObject((ChamadoStatusViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-chamado-status";
        }
    }
}