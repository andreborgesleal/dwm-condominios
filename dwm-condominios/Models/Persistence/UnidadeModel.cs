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
        public override UnidadeViewModel BeforeInsert(UnidadeViewModel value)
        {
            if ((from u in db.Unidades
                 where u.CondominioID == sessaoCorrente.empresaId
                       && u.EdificacaoID == value.EdificacaoID
                 select u.UnidadeID).Count() == 0)
                value.UnidadeID = 1;
            else
                value.UnidadeID = (from u in db.Unidades
                                   where u.CondominioID == sessaoCorrente.empresaId
                                         && u.EdificacaoID == value.EdificacaoID
                                   select u.UnidadeID).Max() + 1;

            value.empresaId = sessaoCorrente.empresaId;
            return base.BeforeInsert(value);
        }

        public override Unidade MapToEntity(UnidadeViewModel value)
        {
            Unidade entity = Find(value);

            if (entity == null)
                entity = new Unidade();

            entity.CondominioID = value.CondominioID;
            entity.EdificacaoID = value.EdificacaoID;
            entity.UnidadeID = value.UnidadeID;
            entity.Codigo = value.Codigo;
            entity.TipoUnidade = value.TipoUnidade;
            entity.TipoCondomino = value.TipoCondomino;
            entity.NumVagas = value.NumVagas;
            if (value.Validador != null && value.Validador != "" && value.Validador != "dwm sistemas")
            {
                entity.Validador = value.Validador;
                entity.DataExpiracao = value.DataExpiracao;
                entity.NomeCondomino = value.NomeCondomino;
                entity.Email = value.Email;
            }

            return entity;
        }

        public override UnidadeViewModel MapToRepository(Unidade entity)
        {
            return new UnidadeViewModel()
            {
                CondominioID = entity.CondominioID,
                empresaId=sessaoCorrente.empresaId,
                EdificacaoID = entity.EdificacaoID,
                DescricaoEdificacao = db.Edificacaos.Find(entity.EdificacaoID).Descricao,
                UnidadeID = entity.UnidadeID,
                Codigo = entity.Codigo,
                TipoUnidade = entity.TipoUnidade,
                TipoCondomino = entity.TipoCondomino,
                NumVagas = entity.NumVagas,
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


            if (operation != Crud.EXCLUIR && String.IsNullOrEmpty(value.Codigo))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Código").ToString();
                value.mensagem.MessageBase = "Código da Unidade deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Verifica se a unidade já existe
            if (operation == Crud.INCLUIR)
            {
                if ((from u in db.Unidades
                     where u.CondominioID == value.CondominioID
                           && u.EdificacaoID == value.EdificacaoID
                           && u.Codigo == value.Codigo
                     select u).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Unidade já cadastrada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            else if (operation == Crud.EXCLUIR)
            {
                if ((from cu in db.CondominoUnidades
                     where cu.CondominioID == value.CondominioID 
                     && cu.EdificacaoID == value.EdificacaoID 
                     && cu.UnidadeID == value.UnidadeID
                     select cu).Count() > 0)
                {
                    value.mensagem.Code = 16;
                    value.mensagem.Message = MensagemPadrao.Message(16).ToString();
                    value.mensagem.MessageBase = MensagemPadrao.Message(16).ToString();
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            #endregion

            if (operation == Crud.ALTERAR && value.Validador != null && value.Validador != "dwm sistemas")
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
            u.TipoUnidade = "R";
            u.TipoCondomino = "F";
            u.Validador = "dwm sistemas";

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
            int _CondominioID = sessaoCorrente.empresaId;

            return (from u in db.Unidades
                    join e in db.Edificacaos on u.EdificacaoID equals e.EdificacaoID
                    where u.CondominioID == _CondominioID
                    orderby e.Descricao, u.UnidadeID
                    select new UnidadeViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        CondominioID = u.CondominioID,
                        EdificacaoID = u.EdificacaoID,
                        DescricaoEdificacao = e.Descricao,
                        UnidadeID = u.UnidadeID,
                        EdificacaoDescricao = e.Descricao,
                        Codigo = u.Codigo,
                        TipoUnidade = u.TipoUnidade,
                        TipoCondomino = u.TipoCondomino,
                        NumVagas = u.NumVagas,
                        Validador = u.Validador,
                        NomeCondomino = u.NomeCondomino,
                        DataExpiracao = u.DataExpiracao,
                        Email = u.Email,
                    }).ToList();
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