﻿using System;
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

namespace DWM.Models.Persistence
{
    public class CredenciadoModel : CrudModel<Credenciado, CredenciadoViewModel, ApplicationContext>
    {
        #region Constructor
        public CredenciadoModel() { }
        public CredenciadoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override Credenciado MapToEntity(CredenciadoViewModel value)
        {
            Credenciado entity = Find(value);

            if (entity == null)
                entity = new Credenciado();

            entity.CredenciadoID = value.CredenciadoID;
            entity.CondominoID = value.CondominoID;
            entity.Nome = value.Nome;
            entity.Email = value.Email;
            entity.Sexo = value.Sexo;
            entity.Observacao = value.Observacao;
            entity.UsuarioID = value.UsuarioID;

            return entity;
        }

        public override CredenciadoViewModel MapToRepository(Credenciado entity)
        {
            return new CredenciadoViewModel()
            {
                CredenciadoID = entity.CredenciadoID,
                CondominoID = entity.CondominoID,
                Nome = entity.Nome,
                Email = entity.Email,
                Sexo = entity.Sexo,
                Observacao = entity.Observacao,
                UsuarioID = entity.UsuarioID,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Credenciado Find(CredenciadoViewModel key)
        {
            return db.Credenciados.Find(key.CredenciadoID);
        }

        public override Validate Validate(CredenciadoViewModel value, Crud operation)
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

            if (value.CondominoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condômino").ToString();
                value.mensagem.MessageBase = "Condômino deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.Nome.Trim().Length == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do credenciado").ToString();
                value.mensagem.MessageBase = "Nome do credenciado deve ser informado";
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

            if (operation == Crud.INCLUIR || operation == Crud.ALTERAR)
            {
                int descricaoCredenciado = (from c in db.Credenciados
                                              where c.CondominoID == value.CondominoID
                                                    && c.CredenciadoID != value.CredenciadoID
                                                    && c.Nome.Equals(value.Nome)
                                              select c.Nome).Count();
                if (descricaoCredenciado > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Nome do credenciado já existente";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        public override CredenciadoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            CredenciadoViewModel value = base.CreateRepository(Request);
            value.Sexo = "M";
            return value;
        }
        #endregion
    }

    public class ListViewCredenciados : ListViewModel<CredenciadoViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewCredenciados() { }
        public ListViewCredenciados(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<CredenciadoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominoID = param != null && param.Count() > 0 && param[0] != null ? int.Parse(param[0].ToString()) : 0;

            return (from c in db.Credenciados
                    where c.CondominoID == _CondominoID
                    orderby c.Nome
                    select new CredenciadoViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        CredenciadoID = c.CredenciadoID,
                        CondominoID = c.CondominoID,
                        Nome = c.Nome,
                        Email = c.Email,
                        Sexo = c.Sexo,
                        Observacao = c.Observacao,
                        UsuarioID = c.UsuarioID,
                        PageSize = pageSize,
                        TotalCount = ((from c1 in db.Credenciados
                                       where c1.CondominoID == _CondominoID
                                       orderby c1.Nome
                                       select c1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new CredenciadoModel().getObject((CredenciadoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-credenciado";
        }
    }
}