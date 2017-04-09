using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using App_Dominio.Enumeracoes;
using System.Web;
using App_Dominio.Security;

namespace DWM.Models.Persistence
{
    public class EdificacaoModel : CrudModelLocal<Edificacao, EdificacaoViewModel>
    {
        #region Constructor
        public EdificacaoModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override Edificacao MapToEntity(EdificacaoViewModel value)
        {
            Edificacao entity = Find(value);

            if (entity == null)
                entity = new Edificacao();

            entity.EdificacaoID = value.EdificacaoID;
            entity.CondominioID = value.CondominioID;
            entity.Descricao = value.Descricao;
            entity.TipoEdificacaoID = value.TipoEdificacao;
            entity.Codigo = value.Codigo;

            return entity;
        }

        public override EdificacaoViewModel MapToRepository(Edificacao entity)
        {
            return new EdificacaoViewModel()
            {
                EdificacaoID = entity.EdificacaoID,
                CondominioID = entity.CondominioID,
                Descricao = entity.Descricao,
                Codigo = entity.Codigo,
                TipoEdificacao = entity.TipoEdificacaoID,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Edificacao Find(EdificacaoViewModel key)
        {
            return db.Edificacaos.Find(key.EdificacaoID);
        }

        public override Validate Validate(EdificacaoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.EdificacaoID == 0)
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
            }
            return value.mensagem;
        }

        public override EdificacaoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            EdificacaoViewModel value = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            value.CondominioID = security.getSessaoCorrente().empresaId;
            return value;
        }
        #endregion
    }

    public class ListViewEdificacoes : ListViewModelLocal<EdificacaoViewModel>
    {
        #region Constructor
        public ListViewEdificacoes() { }

        public ListViewEdificacoes(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EdificacaoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? (string)param[0] : null;

            return (from gru in db.Edificacaos
                    where gru.CondominioID == sessaoCorrente.empresaId
                    && (_nome == null || gru.Descricao == _nome)
                    orderby gru.Descricao
                    select new EdificacaoViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        EdificacaoID = gru.EdificacaoID,
                        CondominioID = gru.CondominioID,
                        Descricao = gru.Descricao,
                        Codigo = gru.Codigo,
                        TipoEdificacao = gru.TipoEdificacaoID,
                        PageSize = pageSize,
                        TotalCount =  ((from gru1 in db.Edificacaos
                                       where gru1.CondominioID == sessaoCorrente.empresaId
                                       && (_nome == null || gru1.Descricao == _nome)
                                       orderby gru1.Descricao
                                       select gru1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new EdificacaoModel().getObject((EdificacaoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-grupo-condomino";
        }
    }
}