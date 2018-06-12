using App_Dominio.Component;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Security;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DWM.Models.Persistence
{
    public class LimpezaInspecaoModel : CrudModelLocal<LimpezaInspecao, LimpezaInspecaoViewModel>
    {
        #region Constructor
        public LimpezaInspecaoModel()
        {
        }

        public LimpezaInspecaoModel(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe CrudContext

        public override LimpezaInspecaoViewModel BeforeInsert(LimpezaInspecaoViewModel value)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            value.LimpezaInspecaoItem = (from x in db.LimpezaRequisitos
                                                                 where x.CondominioID == sessaoCorrente.empresaId
                                                                 && x.EspacoID == value.EspacoID
                                                                 select new LimpezaInspecaoItemViewModel()
                                                                 {
                                                                     empresaId = x.CondominioID,
                                                                     LimpezaRequisitoID = x.LimpezaRequisitoID,
                                                                     LimpezaInspecaoID = value.LimpezaInspecaoID,
                                                                 }).ToList();
            return base.BeforeInsert(value);
        }

        public override LimpezaInspecaoViewModel BeforeUpdate(LimpezaInspecaoViewModel value)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            return base.BeforeUpdate(value);
        }

        public override LimpezaInspecao MapToEntity(LimpezaInspecaoViewModel value)
        {

            LimpezaInspecao entity = Find(value);

            if (entity == null)
                entity = new LimpezaInspecao();

            entity.credorId = value.credorId;
            entity.EspacoID = value.EspacoID;
            entity.LimpezaInspecaoID = value.LimpezaInspecaoID;
            entity.CondominioID = value.CondominioID;
            entity.DataInspecao = value.DataInspecao;
            entity.NotaFinal = value.NotaFinal == 0 ? null : value.NotaFinal;
            entity.Laudo = value.Laudo;

            entity.LimpezaInspecaoItem = new List<LimpezaInspecaoItem>();
            foreach(var x in value.LimpezaInspecaoItem)
            {
                entity.LimpezaInspecaoItem.Add(new LimpezaInspecaoItem()
                {
                    Justificativa = x.Justificativa,
                    LimpezaInspecaoID = x.LimpezaInspecaoID,
                    LimpezaRequisitoID = x.LimpezaRequisitoID,
                    Nota = x.Nota
                });
            }
            

            return entity;
        }

        public override LimpezaInspecaoViewModel MapToRepository(LimpezaInspecao entity)
        {
            LimpezaInspecaoViewModel v = new LimpezaInspecaoViewModel()
            {
                credorId = entity.credorId,
                EspacoID = entity.EspacoID,
                LimpezaInspecaoID = entity.LimpezaInspecaoID,
                CondominioID = entity.CondominioID,
                DataInspecao = entity.DataInspecao,
                NotaFinal = entity.NotaFinal,
                Laudo = entity.Laudo,
                sessionId = SessaoLocal.sessaoId,
                empresaId = SessaoLocal.CondominoID,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };

            return v;
        }

        public override LimpezaInspecao Find(LimpezaInspecaoViewModel key)
        {
            return db.LimpezaInspecaos.Find(key.LimpezaInspecaoID);
        }

        public override Validate Validate(LimpezaInspecaoViewModel value, Crud operation)
        {
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

            if (value.EspacoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "A área deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.CondominioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "O Condomínio deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.DataInspecao.Date == DateTime.MinValue)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "A data de inspeção deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.ALTERAR || operation == Crud.EXCLUIR)
            {
                if (value.LimpezaInspecaoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "LimpezaInspecao").ToString();
                    value.mensagem.MessageBase = "Requisito de limpeza deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            return value.mensagem;
        }

        public override LimpezaInspecaoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            LimpezaInspecaoViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }
        #endregion
    }

    public class ListViewLimpezaInspecao : ListViewModelLocal<LimpezaInspecaoViewModel>
    {
        #region Constructor
        public ListViewLimpezaInspecao() { }
        public ListViewLimpezaInspecao(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<LimpezaInspecaoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            var q = (from value in db.LimpezaInspecaos
                     join esp in db.EspacoComums on value.EspacoID equals esp.EspacoID
                     orderby value.DataInspecao
                     select new LimpezaInspecaoViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         LimpezaInspecaoID = value.LimpezaInspecaoID,
                         CondominioID = value.CondominioID,
                         credorId = value.credorId,
                         FornecedorDescricao = (from z in db.Credores where z.credorId == value.credorId select z).FirstOrDefault().nome,
                         DataInspecao = value.DataInspecao,
                         EspacoID = value.EspacoID,
                         Laudo = value.Laudo,
                         NotaFinal = value.NotaFinal,
                         EspacoDescricao = esp.Descricao,
                         LimpezaInspecaoItem = (from x in db.LimpezaInspecaoItem where x.LimpezaInspecaoID == value.LimpezaInspecaoID
                                                select new LimpezaInspecaoItemViewModel
                                                {
                                                    ItemDescricao = (from y in db.LimpezaRequisitos where y.LimpezaRequisitoID == x.LimpezaRequisitoID select y).FirstOrDefault().Descricao,
                                                    Justificativa = x.Justificativa,
                                                    LimpezaInspecaoID = x.LimpezaInspecaoID,
                                                    Nota = x.Nota,
                                                    LimpezaRequisitoID = x.LimpezaRequisitoID,
                                                }).ToList(),
                         sessionId = sessaoCorrente.sessaoId,
                     }).ToList();

            return q;
        }

        public override Repository getRepository(Object id)
        {
            return new LimpezaInspecaoModel().getObject((LimpezaInspecaoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-visitantes";
        }
    }

    public class ListViewLimpezaInspecaoLaudo : ListViewModelLocal<LimpezaInspecaoViewModel>
    {
        #region Constructor
        public ListViewLimpezaInspecaoLaudo() { }
        public ListViewLimpezaInspecaoLaudo(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<LimpezaInspecaoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;

            List<LimpezaInspecaoViewModel> ListInspecoesViewModel = new List<LimpezaInspecaoViewModel>();
            int count = 0;
            int size = 0;

            foreach (var inspecao in db.LimpezaInspecaos)
            {
                size = 0;
                foreach(var inspecaoItem in db.LimpezaInspecaoItem)
                {
                    size++;
                    if (inspecaoItem.Nota >= 0)
                    {
                        count++;
                    }
                }
                if (count == size && inspecao.Laudo == null)
                {
                    ListInspecoesViewModel.Add(new LimpezaInspecaoViewModel()
                    {
                        sessionId = sessaoCorrente.sessaoId,
                        empresaId = sessaoCorrente.empresaId,
                        CondominioID = inspecao.CondominioID,
                        credorId = inspecao.credorId,
                        DataInspecao = inspecao.DataInspecao,
                        EspacoID = inspecao.EspacoID,
                        EspacoDescricao = (db.EspacoComums.Find(inspecao.EspacoID).Descricao),
                        Laudo = inspecao.Laudo,
                        LimpezaInspecaoID = inspecao.LimpezaInspecaoID,
                    });
                }

                count = 0;
            }

            return ListInspecoesViewModel;
        }

        public override Repository getRepository(Object id)
        {
            return new LimpezaInspecaoModel().getObject((LimpezaInspecaoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-visitantes";
        }
    }
}