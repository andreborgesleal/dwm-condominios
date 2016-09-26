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
    public class GrupoCondominoModel : CrudModel<GrupoCondomino, GrupoCondominoViewModel, ApplicationContext>
    {
        #region Constructor
        public GrupoCondominoModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override GrupoCondomino MapToEntity(GrupoCondominoViewModel value)
        {
            GrupoCondomino entity = Find(value);

            if (entity == null)
                entity = new GrupoCondomino();

            entity.GrupoCondominoID = value.GrupoCondominoID;
            entity.CondominioID = value.CondominioID;
            entity.Descricao = value.Descricao;
            entity.Objetivo = value.Objetivo;
            entity.PrivativoAdmin = value.PrivativoAdmin;

            return entity;
        }

        public override GrupoCondominoViewModel MapToRepository(GrupoCondomino entity)
        {
            return new GrupoCondominoViewModel()
            {
                GrupoCondominoID = entity.GrupoCondominoID,
                CondominioID = entity.CondominioID,
                Descricao = entity.Descricao,
                Objetivo = entity.Objetivo,
                PrivativoAdmin = entity.PrivativoAdmin,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override GrupoCondomino Find(GrupoCondominoViewModel key)
        {
            return db.GrupoCondominos.Find(key.GrupoCondominoID);
        }

        public override Validate Validate(GrupoCondominoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.GrupoCondominoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Grupo ID").ToString();
                value.mensagem.MessageBase = "Código identificador do grupo deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            if (operation != Crud.EXCLUIR)
            {
                if (value.CondominioID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                    value.mensagem.MessageBase = "Condomínio deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.empresaId == 0)
                {
                    value.mensagem.Code = 35;
                    value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                    value.mensagem.MessageBase = "Sua sessão expirou. Faça um novo login no sistema";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Descricao == null || value.Descricao.Trim().Length <= 3)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Descrição").ToString();
                    value.mensagem.MessageBase = "Descrição do grupo deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.PrivativoAdmin == null || value.PrivativoAdmin.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Privativo Administração").ToString();
                    value.mensagem.MessageBase = "Atributo Privativo da Administração deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        public override GrupoCondominoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            GrupoCondominoViewModel value = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            value.CondominioID = security.getSessaoCorrente().empresaId;
            value.PrivativoAdmin = "N";
            return value;
        }
        #endregion
    }

    public class ListViewGrupoCondominos : ListViewModel<GrupoCondominoViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewGrupoCondominos() { }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<GrupoCondominoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? (string)param[0] : null;

            return (from gru in db.GrupoCondominos
                    where gru.CondominioID == sessaoCorrente.empresaId
                    && (_nome == null || gru.Descricao == _nome)
                    orderby gru.Descricao
                    select new GrupoCondominoViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        GrupoCondominoID = gru.GrupoCondominoID,
                        CondominioID = gru.CondominioID,
                        Descricao = gru.Descricao,
                        PrivativoAdmin = gru.PrivativoAdmin,
                        Objetivo = gru.Objetivo,
                        PageSize = pageSize,
                        TotalCount = ((from gru1 in db.GrupoCondominos
                                       where gru1.CondominioID == sessaoCorrente.empresaId
                                       && (_nome == null || gru1.Descricao == _nome)
                                       orderby gru1.Descricao
                                       select gru1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new GrupoCondominoModel().getObject((GrupoCondominoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-grupo-condomino";
        }
    }
}