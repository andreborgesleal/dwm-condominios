using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App_Dominio.Contratos;
using App_Dominio.Enumeracoes;
using App_Dominio.Entidades;

namespace dwm_condominios.Models.Persistence
{
    public class VisitanteUnidadeModel : CrudModelLocal<VisitanteUnidade, VisitanteUnidadeViewModel>
    {
        #region Constructor
        public VisitanteUnidadeModel()
        {
        }

        public VisitanteUnidadeModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override VisitanteUnidade Find(VisitanteUnidadeViewModel key)
        {
            return db.VisitanteUnidades.Find(key.VisitanteID, key.CondominioID, key.EdificacaoID, key.UnidadeID, key.CondominoID);
        }

        public override VisitanteUnidade MapToEntity(VisitanteUnidadeViewModel value)
        {
            VisitanteUnidade entity = Find(value);

            if (entity == null)
                entity = new VisitanteUnidade();

            entity.VisitanteID = value.VisitanteID;
            entity.CondominioID = value.CondominioID;
            entity.CondominoID = value.CondominoID;
            entity.CredenciadoID = value.CredenciadoID;
            entity.EdificacaoID = value.EdificacaoID;
            entity.UnidadeID = value.UnidadeID;

            return entity;
        }

        public override VisitanteUnidadeViewModel MapToRepository(VisitanteUnidade entity)
        {
            return new VisitanteUnidadeViewModel()
            {
                CondominioID = entity.CondominioID,
                EdificacaoID = entity.EdificacaoID,
                CondominoID = entity.CondominoID,
                CredenciadoID = entity.CredenciadoID,
                UnidadeID = entity.UnidadeID,
                VisitanteID = entity.VisitanteID,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Validate Validate(VisitanteUnidadeViewModel value, Crud operation)
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

            return value.mensagem;
        }
        #endregion

    }
}