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
using System.Web;
using App_Dominio.Security;
using dwm_condominios.Models.Persistence;
using App_Dominio.Repositories;
using System.Data;
using System.Globalization;

namespace DWM.Models.Persistence
{
    public class AluguelEspacoModel : CrudModelLocal<AluguelEspaco, AluguelEspacoViewModel>
    {
        #region Constructor
        public AluguelEspacoModel()
        {
        }

        public AluguelEspacoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override AluguelEspaco MapToEntity(AluguelEspacoViewModel value)
        {
            AluguelEspaco entity = Find(value);

            if (entity == null)
                entity = new AluguelEspaco();

            entity.CondominioID = value.CondominioID;
            entity.AluguelID = value.AluguelID;
            entity.CondominoID = value.CondominoID;
            entity.CredenciadoID = value.CredenciadoID;
            //entity.DataAutorizacao = value.DataAutorizacao;
            entity.DataEvento = DateTime.Now.Date;
            entity.DataReserva = DateTime.Now.Date;
            entity.EdificacaoID = value.EdificacaoID;
            entity.EspacoID = value.EspacoID;
            entity.Observacao = value.Observacao;
            entity.UnidadeID = value.UnidadeID;
            entity.Valor = (from ec in db.EspacoComums where ec.EspacoID == value.EspacoID select ec.Valor).FirstOrDefault();

            entity.CondominoUnidade = new CondominoUnidade()
            {
                CondominioID = entity.CondominioID,
                CondominoID = entity.CondominoID,
                EdificacaoID = entity.EdificacaoID,
                UnidadeID = entity.UnidadeID,
            };
            
            return entity;
        }

        public override AluguelEspacoViewModel MapToRepository(AluguelEspaco entity)
        {
            AluguelEspacoViewModel a = new AluguelEspacoViewModel()
            {
                CondominioID = entity.CondominioID,
                AluguelID = entity.AluguelID,
                CondominoID = entity.CondominoID,
                CredenciadoID = entity.CredenciadoID,
                DataAutorizacao = entity.DataAutorizacao,
                DataEvento = entity.DataEvento.Date,
                DataReserva = entity.DataReserva.Date,
                EdificacaoID = entity.EdificacaoID,
                EspacoID = entity.EspacoID,
                Observacao = entity.Observacao,
                UnidadeID = entity.UnidadeID,
                Valor = entity.Valor,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };

            return a;
        }

        public override AluguelEspaco Find(AluguelEspacoViewModel key)
        {
            return db.AluguelEspacos.Find(key.AluguelID);
        }

        public override Validate Validate(AluguelEspacoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.mensagem.Code != 0)
                return value.mensagem;

            if (value.empresaId == 0)
            {
                value.mensagem.Code = 35;
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "Sua sessão expirou. Faça um novo login no sistema";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            return value.mensagem;
        }

        public override AluguelEspacoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            AluguelEspacoViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }
        #endregion
    }

    public class ListViewAluguelEspaco : ListViewModelLocal<AluguelEspacoViewModel>
    {
        #region Constructor
        public ListViewAluguelEspaco() { }
        public ListViewAluguelEspaco(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<AluguelEspacoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;
            DateTime dataHoje = Funcoes.Brasilia();

            var q = new List<AluguelEspacoViewModel>();

            if (SessaoLocal.CondominoID > 0 || SessaoLocal.CredenciadoID > 0)
            {
                q = (from ae in db.AluguelEspacos
                     join ed in db.Edificacaos on ae.EdificacaoID equals ed.EdificacaoID
                     join ec in db.EspacoComums on ae.EspacoID equals ec.EspacoID
                     join con in db.Condominos on ae.CondominoID equals con.CondominoID
                     join cred in db.Credenciados on ae.CredenciadoID equals cred.CredenciadoID into credleft
                     from cred in credleft.DefaultIfEmpty()
                     where ae.CondominoID == SessaoLocal.CondominoID || ae.CredenciadoID == SessaoLocal.CredenciadoID
                     orderby ae.DataReserva
                     select new AluguelEspacoViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         CondominioID = ae.CondominioID,
                         DataEvento = ae.DataEvento,
                         AluguelID = ae.AluguelID,
                         CredenciadoID = ae.CredenciadoID,
                         Valor = ae.Valor,
                         CondominoID = ae.CondominoID,
                         DataAutorizacao = ae.DataAutorizacao,
                         DataReserva = ae.DataReserva,
                         EdificacaoID = ae.EdificacaoID,
                         EspacoID = ae.EspacoID,
                         Observacao = ae.Observacao,

                         DescricaoEdificacao = ed.Descricao,
                         UnidadeID = ae.UnidadeID,
                         DescricaoEspaco = ec.Descricao,
                         LimitePessoas = ec.LimitePessoas,
                         NomeCondomino = con.Nome,
                         NomeCredenciado = cred.Nome,
                     }).ToList();
            }
            else
            {
                q = (from ae in db.AluguelEspacos
                     join ed in db.Edificacaos on ae.EdificacaoID equals ed.EdificacaoID
                     join ec in db.EspacoComums on ae.EspacoID equals ec.EspacoID
                     join con in db.Condominos on ae.CondominoID equals con.CondominoID
                     join cred in db.Credenciados on ae.CredenciadoID equals cred.CredenciadoID into credleft
                     from cred in credleft.DefaultIfEmpty()
                     orderby ae.DataReserva
                     where System.Data.Entity.DbFunctions.TruncateTime(ae.DataEvento) >= dataHoje.Date
                     select new AluguelEspacoViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         CondominioID = ae.CondominioID,
                         DataEvento = ae.DataEvento,
                         AluguelID = ae.AluguelID,
                         CredenciadoID = ae.CredenciadoID,
                         Valor = ae.Valor,
                         CondominoID = ae.CondominoID,
                         DataAutorizacao = ae.DataAutorizacao,
                         DataReserva = ae.DataReserva,
                         EdificacaoID = ae.EdificacaoID,
                         EspacoID = ae.EspacoID,
                         Observacao = ae.Observacao,

                         DescricaoEdificacao = ed.Descricao,
                         UnidadeID = ae.UnidadeID,
                         DescricaoEspaco = ec.Descricao,
                         LimitePessoas = ec.LimitePessoas,
                         NomeCondomino = con.Nome,
                         NomeCredenciado = cred.Nome,
                     }).ToList();
            }
            
            return q;
        }

        public override Repository getRepository(Object id)
        {
            return new FuncionarioModel().getObject((FuncionarioViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-AluguelEspacos";
        }
    }
}