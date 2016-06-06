using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using DWM.Models.Entidades;
using DWM.Models.Repositories;

namespace DWM.Models.Persistence
{
    public class MembroModel : CrudModel<Membro, MembroViewModel, ApplicationContext>
    {
        #region Constructor
        public MembroModel() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public MembroModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override Membro MapToEntity(MembroViewModel value)
        {
            Membro membro = Find(value);

            if (membro == null)
            {
                membro = new Membro();
                value.IndSituacao = "D";
            }

            membro.MembroID = value.MembroID;
            membro.Nome = value.Nome;
            membro.Telefone = value.Telefone != null ? value.Telefone.Replace("(","").Replace(")","").Replace("-","") : value.Telefone;
            membro.Email = value.Email.ToLower();
            membro.CPF = value.CPF != null ? value.CPF.Replace(".", "").Replace("-", "") : value.CPF;
            membro.Banco = value.Banco;
            membro.Agencia = value.Agencia;
            membro.Conta = value.Conta;
            membro.IndSituacao = value.IndSituacao;
            membro.Avatar = value.Avatar;

            return membro;
        }

        public override MembroViewModel MapToRepository(Membro entity)
        {
            MembroViewModel membroViewModel = new MembroViewModel()
            {
                MembroID = entity.MembroID,
                Nome = entity.Nome,
                CPF = entity.CPF,
                Telefone = entity.Telefone,
                Email = entity.Email,
                Banco = entity.Banco,
                Agencia = entity.Agencia,
                Conta = entity.Conta,
                IndSituacao = entity.IndSituacao,
                Avatar = entity.Avatar,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };

            return membroViewModel;
        }

        public override Membro Find(MembroViewModel key)
        {
            return db.Membros.Find(key.MembroID);
        }

        public override Validate Validate(MembroViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation == Crud.EXCLUIR || operation == Crud.ALTERAR)
            {
                if (value.MembroID <= 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "ID Membro").ToString();
                    value.mensagem.MessageBase = "ID do apostador deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            if (value.Nome == null || value.Nome == "" || value.Nome.Length < 10)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome").ToString();
                value.mensagem.MessageBase = "Nome do apostador deve ser preenchido e ter no mínimo 10 caracteres";
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

            if (value.Telefone == null || value.Telefone == "" || value.Telefone.Replace("(","").Replace(")","").Replace("-","").Length < 11)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Telefone").ToString();
                value.mensagem.MessageBase = "Telefone deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Valida CPF
            if (!Funcoes.ValidaCpf(value.CPF.Replace(".", "").Replace("-", "")))
            {
                value.mensagem.Code = 29;
                value.mensagem.Message = MensagemPadrao.Message(29, value.CPF).ToString();
                value.mensagem.MessageBase = "CPF informado está incorreto.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            #endregion

            #region Verifica se o e-mail já existe
            if (db.Membros.Where(info => info.Email == value.Email).Count() > 0 && operation == Crud.INCLUIR)
            {
                value.mensagem.Code = 41;
                value.mensagem.Message = MensagemPadrao.Message(41, "E-mail: " + value.Email).ToString();
                value.mensagem.MessageBase = "E-mail já cadastrado.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            #endregion

            #region Verifica se existe o CPF 
            if (db.Membros.Where(info => info.CPF == value.CPF.Replace(".","").Replace("-","")).Count() > 0 && operation == Crud.INCLUIR)
            {
                value.mensagem.Code = 41;
                value.mensagem.Message = MensagemPadrao.Message(41, "CPF: " + value.CPF).ToString();
                value.mensagem.MessageBase = "CPF já cadastrado.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #endregion

            return value.mensagem;
        }
        #endregion
    }

    public class ListViewMembro : ListViewModel<MembroViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewMembro() { }
        public ListViewMembro(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<MembroViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param[0] != null && param[0].ToString() != "" ? param[0].ToString() : null;

            IEnumerable<MembroViewModel> query = (from m in db.Membros
                                                  where (_nome == null || m.Nome.Contains(_nome) || m.CPF == _nome || m.Email == _nome)
                                                  select new MembroViewModel()
                                                  {
                                                      MembroID = m.MembroID,
                                                      Nome = m.Nome,
                                                      Telefone = m.Telefone,
                                                      Email = m.Email,
                                                      CPF = m.CPF,
                                                      Avatar = m.Avatar,
                                                      Banco = m.Banco,
                                                      Agencia = m.Agencia,
                                                      Conta = m.Conta,
                                                      IndSituacao = m.IndSituacao
                                                  }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            return query;
        }

        public override string action()
        {
            return "../Home/ListMembros";
        }

        public override string DivId()
        {
            return "div-membro";
        }

        public override Repository getRepository(Object id)
        {
            return new MembroModel().getObject((MembroViewModel)id);
            //return new ApostaModel().getObject((ApostaViewModel)id);
        }
        #endregion
    }
}