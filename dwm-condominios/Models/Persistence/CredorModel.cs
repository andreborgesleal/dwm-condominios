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

namespace DWM.Models.Persistence
{
    public class CredorModel : CrudContext<Credor, CredorViewModel, ApplicationContext>
    {
        #region Métodos da classe CrudContext
        public override Credor MapToEntity(CredorViewModel value)
        {
            Credor credor = Find(value);

            if (credor == null)
                credor = new Credor();

            credor.credorId = value.credorId;
            credor.CondominioID = value.empresaId;
            credor.nome = value.nome.Replace("&", "e");
            credor.grupoCredorId = value.grupoCredorId;
            credor.ind_tipo_pessoa = value.ind_tipo_pessoa.Substring(1, 1);
            credor.cpf_cnpj = value.cpf_cnpj != null ? value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "") : null;
            credor.dt_inclusao = value.dt_inclusao;
            credor.dt_alteracao = Funcoes.Brasilia();
            credor.codigo = value.codigo;
            credor.endereco = value.endereco != null ? value.endereco.Replace("&", "e") : null;
            credor.complemento = value.complemento != null ? value.complemento.Replace("&", "e") : null;
            credor.cidade = value.cidade != null ? value.cidade.Replace("&", "e") : null;
            credor.uf = value.uf;
            credor.cep = value.cep != null ? value.cep.Replace(".", "").Replace("-", "") : null;
            credor.bairro = value.bairro != null ? value.bairro.Replace("&", "e") : null;
            credor.fone1 = value.fone1 != null ? value.fone1.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : null;
            credor.fone2 = value.fone2 != null ? value.fone2.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : null;
            credor.fone3 = value.fone3 != null ? value.fone3.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : null;
            credor.email = value.email != null ? value.email.ToLower() : null;
            credor.sexo = value.sexo;
            credor.dt_nascimento = value.dt_nascimento;
            credor.observacao = value.observacao != null ? value.observacao.Replace("&", "e") : null;

            return credor;

        }

        public override CredorViewModel MapToRepository(Credor entity)
        {
            return new CredorViewModel()
            {
                credorId = entity.credorId,
                nome = entity.nome,
                CondominioID = entity.CondominioID,
                grupoCredorId = entity.grupoCredorId,
                ind_tipo_pessoa = "P" + entity.ind_tipo_pessoa,
                cpf_cnpj = entity.cpf_cnpj,
                dt_inclusao = entity.dt_inclusao,
                dt_alteracao = entity.dt_alteracao,
                codigo = entity.codigo,
                endereco = entity.endereco,
                complemento = entity.complemento,
                cidade = entity.cidade,
                uf = entity.uf,
                cep = entity.cep,
                bairro = entity.bairro,
                fone1 = entity.fone1,
                fone2 = entity.fone2,
                fone3 = entity.fone3,
                email = entity.email,
                sexo = entity.sexo,
                dt_nascimento = entity.dt_nascimento,
                observacao = entity.observacao,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Credor Find(CredorViewModel key)
        {
            return db.Credores.Find(key.credorId);
        }

        public override Validate Validate(CredorViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString(), MessageType = MsgType.SUCCESS };

            if (value.nome.Trim().Length == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome").ToString();
                value.mensagem.MessageBase = "Campo Nome do Credor deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (operation == Crud.INCLUIR)
            {
                // Verifica se o credor já foi cadastrado com o mesmo nome
                if (db.Credores.Where(info => info.nome == value.nome && info.CondominioID == value.CondominioID).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Campo Nome do Credor já existe";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            else if (operation == Crud.ALTERAR)
                // Verifica se o credor já foi cadastrado com o mesmo nome
                if (db.Credores.Where(info => info.nome == value.nome && info.CondominioID == value.CondominioID && info.credorId != value.credorId).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Campo Nome do Credor já existe";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

            #region Valida CPF/CNPJ
            if (value.cpf_cnpj != null)
            {
                // CPF
                if (value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Length == 11)
                {
                    if (!Funcoes.ValidaCpf(value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "")))
                    {
                        value.mensagem.Code = 29;
                        value.mensagem.Message = MensagemPadrao.Message(29).ToString();
                        value.mensagem.MessageBase = "Número de CPF incorreto.";
                        return value.mensagem;
                    }
                } // CNPJ
                else if (!Funcoes.ValidaCnpj(value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "")))
                {
                    value.mensagem.Code = 30;
                    value.mensagem.Message = MensagemPadrao.Message(30).ToString();
                    value.mensagem.MessageBase = "Número de CNPJ incorreto.";
                    return value.mensagem;
                }
                if (operation == Crud.ALTERAR)
                {
                    if (db.Credores.Where(info => info.cpf_cnpj == value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "") && info.credorId != value.credorId && info.CondominioID == sessaoCorrente.empresaId).Count() > 0)
                    {
                        value.mensagem.Code = 31;
                        value.mensagem.Message = MensagemPadrao.Message(31).ToString();
                        value.mensagem.MessageBase = "CPF/CNPJ informado para o fornecedor já se encontra cadastrado para outro fornecedor.";
                        return value.mensagem;
                    }
                }
                else
                {
                    if (db.Credores.Where(info => info.cpf_cnpj == value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "") && info.CondominioID == sessaoCorrente.empresaId).Count() > 0)
                    {
                        value.mensagem.Code = 31;
                        value.mensagem.Message = MensagemPadrao.Message(31).ToString();
                        value.mensagem.MessageBase = "CPF/CNPJ informado para o fornecedor já se encontra cadastrado para outro fornecedor.";
                        return value.mensagem;
                    }
                }
            }
            #endregion

            return value.mensagem;
        }

        public override CredorViewModel CreateRepository(System.Web.HttpRequestBase Request = null)
        {

            CredorViewModel c = base.CreateRepository(Request);
            c.dt_inclusao = Funcoes.Brasilia();
            c.ind_tipo_pessoa = "PJ";
            return c;
        }
        #endregion
    }

    public class ListViewCredor : ListViewRepository<CredorViewModel, ApplicationContext>
    {
        #region Métodos da classe ListViewRepository
        public override IEnumerable<CredorViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? param[0].ToString() : null;
            return (from cre in db.Credores
                    join gru in db.GrupoCredores on cre.grupoCredorId equals gru.grupoCredorId into GRU
                    from gru in GRU.DefaultIfEmpty()
                    where cre.CondominioID == sessaoCorrente.empresaId &&
                          (_nome == null || String.IsNullOrEmpty(_nome) || cre.nome.Contains(_nome.Trim()) || cre.cpf_cnpj == _nome || (gru != null && gru.nome.Contains(_nome.Trim())))
                    orderby cre.nome
                    select new CredorViewModel
                    {
                        credorId = cre.credorId,
                        CondominioID = cre.CondominioID,
                        cpf_cnpj = cre.cpf_cnpj,
                        nome = cre.nome,
                        nome_grupo = gru.nome,
                        fone1 = cre.fone1,
                        fone2 = cre.fone2,
                        email = cre.email,
                        endereco = cre.endereco,
                        complemento = cre.complemento,
                        PageSize = pageSize,
                        TotalCount = 0
                        //TotalCount = (from cre1 in db.Credores
                        //              join gru1 in db.GrupoCredores on cre1.grupoCredorId equals gru1.grupoCredorId into GRU1
                        //              from gru1 in GRU1.DefaultIfEmpty()
                        //              where cre1.CondominioID == sessaoCorrente.CondominioID &&
                        //                    (_nome == null || String.IsNullOrEmpty(_nome) || cre1.nome.Contains(_nome.Trim()) || cre1.cpf_cnpj == _nome || (gru1 != null && gru1.nome.Contains(_nome.Trim())))
                        //              select cre1.credorId).Count()
                    }).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new CredorModel().getObject((CredorViewModel)id);
        }
        #endregion
    }

    public class LookupCredorModel : ListViewCredor
    {
        public override string action()
        {
            return "../Fornecedores/ListCredorModal";
        }

        public override string DivId()
        {
            return "div-cre";
        }
    }

    public class LookupCredorFiltroModel : ListViewCredor
    {
        public override string action()
        {
            return "../Fornecedores/_ListCredorModal";
        }

        public override string DivId()
        {
            return "div-cre";
        }
    }

}