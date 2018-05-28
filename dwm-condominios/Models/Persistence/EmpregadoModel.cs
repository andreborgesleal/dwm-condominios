using App_Dominio.Component;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dwm_condominios.Models.Persistence
{
    public class EmpregadoModel : CrudModelLocal<Empregado, EmpregadoViewModel>
    {
        #region Constructor
        public EmpregadoModel()
        {
        }

        public EmpregadoModel(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override Empregado MapToEntity(EmpregadoViewModel value)
        {
            Empregado entity = Find(value);

            if (entity == null)
                entity = new Empregado();

            entity.CondominioID = value.CondominioID;
            entity.CargoID = value.CargoID;
            entity.EmpregadoID = value.EmpregadoID;
            entity.Nome = value.Nome;
            entity.RG = value.RG;
            entity.OrgaoEmissor = value.OrgaoEmissor;
            entity.CPF = value.CPF?.Replace(".", "").Replace("-", "").Replace("/", "");
            entity.ComplementoEnd = value.ComplementoEnd;
            entity.Telefone = value.Telefone?.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            entity.Email = value.Email;
            entity.Matricula = value.Matricula;
            entity.CEP = value.CEP;
            entity.Cidade = value.Cidade;
            entity.UF = value.UF;
            entity.DataAdmissao = value.DataAdmissao;
            entity.DataDesativacao = value.DataDesativacao;
            entity.Dia = value.Dia;
            entity.HoraFinal = value.HoraFinal;
            entity.HoraInicial = value.HoraInicial;

            return entity;
        }

        public override EmpregadoViewModel MapToRepository(Empregado entity)
        {
            EmpregadoViewModel v = new EmpregadoViewModel()
            {
                CondominioID = entity.CondominioID,
                CargoID = entity.CargoID,
                EmpregadoID = entity.EmpregadoID,
                Nome = entity.Nome,
                RG = entity.RG,
                OrgaoEmissor = entity.OrgaoEmissor,
                CPF = entity.CPF,
                ComplementoEnd = entity.ComplementoEnd,
                Telefone = entity.Telefone,
                Email = entity.Email,
                Matricula = entity.Matricula,
                CEP = entity.CEP,
                Cidade = entity.Cidade,
                UF = entity.UF,
                DataAdmissao = entity.DataAdmissao,
                DataDesativacao = entity.DataDesativacao,
                Dia = entity.Dia,
                HoraFinal = entity.HoraFinal,
                HoraInicial = entity.HoraInicial,
                sessionId = SessaoLocal.sessaoId,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };

            return v;
        }

        public override Empregado Find(EmpregadoViewModel key)
        {
            return db.Empregados.Find(key.EmpregadoID);
        }

        public override Validate Validate(EmpregadoViewModel value, Crud operation)
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


            if (!string.IsNullOrEmpty(value.CPF))
            {
                if (!Funcoes.ValidaCpf(value.CPF.Replace(".", "").Replace(".", "").Replace("-", "")))
                {
                    value.mensagem.Code = 4;
                    value.mensagem.Message = MensagemPadrao.Message(4, "Condomínio").ToString();
                    value.mensagem.MessageBase = "O CPF Digitado é inválido";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            if (operation == Crud.ALTERAR)
            {
                if (value.EmpregadoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Empregado").ToString();
                    value.mensagem.MessageBase = "Empregado deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Nome.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Empregado").ToString();
                    value.mensagem.MessageBase = "Nome do Condômino deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            if (operation == Crud.EXCLUIR)
            {
                if (value.EmpregadoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Empregado").ToString();
                    value.mensagem.MessageBase = "Empregado deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            return value.mensagem;
        }

        public override EmpregadoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            EmpregadoViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }
        #endregion
    }

    public class ListViewEmpregado : ListViewModelLocal<EmpregadoViewModel>
    {
        #region Constructor
        public ListViewEmpregado() { }
        public ListViewEmpregado(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EmpregadoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            var q = (from value in db.Empregados
                     orderby value.Nome
                     select new EmpregadoViewModel
                     {
                        empresaId = sessaoCorrente.empresaId,
                        CondominioID = value.CondominioID,
                        CargoID = value.CargoID,
                        EmpregadoID = value.EmpregadoID,
                        Nome = value.Nome,
                        RG = value.RG,
                        OrgaoEmissor = value.OrgaoEmissor,
                        CPF = value.CPF,
                        ComplementoEnd = value.ComplementoEnd,
                        Telefone = value.Telefone,
                        Email = value.Email,
                        Matricula = value.Matricula,
                        CEP = value.CEP,
                        Cidade = value.Cidade,
                        UF = value.UF,
                        DataAdmissao = value.DataAdmissao,
                        DataDesativacao = value.DataDesativacao,
                        Dia = value.Dia,
                        HoraFinal = value.HoraFinal,
                        HoraInicial = value.HoraInicial,
                        sessionId = sessaoCorrente.sessaoId,
                     }).ToList();

            return q;
        }

        public override Repository getRepository(Object id)
        {
            return new EmpregadoModel().getObject((EmpregadoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-visitantes";
        }
    }
}