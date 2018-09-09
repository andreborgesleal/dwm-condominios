using App_Dominio.Entidades;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App_Dominio.Contratos;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using App_Dominio.Component;
using System.IO;
using App_Dominio.Security;
using System.Data.Entity.SqlServer;

namespace DWM.Models.Persistence
{
    public class EspacoComumModel : CrudModelLocal<EspacoComum, EspacoComumViewModel>
    {
        #region Constructor
        public EspacoComumModel() { }
        public EspacoComumModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        public EspacoComumModel(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }

        #endregion

        #region Métodos da classe CrudModel

        public override EspacoComum MapToEntity(EspacoComumViewModel value)
        {
            EspacoComum entity = Find(value);

            if (entity == null)
                entity = new EspacoComum();

            entity.EspacoID = value.EspacoID;
            entity.CondominioID = value.CondominioID;
            entity.Descricao = value.Descricao;
            entity.LimitePessoas = value.LimitePessoas;
            entity.Valor = value.Valor;

            return entity;
        }

        public override EspacoComumViewModel MapToRepository(EspacoComum entity)
        {
            return new EspacoComumViewModel()
            {
                EspacoID = entity.EspacoID,
                CondominioID = entity.CondominioID,
                Descricao = entity.Descricao,
                LimitePessoas = entity.LimitePessoas,
                Valor = entity.Valor,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override EspacoComum Find(EspacoComumViewModel key)
        {
            return db.EspacoComums.Find(key.EspacoID);
        }

        public override Validate Validate(EspacoComumViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.EspacoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Espaco").ToString();
                value.mensagem.MessageBase = "Código identificador do Espaço deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (value.CondominioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Código identificador do condomínio deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            else if (String.IsNullOrEmpty(value.Descricao))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Descrição").ToString();
                value.mensagem.MessageBase = "Descrição do espaço deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            else if (value.LimitePessoas <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Limite de Pessoas").ToString();
                value.mensagem.MessageBase = "Limite de Pessoas deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation != Crud.EXCLUIR)
            {
                if (value.empresaId == 0)
                {
                    value.mensagem.Code = 35;
                    value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                    value.mensagem.MessageBase = "Sua sessão expirou. Faça um novo login no sistema";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        public override EspacoComumViewModel CreateRepository(HttpRequestBase Request = null)
        {
            EspacoComumViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }

        #endregion
    }

    public class ListViewEspacoComum : ListViewModelLocal<EspacoComumViewModel>
    {
        public ListViewEspacoComum()
        {
        }

        public ListViewEspacoComum(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EspacoComumViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            var q = (from value in db.EspacoComums
                     select new EspacoComumViewModel
                     {
                        empresaId = sessaoCorrente.empresaId,
                        CondominioID = value.CondominioID,
                        Descricao = value.Descricao,
                        EspacoID = value.EspacoID,
                        LimitePessoas = value.LimitePessoas,
                        sessionId = sessaoCorrente.sessaoId,
                        Valor = value.Valor,
                     }).ToList();

            return q;
        }

        public override string action()
        {
            return "../Home/ListEspacoComum";
        }

        public override string DivId()
        {
            return "div-EspacoComum";
        }

        public override Repository getRepository(Object id)
        {
            return new EspacoComumModel().getObject((EspacoComumViewModel)id);
        }
        #endregion
    }
}