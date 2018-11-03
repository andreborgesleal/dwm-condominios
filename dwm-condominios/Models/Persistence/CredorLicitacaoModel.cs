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
    public class CredorLicitacaoModel : CrudModelLocal<CredorLicitacao, CredorLicitacaoViewModel>
    {
        #region Constructor
        public CredorLicitacaoModel() { }
        public CredorLicitacaoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        public CredorLicitacaoModel(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }

        #endregion

        #region Métodos da classe CrudModel

        public override CredorLicitacao MapToEntity(CredorLicitacaoViewModel value)
        {
            CredorLicitacao entity = Find(value);

            if (entity == null)
                entity = new CredorLicitacao();

            entity.LicitacaoID = value.LicitacaoID;
            entity.CondominioID = value.CondominioID;
            entity.TipoServicoID = value.TipoServicoID;
            entity.credorId = value.credorId;
            entity.Historico = value.Historico;
            entity.DataEdital = value.DataEdital;
            entity.DataEncerramento = value.DataEncerramento;
            entity.DataResultado = value.DataResultado;
            entity.Valor = value.Valor;
            entity.Justificativa = value.Justificativa;

            return entity;
        }

        public override CredorLicitacaoViewModel MapToRepository(CredorLicitacao entity)
        {
            return new CredorLicitacaoViewModel()
            {
                LicitacaoID = entity.LicitacaoID,
                CondominioID = entity.CondominioID,
                TipoServicoID = entity.TipoServicoID,
                credorId = entity.credorId,
                Historico = entity.Historico,
                DataEdital = entity.DataEdital,
                DataEncerramento = entity.DataEncerramento,
                DataResultado = entity.DataResultado,
                Valor = entity.Valor,
                Justificativa = entity.Justificativa,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override CredorLicitacao Find(CredorLicitacaoViewModel key)
        {
            return db.CredorLicitacaos.Find(key.LicitacaoID);
        }

        public override Validate Validate(CredorLicitacaoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.LicitacaoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Espaco").ToString();
                value.mensagem.MessageBase = "Código identificador do Espaço deve ser informado";
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
            else if (value.TipoServicoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Código identificador Tipo de Serviço deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            else if (DateTime.MinValue == value.DataEdital)
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

        public override CredorLicitacaoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            CredorLicitacaoViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }

        #endregion
    }

    public class ListViewCredorLicitacao : ListViewModelLocal<CredorLicitacaoViewModel>
    {
        public ListViewCredorLicitacao()
        {
        }

        public ListViewCredorLicitacao(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #region Métodos da classe ListViewRepository
        public override IEnumerable<CredorLicitacaoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            var q = (from value in db.CredorLicitacaos
                     join ts in db.CredorTipoServicos on value.TipoServicoID equals ts.TipoServicoID
                     select new CredorLicitacaoViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         CondominioID = value.CondominioID,
                         LicitacaoID = value.LicitacaoID,
                         TipoServicoID = value.TipoServicoID,
                         credorId = value.credorId,
                         Historico = value.Historico,
                         DataEdital = value.DataEdital,
                         DataEncerramento = value.DataEncerramento,
                         DataResultado = value.DataResultado,
                         Valor = value.Valor,
                         Justificativa = value.Justificativa,
                         sessionId = sessaoCorrente.sessaoId,
                         DescricaoTipoServico = ts.Descricao,
                     }).ToList();

            return q;
        }

        public override string action()
        {
            return "../Home/ListCredorLicitacao";
        }

        public override string DivId()
        {
            return "div-CredorLicitacao";
        }

        public override Repository getRepository(Object id)
        {
            return new CredorLicitacaoModel().getObject((CredorLicitacaoViewModel)id);
        }
        #endregion
    }
}