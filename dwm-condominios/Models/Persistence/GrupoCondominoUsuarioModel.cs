using App_Dominio.Entidades;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using App_Dominio.Contratos;
using App_Dominio.Enumeracoes;

namespace dwm_condominios.Models.Persistence
{
    public class GrupoCondominoUsuarioModel : CrudModelLocal<GrupoCondominoUsuario, GrupoCondominoUsuarioViewModel>
    {
        #region Constructor
        public GrupoCondominoUsuarioModel() { }
        public GrupoCondominoUsuarioModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel

        public override GrupoCondominoUsuario MapToEntity(GrupoCondominoUsuarioViewModel value)
        {
            GrupoCondominoUsuario entity = Find(value);

            if (entity == null)
                entity = new GrupoCondominoUsuario();

            entity.GrupoCondominoID = value.GrupoCondominoID;
            entity.CondominoID = value.CondominoID;

            return entity;
        }

        public override GrupoCondominoUsuarioViewModel MapToRepository(GrupoCondominoUsuario entity)
        {
            return new GrupoCondominoUsuarioViewModel()
            {
                GrupoCondominoID = entity.GrupoCondominoID,
                CondominoID = entity.CondominoID,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override GrupoCondominoUsuario Find(GrupoCondominoUsuarioViewModel key)
        {
            return db.GrupoCondominoUsuarios.Find(key.GrupoCondominoID, key.CondominoID);
        }

        public override Validate Validate(GrupoCondominoUsuarioViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.GrupoCondominoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Credenciado").ToString();
                value.mensagem.MessageBase = "Código identificador do Grupo do Condômino deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (value.CondominoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Código identificador do condômino deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            return value.mensagem;
        }

        #endregion
    }
}