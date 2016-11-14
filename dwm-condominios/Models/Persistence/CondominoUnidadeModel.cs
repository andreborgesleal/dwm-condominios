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

namespace DWM.Models.Persistence
{
    public class CondominoUnidadeModel : CrudModelLocal<CondominoUnidade, CondominoUnidadeViewModel>
    {
        #region Constructor
        public CondominoUnidadeModel() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public CondominoUnidadeModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override CondominoUnidade MapToEntity(CondominoUnidadeViewModel value)
        {
            CondominoUnidade condominoUnidade = Find(value);

            if (condominoUnidade == null)
                condominoUnidade = new CondominoUnidade();

            condominoUnidade.CondominioID = value.CondominioID;
            condominoUnidade.EdificacaoID = value.EdificacaoID;
            condominoUnidade.UnidadeID = value.UnidadeID;
            condominoUnidade.CondominoID = value.CondominoID;
            condominoUnidade.DataInicio = value.DataInicio;
            condominoUnidade.DataFim = value.DataFim;

            if (value.CondominoViewModel is CondominoPFViewModel)
            {
                CondominoPFModel model = new CondominoPFModel(this.db, this.seguranca_db);
                condominoUnidade.Condomino = model.MapToEntity((CondominoPFViewModel)value.CondominoViewModel);
            }

            return condominoUnidade;
        }

        public override CondominoUnidadeViewModel MapToRepository(CondominoUnidade entity)
        {
            CondominoUnidadeViewModel condominoUnidadeViewModel = new CondominoUnidadeViewModel()
            {
                CondominioID = entity.CondominioID,
                EdificacaoID = entity.EdificacaoID,
                UnidadeID = entity.UnidadeID,
                CondominoID = entity.CondominoID,
                DataInicio = entity.DataInicio,
                DataFim = entity.DataFim,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };

            if (entity.Condomino is CondominoPF)
            {
                CondominoPFModel model = new CondominoPFModel(this.db, this.seguranca_db);
                condominoUnidadeViewModel.CondominoViewModel = model.MapToRepository((CondominoPF)entity.Condomino);
            }

            return condominoUnidadeViewModel;
        }

        public override CondominoUnidade Find(CondominoUnidadeViewModel key)
        {
            return db.CondominoUnidades.Find(key.CondominioID, key.EdificacaoID, key.UnidadeID, key.CondominoID);
        }

        public override Validate Validate(CondominoUnidadeViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };
            if (value.CondominioID <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID Condominio").ToString();
                value.mensagem.MessageBase = "Identificador do condomínio deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.EdificacaoID <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID Edificação").ToString();
                value.mensagem.MessageBase = "Torre/Sala/Casa deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.UnidadeID <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID Unidade").ToString();
                value.mensagem.MessageBase = "Identificador da unidade deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.EXCLUIR || operation == Crud.ALTERAR)
            {
                if (value.CondominoID <= 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "ID Condomino").ToString();
                    value.mensagem.MessageBase = "Identificador do condômino deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            if (value.DataInicio == null || value.DataInicio > Funcoes.Brasilia().Date || value.DataInicio < new DateTime(2011, 1, 1))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Data de Início").ToString();
                value.mensagem.MessageBase = "Data de entrada do condômino no condomínio deve ser informada corretamente";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.DataFim != null && (value.DataFim > Funcoes.Brasilia().Date || value.DataFim < new DateTime(2011, 1, 1)))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Data de Início").ToString();
                value.mensagem.MessageBase = "Data de saída do condômino do condomínio deve ser informada corretamente";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            return value.mensagem;
        }
        #endregion
    }

    public class ListViewCondominoUnidade : ListViewModelLocal<CondominoUnidadeViewModel>
    {
        #region Constructor
        public ListViewCondominoUnidade() { }
        public ListViewCondominoUnidade(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<CondominoUnidadeViewModel> Bind(int? index, int pageSize = 20, params object[] param)
        {
            int _EdificacaoID = (int)param[0];
            int _UnidadeID = (int)param[1];
            string _nome = param != null && param[2] != null && param[2].ToString() != "" ? param[2].ToString() : null;

            IEnumerable<CondominoUnidadeViewModel> query = null;

            if (_EdificacaoID > 0 && _UnidadeID > 0)
            {
                query = (from c in db.CondominoUnidades join e in db.Edificacaos on c.EdificacaoID equals e.EdificacaoID
                         where c.CondominioID == sessaoCorrente.empresaId
                               && c.Condomino.IndFiscal.Length == 11
                               && c.Condomino.IndSituacao == "A"
                               && (c.CondominioID == sessaoCorrente.empresaId 
                                   && c.EdificacaoID == _EdificacaoID
                                   && c.UnidadeID == _UnidadeID)
                               && c.DataFim == null
                         orderby c.Condomino.Nome
                         select new CondominoUnidadeViewModel()
                         {
                             CondominioID = c.CondominioID,
                             EdificacaoID = c.EdificacaoID,
                             UnidadeID = c.UnidadeID,
                             CondominoID = c.CondominoID,
                             DataInicio = c.DataInicio,
                             EdificacaoDescricao = e.Descricao,
                             CondominoViewModel = new CondominoPFViewModel()
                             {
                                 Nome = c.Condomino.Nome,
                                 IndFiscal = c.Condomino.IndFiscal,
                                 IndProprietario = c.Condomino.IndProprietario,
                                 TelParticular1 = c.Condomino.TelParticular1,
                                 TelParticular2 = c.Condomino.TelParticular2,
                                 Email = c.Condomino.Email,
                                 UsuarioID = c.Condomino.UsuarioID,
                                 DataCadastro = c.Condomino.DataCadastro,
                                 Avatar = c.Condomino.Avatar,
                             },
                             PageSize = pageSize,
                             TotalCount = (from c1 in db.CondominoUnidades
                                           join e1 in db.Edificacaos on c1.EdificacaoID equals e1.EdificacaoID
                                           where c1.CondominioID == sessaoCorrente.empresaId
                                                 && c1.Condomino.IndFiscal.Length == 11
                                                 && c1.Condomino.IndSituacao == "A"
                                                 && (c1.CondominioID == sessaoCorrente.empresaId
                                                     && c1.EdificacaoID == _EdificacaoID
                                                     && c1.UnidadeID == _UnidadeID)
                                                 && c1.DataFim == null
                                           orderby c1.Condomino.Nome
                                           select c1).Count()
                         }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                query = (from c in db.CondominoUnidades
                         join e in db.Edificacaos on c.EdificacaoID equals e.EdificacaoID
                         where c.CondominioID == sessaoCorrente.empresaId
                               && c.Condomino.IndFiscal.Length == 11
                               && c.Condomino.IndSituacao == "A"
                               && (
                                     (_EdificacaoID > 0 && (c.CondominioID == sessaoCorrente.empresaId && c.EdificacaoID == _EdificacaoID) && (_nome != null && _nome != "" && (c.Condomino.Nome.StartsWith(_nome) || c.Condomino.IndFiscal == _nome || c.Condomino.Email == _nome))) ||
                                     (_EdificacaoID > 0 && (c.CondominioID == sessaoCorrente.empresaId && c.EdificacaoID == _EdificacaoID) && (_nome == null || _nome == "") ) ||
                                     (_nome != null && _nome != "" && (c.Condomino.Nome.StartsWith(_nome) || c.Condomino.IndFiscal == _nome || c.Condomino.Email == _nome)) ||
                                     ( _EdificacaoID == 0 && (_nome == null || _nome == ""))
                                  )
                               && c.DataFim == null
                         orderby c.Condomino.Nome
                         select new CondominoUnidadeViewModel()
                         {
                             CondominioID = c.CondominioID,
                             EdificacaoID = c.EdificacaoID,
                             UnidadeID = c.UnidadeID,
                             CondominoID = c.CondominoID,
                             DataInicio = c.DataInicio,
                             EdificacaoDescricao = e.Descricao,
                             CondominoViewModel = new CondominoPFViewModel()
                             {
                                 Nome = c.Condomino.Nome,
                                 IndFiscal = c.Condomino.IndFiscal,
                                 IndProprietario = c.Condomino.IndProprietario,
                                 TelParticular1 = c.Condomino.TelParticular1,
                                 TelParticular2 = c.Condomino.TelParticular2,
                                 Email = c.Condomino.Email,
                                 UsuarioID = c.Condomino.UsuarioID,
                                 DataCadastro = c.Condomino.DataCadastro,
                                 Avatar = c.Condomino.Avatar,
                             },
                             PageSize = pageSize,
                             TotalCount = (from c1 in db.CondominoUnidades
                                           join e1 in db.Edificacaos on c1.EdificacaoID equals e1.EdificacaoID
                                           where c1.CondominioID == sessaoCorrente.empresaId
                                                 && c1.Condomino.IndFiscal.Length == 11
                                                 && c1.Condomino.IndSituacao == "A"
                                                 && (
                                                       (_EdificacaoID > 0 && (c1.CondominioID == sessaoCorrente.empresaId && c1.EdificacaoID == _EdificacaoID) && (_nome != null && _nome != "" && (c1.Condomino.Nome.StartsWith(_nome) || c1.Condomino.IndFiscal == _nome || c1.Condomino.Email == _nome))) ||
                                                       (_EdificacaoID > 0 && (c1.CondominioID == sessaoCorrente.empresaId && c1.EdificacaoID == _EdificacaoID) && (_nome == null || _nome == "")) ||
                                                       (_nome != null && _nome != "" && (c1.Condomino.Nome.StartsWith(_nome) || c1.Condomino.IndFiscal == _nome || c1.Condomino.Email == _nome)) ||
                                                       (_EdificacaoID == 0 && (_nome == null || _nome == ""))
                                                    )
                                                 && c1.DataFim == null
                                           orderby c1.Condomino.Nome
                                           select c1).Count()
                         }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            }
            return query;
        }

        public override string action()
        {
            return "../Condomino/ListParam";
        }

        public override string DivId()
        {
            return "div-condomino-pf";
        }

        public override Repository getRepository(Object id)
        {
            return new CondominoPFModel().getObject((CondominoPFViewModel)id);
        }
        #endregion
    }

}