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
    public class FilaAtendimentoUsuarioModel : CrudModelLocal<FilaAtendimentoUsuario, FilaAtendimentoUsuarioViewModel>
    {
        #region Constructor
        public FilaAtendimentoUsuarioModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override FilaAtendimentoUsuario MapToEntity(FilaAtendimentoUsuarioViewModel value)
        {
            FilaAtendimentoUsuario entity = Find(value);

            if (entity == null)
                entity = new FilaAtendimentoUsuario();

            entity.FilaAtendimentoID = value.FilaAtendimentoID;
            entity.UsuarioID = value.UsuarioID;
            entity.Situacao = value.Situacao;
            entity.Nome = value.Nome;
            entity.Login = seguranca_db.Usuarios.Find(value.UsuarioID).login;

            return entity;
        }

        public override FilaAtendimentoUsuarioViewModel MapToRepository(FilaAtendimentoUsuario entity)
        {
            return new FilaAtendimentoUsuarioViewModel()
            {
                FilaAtendimentoID = entity.FilaAtendimentoID,
                UsuarioID = entity.UsuarioID,
                Nome = entity.Nome,
                Login = entity.Login,
                Situacao = entity.Situacao,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override FilaAtendimentoUsuario Find(FilaAtendimentoUsuarioViewModel key)
        {
            return db.FilaAtendimentoUsuarios.Find(key.FilaAtendimentoID, key.UsuarioID);
        }

        public override Validate Validate(FilaAtendimentoUsuarioViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.FilaAtendimentoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Fila ID").ToString();
                value.mensagem.MessageBase = "Código identificador da fila de atendimento deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };
            if (value.UsuarioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Fila ID").ToString();
                value.mensagem.MessageBase = "Usuário deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            if (operation != Crud.EXCLUIR)
            {
                if (value.Situacao == null || value.Situacao.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Situação").ToString();
                    value.mensagem.MessageBase = "Situação deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        public override FilaAtendimentoUsuarioViewModel CreateRepository(HttpRequestBase Request = null)
        {
            FilaAtendimentoUsuarioViewModel value = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            if (Request["FilaAtendimentoID"] != null)
                value.FilaAtendimentoID = int.Parse(Request["FilaAtendimentoID"]);
            value.empresaId = security.getSessaoCorrente().empresaId;
            return value;
        }
        #endregion
    }

    public class ListViewFilaAtendimentoUsuario : ListViewModelLocal<FilaAtendimentoUsuarioViewModel>
    {
        #region Constructor
        public ListViewFilaAtendimentoUsuario() { }

        public ListViewFilaAtendimentoUsuario(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<FilaAtendimentoUsuarioViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _FilaAtendimentoID = (int)param[0];

            return (from fil in db.FilaAtendimentos
                    join usu in db.FilaAtendimentoUsuarios on fil.FilaAtendimentoID equals usu.FilaAtendimentoID
                    where fil.CondominioID == SessaoLocal.empresaId
                            && fil.FilaAtendimentoID == _FilaAtendimentoID
                    orderby usu.UsuarioID
                    select new FilaAtendimentoUsuarioViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        FilaAtendimentoID = fil.FilaAtendimentoID,
                        UsuarioID = usu.UsuarioID,
                        Nome = usu.Nome,
                        Login = usu.Login,
                        Situacao = usu.Situacao,
                        PageSize = pageSize,
                        TotalCount = ((from fil1 in db.FilaAtendimentos
                                       join usu1 in db.FilaAtendimentoUsuarios on fil1.FilaAtendimentoID equals usu1.FilaAtendimentoID
                                       where fil1.CondominioID == SessaoLocal.empresaId
                                               && fil1.FilaAtendimentoID == _FilaAtendimentoID
                                       orderby usu1.UsuarioID
                                       select fil1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new FilaAtendimentoUsuarioModel().getObject((FilaAtendimentoUsuarioViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-fila-atendimento-usu";
        }
    }
}