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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

        public override AluguelEspacoViewModel BeforeInsert(AluguelEspacoViewModel value)
        {
            value.DataRevogacao = null;
            value.DataCancelamento = null;
            value.DataAutorizacao = null;
            value.DataReserva = Funcoes.Brasilia();
            value.Valor = db.EspacoComums.Find(value.EspacoID).Valor;

            if (value.CondominioID > 0 && value.EdificacaoID > 0 && value.UnidadeID > 0)
            {
                value.CondominoID = db.CondominoUnidades.Where(info => info.CondominioID == value.CondominioID 
                                                                && info.EdificacaoID == value.EdificacaoID 
                                                                && info.UnidadeID == value.UnidadeID).FirstOrDefault().CondominoID;
                if (SessaoLocal.CredenciadoID.HasValue)
                    value.CredenciadoID = SessaoLocal.CredenciadoID;
            }

            return value;
        }

        public override AluguelEspacoViewModel BeforeUpdate(AluguelEspacoViewModel value)
        {
            string Situacao = value.Situacao;
            string uri = value.uri;
            AluguelEspacoModel model = new AluguelEspacoModel(db, seguranca_db);
            value = model.getObject(value);
            if (Situacao == "A")
                value.DataAutorizacao = Funcoes.Brasilia();
            else
                value.DataRevogacao = Funcoes.Brasilia();
            value.uri = uri;
            return value;
        }
        #endregion

        #region Métodos da classe CrudContext
        public override AluguelEspaco MapToEntity(AluguelEspacoViewModel value)
        {
            AluguelEspaco entity = Find(value);

            if (entity == null)
            {
                entity = new AluguelEspaco();
            }

            entity.AluguelID = value.AluguelID;
            entity.CondominioID = value.CondominioID;
            entity.EdificacaoID = value.EdificacaoID;
            entity.UnidadeID = value.UnidadeID;
            entity.CondominoID = value.CondominoID;
            entity.CredenciadoID = value.CredenciadoID;
            entity.DataAutorizacao = value.DataAutorizacao;
            entity.DataEvento = value.DataEvento;
            entity.DataReserva = value.DataReserva;
            entity.DataRevogacao = value.DataRevogacao;
            entity.DataCancelamento = value.DataCancelamento;
            entity.EspacoID = value.EspacoID;
            entity.Observacao = value.Observacao;
            entity.Valor = value.Valor;

            return entity;
        }

        public override AluguelEspacoViewModel MapToRepository(AluguelEspaco entity)
        {
            AluguelEspacoViewModel a = new AluguelEspacoViewModel()
            {
                empresaId = entity.CondominioID,
                CondominioID = entity.CondominioID,
                AluguelID = entity.AluguelID,
                CondominoID = entity.CondominoID,
                CredenciadoID = entity.CredenciadoID,
                DataAutorizacao = entity.DataAutorizacao,
                DataEvento = entity.DataEvento.Date,
                DataReserva = entity.DataReserva.Date,
                DataRevogacao = entity.DataRevogacao,
                DataCancelamento = entity.DataCancelamento,
                EdificacaoID = entity.EdificacaoID,
                DescricaoEdificacao = db.Edificacaos.Find(entity.EdificacaoID).Descricao,
                NomeCondomino = db.CondominoPFs.Find(entity.CondominoID).Nome,
                NomeCredenciado = entity.CredenciadoID.HasValue ? db.Credenciados.Find(entity.CredenciadoID).Nome : "",
                EspacoID = entity.EspacoID,
                DescricaoEspaco = db.EspacoComums.Find(entity.EspacoID).Descricao,
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

            if (value.CondominioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Identificador do Condomínio").ToString();
                value.mensagem.MessageBase = "Identificador do condomínio deve ser informado.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.EspacoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Identificador do Espaço Condominial").ToString();
                value.mensagem.MessageBase = "Identificador do espaço condominial deve ser informado.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.EdificacaoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Torre/Bloco").ToString();
                value.mensagem.MessageBase = "Torre/Bloco deve ser informado.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.UnidadeID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Unidade").ToString();
                value.mensagem.MessageBase = "Unidade responsável pela reserva deve ser informada.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            if (value.CondominoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condômino").ToString();
                value.mensagem.MessageBase = "Condômino deve ser informado.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            if (value.DataEvento < Funcoes.Brasilia().Date)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Data do evento").ToString();
                value.mensagem.MessageBase = "Data do evento deve ser informada e deve ser maior ou igual que a data atual";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Verifica se a unidade já não reservou algum espaço no respectivo mês
            if (operation == Crud.INCLUIR)
            {
                string mmaaaa = value.DataEvento.ToString("yyyy/MM");
                if ((from x in db.AluguelEspacos
                     where x.CondominioID == value.CondominioID
                             && x.EspacoID == value.EspacoID
                             && x.EdificacaoID == value.EdificacaoID
                             && x.UnidadeID == value.UnidadeID
                             && System.Data.Entity.DbFunctions.DiffMonths(x.DataEvento, value.DataEvento) == 0
                     select x.AluguelID).Count() > 0)
                {
                    value.mensagem.Code = 41;
                    value.mensagem.Message = MensagemPadrao.Message(41, "Reserva já existente no mês").ToString();
                    value.mensagem.MessageBase = "Já existe uma reserva para esta unidade no mês informado.";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            #endregion

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

            int? _EspacoID = null;

            if (param != null && param.Count() > 0)
                _EspacoID = (int)param[0];

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
                            && ae.DataCancelamento == null
                            && (!_EspacoID.HasValue || ae.EspacoID == _EspacoID)
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
                         DataRevogacao = ae.DataRevogacao,
                         DataCancelamento = ae.DataCancelamento,
                         DescricaoEdificacao = ed.Descricao,
                         UnidadeID = ae.UnidadeID,
                         DescricaoEspaco = ec.Descricao,
                         LimitePessoas = ec.LimitePessoas,
                         NomeCondomino = con.Nome,
                         NomeCredenciado = cred.Nome,
                         calendar = new Repositories.Calendar()
                         {
                             title = ed.Descricao + "-" + ae.UnidadeID,
                             start = System.Data.Entity.DbFunctions.CreateDateTime(ae.DataReserva.Year, ae.DataReserva.Month, ae.DataReserva.Day, 0, 0, 0).Value,
                             allDay = true
                         }
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
                            && ae.DataCancelamento == null
                            && (!_EspacoID.HasValue || ae.EspacoID == _EspacoID)
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
                         DataRevogacao = ae.DataRevogacao,
                         DataCancelamento = ae.DataCancelamento,
                         DescricaoEdificacao = ed.Descricao,
                         UnidadeID = ae.UnidadeID,
                         DescricaoEspaco = ec.Descricao,
                         LimitePessoas = ec.LimitePessoas,
                         NomeCondomino = con.Nome,
                         NomeCredenciado = cred.Nome,
                         calendar = new Repositories.Calendar() { title = ed.Descricao + "-" + ae.UnidadeID,
                                                                  start = System.Data.Entity.DbFunctions.CreateDateTime(ae.DataReserva.Year, ae.DataReserva.Month, ae.DataReserva.Day, 0,0,0).Value,
                                                                  end = System.Data.Entity.DbFunctions.CreateDateTime(ae.DataReserva.Year, ae.DataReserva.Month, ae.DataReserva.Day, 0, 0, 0),
                                                                  allDay = true}
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