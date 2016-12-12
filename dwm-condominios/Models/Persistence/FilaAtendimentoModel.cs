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
    public class FilaAtendimentoModel : CrudModelLocal<FilaAtendimento, FilaAtendimentoViewModel>
    {
        #region Constructor
        public FilaAtendimentoModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override FilaAtendimento MapToEntity(FilaAtendimentoViewModel value)
        {
            FilaAtendimento entity = Find(value);

            if (entity == null)
                entity = new FilaAtendimento();

            entity.FilaAtendimentoID = value.FilaAtendimentoID;
            entity.CondominioID = value.CondominioID;
            entity.Descricao = value.Descricao;
            entity.VisibilidadeCondomino = value.VisibilidadeCondomino;
            entity.IsFornecedor = value.IsFornecedor;

            return entity;
        }

        public override FilaAtendimentoViewModel MapToRepository(FilaAtendimento entity)
        {
            return new FilaAtendimentoViewModel()
            {
                FilaAtendimentoID = entity.FilaAtendimentoID,
                CondominioID = entity.CondominioID,
                Descricao = entity.Descricao,
                VisibilidadeCondomino = entity.VisibilidadeCondomino,
                IsFornecedor = entity.IsFornecedor,
                FilaCondominoID = DWMSessaoLocal.FilaCondominoID(sessaoCorrente, this.db),
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override FilaAtendimento Find(FilaAtendimentoViewModel key)
        {
            return db.FilaAtendimentos.Find(key.FilaAtendimentoID);
        }

        public override Validate Validate(FilaAtendimentoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.FilaAtendimentoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Fila ID").ToString();
                value.mensagem.MessageBase = "Código identificador da fila de atendimento deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            if (operation == Crud.INCLUIR && value.Descricao.ToLower() == "condôminos")
            {
                value.mensagem.Code = 37;
                value.mensagem.Message = MensagemPadrao.Message(59).ToString();
                value.mensagem.MessageBase = "Registro não pode ser incluído. Esta descrição é um nome reservado internamente pelo sistema";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

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


                if (value.VisibilidadeCondomino == null || value.VisibilidadeCondomino.Trim().Length <= 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Visibilidade Condômino").ToString();
                    value.mensagem.MessageBase = "Campo Visibilidade Condômino deve ser informado.";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.IsFornecedor == null || value.IsFornecedor.Trim().Length <= 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "É Fornecedor").ToString();
                    value.mensagem.MessageBase = "Campo É Fornecedor deve ser informado.";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

            }
            return value.mensagem;
        }

        public override FilaAtendimentoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            FilaAtendimentoViewModel value = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            value.CondominioID = security.getSessaoCorrente().empresaId;
            return value;
        }
        #endregion
    }

    public class ListViewFilaAtendimento : ListViewModelLocal<FilaAtendimentoViewModel>
    {
        #region Constructor
        public ListViewFilaAtendimento() { }

        public ListViewFilaAtendimento(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<FilaAtendimentoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _descricao = param != null && param.Count() > 0 && param[0] != null ? (string)param[0] : null;
            int _FilaCondominoID = DWMSessaoLocal.FilaCondominoID(sessaoCorrente, this.db);

            return (from fil in db.FilaAtendimentos
                    where fil.CondominioID == SessaoLocal.empresaId
                    && (_descricao == null || fil.Descricao == _descricao)
                    orderby fil.Descricao
                    select new FilaAtendimentoViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        FilaAtendimentoID = fil.FilaAtendimentoID,
                        CondominioID = fil.CondominioID,
                        Descricao = fil.Descricao,
                        VisibilidadeCondomino = fil.VisibilidadeCondomino,
                        FilaCondominoID = _FilaCondominoID,
                        IsFornecedor = fil.IsFornecedor,
                        PageSize = pageSize,
                        TotalCount = ((from fil1 in db.FilaAtendimentos
                                       where fil1.CondominioID == SessaoLocal.empresaId
                                       && (_descricao == null || fil1.Descricao == _descricao)
                                       orderby fil1.Descricao
                                       select fil1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new FilaAtendimentoModel().getObject((FilaAtendimentoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-fila-atendimento";
        }
    }
}