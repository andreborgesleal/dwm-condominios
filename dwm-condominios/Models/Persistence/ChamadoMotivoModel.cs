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
    public class ChamadoMotivoModel : CrudModelLocal<ChamadoMotivo, ChamadoMotivoViewModel>
    {
        #region Constructor
        public ChamadoMotivoModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override ChamadoMotivoViewModel BeforeInsert(ChamadoMotivoViewModel value)
        {
            if (value.ChamadoMotivoID == 0)
                value.ChamadoMotivoID = GetID();

            return value;
        }

        public override ChamadoMotivo MapToEntity(ChamadoMotivoViewModel value)
        {
            ChamadoMotivo entity = Find(value);

            if (entity == null)
                entity = new ChamadoMotivo();

            entity.ChamadoMotivoID = value.ChamadoMotivoID;
            entity.CondominioID = SessaoLocal.empresaId;
            entity.FilaAtendimentoID = value.FilaAtendimentoID;
            entity.Descricao = value.Descricao;
            entity.IndFixo = "N";

            return entity;
        }

        public override ChamadoMotivoViewModel MapToRepository(ChamadoMotivo entity)
        {
            return new ChamadoMotivoViewModel()
            {
                ChamadoMotivoID = entity.ChamadoMotivoID,
                CondominioID = entity.CondominioID,
                FilaAtendimentoID = entity.FilaAtendimentoID,
                Descricao = entity.Descricao,
                IndFixo = entity.IndFixo,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override ChamadoMotivo Find(ChamadoMotivoViewModel key)
        {
            return db.ChamadoMotivos.Find(key.ChamadoMotivoID, key.CondominioID);
        }

        public override Validate Validate(ChamadoMotivoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.ChamadoMotivoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Identificador do Motivo").ToString();
                value.mensagem.MessageBase = "Código identificador do motivo do chamado deve ser informado";
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

                if (value.FilaAtendimentoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Fila de atendimento").ToString();
                    value.mensagem.MessageBase = "Fila de atendimento deve ser informado";
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

        public override ChamadoMotivoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            ChamadoMotivoViewModel value = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            value.CondominioID = security.getSessaoCorrente().empresaId;
            value.IndFixo = "N";
            return value;
        }
        #endregion

        #region Métodos Customizados
        private int GetID()
        {
            return (from mot in db.ChamadoMotivos where mot.CondominioID == SessaoLocal.empresaId select mot.ChamadoMotivoID).Max() + 1;
        }
        #endregion
    }

    public class ListViewChamadoMotivo : ListViewModelLocal<ChamadoMotivoViewModel>
    {
        #region Constructor
        public ListViewChamadoMotivo() { }

        public ListViewChamadoMotivo(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ChamadoMotivoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? (string)param[0] : null;

            return (from mot in db.ChamadoMotivos join fil in db.FilaAtendimentos on mot.FilaAtendimentoID equals fil.FilaAtendimentoID
                    where mot.CondominioID == SessaoLocal.empresaId
                    && (_nome == null || mot.Descricao == _nome)
                    orderby mot.Descricao
                    select new ChamadoMotivoViewModel
                    {
                        empresaId = SessaoLocal.empresaId,
                        ChamadoMotivoID = mot.ChamadoMotivoID,
                        CondominioID = mot.CondominioID,
                        FilaAtendimentoID = mot.FilaAtendimentoID,
                        DescricaoFilaAtendimento = fil.Descricao,
                        Descricao = mot.Descricao,
                        IndFixo = mot.IndFixo,
                        PageSize = pageSize,
                        TotalCount = ((from mot1 in db.ChamadoMotivos
                                       join fil1 in db.FilaAtendimentos on mot1.FilaAtendimentoID equals fil1.FilaAtendimentoID
                                       where mot1.CondominioID == SessaoLocal.empresaId
                                       && (_nome == null || mot1.Descricao == _nome)
                                       orderby mot1.Descricao
                                       select mot1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new ChamadoMotivoModel().getObject((ChamadoMotivoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-chamado-motivo";
        }
    }
}