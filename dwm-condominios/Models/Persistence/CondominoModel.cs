﻿using System;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System.Data.Entity;

namespace DWM.Models.Persistence
{
    public abstract class CondominoModel<E,R> : CrudModelLocal<E, R>
        where E : Condomino
        where R : CondominoViewModel
    {
        #region Constructor
        public CondominoModel() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public CondominoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }

        protected R getRepositoryInstance()
        {
            Type typeInstance = typeof(R);
            return (R)Activator.CreateInstance(typeInstance);
        }

        #endregion

        #region Métodos da classe CrudModel
        public override R AfterUpdate(R value)
        {
            Usuario u = seguranca_db.Usuarios.Find(value.UsuarioID);
            u.situacao = value.IndSituacao;
            seguranca_db.Entry(u).State = EntityState.Modified;

            return base.AfterUpdate(value);
        }

        public override E MapToEntity(R value)
        {
            E condomino = (E)Find(value);

            if (condomino == null)
            {
                condomino = getEntityInstance();
                condomino.DataCadastro = Funcoes.Brasilia();
                condomino.UsuarioID = value.UsuarioID;
            }

            condomino.CondominoID = value.CondominoID;
            condomino.CondominioID = value.CondominioID;
            condomino.Nome = value.Nome.ToUpper();
            condomino.IndFiscal = value.IndFiscal != null ? value.IndFiscal.Replace(".", "").Replace("-", "").Replace("/","") : value.IndFiscal;
            condomino.IndProprietario = value.IndProprietario;
            condomino.TelParticular1 = value.TelParticular1 != null ? value.TelParticular1.Replace("(", "").Replace(")", "").Replace("-", "") : value.TelParticular1;
            condomino.TelParticular2 = value.TelParticular2 != null ? value.TelParticular2.Replace("(", "").Replace(")", "").Replace("-", "") : value.TelParticular2;
            condomino.IndSituacao = value.IndSituacao;
            condomino.Email = value.Email.ToLower();
            condomino.Observacao = value.Observacao; 
            condomino.Avatar = value.Avatar;

            return condomino;
        }

        public override R MapToRepository(E entity)
        {
            R condominoViewModel = getRepositoryInstance();

            condominoViewModel.CondominoID = entity.CondominoID;
            condominoViewModel.CondominioID = entity.CondominioID;
            condominoViewModel.Nome = entity.Nome;
            condominoViewModel.IndFiscal = entity.IndFiscal;
            condominoViewModel.IndProprietario = entity.IndProprietario;
            condominoViewModel.TelParticular1 = entity.TelParticular1;
            condominoViewModel.TelParticular2 = entity.TelParticular2;
            condominoViewModel.IndSituacao = entity.IndSituacao;
            condominoViewModel.Email = entity.Email.ToLower();
            condominoViewModel.UsuarioID = entity.UsuarioID;
            condominoViewModel.Observacao = entity.Observacao;
            condominoViewModel.DataCadastro = entity.DataCadastro;
            condominoViewModel.Avatar = entity.Avatar;
            #region Avatar do condômino
            condominoViewModel.UsuarioViewModel = new UsuarioViewModel()
            {
                usuarioId = entity.UsuarioID ?? 0,
                empresaId = entity.CondominioID
            };
            #endregion

            condominoViewModel.mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS };

            return condominoViewModel;
        }

        public override Validate Validate(R value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.CondominioID <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID Condomínio").ToString();
                value.mensagem.MessageBase = "ID do condomínio deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.EXCLUIR || operation == Crud.ALTERAR)
            {
                if (value.CondominoID <= 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "ID Condomino").ToString();
                    value.mensagem.MessageBase = "ID do condômino deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            if (value.Nome == null || value.Nome == "" || value.Nome.Length < 10)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome").ToString();
                value.mensagem.MessageBase = "Nome do condômino deve ser preenchido e ter no mínimo 10 caracteres";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.Email == null || value.Email == "" || !Funcoes.validaEmail(value.Email))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "E-mail").ToString();
                value.mensagem.MessageBase = "E-mail deve ser preenchido e ter um formato válido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.TelParticular1 == null || value.TelParticular1 == "" || value.TelParticular1.Replace("(", "").Replace(")", "").Replace("-", "").Length < 10)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Telefone").ToString();
                value.mensagem.MessageBase = "Telefone deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Valida CPF/CNPJ
            if (value.IndFiscal.Replace(".", "").Replace("-", "").Length <= 11)
            {
                if (!Funcoes.ValidaCpf(value.IndFiscal.Replace(".", "").Replace("-", "")))
                {
                    value.mensagem.Code = 29;
                    value.mensagem.Message = MensagemPadrao.Message(29, value.IndFiscal).ToString();
                    value.mensagem.MessageBase = "CPF informado está incorreto.";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            else if (!Funcoes.ValidaCnpj(value.IndFiscal.Replace(".", "").Replace("-", "").Replace("/", "")))
            {
                value.mensagem.Code = 30;
                value.mensagem.Message = MensagemPadrao.Message(30, value.IndFiscal).ToString();
                value.mensagem.MessageBase = "CNPJ informado está incorreto.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            #endregion

            #region Verifica se o e-mail já existe
            if (db.Condominos.Where(info => info.Email == value.Email).Count() > 0 && operation == Crud.INCLUIR)
            {
                value.mensagem.Code = 41;
                value.mensagem.Message = MensagemPadrao.Message(41, "E-mail: " + value.Email).ToString();
                value.mensagem.MessageBase = "E-mail já cadastrado.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            #endregion

            #region Verifica se existe o IndFiscal 
            if (db.Condominos.Where(info => info.IndFiscal == value.IndFiscal.Replace(".", "").Replace("-", "").Replace("/","")).Count() > 0 && operation == Crud.INCLUIR)
            {
                value.mensagem.Code = 41;
                value.mensagem.Message = MensagemPadrao.Message(41, "CPF/CNPJ: " + value.IndFiscal).ToString();
                value.mensagem.MessageBase = "CPF/CNPJ já cadastrado.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            #endregion

            return value.mensagem;
        }
        #endregion
    }
}