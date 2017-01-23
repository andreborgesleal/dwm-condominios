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
using App_Dominio.Models;

namespace DWM.Models.Persistence
{
    public class ChamadoAnotacaoModel : CrudModelLocal<ChamadoAnotacao, ChamadoAnotacaoViewModel>
    {
        #region Constructor
        public ChamadoAnotacaoModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override ChamadoAnotacaoViewModel BeforeInsert(ChamadoAnotacaoViewModel value)
        {
            // Recupera o usuario
            value = GetUsuario(value);

            // Recupera a data do sistema
            value.DataAnotacao = Funcoes.Brasilia();

            return value;
        }

        public override ChamadoAnotacao MapToEntity(ChamadoAnotacaoViewModel value)
        {
            ChamadoAnotacao entity = Find(value);

            if (entity == null)
                entity = new ChamadoAnotacao();

            entity.ChamadoID = value.ChamadoID;
            entity.DataAnotacao = value.DataAnotacao;
            entity.Mensagem = value.Mensagem;
            entity.UsuarioID = value.UsuarioID;
            entity.Nome = value.Nome;
            entity.Login = value.Login;

            return entity;
        }

        public override ChamadoAnotacaoViewModel MapToRepository(ChamadoAnotacao entity)
        {
            return new ChamadoAnotacaoViewModel()
            {
                ChamadoID = entity.ChamadoID,
                DataAnotacao = entity.DataAnotacao,
                Mensagem = entity.Mensagem,
                UsuarioID = entity.UsuarioID,
                Nome = entity.Nome,
                Login = entity.Login,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override ChamadoAnotacao Find(ChamadoAnotacaoViewModel key)
        {
            return db.ChamadoAnotacaos.Find(key.ChamadoID, key.DataAnotacao);
        }

        public override Validate Validate(ChamadoAnotacaoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.ChamadoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Identificador do Chamado").ToString();
                value.mensagem.MessageBase = "Código identificador do chamado deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            if (value.DataAnotacao <= new DateTime(1980, 1, 1))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Data").ToString();
                value.mensagem.MessageBase = "Data da anotação deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (String.IsNullOrEmpty(value.Mensagem))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Texto da anotação").ToString();
                value.mensagem.MessageBase = "Texto da anotação deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.UsuarioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Usuário").ToString();
                value.mensagem.MessageBase = "Código identificador do usuário deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            return value.mensagem;
        }

        public override ChamadoAnotacaoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            ChamadoAnotacaoViewModel value = base.CreateRepository(Request);
            if (Request != null && Request["ChamadoID"] != null)
                value.ChamadoID = int.Parse(Request["ChamadoID"]);
            return value;
        }
        #endregion

        #region Métodos customizados
        private ChamadoAnotacaoViewModel GetUsuario(ChamadoAnotacaoViewModel value)
        {
            if (value.UsuarioID == 0)
                value.UsuarioID = SessaoLocal.usuarioId;

            value.Nome = seguranca_db.Usuarios.Find(value.UsuarioID).nome; ;
            value.Login = seguranca_db.Usuarios.Find(value.UsuarioID).login;

            return value;
        }
        #endregion
    }

    public class ListViewChamadoAnotacao : ListViewModelLocal<ChamadoAnotacaoViewModel>
    {
        #region Constructor
        public ListViewChamadoAnotacao() { }

        public ListViewChamadoAnotacao(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ChamadoAnotacaoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _ChamadoID = param != null && param.Count() > 0 && param[0] != null ? (int)param[0] : 0;

            if (_ChamadoID > 0)
                return (from cha in db.ChamadoAnotacaos
                        where cha.ChamadoID == _ChamadoID
                        orderby cha.DataAnotacao
                        select new ChamadoAnotacaoViewModel
                        {
                            empresaId = SessaoLocal.empresaId,
                            ChamadoID = cha.ChamadoID,
                            DataAnotacao = cha.DataAnotacao,
                            Mensagem = cha.Mensagem,
                            UsuarioID = cha.UsuarioID,
                            Nome = cha.Nome,
                            Login = cha.Login,
                            UsuarioViewModel = new UsuarioViewModel()
                            {
                                empresaId = SessaoLocal.empresaId,
                                usuarioId = cha.UsuarioID
                            },
                            PageSize = pageSize,
                            TotalCount = ((from cha1 in db.ChamadoAnotacaos
                                           where cha1.ChamadoID == _ChamadoID
                                           orderby cha1.DataAnotacao
                                           select cha1).Count())
                        }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            else
                return new List<ChamadoAnotacaoViewModel>();
        }

        public override Repository getRepository(Object id)
        {
            return new ChamadoAnotacaoModel().getObject((ChamadoAnotacaoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-chamado-anotacao";
        }
    }
}