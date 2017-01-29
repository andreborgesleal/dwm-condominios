using App_Dominio.Entidades;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App_Dominio.Contratos;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using App_Dominio.Component;
using System.IO;
using App_Dominio.Security;
using System.Data.Entity.SqlServer;

namespace DWM.Models.Persistence
{
    public class EmailLogModel : CrudModelLocal<EmailLog, EmailLogViewModel>
    {
        #region Constructor
        public EmailLogModel() { }
        public EmailLogModel(ApplicationContext _db, SecurityContext _seguranca_db, bool anonymous = false)
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
        public override EmailLog MapToEntity(EmailLogViewModel value)
        {
            EmailLog entity = Find(value);

            if (entity == null)
                entity = new EmailLog();

            entity.EmailLogID = value.EmailLogID;
            entity.EmailTipoID = value.EmailTipoID;
            entity.CondominioID = value.CondominioID;
            entity.EdificacaoID = value.EdificacaoID;
            entity.UnidadeID = value.UnidadeID;
            entity.GrupoCondominoID = value.GrupoCondominoID;
            entity.DataEmail = Funcoes.Brasilia();
            entity.Assunto = value.Assunto;
            entity.EmailMensagem = value.EmailMensagem;
            return entity;
        }

        public override EmailLogViewModel MapToRepository(EmailLog entity)
        {
            return new EmailLogViewModel()
            {
                empresaId = entity.CondominioID,
                EmailLogID = entity.EmailLogID,
                EmailTipoID = entity.EmailTipoID,
                CondominioID = entity.CondominioID,
                EdificacaoID = entity.EdificacaoID,
                UnidadeID = entity.UnidadeID,
                GrupoCondominoID = entity.GrupoCondominoID,
                DataEmail = entity.DataEmail,
                Assunto = entity.Assunto,
                EmailMensagem = entity.EmailMensagem,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override EmailLog Find(EmailLogViewModel key)
        {
            return db.EmailLogs.Find(key.EmailLogID);
        }

        public override Validate Validate(EmailLogViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.EmailLogID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID").ToString();
                value.mensagem.MessageBase = "Código identificador do LOG de auditoria do E-mail deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (value.CondominioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Código identificador do condomínio deve ser informado";
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

                if (value.EmailTipoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Tipo do E-mail").ToString();
                    value.mensagem.MessageBase = "Tipo do e-mail deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Assunto == null || value.Assunto.Trim().Length < 5)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Assunto").ToString();
                    value.mensagem.MessageBase = "Assunto deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.EmailMensagem == null || value.EmailMensagem.Trim().Length <= 20)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "EmailMensagem").ToString();
                    value.mensagem.MessageBase = "Mensagem do e-mail deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        public override EmailLogViewModel CreateRepository(HttpRequestBase Request = null)
        {
            EmailLogViewModel log = base.CreateRepository(Request);
            log.CondominioID = SessaoLocal.empresaId;
            log.empresaId = SessaoLocal.empresaId;
            return log;
        }
        #endregion
    }

    public class ListViewEmailLog : ListViewModelLocal<EmailLogViewModel>
    {
        public ListViewEmailLog()
        {
        }

        public ListViewEmailLog(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EmailLogViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            DateTime data1 = param.Count() > 0 && param[0] != null ? (DateTime)param[0] : new DateTime(1980, 1, 1);
            DateTime data2 = param.Count() > 1 && param[1] != null ? (DateTime)param[1] : Funcoes.Brasilia().Date.AddDays(30);
            string EdificacaoID = "";
            string GrupoCondominoID = "";
            string IsHome = "N";

            if (param.Count() > 2)
            {
                IsHome = "S";
                if (param[2] != null)
                {
                    int[] GrupoCondomino = (int[])param[2];
                    for (int i = 0; i <= GrupoCondomino.Count() - 1; i++)
                    {
                        GrupoCondominoID += GrupoCondominoID[i].ToString() + ";";
                    }
                }

                if (param[3] != null)
                {
                    IEnumerable<Unidade> Unidades = (IEnumerable<Unidade>)param[3];
                    foreach (var unidade in Unidades)
                    {
                        EdificacaoID += unidade.EdificacaoID + ";";
                    }
                }
            }

            return (from info in db.EmailLogs
                    join gru in db.GrupoCondominos on info.GrupoCondominoID equals gru.GrupoCondominoID into GRU
                    from gru in GRU.DefaultIfEmpty()
                    join edi in db.Edificacaos on info.EdificacaoID equals edi.EdificacaoID into EDI
                    from edi in EDI.DefaultIfEmpty()
                    join eti in db.EmailTipos on info.EmailTipoID equals eti.EmailTipoID
                    where info.DataEmail >= data1 && info.DataEmail <= data2
                            && info.CondominioID == SessaoLocal.empresaId
                            && (IsHome == "N" || info.DataEmail >= SqlFunctions.GetDate())
                            && (GrupoCondominoID == "" || GrupoCondominoID.Contains(info.GrupoCondominoID.ToString()))
                            && (EdificacaoID == "" || EdificacaoID.Contains(info.EdificacaoID.ToString()))
                    orderby info.DataEmail descending
                    select new EmailLogViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        EmailLogID = info.EmailLogID,
                        CondominioID = info.CondominioID,
                        EdificacaoID = info.EdificacaoID,
                        Descricao_Edificacao = edi.Descricao,
                        GrupoCondominoID = gru.GrupoCondominoID,
                        Descricao_GrupoCondomino = gru.Descricao,
                        DataEmail = info.DataEmail,
                        Assunto = info.Assunto,
                        EmailMensagem = info.EmailMensagem,
                        EmailTipoID = info.EmailTipoID,
                        UnidadeID = info.UnidadeID,
                        sessionId = sessaoCorrente.sessaoId,
                        Descricao_EmailTipo = eti.Descricao,
                        
                        PageSize = pageSize,
                        TotalCount = (from info1 in db.Informativos
                                      join gru1 in db.GrupoCondominos on info1.GrupoCondominoID equals gru1.GrupoCondominoID into GRU1
                                      from gru1 in GRU1.DefaultIfEmpty()
                                      join edi1 in db.Edificacaos on info1.EdificacaoID equals edi1.EdificacaoID into EDI1
                                      from edi1 in EDI.DefaultIfEmpty()
                                      where info1.DataPublicacao >= data1 && info1.DataPublicacao <= data2
                                              && info1.CondominioID == sessaoCorrente.empresaId
                                      orderby info1.DataPublicacao descending
                                      select info1).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override string action()
        {
            return "../Home/ListInformativo";
        }

        public override string DivId()
        {
            return "div-Informativo";
        }

        public override Repository getRepository(Object id)
        {
            return new InformativoModel().getObject((InformativoViewModel)id);
        }
        #endregion
    }

}
