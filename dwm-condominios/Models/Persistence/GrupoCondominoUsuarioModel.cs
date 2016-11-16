using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using System.Web;
using App_Dominio.Security;

namespace DWM.Models.Persistence
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

    public class ListViewGrupoCondominoUsuarios : ListViewModelLocal<GrupoCondominoUsuarioViewModel>
    {
        #region Constructor
        public ListViewGrupoCondominoUsuarios() { }

        public ListViewGrupoCondominoUsuarios(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<GrupoCondominoUsuarioViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominoID = (int)param[0];

            return (from gru in db.GrupoCondominos 
                    join usu in db.GrupoCondominoUsuarios on gru.GrupoCondominoID equals usu.GrupoCondominoID into USU
                    from usu in USU.DefaultIfEmpty()
                    where gru.CondominioID == SessaoLocal.empresaId
                          && (usu.CondominoID == _CondominoID || usu == null)
                          && (SessaoLocal.CondominoID == 0 || gru.PrivativoAdmin == "N")
                    orderby gru.Descricao
                    select new GrupoCondominoUsuarioViewModel()
                    {
                        empresaId = SessaoLocal.empresaId,
                        GrupoCondominoID = gru.GrupoCondominoID,
                        Situacao = usu != null && usu.CondominoID > 0 ? "A" : "D",
                        DescricaoGrupo = gru.Descricao,
                        PageSize = pageSize,
                        TotalCount = ((from gru1 in db.GrupoCondominos
                                       join usu1 in db.GrupoCondominoUsuarios on gru1.GrupoCondominoID equals usu1.GrupoCondominoID into USU1
                                       from usu1 in USU1.DefaultIfEmpty()
                                       where gru1.CondominioID == SessaoLocal.empresaId
                                             && (usu1.CondominoID == _CondominoID || usu1 == null)
                                       && (SessaoLocal.CondominoID == 0 || gru1.PrivativoAdmin == "N")
                                       orderby gru1.Descricao
                                       select gru1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new GrupoCondominoUsuarioModel().getObject((GrupoCondominoUsuarioViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-grupo-condomino-usuario";
        }
    }

}