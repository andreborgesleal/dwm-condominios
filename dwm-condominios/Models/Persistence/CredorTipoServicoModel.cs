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
    public class CredorTipoServicoModel : CrudModelLocal<CredorTipoServico, CredorTipoServicoViewModel>
    {
        #region Constructor
        public CredorTipoServicoModel() { }
        public CredorTipoServicoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel

        public override CredorTipoServico MapToEntity(CredorTipoServicoViewModel value)
        {
            CredorTipoServico entity = Find(value);

            if (entity == null)
                entity = new CredorTipoServico();

            entity.TipoServicoID = value.TipoServicoID;
            entity.CondominioID = value.CondominioID;
            entity.Descricao = value.Descricao;
            entity.Situacao = "A";

            return entity;
        }

        public override CredorTipoServicoViewModel MapToRepository(CredorTipoServico entity)
        {
            return new CredorTipoServicoViewModel()
            {
                TipoServicoID = entity.TipoServicoID,
                CondominioID = entity.CondominioID,
                Descricao = entity.Descricao,
                Situacao = entity.Situacao,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override CredorTipoServico Find(CredorTipoServicoViewModel key)
        {
            return db.CredorTipoServicos.Find(key.TipoServicoID);
        }

        public override Validate Validate(CredorTipoServicoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.TipoServicoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Espaco").ToString();
                value.mensagem.MessageBase = "Código identificador deve ser informado";
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

            else if (String.IsNullOrEmpty(value.Descricao))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Descrição").ToString();
                value.mensagem.MessageBase = "Descrição do espaço deve ser informada";
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

        public override CredorTipoServicoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            CredorTipoServicoViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }

        #endregion
    }

    public class ListViewCredorTipoServico : ListViewModelLocal<CredorTipoServicoViewModel>
    {
        public ListViewCredorTipoServico()
        {
        }

        public ListViewCredorTipoServico(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #region Métodos da classe ListViewRepository
        public override IEnumerable<CredorTipoServicoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            var q = (from value in db.CredorTipoServicos
                     select new CredorTipoServicoViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         CondominioID = value.CondominioID,
                         Descricao = value.Descricao,
                         Situacao = value.Situacao,
                         sessionId = sessaoCorrente.sessaoId,
                         TipoServicoID = value.TipoServicoID,
                     }).ToList();

            return q;
        }

        public override string action()
        {
            return "../Home/ListCredorTipoServico";
        }

        public override string DivId()
        {
            return "div-CredorTipoServico";
        }

        public override Repository getRepository(Object id)
        {
            return new CredorTipoServicoModel().getObject((CredorTipoServicoViewModel)id);
        }
        #endregion
    }
}