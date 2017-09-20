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
            entity.UF = value.UF.ToUpper();
            entity.CEP = value.CEP != null && value.CEP != "" ? value.CEP.Replace("-", "") : value.CEP;

            foreach(ProprietarioUnidadeViewModel p in value.ProprietarioUnidades)
            {
                ProprietarioUnidade pu = new ProprietarioUnidade()
                {
                    ProprietarioID = entity.ProprietarioID,
                    CondominioID = SessaoLocal.empresaId,
                    EdificacaoID = p.EdificacaoID,
                    UnidadeID = p.UnidadeID,
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
                    ProprietarioID = pu.ProprietarioID,
                    empresaId = sessaoCorrente.empresaId,
                    CondominioID = pu.CondominioID,
                    EdificacaoID = pu.EdificacaoID,
                    UnidadeID = pu.UnidadeID,
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

                #region Unidades do Proprietário
                if (value.ProprietarioUnidades == null || value.ProprietarioUnidades.Count() == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Unidade").ToString();
                    value.mensagem.MessageBase = "O proprietário deve possuir pelo menos uma unidade.";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                foreach (ProprietarioUnidadeViewModel pu in value.ProprietarioUnidades)
                {
                    if (pu.CondominioID == 0)
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(5, "Unidade").ToString();
                        value.mensagem.MessageBase = "Condomínio deve ser informada";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }

                    if (pu.EdificacaoID == 0)
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(5, "Bloco").ToString();
                        value.mensagem.MessageBase = "Bloco deve ser informado";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }

                    if (pu.UnidadeID == 0)
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(5, "Unidade").ToString();
                        value.mensagem.MessageBase = "Unidade deve ser informada";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }

                    #region Verifica se a torre/unidade já não foi gravada para outro proprietário
                    if (operation == Crud.INCLUIR && 
                        (from punid in db.ProprietarioUnidades
                         where punid.CondominioID == pu.CondominioID
                                && punid.EdificacaoID == pu.EdificacaoID
                                && punid.UnidadeID == pu.UnidadeID
                                && punid.DataFim == null
                         select punid).Count() > 0)
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                        value.mensagem.MessageBase = "Unidade já pertence a outro proprietário";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                    else if (operation == Crud.ALTERAR &&
                            (from punid in db.ProprietarioUnidades
                             where punid.ProprietarioID != pu.ProprietarioID
                                    && punid.CondominioID == pu.CondominioID
                                    && punid.EdificacaoID == pu.EdificacaoID
                                    && punid.UnidadeID == pu.UnidadeID
                                    && punid.DataFim == null
                             select punid).Count() > 0)
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                        value.mensagem.MessageBase = "Unidade já pertence a outro proprietário";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                    #endregion
                }
                #endregion

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
                    if (value.IndFiscal.Trim().Replace(".","").Replace("-","").Replace("/","").Length <= 11 && !Funcoes.ValidaCpf(value.IndFiscal.Replace(".", "").Replace("-", "").Replace("/", "")))
                    {
                        value.mensagem.Code = 29;
                        value.mensagem.Message = MensagemPadrao.Message(29).ToString();
                        value.mensagem.MessageBase = "CPF inválido";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                    else if (value.IndFiscal.Trim().Replace(".", "").Replace("-", "").Replace("/", "").Length == 14 && !Funcoes.ValidaCnpj(value.IndFiscal.Trim().Replace(".", "").Replace("-", "").Replace("/", "")))
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

                #region Verifica se o CPF/CNPJ já não foi cadastrado para outro Proprietário
                if (operation == Crud.INCLUIR &&
                    (from p in db.Proprietarios join pu in db.ProprietarioUnidades on p.ProprietarioID equals pu.ProprietarioID
                     where pu.CondominioID == value.empresaId
                            && p.IndFiscal == value.IndFiscal.Replace(".","").Replace("/","").Replace("-","")
                     select p).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "CPF/CNPJ já cadastrado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
                else if (operation == Crud.ALTERAR &&
                        (from p in db.Proprietarios
                         join pu in db.ProprietarioUnidades on p.ProprietarioID equals pu.ProprietarioID
                         where pu.CondominioID == value.empresaId
                                && p.ProprietarioID != value.ProprietarioID
                                && p.IndFiscal == value.IndFiscal.Replace(".", "").Replace("/", "").Replace("-", "")
                         select p).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "CPF/CNPJ já cadastrado para outro proprietário";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
                #endregion

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

                #region Verifica se já não existe e-mail cadastrado para outro proprietário
                if (operation == Crud.INCLUIR &&
                    (from p in db.Proprietarios
                     join pu in db.ProprietarioUnidades on p.ProprietarioID equals pu.ProprietarioID
                     where pu.CondominioID == value.empresaId
                            && p.Email == value.Email
                     select p).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "E-mail já cadastrado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
                else if (operation == Crud.ALTERAR &&
                        (from p in db.Proprietarios
                         join pu in db.ProprietarioUnidades on p.ProprietarioID equals pu.ProprietarioID
                         where pu.CondominioID == value.empresaId
                                && p.ProprietarioID != value.ProprietarioID
                                && p.Email == value.Email
                         select p).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "E-mail já cadastrado para outro proprietário";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
                #endregion

                if (value.CidadeID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Cidade").ToString();
                    value.mensagem.MessageBase = "Cidade deve ser informada";
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

    public class ListViewProprietario : ListViewModelLocal<ProprietarioViewModel>
    {
        #region Constructor
        public ListViewProprietario() { }
        public ListViewProprietario(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ProprietarioViewModel> Bind(int? index, int pageSize = 20, params object[] param)
        {
            int CondominioID = SessaoLocal.empresaId;

            IEnumerable<ProprietarioViewModel> query = null;

            query = (from p in db.Proprietarios
                     join c in db.Cidades on p.CidadeID equals c.CidadeID
                     where (from pu in db.ProprietarioUnidades
                            where pu.CondominioID == CondominioID
                                    && pu.ProprietarioID == p.ProprietarioID
                                    && pu.DataFim == null
                            select pu.ProprietarioID).Count() > 0
                     orderby p.Nome
                     select new ProprietarioViewModel()
                     {
                         ProprietarioID = p.ProprietarioID,
                         Nome = p.Nome,
                         IndTipoPessoa = p.IndTipoPessoa,
                         IndFiscal = p.IndFiscal,
                         Email = p.Email,
                         Telefone = p.Telefone,
                         Endereco = p.Endereco,
                         Complemento = p.Complemento,
                         CidadeDescricao = c.Nome,
                         UF = p.UF,
                         CEP = p.CEP,
                         ProprietarioUnidades = (from pu in db.ProprietarioUnidades
                                                 join u in db.Unidades on new {pu.CondominioID, pu.EdificacaoID, pu.UnidadeID } equals new { u.CondominioID, u.EdificacaoID, u.UnidadeID }
                                                 join e in db.Edificacaos on pu.EdificacaoID equals e.EdificacaoID
                                                 where pu.CondominioID == CondominioID
                                                        && pu.ProprietarioID == p.ProprietarioID
                                                        && pu.DataFim == null
                                                 select new ProprietarioUnidadeViewModel()
                                                 {
                                                     UnidadeID = pu.UnidadeID,
                                                     Codigo = u.Codigo,
                                                     EdificacaoID = pu.EdificacaoID,
                                                     EdificacaoDescricao = e.Descricao
                                                 }).ToList()
                     }).ToList();

            return query;
        }

        public override string action()
        {
            return "../Proprietario/ListParam";
        }

        public override string DivId()
        {
            return "div-proprietario";
        }

        public override App_Dominio.Component.Repository getRepository(Object id)
        {
            return new ProprietarioModel().getObject((ProprietarioViewModel)id);
        }
        #endregion
    }
}   