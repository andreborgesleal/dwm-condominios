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
using System.Web;
using App_Dominio.Negocio;
using System.Data.Entity;

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
        public override CondominoUnidadeViewModel BeforeInsert(CondominoUnidadeViewModel value)
        {
            if (value.CondominoViewModel is CondominoPFViewModel && value.CondominoViewModel.CondominoID == 0)
            {
                value.CondominoViewModel.CondominoID = value.CondominoID;
                CondominoPFModel model = new CondominoPFModel(this.db, this.seguranca_db);
                value.CondominoViewModel = model.getObject((CondominoPFViewModel)value.CondominoViewModel); 
            }

            return value;
        }

        public override CondominoUnidadeViewModel BeforeUpdate(CondominoUnidadeViewModel value)
        {
            value = BeforeInsert(value);

            // Verifica se o condômino não está mais vinculado a nenhuma unidade
            if ((from cu in db.CondominoUnidades
                 where cu.CondominioID == value.CondominioID && cu.CondominoID == value.CondominoID
                        && !cu.DataFim.HasValue && cu.EdificacaoID + cu.UnidadeID != value.EdificacaoID + value.UnidadeID
                 select cu).Count() == 0)
            {
                value.CondominoViewModel.IndSituacao = "D";
            }

            return value;
        }

        public override CondominoUnidadeViewModel AfterUpdate(CondominoUnidadeViewModel value)
        {
            if ((from cu in db.CondominoUnidades
                 where cu.CondominioID == value.CondominioID && cu.CondominoID == value.CondominoID
                        && !cu.DataFim.HasValue && cu.EdificacaoID + cu.UnidadeID != value.EdificacaoID + value.UnidadeID
                 select cu).Count() == 0)
            {
                // Desativar o usuário em seguranca
                Usuario u = seguranca_db.Usuarios.Find(value.CondominoViewModel.UsuarioID);
                if (u != null)
                {
                    u.situacao = "D";
                    seguranca_db.Entry(u).State = EntityState.Modified;
                }

                // Recuperar a lista de usuários credenciados
                ListViewCredenciados list = new ListViewCredenciados(this.db, this.seguranca_db);
                IEnumerable<CredenciadoViewModel> listCredenciados = list.Bind(0, 50, value.CondominoID);

                // Desativar os credenciados em seguranca
                foreach (CredenciadoViewModel cred in listCredenciados.Where(info => info.UsuarioID.HasValue))
                {
                    u = seguranca_db.Usuarios.Find(cred.UsuarioID);
                    if (u != null)
                    {
                        u.situacao = "D";
                        seguranca_db.Entry(u).State = EntityState.Modified;
                    }
                }
            }

            return value;
        }

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
            else if (value.CondominoViewModel is CondominoPJViewModel)
            {
                CondominoPJModel model = new CondominoPJModel(this.db, this.seguranca_db);
                condominoUnidade.Condomino = model.MapToEntity((CondominoPJViewModel)value.CondominoViewModel);
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
                Codigo = db.Unidades.Find(entity.CondominioID, entity.EdificacaoID, entity.UnidadeID).Codigo,
                CondominoID = entity.CondominoID,
                DataInicio = entity.DataInicio,
                DataFim = entity.DataFim,
                EdificacaoDescricao = db.Edificacaos.Find(entity.EdificacaoID).Descricao,
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
                value.mensagem.MessageBase = "Edificação deve ser informada";
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

            #region Verifica se a unidade já não está ocupada por outro condômino
            if (operation == Crud.INCLUIR)
            {
                if ((from cu in db.CondominoUnidades
                     where cu.CondominioID == value.CondominioID &&
                           cu.EdificacaoID == value.EdificacaoID &&
                           cu.UnidadeID == value.UnidadeID &&
                           !cu.DataFim.HasValue 
                     select cu.CondominoID).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "A unidade selecionada está ocupada por outro condômino";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            #endregion

            return value.mensagem;
        }

        public override CondominoUnidadeViewModel CreateRepository(HttpRequestBase Request = null)
        {
            CondominoUnidadeViewModel obj = new CondominoUnidadeViewModel();
            obj.CondominoViewModel = new CondominoPFViewModel();

            if (Request ["CondominoID"] != "")
            {
                CondominoPFModel CondominoPFModel = new CondominoPFModel(this.db, this.seguranca_db);

                obj.CondominioID = SessaoLocal.empresaId;
                obj.CondominoID = int.Parse(Request["CondominoID"]);
                obj.DataInicio = Funcoes.Brasilia().Date;
                obj.CondominoViewModel.CondominoID = obj.CondominoID;
                obj.CondominoViewModel = CondominoPFModel.getObject((CondominoPFViewModel)obj.CondominoViewModel);
            }

            return obj;
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
            int? _CondominoID = null;
            int _EdificacaoID = 0;
            int _UnidadeID = 0;
            string _nome = null;

            if (param.Count() == 1)
                _CondominoID = (int)param[0];
            else
            {
                _EdificacaoID = (int)param[0];
                _UnidadeID = (int)param[1];
                _nome = param != null && param[2] != null && param[2].ToString() != "" ? param[2].ToString() : null;
            }

            IEnumerable<CondominoUnidadeViewModel> query = null;

            if (_CondominoID.HasValue)
                query = (from c in db.CondominoUnidades
                         join e in db.Edificacaos on c.EdificacaoID equals e.EdificacaoID
                         join u in db.Unidades on new { c.CondominioID, c.EdificacaoID, c.UnidadeID } equals new { u.CondominioID, u.EdificacaoID, u.UnidadeID}
                         where c.CondominioID == sessaoCorrente.empresaId
                               && c.Condomino.IndSituacao == "A"
                               && (c.CondominioID == sessaoCorrente.empresaId
                                   && c.CondominoID == _CondominoID)
                               && c.DataFim == null
                         orderby c.EdificacaoID, c.UnidadeID
                         select new CondominoUnidadeViewModel()
                         {
                             CondominioID = c.CondominioID,
                             EdificacaoID = c.EdificacaoID,
                             UnidadeID = c.UnidadeID,
                             Codigo = u.Codigo,
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
                                 Sexo = (from sex in db.CondominoPFs where sex.CondominoID == c.Condomino.CondominoID select sex.Sexo).FirstOrDefault()
                             },
                         }).ToList();
            else if (_EdificacaoID > 0 && _UnidadeID > 0)
            {
                query = (from c in db.CondominoUnidades join e in db.Edificacaos on c.EdificacaoID equals e.EdificacaoID
                         join u in db.Unidades on new { c.CondominioID, c.EdificacaoID, c.UnidadeID } equals new { u.CondominioID, u.EdificacaoID, u.UnidadeID }
                         where c.CondominioID == sessaoCorrente.empresaId
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
                             Codigo = u.Codigo,
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
                                 Sexo = (from sex in db.CondominoPFs where sex.CondominoID == c.Condomino.CondominoID select sex.Sexo).FirstOrDefault()
                             },
                         }).ToList();
            }
            else
            {
                query = (from c in db.CondominoUnidades
                         join e in db.Edificacaos on c.EdificacaoID equals e.EdificacaoID
                         join u in db.Unidades on new { c.CondominioID, c.EdificacaoID, c.UnidadeID } equals new { u.CondominioID, u.EdificacaoID, u.UnidadeID }
                         where c.CondominioID == sessaoCorrente.empresaId
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
                             Codigo = u.Codigo,
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
                                 Sexo = (from sex in db.CondominoPFs where sex.CondominoID == c.Condomino.CondominoID select sex.Sexo).FirstOrDefault()
                             },
                         }).ToList();
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

    public class ListViewCondominosDesativados : ListViewModelLocal<CondominoUnidadeViewModel>
    {
        #region Constructor
        public ListViewCondominosDesativados() { }
        public ListViewCondominosDesativados(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<CondominoUnidadeViewModel> Bind(int? index, int pageSize = 20, params object[] param)
        {
            int? _CondominoID = null;
            int _EdificacaoID = 0;
            int _UnidadeID = 0;
            string _nome = null;

            if (param.Count() == 1)
                _CondominoID = (int)param[0];
            else
            {
                _EdificacaoID = (int)param[0];
                _UnidadeID = (int)param[1];
                _nome = param != null && param[2] != null && param[2].ToString() != "" ? param[2].ToString() : null;
            }

            IEnumerable<CondominoUnidadeViewModel> query = null;

            if (_CondominoID.HasValue)
                query = (from c in db.CondominoUnidades
                         join e in db.Edificacaos on c.EdificacaoID equals e.EdificacaoID
                         join u in db.Unidades on new { c.CondominioID, c.EdificacaoID, c.UnidadeID } equals new { u.CondominioID, u.EdificacaoID, u.UnidadeID }
                         where c.CondominioID == sessaoCorrente.empresaId
                               && c.Condomino.IndSituacao == "D"
                               && (c.CondominioID == sessaoCorrente.empresaId
                                   && c.CondominoID == _CondominoID)
                               && c.DataFim == null
                         orderby c.EdificacaoID, c.UnidadeID
                         select new CondominoUnidadeViewModel()
                         {
                             CondominioID = c.CondominioID,
                             EdificacaoID = c.EdificacaoID,
                             UnidadeID = c.UnidadeID,
                             Codigo = u.Codigo,
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
                         }).ToList();
            else if (_EdificacaoID > 0 && _UnidadeID > 0)
            {
                query = (from c in db.CondominoUnidades
                         join e in db.Edificacaos on c.EdificacaoID equals e.EdificacaoID
                         join u in db.Unidades on new { c.CondominioID, c.EdificacaoID, c.UnidadeID } equals new { u.CondominioID, u.EdificacaoID, u.UnidadeID }
                         where c.CondominioID == sessaoCorrente.empresaId
                               && c.Condomino.IndSituacao == "D"
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
                             Codigo = u.Codigo,
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
                         }).ToList();
            }
            else
            {
                query = (from c in db.CondominoUnidades
                         join e in db.Edificacaos on c.EdificacaoID equals e.EdificacaoID
                         join u in db.Unidades on new { c.CondominioID, c.EdificacaoID, c.UnidadeID } equals new { u.CondominioID, u.EdificacaoID, u.UnidadeID }
                         where c.CondominioID == sessaoCorrente.empresaId
                               && c.Condomino.IndSituacao == "D"
                               && (
                                     (_EdificacaoID > 0 && (c.CondominioID == sessaoCorrente.empresaId && c.EdificacaoID == _EdificacaoID) && (_nome != null && _nome != "" && (c.Condomino.Nome.StartsWith(_nome) || c.Condomino.IndFiscal == _nome || c.Condomino.Email == _nome))) ||
                                     (_EdificacaoID > 0 && (c.CondominioID == sessaoCorrente.empresaId && c.EdificacaoID == _EdificacaoID) && (_nome == null || _nome == "")) ||
                                     (_nome != null && _nome != "" && (c.Condomino.Nome.StartsWith(_nome) || c.Condomino.IndFiscal == _nome || c.Condomino.Email == _nome)) ||
                                     (_EdificacaoID == 0 && (_nome == null || _nome == ""))
                                  )
                               && c.DataFim == null
                         orderby c.Condomino.Nome
                         select new CondominoUnidadeViewModel()
                         {
                             CondominioID = c.CondominioID,
                             EdificacaoID = c.EdificacaoID,
                             UnidadeID = c.UnidadeID,
                             Codigo = u.Codigo,
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
                         }).ToList();
            }
            return query;
        }

        public override string action()
        {
            return "../Condomino/ListViewCondominosDesativados";
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

    public class ListViewCondominoUnidadeChamado : ListViewCondominoUnidade
    {
        public ListViewCondominoUnidadeChamado() { }
        public ListViewCondominoUnidadeChamado(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #region Métodos da classe ListViewRepository
        public override string action()
        {
            return "../Condomino/ListCondomino";
        }
        #endregion
    }

    public class ListViewCondominosInativos : ListViewModelLocal<CondominoUnidadeViewModel>
    {
        #region Constructor
        public ListViewCondominosInativos() { }
        public ListViewCondominosInativos(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<CondominoUnidadeViewModel> Bind(int? index, int pageSize = 20, params object[] param)
        {
            IEnumerable<CondominoUnidadeViewModel> query = null;
            query = (from c in db.CondominoUnidades
                     join e in db.Edificacaos on c.EdificacaoID equals e.EdificacaoID
                     join u in db.Unidades on new { c.CondominioID, c.EdificacaoID, c.UnidadeID } equals new { u.CondominioID, u.EdificacaoID, u.UnidadeID}
                     where c.CondominioID == sessaoCorrente.empresaId
                           && c.Condomino.CondominioID == sessaoCorrente.empresaId
                           && c.Condomino.IndSituacao == "D"
                           && c.DataFim == null
                     orderby c.Condomino.Nome
                     select new CondominoUnidadeViewModel()
                     {
                         CondominioID = c.CondominioID,
                         EdificacaoID = c.EdificacaoID,
                         UnidadeID = c.UnidadeID,
                         Codigo = u.Codigo,
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
                     }).ToList();
            return query;
        }

        public override string action()
        {
            return "../Condomino/ListViewCondominosInativos";
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