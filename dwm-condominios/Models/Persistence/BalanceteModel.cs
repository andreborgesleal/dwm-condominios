using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using App_Dominio.Enumeracoes;
using System.Web;
using App_Dominio.Security;

namespace DWM.Models.Persistence
{
    public class BalanceteModel : CrudModelLocal<Balancete, BalanceteViewModel>
    {
        #region Constructor
        public BalanceteModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override BalanceteViewModel BeforeInsert(BalanceteViewModel value)
        {
            value.empresaId = sessaoCorrente.empresaId;
            value.CondominioID = sessaoCorrente.empresaId;
            return base.BeforeInsert(value);
        }

        public override Balancete MapToEntity(BalanceteViewModel value)
        {
            Balancete entity = Find(value);

            if (entity == null)
                entity = new Balancete();

            entity.CondominioID = value.CondominioID;
            entity.planoContaID = value.planoContaID;
            entity.descricao = value.descricao;
            entity.Natureza = value.Natureza;

            return entity;
        }

        public override BalanceteViewModel MapToRepository(Balancete entity)
        {
            BalanceteViewModel value = new BalanceteViewModel()
            {
                CondominioID = entity.CondominioID,
                planoContaID = entity.planoContaID,
                descricao = entity.descricao,
                Natureza = entity.Natureza,
                SaldosContabeis = (from sal in db.SaldosContabeis
                                   where sal.CondominioID == entity.CondominioID
                                            && sal.planoContaID == entity.planoContaID
                                   orderby sal.Competencia descending
                                   select new SaldoContabilViewModel()
                                   {
                                       CondominioID = sal.CondominioID,
                                       planoContaID = sal.planoContaID,
                                       Competencia = sal.Competencia,
                                       ValorSaldo = sal.ValorSaldo,
                                       mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
                                   }).Take(7),
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };

            value.SaldoContabil = value.SaldosContabeis.FirstOrDefault();

            return value;
        }

        public override Balancete Find(BalanceteViewModel key)
        {
            return db.Balancetes.Find(key.CondominioID, key.planoContaID);
        }

        public override Validate Validate(BalanceteViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.CondominioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Condomínio deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.planoContaID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Conta Contábil ID").ToString();
                value.mensagem.MessageBase = "Identificador da Conta Contábil deve ser informado";
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

                if (value.descricao == null || value.descricao.Trim().Length <= 3)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Descrição da conta").ToString();
                    value.mensagem.MessageBase = "Descrição da conta contábil deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Natureza == null || value.Natureza == "")
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Natureza da Conta deve ser informada").ToString();
                    value.mensagem.MessageBase = "Natureza da conta deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        public override BalanceteViewModel CreateRepository(HttpRequestBase Request = null)
        {
            BalanceteViewModel value = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            value.CondominioID = security.getSessaoCorrente().empresaId;
            value.empresaId = security.getSessaoCorrente().empresaId;
            value.Natureza = "D";
            return value;
        }
        #endregion
    }

    public class ListViewBalanceteMensal : ListViewModelLocal<BalanceteViewModel>
    {
        #region Constructor
        public ListViewBalanceteMensal() { }

        public ListViewBalanceteMensal(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<BalanceteViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _descricao = param != null && param.Count() > 0 && param[0] != null ? (string)param[0] : null;

            return (from bal in db.Balancetes
                    where bal.CondominioID == sessaoCorrente.empresaId
                            && (_descricao == null || bal.descricao == _descricao)
                    orderby bal.Natureza, bal.descricao
                    select new BalanceteViewModel
                    {
                        empresaId = bal.CondominioID,
                        CondominioID = bal.CondominioID,
                        planoContaID = bal.planoContaID,
                        descricao = bal.descricao,
                        Natureza = bal.Natureza,
                        PageSize = pageSize,
                        TotalCount = ((from bal1 in db.Balancetes
                                       where bal1.CondominioID == sessaoCorrente.empresaId
                                               && (_descricao == null || bal1.descricao == _descricao)
                                       orderby bal1.Natureza, bal1.descricao
                                       select bal1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new BalanceteModel().getObject((BalanceteViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-bal";
        }
    }
}