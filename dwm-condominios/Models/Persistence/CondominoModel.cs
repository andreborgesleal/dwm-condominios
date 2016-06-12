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
using App_Dominio.Security;
using System.Web;

namespace DWM.Models.Persistence
{
    public abstract class CondominoModel<E,R> : CrudModel<E, R, ApplicationContext>
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
        public override E MapToEntity(R value)
        {
            E condomino = (E)Find(value);

            if (condomino == null)
                condomino = getEntityInstance();

            condomino.CondominoID = value.CondominoID;
            condomino.Nome = value.Nome;
            condomino.IndFiscal = value.IndFiscal != null ? value.IndFiscal.Replace(".", "").Replace("-", "").Replace("/","") : value.IndFiscal;
            condomino.IndProprietario = value.IndProprietario;
            condomino.TelParticular1 = value.TelParticular1 != null ? value.TelParticular1.Replace("(", "").Replace(")", "").Replace("-", "") : value.TelParticular1;
            condomino.TelParticular2 = value.TelParticular2 != null ? value.TelParticular1.Replace("(", "").Replace(")", "").Replace("-", "") : value.TelParticular2;
            condomino.IndSituacao = value.IndSituacao;
            condomino.Email = value.Email.ToLower();
            condomino.UsuarioID = value.UsuarioID;
            condomino.Observacao = value.Observacao;
            condomino.DataCadastro = Funcoes.Brasilia();
            condomino.Avatar = value.Avatar;

            condomino.CondominoUnidade = new CondominoUnidade()
            {
                CondominioID = value.CondominoUnidadeViewModel.CondominioID,
                EdificacaoID = value.CondominoUnidadeViewModel.EdificacaoID,
                UnidadeID = value.CondominoUnidadeViewModel.UnidadeID,
                CondominoID = value.CondominoUnidadeViewModel.CondominoID,
                DataInicio = value.CondominoUnidadeViewModel.DataInicio,
                DataFim = value.CondominoUnidadeViewModel.DataFim
            };

            return condomino;
        }

        public override R MapToRepository(E entity)
        {
            R condominoViewModel = getRepositoryInstance();

            condominoViewModel.CondominoID = entity.CondominoID;
            condominoViewModel.Nome = entity.Nome;
            condominoViewModel.IndFiscal = entity.IndFiscal;
            condominoViewModel.IndProprietario = entity.IndProprietario;
            condominoViewModel.TelParticular1 = entity.TelParticular1;
            condominoViewModel.TelParticular2 = entity.TelParticular2;
            condominoViewModel.Email = entity.Email.ToLower();
            condominoViewModel.UsuarioID = entity.UsuarioID;
            condominoViewModel.Observacao = entity.Observacao;
            condominoViewModel.DataCadastro = Funcoes.Brasilia();
            condominoViewModel.Avatar = entity.Avatar;
            condominoViewModel.mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS };

            condominoViewModel.CondominoUnidadeViewModel = new CondominoUnidadeViewModel()
            {
                CondominioID = entity.CondominoUnidade.CondominioID,
                EdificacaoID = entity.CondominoUnidade.EdificacaoID,
                UnidadeID = entity.CondominoUnidade.UnidadeID,
                CondominoID = entity.CondominoUnidade.CondominoID,
                DataInicio = entity.CondominoUnidade.DataInicio,
                DataFim = entity.CondominoUnidade.DataFim
            };

            return condominoViewModel;
        }

        public override Validate Validate(R value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

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

            if (value.TelParticular1 == null || value.TelParticular1 == "" || value.TelParticular1.Replace("(", "").Replace(")", "").Replace("-", "").Length < 11)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Telefone").ToString();
                value.mensagem.MessageBase = "Telefone deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Valida CPF/CNPJ
            if (value.IndFiscal.Replace(".", "").Replace("-", "").Length <= 11 && !Funcoes.ValidaCpf(value.IndFiscal.Replace(".", "").Replace("-", "")))
            {
                value.mensagem.Code = 29;
                value.mensagem.Message = MensagemPadrao.Message(29, value.IndFiscal).ToString();
                value.mensagem.MessageBase = "CPF informado está incorreto.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
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

            CondominoUnidadeModel condominoUnidadeModel = new CondominoUnidadeModel(this.db, this.seguranca_db);
            value.CondominoUnidadeViewModel.mensagem = condominoUnidadeModel.Validate(value.CondominoUnidadeViewModel, operation);
            if (value.CondominoUnidadeViewModel.mensagem.Code > 0)
                throw new App_DominioException(value.CondominoUnidadeViewModel.mensagem);

            return value.mensagem;
        }
        #endregion
    }
}