using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using App_Dominio.Enumeracoes;

namespace DWM.Models.Persistence
{
    public class GrupoCredorModel : CrudContext<GrupoCredor, GrupoCredorViewModel, ApplicationContext>
    {
        #region Métodos da classe CrudContext
        public override GrupoCredor MapToEntity(GrupoCredorViewModel value)
        {
            return new GrupoCredor()
            {
                grupoCredorId = value.grupoCredorId == 0 ? getId() : value.grupoCredorId,
                CondominioID = value.empresaId,
                nome = value.nome
            };
        }

        public override GrupoCredorViewModel MapToRepository(GrupoCredor entity)
        {
            return new GrupoCredorViewModel()
            {
                grupoCredorId = entity.grupoCredorId,
                nome = entity.nome,
                CondominioID = entity.CondominioID,
                empresaId = entity.CondominioID,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override GrupoCredor Find(GrupoCredorViewModel key)
        {
            return db.GrupoCredores.Find(key.grupoCredorId);
        }

        public override Validate Validate(GrupoCredorViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString(), MessageType = MsgType.SUCCESS };

            if (value.nome.Trim().Length == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Descrição").ToString();
                value.mensagem.MessageBase = "Campo Nome do Grupo deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (operation == Crud.INCLUIR)
            {
                // Verifica se o grupo já foi cadastrado com o mesmo nome
                if (db.GrupoCredores.Where(info => info.nome == value.nome && info.CondominioID == value.CondominioID).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Campo Nome do Grupo já existe";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            else if (operation == Crud.ALTERAR)
                // Verifica se o grupo já foi cadastrado com o mesmo nome
                if (db.GrupoCredores.Where(info => info.nome == value.nome && info.CondominioID == value.CondominioID && info.grupoCredorId != value.grupoCredorId).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Campo Nome do Grupo já existe";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            return value.mensagem;
        }

        private int getId()
        {
            int value = 1;
            if (db.GrupoCredores.Count() > 0)
                value = db.GrupoCredores.Max(info => info.grupoCredorId) + 1;

            return value;
        }

        #endregion
    }

    public class ListViewGrupoCredor : ListViewRepository<GrupoCredorViewModel, ApplicationContext>
    {
        #region Métodos da classe ListViewRepository
        public override IEnumerable<GrupoCredorViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? param[0].ToString() : null;
            return (from gru in db.GrupoCredores
                    where (gru.CondominioID == sessaoCorrente.empresaId && (_nome == null || String.IsNullOrEmpty(_nome) || gru.nome.Contains(_nome.Trim())))
                    orderby gru.nome
                    select new GrupoCredorViewModel
                    {
                        grupoCredorId = gru.grupoCredorId,
                        CondominioID = gru.CondominioID,
                        empresaId = gru.CondominioID,
                        nome = gru.nome,
                        PageSize = pageSize,
                        TotalCount = (from gru1 in db.GrupoCredores
                                      where (gru1.CondominioID == sessaoCorrente.empresaId && (_nome == null || String.IsNullOrEmpty(_nome) || gru1.nome.Contains(_nome.Trim())))
                                      select gru1).Count()
                    }).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new GrupoCredorModel().getObject((GrupoCredorViewModel)id);
        }
        #endregion
    }

    public class LookupGrupoCredorModel : ListViewGrupoCredor
    {
        public override string action()
        {
            return "../GrupoFornecedores/ListGrupoCredorModal";
        }

        public override string DivId()
        {
            return "div-gcre";
        }
    }

    public class LookupGrupoCredorFiltroModel : ListViewGrupoCredor
    {
        public override string action()
        {
            return "../GrupoFornecedores/_ListGrupoCredorModal";
        }

        public override string DivId()
        {
            return "div-gcre";
        }
    }

}