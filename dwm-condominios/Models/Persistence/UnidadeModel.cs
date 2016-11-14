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
    public class UnidadeModel : CrudModelLocal<Unidade, UnidadeViewModel>
    {
        #region Constructor
        public UnidadeModel() { }
        public UnidadeModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override Unidade MapToEntity(UnidadeViewModel value)
        {
            Unidade entity = Find(value);

            if (entity == null)
                entity = new Unidade();

            entity.CondominioID = value.CondominioID;
            entity.EdificacaoID = value.EdificacaoID;
            entity.UnidadeID = value.UnidadeID;
            entity.Validador = value.Validador;
            entity.DataExpiracao = value.DataExpiracao;
            entity.NomeCondomino = value.NomeCondomino;
            entity.Email = value.Email;

            return entity;
        }

        public override UnidadeViewModel MapToRepository(Unidade entity)
        {
            return new UnidadeViewModel()
            {
                CondominioID = entity.CondominioID,
                EdificacaoID = entity.EdificacaoID,
                UnidadeID = entity.UnidadeID,
                Validador = entity.Validador,
                DataExpiracao = entity.DataExpiracao,
                NomeCondomino = entity.NomeCondomino,
                Email = entity.Email,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Unidade Find(UnidadeViewModel key)
        {
            return db.Unidades.Find(key.CondominioID, key.EdificacaoID, key.UnidadeID);
        }

        public override Validate Validate(UnidadeViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.empresaId == 0)
            {
                value.mensagem.Code = 35;
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "Sua sessão expirou. Faça um novo login no sistema";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.CondominioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Condomínio deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.EdificacaoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Edificação").ToString();
                value.mensagem.MessageBase = "Edificação deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.UnidadeID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Unidade").ToString();
                value.mensagem.MessageBase = "Unidade deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.ALTERAR)
            {
                if (value.Validador.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Token").ToString();
                    value.mensagem.MessageBase = "Token deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (!value.DataExpiracao.HasValue || value.DataExpiracao < Funcoes.Brasilia().Date )
                {
                    value.mensagem.Code = 7;
                    value.mensagem.Message = MensagemPadrao.Message(7, "Data Expiração").ToString();
                    value.mensagem.MessageBase = "Data de expiração do token inválida";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }


                if (value.NomeCondomino.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Condômino").ToString();
                    value.mensagem.MessageBase = "Nome do Condômino deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Email == "" || value.Email == "")
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "E-mail", value.Email).ToString();
                    value.mensagem.MessageBase = "E-mail deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (!Funcoes.validaEmail(value.Email))
                {
                    value.mensagem.Code = 4;
                    value.mensagem.Message = MensagemPadrao.Message(4, "E-mail", value.Email).ToString();
                    value.mensagem.MessageBase = "E-mail inválido";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            
            return value.mensagem;
        }

        public override UnidadeViewModel CreateRepository(HttpRequestBase Request = null)
        {
            UnidadeViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }
        #endregion
    }

    public class ListViewUnidades : ListViewModelLocal<UnidadeViewModel>
    {
        #region Constructor
        public ListViewUnidades() { }
        public ListViewUnidades(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<UnidadeViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = param != null && param.Count() > 0 && param[0] != null ? int.Parse(param[0].ToString()) : 0;

            return (from u in db.Unidades
                    join e in db.Edificacaos on u.EdificacaoID equals e.EdificacaoID
                    where u.CondominioID == _CondominioID
                    orderby e.Descricao, u.UnidadeID
                    select new UnidadeViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        CondominioID = u.CondominioID,
                        EdificacaoID = u.EdificacaoID,
                        UnidadeID = u.UnidadeID,
                        EdificacaoDescricao = e.Descricao,
                        Validador = u.Validador,
                        NomeCondomino = u.NomeCondomino,
                        DataExpiracao = u.DataExpiracao,
                        Email = u.Email,
                        PageSize = pageSize,
                        TotalCount = ((from u1 in db.Unidades
                                       join e1 in db.Edificacaos on u1.EdificacaoID equals e1.EdificacaoID
                                       where u1.CondominioID == _CondominioID
                                       orderby e1.Descricao, u1.UnidadeID
                                       select u1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new UnidadeModel().getObject((UnidadeViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-unidade";
        }
    }
}