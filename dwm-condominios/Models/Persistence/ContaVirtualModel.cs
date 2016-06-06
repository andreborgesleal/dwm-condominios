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

namespace DWM.Models.Persistence
{
    public class ContaVirtualModel : CrudModel<ContaVirtual, ContaVirtualViewModel, ApplicationContext>
    {
        #region Constructor
        public ContaVirtualModel() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public ContaVirtualModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override ContaVirtual MapToEntity(ContaVirtualViewModel value)
        {
            ContaVirtual contaVirtual = Find(value);

            if (contaVirtual == null)
            {
                contaVirtual = new ContaVirtual();
                contaVirtual.DataAbertura = Funcoes.Brasilia();
            }

            contaVirtual.MembroID = value.MembroID;
            contaVirtual.TipoContaID = value.TipoContaID;
            contaVirtual.DataEncerramento = value.DataEncerramento;
            contaVirtual.ValorSaldo = value.ValorSaldo;
            contaVirtual.Membro = new Membro()
            {
                MembroID = value.MembroID,
                Nome = value.MembroViewModel.Nome,
                Email = value.MembroViewModel.Email.ToLower(),
                CPF = value.MembroViewModel.CPF.Replace(".", "").Replace("-", ""),
                Telefone = value.MembroViewModel.Telefone.Replace("(", "").Replace(")", "").Replace("-", ""),
                IndSituacao = value.MembroViewModel.IndSituacao
            };

            return contaVirtual;
        }

        public override ContaVirtualViewModel MapToRepository(ContaVirtual entity)
        {
            ContaVirtualViewModel contaVirtualViewModel = new ContaVirtualViewModel()
            {
                ContaVirtualID = entity.ContaVirtualID,
                MembroID = entity.MembroID,
                TipoContaID = entity.TipoContaID,
                Nome = db.Membros.Find(entity.MembroID).Nome,
                CPF = db.Membros.Find(entity.MembroID).CPF,
                Telefone = db.Membros.Find(entity.MembroID).Telefone,
                Email = db.Membros.Find(entity.MembroID).Email,
                DataAbertura = entity.DataAbertura,
                DataEncerramento = entity.DataEncerramento,
                ValorSaldo = entity.ValorSaldo,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };

            contaVirtualViewModel.MembroViewModel = new MembroViewModel()
            {
                MembroID = entity.MembroID,
                Nome = entity.Membro.Nome,
                Email = entity.Membro.Email,
                CPF = entity.Membro.CPF,
                Telefone = entity.Membro.Telefone,
                IndSituacao = entity.Membro.IndSituacao,
                Banco = entity.Membro.Banco,
                Agencia = entity.Membro.Agencia,
                Conta = entity.Membro.Conta,
                Avatar = entity.Membro.Avatar
            };

            return contaVirtualViewModel;
        }

        public override ContaVirtual Find(ContaVirtualViewModel key)
        {
            return db.ContaVirtuals.Find(key.ContaVirtualID);
        }

        public override Validate Validate(ContaVirtualViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation == Crud.EXCLUIR || operation == Crud.ALTERAR)
            {
                if (value.ContaVirtualID <= 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "ID Conta").ToString();
                    value.mensagem.MessageBase = "ID da Conta deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.MembroID <= 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "ID Apostador").ToString();
                    value.mensagem.MessageBase = "ID do apostador deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            if (value.TipoContaID <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID Tipo Conta").ToString();
                value.mensagem.MessageBase = "Tipo de Conta deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            MembroModel membroModel = new MembroModel(this.db, this.seguranca_db);
            value.MembroViewModel.mensagem = membroModel.Validate(value.MembroViewModel, operation);
            if (value.MembroViewModel.mensagem.Code > 0)
                throw new App_DominioException(value.MembroViewModel.mensagem);

            return value.mensagem;
        }
        #endregion
    }

    public class ListViewContaVirtual : ListViewModel<ContaVirtualViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewContaVirtual() { }
        public ListViewContaVirtual(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ContaVirtualViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param[0] != null && param[0].ToString() != "" ? param[0].ToString() : null;

            IEnumerable<ContaVirtualViewModel> query = (from m in db.Membros join c in db.ContaVirtuals on m.MembroID equals c.MembroID
                                                  join t in db.TipoContas on c.TipoContaID equals t.TipoContaID
                                                  where (_nome == null || m.Nome.Contains(_nome) || m.CPF == _nome || m.Email == _nome)
                                                  select new ContaVirtualViewModel()
                                                  {
                                                      ContaVirtualID = c.ContaVirtualID,
                                                      TipoContaID = c.TipoContaID,
                                                      Descricao = t.Descricao,
                                                      MembroID = m.MembroID,
                                                      Nome = m.Nome,
                                                      Telefone = m.Telefone,
                                                      Email = m.Email,
                                                      CPF = m.CPF,
                                                      DataAbertura = c.DataAbertura,
                                                      DataEncerramento = c.DataEncerramento,
                                                      ValorSaldo = c.ValorSaldo
                                                  }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            return query;
        }

        public override string action()
        {
            return "../Home/ListContaVirtual";
        }

        public override string DivId()
        {
            return "div-conta-virtual";
        }

        public override Repository getRepository(Object id)
        {
            return new ContaVirtualModel().getObject((ContaVirtualViewModel)id);
            //return new ApostaModel().getObject((ApostaViewModel)id);
        }
        #endregion
    }
}