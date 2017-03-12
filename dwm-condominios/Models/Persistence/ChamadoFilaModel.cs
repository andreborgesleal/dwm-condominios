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
    public class ChamadoFilaModel : CrudModelLocal<ChamadoFila, ChamadoFilaViewModel>
    {
        #region Constructor
        public ChamadoFilaModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override ChamadoFilaViewModel BeforeInsert(ChamadoFilaViewModel value)
        {
            // Recupera o usuario
            value = GetUsuario(value);

            // Recupera a data do sistema
            value.Data = Funcoes.Brasilia();

            return value;
        }

        public override ChamadoFilaViewModel BeforeUpdate(ChamadoFilaViewModel value)
        {
            return GetUsuario(value); 
        }

        public override ChamadoFila MapToEntity(ChamadoFilaViewModel value)
        {
            ChamadoFila entity = Find(value);

            if (entity == null)
                entity = new ChamadoFila();

            entity.ChamadoID = value.ChamadoID;
            entity.Data = value.Data;
            entity.FilaAtendimentoID = value.FilaAtendimentoID;
            entity.UsuarioID = value.UsuarioID;
            entity.Nome = value.Nome;
            entity.Login = value.Login;

            return entity;
        }

        public override ChamadoFilaViewModel MapToRepository(ChamadoFila entity)
        {
            return new ChamadoFilaViewModel()
            {
                ChamadoID = entity.ChamadoID,
                Data = entity.Data,
                FilaAtendimentoID = entity.FilaAtendimentoID,
                UsuarioID = entity.UsuarioID,
                Nome = entity.Nome,
                Login = entity.Login,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override ChamadoFila Find(ChamadoFilaViewModel key)
        {
            return db.ChamadoFilas.Find(key.ChamadoID, key.Data);
        }

        public override Validate Validate(ChamadoFilaViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            //if (value.ChamadoID == 0)
            //{
            //    value.mensagem.Code = 5;
            //    value.mensagem.Message = MensagemPadrao.Message(5, "Identificador do Chamado").ToString();
            //    value.mensagem.MessageBase = "Código identificador do chamado deve ser informado";
            //    value.mensagem.MessageType = MsgType.WARNING;
            //    return value.mensagem;
            //};

            if (value.Data <= new DateTime(1980, 1, 1))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Data").ToString();
                value.mensagem.MessageBase = "Data deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.FilaAtendimentoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Identificador da Fila").ToString();
                value.mensagem.MessageBase = "Código identificador da Fila de atendimento deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            return value.mensagem;
        }

        public override ChamadoFilaViewModel CreateRepository(HttpRequestBase Request = null)
        {
            ChamadoFilaViewModel value = base.CreateRepository(Request);
            if (Request != null && Request["ChamadoID"] != null)
                value.ChamadoID = int.Parse(Request["ChamadoID"]);
            return value;
        }
        #endregion

        #region Métodos customizados
        private ChamadoFilaViewModel GetUsuario(ChamadoFilaViewModel value)
        {
            if (value.UsuarioID.HasValue)
            {
                value.Nome = seguranca_db.Usuarios.Find(value.UsuarioID).nome; ;
                value.Login = seguranca_db.Usuarios.Find(value.UsuarioID).login;
            }

            return value;
        }
        #endregion
    }

    public class ListViewChamadoFila : ListViewModelLocal<ChamadoFilaViewModel>
    {
        public ListViewChamadoFila(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null) : base(_db, _seguranca_db, Token)
        {
        }
        #region Constructor
        //public ListViewChamadoFila() { }

        //public ListViewChamadoFila(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        //{
        //    this.Create(_db, _seguranca_db, Token);
        //}

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ChamadoFilaViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _ChamadoID = param != null && param.Count() > 0 && param[0] != null ? (int)param[0] : 0;

            if (_ChamadoID > 0)
                return (from chf in db.ChamadoFilas
                        join fil in db.FilaAtendimentos on chf.FilaAtendimentoID equals fil.FilaAtendimentoID into FIL
                        from fil in FIL.DefaultIfEmpty()
                        where chf.ChamadoID == _ChamadoID
                        orderby chf.Data
                        select new ChamadoFilaViewModel
                        {
                            empresaId = SessaoLocal.empresaId,
                            ChamadoID = chf.ChamadoID,
                            Data = chf.Data,
                            FilaAtendimentoID = chf.FilaAtendimentoID,
                            DescricaoFilaAtendimento = fil != null ? fil.Descricao : "",
                            UsuarioID = chf.UsuarioID,
                            Nome = chf.Nome,
                            Login = chf.Login,
                            PageSize = pageSize,
                            TotalCount = ((from chf1 in db.ChamadoFilas
                                           join fil1 in db.FilaAtendimentos on chf1.FilaAtendimentoID equals fil1.FilaAtendimentoID into FIL1
                                           from fil1 in FIL1.DefaultIfEmpty()
                                           where chf1.ChamadoID == _ChamadoID
                                           orderby chf1.Data
                                           select chf1).Count())
                        }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            else
                return new List<ChamadoFilaViewModel>();
        }

        public override Repository getRepository(Object id)
        {
            return new ChamadoFilaModel().getObject((ChamadoFilaViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-chamado-fila";
        }
    }

}