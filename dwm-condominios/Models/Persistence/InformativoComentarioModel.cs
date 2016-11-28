using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using System.Web;

namespace DWM.Models.Persistence
{
    public class InformativoComentarioModel : CrudModelLocal<InformativoComentario, InformativoComentarioViewModel>
    {
        #region Constructor
        public InformativoComentarioModel() { }
        public InformativoComentarioModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override InformativoComentario MapToEntity(InformativoComentarioViewModel value)
        {
            InformativoComentario entity = Find(value);

            if (entity == null)
            {
                entity = new InformativoComentario();
                entity.DataComentario = Funcoes.Brasilia();
            }
            else
                entity.DataComentario = value.DataComentario;

            entity.InformativoID = value.InformativoID;
            entity.CondominoID = value.CondominoID;
            entity.Descricao = value.Descricao;
            entity.DataDesativacao = value.DataDesativacao;
            entity.Motivo = value.Motivo;

            return entity;
        }

        public override InformativoComentarioViewModel MapToRepository(InformativoComentario entity)
        {
            int _EdificacaoID = db.CondominoUnidades.Where(info => info.CondominioID == SessaoLocal.empresaId && info.CondominoID == entity.CondominoID).FirstOrDefault().EdificacaoID;
            string _DescricaoEdificacao = db.Edificacaos.Find(_EdificacaoID).Descricao;
            return new InformativoComentarioViewModel()
            {
                InformativoID = entity.InformativoID,
                DataComentario = entity.DataComentario,
                CondominoID = entity.CondominoID,
                Nome = db.Condominos.Find(entity.CondominoID).Nome,
                EdificacaoID = _EdificacaoID,
                DescricaoEdificacao = _DescricaoEdificacao,
                UnidadeID = db.CondominoUnidades.Where(info => info.CondominioID == SessaoLocal.empresaId && info.CondominoID == entity.CondominoID).FirstOrDefault().UnidadeID,
                Descricao = entity.Descricao,
                DataDesativacao = entity.DataDesativacao,
                Motivo = entity.Motivo,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override InformativoComentario Find(InformativoComentarioViewModel key)
        {
            return db.InformativoComentarios.Find(key.InformativoID, key.DataComentario);
        }

        public override Validate Validate(InformativoComentarioViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.InformativoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Identificador do informativo").ToString();
                value.mensagem.MessageBase = "Identificador do informativo deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.EXCLUIR && value.DataComentario == null)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Data do comentário").ToString();
                value.mensagem.MessageBase = "Data do comentário deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation != Crud.EXCLUIR)
            {
                if (value.CondominoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Condômino").ToString();
                    value.mensagem.MessageBase = "Condômino deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Descricao.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Comentário").ToString();
                    value.mensagem.MessageBase = "Comentário deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            return value.mensagem;
        }

        public override InformativoComentarioViewModel CreateRepository(HttpRequestBase Request = null)
        {
            InformativoComentarioViewModel value = base.CreateRepository(Request);
            if (Request != null && Request["InformativoID"] != null)
                value.InformativoID = int.Parse(Request["InformativoID"]);

            value.CondominoID = SessaoLocal.CondominoID;

            return value;
        }
        #endregion
    }
}