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

namespace dwm_condominios.Models.Persistence
{
    public class PatrimonioModel : CrudModelLocal<Patrimonio, PatrimonioViewModel>
    {
        #region Constructor
        public PatrimonioModel()
        {
        }

        public PatrimonioModel(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override Patrimonio MapToEntity(PatrimonioViewModel value)
        {
            Patrimonio entity = Find(value);

            if (entity == null)
                entity = new Patrimonio();

            entity.CondominioID = value.CondominioID;
            entity.credorId = value.credorId;
            entity.DataBaixa = value.DataBaixa;
            entity.DataTombamento = value.DataTombamento;
            entity.Descricao = value.Descricao;
            entity.Observacao = value.Observacao;
            entity.PatrimonioClassificacaoID = value.PatrimonioClassificacaoID;
            entity.PatrimonioID = value.PatrimonioID;
            entity.PatrimonioLocalizacaoID = value.PatrimonioLocalizacaoID;
            entity.TombamentoID = value.TombamentoID;
            entity.ValorAtual = value.ValorAtual;
            entity.ValorCompra = value.ValorCompra;

            return entity;
        }

        public override PatrimonioViewModel MapToRepository(Patrimonio entity)
        {
            PatrimonioViewModel v = new PatrimonioViewModel()
            {
                CondominioID = entity.CondominioID,
                credorId = entity.credorId,
                DataBaixa = entity.DataBaixa,
                DataTombamento = entity.DataTombamento,
                Descricao = entity.Descricao,
                Observacao = entity.Observacao,
                PatrimonioClassificacaoID = entity.PatrimonioClassificacaoID,
                PatrimonioID = entity.PatrimonioID,
                PatrimonioLocalizacaoID = entity.PatrimonioLocalizacaoID,
                TombamentoID = entity.TombamentoID,
                ValorAtual = entity.ValorAtual,
                ValorCompra = entity.ValorCompra,
                sessionId = SessaoLocal.sessaoId,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };

            return v;
        }

        public override Patrimonio Find(PatrimonioViewModel key)
        {
            return db.Patrimonios.Find(key.PatrimonioID);
        }

        public override Validate Validate(PatrimonioViewModel value, Crud operation)
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
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Condomínio deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.ALTERAR)
            {
                if (value.PatrimonioID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Patrimonio").ToString();
                    value.mensagem.MessageBase = "Patrimonio deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            if (operation == Crud.EXCLUIR)
            {
                if (value.PatrimonioID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Patrimonio").ToString();
                    value.mensagem.MessageBase = "Patrimonio deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            return value.mensagem;
        }

        public override PatrimonioViewModel CreateRepository(HttpRequestBase Request = null)
        {
            PatrimonioViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }
        #endregion
    }

    public class ListViewPatrimonio : ListViewModelLocal<PatrimonioViewModel>
    {
        #region Constructor
        public ListViewPatrimonio() { }
        public ListViewPatrimonio(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<PatrimonioViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            var q = (from value in db.Patrimonios
                     select new PatrimonioViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         CondominioID = value.CondominioID,
                         credorId = value.credorId,
                         DataBaixa = value.DataBaixa,
                         DataTombamento = value.DataTombamento,
                         Descricao = value.Descricao,
                         Observacao = value.Observacao,
                         PatrimonioClassificacaoID = value.PatrimonioClassificacaoID,
                         PatrimonioID = value.PatrimonioID,
                         PatrimonioLocalizacaoID = value.PatrimonioLocalizacaoID,
                         TombamentoID = value.TombamentoID,
                         ValorAtual = value.ValorAtual,
                         ValorCompra = value.ValorCompra,
                         sessionId = sessaoCorrente.sessaoId,
                     }).ToList();

            return q;
        }

        public override Repository getRepository(Object id)
        {
            return new PatrimonioModel().getObject((PatrimonioViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-visitantes";
        }
    }
}