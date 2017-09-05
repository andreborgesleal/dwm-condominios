using System;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web;
using App_Dominio.Security;

namespace DWM.Models.Persistence
{
    public class ProprietarioModel : CrudModelLocal<Proprietario, ProprietarioViewModel>
    {
        #region Constructor
        public ProprietarioModel() { }
        public ProprietarioModel(ApplicationContext _db, SecurityContext _seguranca_db, bool anonymous = false)
        {
            if (!anonymous)
                Create(_db, _seguranca_db);
            else
            {
                this.db = _db;
                this.seguranca_db = _seguranca_db;
            }
        }
        #endregion

        #region Métodos da classe CrudModel
        public override Proprietario MapToEntity(ProprietarioViewModel value)
        {
            Proprietario entity = Find(value);

            if (entity == null)
            {
                entity = new Proprietario();
                entity.ProprietarioUnidades = new List<ProprietarioUnidade>();
            }
            else
            {
                entity.ProprietarioUnidades.Clear();
            };

            entity.ProprietarioID = value.ProprietarioID;
            entity.Nome = value.Nome;
            entity.IndTipoPessoa = value.IndTipoPessoa;
            entity.IndFiscal = value.IndFiscal != null && value.IndFiscal.Trim() != "" ? value.IndFiscal.Replace("-","").Replace(".","").Replace("/","") : value.IndFiscal;
            entity.Email = value.Email;
            entity.Telefone = value.Telefone != null && value.Telefone.Trim().Length > 0 ? value.Telefone.Replace("(", "").Replace(")", "").Replace("-", "") : value.Telefone;
            entity.Endereco = value.Endereco;
            entity.Complemento = value.Complemento;
            entity.CidadeID = value.CidadeID;
            entity.UF = value.UF;
            entity.CEP = value.CEP != null && value.CEP != "" ? value.CEP.Replace("-", "") : value.CEP;

            foreach(ProprietarioUnidadeViewModel p in value.ProprietarioUnidades)
            {
                ProprietarioUnidade pu = new ProprietarioUnidade()
                {
                    CondominioID = SessaoLocal.empresaId,
                    EdificacaoID = p.EdificacaoID,
                    UnidadeID = p.UnidadeID,
                    ProprietarioID = p.ProprietarioID,
                    DataFim = p.DataFim
                };

                entity.ProprietarioUnidades.Add(pu);
            }

            return entity;
        }

        public override ProprietarioViewModel MapToRepository(Proprietario entity)
        {
            ProprietarioViewModel value = new ProprietarioViewModel()
            {
                ProprietarioID = entity.ProprietarioID,
                empresaId = sessaoCorrente.empresaId,
                Nome = entity.Nome,
                IndTipoPessoa = entity.IndTipoPessoa,
                IndFiscal = entity.IndFiscal,
                Email = entity.Email,
                Telefone = entity.Telefone,
                Endereco = entity.Endereco,
                Complemento = entity.Complemento,
                CidadeID = entity.CidadeID,
                UF = entity.UF,
                CEP = entity.CEP,
                ProprietarioUnidades = null,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };

            if (entity.ProprietarioUnidades != null && entity.ProprietarioUnidades.Count > 0)
                value.ProprietarioUnidades = new List<ProprietarioUnidadeViewModel>();

            foreach (ProprietarioUnidade pu in entity.ProprietarioUnidades)
            {
                ProprietarioUnidadeViewModel puRepository = new ProprietarioUnidadeViewModel()
                {
                    empresaId = sessaoCorrente.empresaId,
                    CondominioID = pu.CondominioID,
                    EdificacaoID = pu.EdificacaoID,
                    UnidadeID = pu.UnidadeID,
                    ProprietarioID = pu.ProprietarioID,
                    DataFim = pu.DataFim,
                    mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
                };
                ((List<ProprietarioUnidadeViewModel>)value.ProprietarioUnidades).Add(puRepository);
            };

            return value;
        }

        public override Proprietario Find(ProprietarioViewModel key)
        {
            return db.Proprietarios.Find(key.ProprietarioID);
        }

        public override Validate Validate(ProprietarioViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.ProprietarioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID").ToString();
                value.mensagem.MessageBase = "Código identificador do Proprietário deve ser informado";
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

                if (value.Nome == null || value.Nome.Trim().Length < 5)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Nome").ToString();
                    value.mensagem.MessageBase = "Nome do Proprietário deve ser informado e deve possuir mais de 5 caracteres";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.IndTipoPessoa == null || !"FJ".Contains(value.IndTipoPessoa))
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Tipo Pessoa").ToString();
                    value.mensagem.MessageBase = "Tipo Pessoa deve ser informado com F ou J";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.IndFiscal != null)
                {
                    if (value.IndFiscal.Trim().Length <= 11 && !Funcoes.ValidaCpf(value.IndFiscal))
                    {
                        value.mensagem.Code = 29;
                        value.mensagem.Message = MensagemPadrao.Message(29).ToString();
                        value.mensagem.MessageBase = "CPF inválido";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                    else if (!Funcoes.ValidaCnpj(value.IndFiscal))
                    {
                        value.mensagem.Code = 30;
                        value.mensagem.Message = MensagemPadrao.Message(30).ToString();
                        value.mensagem.MessageBase = "CNPJ inválido";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                }
                else
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "CPF/CNPJ").ToString();
                    value.mensagem.MessageBase = "CPF/CNPJ deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Email == null || value.Email.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "E-mail").ToString();
                    value.mensagem.MessageBase = "E-mail deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
                else if (!Funcoes.validaEmail(value.Email))
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "E-mail").ToString();
                    value.mensagem.MessageBase = "E-mail em formato inválido";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        public override ProprietarioViewModel CreateRepository(HttpRequestBase Request = null)
        {
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            ProprietarioViewModel p = base.CreateRepository(Request);
            p.empresaId = security.getSessaoCorrente().empresaId;
            return p;
        }
        #endregion
    }
}   