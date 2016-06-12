using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using DWM.Models.Entidades;
using DWM.Models.Repositories;

namespace DWM.Models.Persistence
{
    public class CondominoUnidadeModel : CrudModel<CondominoUnidade, CondominoUnidadeViewModel, ApplicationContext>
    {
        #region Constructor
        public CondominoUnidadeModel() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public CondominoUnidadeModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override CondominoUnidade MapToEntity(CondominoUnidadeViewModel value)
        {
            CondominoUnidade condominoUnidade = Find(value);

            if (condominoUnidade == null)
                condominoUnidade = new CondominoUnidade();

            condominoUnidade.CondominioID = value.CondominioID;
            condominoUnidade.EdificacaoID = value.EdificacaoID;
            condominoUnidade.UnidadeID = value.UnidadeID;
            condominoUnidade.CondominoID = value.CondominoID;
            condominoUnidade.DataInicio = value.DataInicio;
            condominoUnidade.DataFim = value.DataFim;

            return condominoUnidade;
        }

        public override CondominoUnidadeViewModel MapToRepository(CondominoUnidade entity)
        {
            CondominoUnidadeViewModel condominoUnidadeViewModel = new CondominoUnidadeViewModel()
            {
                CondominioID = entity.CondominioID,
                EdificacaoID = entity.EdificacaoID,
                UnidadeID = entity.UnidadeID,
                CondominoID = entity.CondominoID,
                DataInicio = entity.DataInicio,
                DataFim = entity.DataFim,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };

            return condominoUnidadeViewModel;
        }

        public override CondominoUnidade Find(CondominoUnidadeViewModel key)
        {
            return db.CondominoUnidades.Find(key.CondominioID, key.EdificacaoID, key.UnidadeID, key.CondominoID);
        }

        public override Validate Validate(CondominoUnidadeViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };
            if (value.CondominioID <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID Condominio").ToString();
                value.mensagem.MessageBase = "Identificador do condomínio deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.EdificacaoID <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID Edificação").ToString();
                value.mensagem.MessageBase = "Torre/Sala/Casa deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.UnidadeID <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID Unidade").ToString();
                value.mensagem.MessageBase = "Identificador da unidade deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.EXCLUIR || operation == Crud.ALTERAR)
            {
                if (value.CondominoID <= 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "ID Condomino").ToString();
                    value.mensagem.MessageBase = "Identificador do condômino deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            if (value.DataInicio == null || value.DataInicio > Funcoes.Brasilia().Date || value.DataInicio < new DateTime(2011, 1, 1))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Data de Início").ToString();
                value.mensagem.MessageBase = "Data de entrada do condômino no condomínio deve ser informada corretamente";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.DataFim != null && (value.DataFim > Funcoes.Brasilia().Date || value.DataFim < new DateTime(2011, 1, 1)))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Data de Início").ToString();
                value.mensagem.MessageBase = "Data de saída do condômino do condomínio deve ser informada corretamente";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            return value.mensagem;
        }
        #endregion
    }
}