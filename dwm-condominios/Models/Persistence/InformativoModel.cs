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

namespace DWM.Models.Persistence
{
    public class InformativoModel : CrudModel<Informativo, InformativoViewModel, ApplicationContext>
    {
        #region Constructor
        public InformativoModel() { }
        public InformativoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override Informativo MapToEntity(InformativoViewModel value)
        {
            return new Informativo()
            {
                InformativoID = value.InformativoID,
                DataInformativo = Funcoes.Brasilia(),
                DataPublicacao = value.DataPublicacao,
                DataExpiracao = value.DataExpiracao,
                Cabecalho = value.Cabecalho,
                Resumo = value.Resumo,
                MensagemDetalhada = value.MensagemDetalhada
            };
        }

        public override InformativoViewModel MapToRepository(Informativo entity)
        {
            return new InformativoViewModel()
            {
                InformativoID = entity.InformativoID,
                DataInformativo = entity.DataInformativo,
                DataPublicacao = entity.DataPublicacao,
                DataExpiracao = entity.DataExpiracao,
                Cabecalho = entity.Cabecalho,
                Resumo = entity.Resumo,
                MensagemDetalhada = entity.MensagemDetalhada,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Informativo Find(InformativoViewModel key)
        {
            return db.Informativos.Find(key.InformativoID);
        }

        public override Validate Validate(InformativoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.Cabecalho.Trim().Length == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Cabeçalho").ToString();
                value.mensagem.MessageBase = "Cabeçalho deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.Cabecalho.Trim().Length == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Resumo").ToString();
                value.mensagem.MessageBase = "Resumo deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.DataExpiracao != null && value.DataExpiracao < value.DataPublicacao)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "DataExpiracao").ToString();
                value.mensagem.MessageBase = "A Data de Expiração deve ser após a Data de Publicação";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            return value.mensagem;
        }

        #endregion
    }

    public class ListViewInformativo : ListViewModel<InformativoViewModel, ApplicationContext>
    {
        public ListViewInformativo(ApplicationContext _db, SecurityContext _seguranca_db) : base(_db, _seguranca_db)
        {
        }
        #region Métodos da classe ListViewRepository
        public override IEnumerable<InformativoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? param[0].ToString() : null;
            return (from c in db.Informativos
                    where (_nome == null || String.IsNullOrEmpty(_nome) || c.Cabecalho.StartsWith(_nome.Trim()))
                    orderby c.DataInformativo
                    select new InformativoViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        InformativoID = c.InformativoID,
                        Cabecalho = c.Cabecalho,
                        DataExpiracao = c.DataExpiracao,
                        DataInformativo = c.DataInformativo,
                        DataPublicacao = c.DataPublicacao,
                        MensagemDetalhada = c.MensagemDetalhada,
                        PageSize = pageSize,
                        TotalCount = (from c1 in db.Informativos
                                      where (_nome == null || String.IsNullOrEmpty(_nome) || c1.Cabecalho.StartsWith(_nome.Trim()))
                                      select c1).Count()
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