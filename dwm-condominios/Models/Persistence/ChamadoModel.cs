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
using App_Dominio.Models;
using App_Dominio.Repositories;
using System.Data.Entity.Infrastructure;
using DWM.Models.BI;
using System.Web.Mvc;

namespace DWM.Models.Persistence
{
    public class ChamadoModel : CrudModelLocal<Chamado, ChamadoViewModel>
    {
        #region Constructor
        public ChamadoModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override ChamadoViewModel BeforeInsert(ChamadoViewModel value)
        {
            try
            {
                value = GetUsuario(value);
                value.empresaId = SessaoLocal.empresaId;
                value.DataChamado = Funcoes.Brasilia();
                value.CondominioID = SessaoLocal.empresaId;
                value.CredenciadoID = SessaoLocal.CredenciadoID;
                value.ChamadoStatusID = 0;
                value.DataRedirecionamento = Funcoes.Brasilia();

                // Se a fila de atendimento destino for a fila do condômino, então preencher o responsável pelo chamado (UsuarioID, Nome e Login)
                if (value.FilaAtendimentoID == DWMSessaoLocal.FilaCondominoID(sessaoCorrente, db))
                {
                    int UsuarioCondominoID = db.Condominos.Find(value.CondominoID).UsuarioID.Value;
                    Usuario u = seguranca_db.Usuarios.Find(UsuarioCondominoID);
                    value.UsuarioFilaID = u.usuarioId;
                    value.NomeUsuarioFila = u.nome;
                    value.LoginUsuarioFila = u.login;
                    value.ChamadoFilaViewModel.UsuarioID = u.usuarioId; // login e nome do usuário serão recuperados do método beforeinsert do ChamadoFilaModel.
                }

                #region Executa o método BeforeInsert do ChamadoFila
                ChamadoFilaModel ChamadoFilaModel = new ChamadoFilaModel();
                ChamadoFilaModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
                value.ChamadoFilaViewModel = ChamadoFilaModel.BeforeInsert(value.ChamadoFilaViewModel);
                value.Rotas = new List<ChamadoFilaViewModel>();
                ((List<ChamadoFilaViewModel>)value.Rotas).Add(value.ChamadoFilaViewModel);
                #endregion

                #region Executa o método BeforeInsert do ChamadoAnexo para cada elemento da coleção
                ChamadoAnexoModel ChamadoAnexoModel = new ChamadoAnexoModel();
                ChamadoAnexoModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
                int index = 0;
                while (index <= value.Anexos.Count() - 1)
                {
                    ChamadoAnexoViewModel ChamadoAnexoViewModel = ChamadoAnexoModel.BeforeInsert(value.Anexos.ElementAt(index));
                    value.Anexos.ElementAt(index).UsuarioID = ChamadoAnexoViewModel.UsuarioID;
                    value.Anexos.ElementAt(index).Nome = ChamadoAnexoViewModel.Nome;
                    value.Anexos.ElementAt(index).Login = ChamadoAnexoViewModel.Login;
                    index++;
                }
                #endregion
            }
            catch (Exception ex)
            {
                value.mensagem = new Validate()
                {
                    Code = 999,
                    Message = "Ocorreu um erro no evento interno que antecede a  inclusão do chamado. Favor entrar em contato com o administrador do sistema",
                    MessageBase = ex.Message
                };
            }

            return value;
        }

        public override ChamadoViewModel AfterInsert(ChamadoViewModel value)
        {
            // Enviar e-mail ao destinatário
            try
            {
                #region Passo único: Executa o método AfterInsert do ChamadoAnexo para cada elemento da coleção
                ChamadoAnexoModel ChamadoAnexoModel = new ChamadoAnexoModel();
                ChamadoAnexoModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
                int index = 0;
                while (index <= value.Anexos.Count() - 1)
                {
                    ChamadoAnexoViewModel ChamadoAnexoViewModel = ChamadoAnexoModel.AfterInsert(value.Anexos.ElementAt(index));
                    index++;
                }
                #endregion
            }
            catch (ArgumentException ex)
            {
                value.mensagem = new Validate() { Code = 997, Message = MensagemPadrao.Message(997).ToString(), MessageBase = ex.Message };
            }
            catch (App_DominioException ex)
            {
                value.mensagem = ex.Result;

                if (ex.InnerException != null)
                    value.mensagem.MessageBase = new App_DominioException(ex.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                else
                    value.mensagem.MessageBase = new App_DominioException(ex.Result.Message, GetType().FullName).Message;
            }
            catch (DbUpdateException ex)
            {
                value.mensagem.MessageBase = ex.InnerException.InnerException.Message ?? ex.Message;
                if (value.mensagem.MessageBase.ToUpper().Contains("REFERENCE") || value.mensagem.MessageBase.ToUpper().Contains("FOREIGN"))
                {
                    if (value.mensagem.MessageBase.ToUpper().Contains("DELETE"))
                    {
                        value.mensagem.Code = 16;
                        value.mensagem.Message = MensagemPadrao.Message(16).ToString();
                        value.mensagem.MessageType = MsgType.ERROR;
                    }
                    else
                    {
                        value.mensagem.Code = 28;
                        value.mensagem.Message = MensagemPadrao.Message(28).ToString();
                        value.mensagem.MessageType = MsgType.ERROR;
                    }
                }
                else if (value.mensagem.MessageBase.ToUpper().Contains("PRIMARY"))
                {
                    value.mensagem.Code = 37;
                    value.mensagem.Message = MensagemPadrao.Message(37).ToString();
                    value.mensagem.MessageType = MsgType.WARNING;
                }
                else if (value.mensagem.MessageBase.ToUpper().Contains("UNIQUE KEY"))
                {
                    value.mensagem.Code = 54;
                    value.mensagem.Message = MensagemPadrao.Message(54).ToString();
                    value.mensagem.MessageType = MsgType.WARNING;
                }
                else
                {
                    value.mensagem.Code = 44;
                    value.mensagem.Message = MensagemPadrao.Message(44).ToString();
                    value.mensagem.MessageType = MsgType.ERROR;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                value.mensagem = new Validate() { Code = 42, Message = MensagemPadrao.Message(42).ToString(), MessageBase = ex.EntityValidationErrors.Select(m => m.ValidationErrors.First().ErrorMessage).First() };
            }
            catch (Exception ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                value.mensagem.MessageType = MsgType.ERROR;
            }
            return value;
        }

        public override Chamado MapToEntity(ChamadoViewModel value)
        {
            Chamado entity = null;

            if (value.ChamadoID > 0)
                entity = Find(value);
            else
                entity = new Chamado();

            entity.ChamadoID = value.ChamadoID;
            entity.ChamadoMotivoID = value.ChamadoMotivoID;
            entity.ChamadoStatusID = value.ChamadoStatusID;
            entity.FilaSolicitanteID = value.FilaSolicitanteID;
            entity.CondominioID = value.CondominioID;
            entity.CondominoID = value.CondominoID == 0 ? null : value.CondominoID;
            entity.CredenciadoID = value.CredenciadoID == 0 ? null : value.CredenciadoID;
            entity.EdificacaoID = value.EdificacaoID;
            entity.UnidadeID = value.UnidadeID;
            entity.DataChamado = value.DataChamado;
            entity.Assunto = value.Assunto;
            entity.MensagemOriginal = value.MensagemOriginal;
            entity.UsuarioID = value.UsuarioID;
            entity.NomeUsuario = value.NomeUsuario;
            entity.LoginUsuario = value.LoginUsuario;
            entity.Prioridade = value.Prioridade;
            entity.DataUltimaAnotacao = value.DataUltimaAnotacao;
            entity.FilaAtendimentoID = value.FilaAtendimentoID;
            entity.DataRedirecionamento = value.DataRedirecionamento;
            entity.UsuarioFilaID = value.UsuarioFilaID;
            entity.NomeUsuarioFila = value.NomeUsuarioFila;
            entity.LoginUsuarioFila = value.LoginUsuarioFila;

            // Executa o MapToEntity do ChamadoFila
            if (value.ChamadoFilaViewModel != null)
            {
                ChamadoFilaModel ChamadoFilaModel = new ChamadoFilaModel();
                ChamadoFilaModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
                entity.Rotas = new List<ChamadoFila>();
                entity.Rotas.Add(ChamadoFilaModel.MapToEntity(value.ChamadoFilaViewModel));
            }

            // Mapear anexos
            ChamadoAnexoModel ChamadoAnexoModel = new ChamadoAnexoModel();
            ChamadoAnexoModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
            entity.Anexos = new List<ChamadoAnexo>();
            foreach (ChamadoAnexoViewModel cha in value.Anexos)
            {
                ChamadoAnexo ChamadoAnexo = ChamadoAnexoModel.MapToEntity(cha);
                entity.Anexos.Add(ChamadoAnexo);
            };

            return entity;
        }

        public override ChamadoViewModel MapToRepository(Chamado entity)
        {
            ChamadoViewModel value = new ChamadoViewModel()
            {
                ChamadoID = entity.ChamadoID,
                ChamadoMotivoID = entity.ChamadoMotivoID,
                DescricaoChamadoMotivo = db.ChamadoMotivos.Find(entity.ChamadoMotivoID, sessaoCorrente.empresaId).Descricao,
                ChamadoStatusID = entity.ChamadoStatusID,
                DescricaoChamadoStatus = db.ChamadoStatuss.Find(entity.ChamadoStatusID, sessaoCorrente.empresaId).Descricao,
                FilaSolicitanteID = entity.FilaSolicitanteID,
                DescricaoFilaSolicitante = db.FilaAtendimentos.Find(entity.FilaSolicitanteID).Descricao,
                FilaCondominoID = DWMSessaoLocal.FilaCondominoID(sessaoCorrente, this.db),
                CondominioID = entity.CondominioID,
                CondominoID = entity.CondominoID,
                NomeCondomino = entity.CondominoID.HasValue ? db.Condominos.Find(entity.CondominoID).Nome : "",
                CredenciadoID = entity.CredenciadoID,
                EdificacaoID = entity.EdificacaoID,
                UnidadeID = entity.UnidadeID,
                DescricaoEdificacao = entity.UnidadeID.HasValue ? db.Edificacaos.Find(entity.EdificacaoID).Descricao : "",
                DataChamado = entity.DataChamado,
                Assunto = entity.Assunto,
                UsuarioID = entity.UsuarioID,
                NomeUsuario = entity.NomeUsuario,
                LoginUsuario = entity.LoginUsuario,
                Prioridade = entity.Prioridade,
                DataUltimaAnotacao = entity.DataUltimaAnotacao,
                MensagemOriginal = entity.MensagemOriginal,
                FilaAtendimentoID = entity.FilaAtendimentoID,
                DescricaoFilaAtendimento = db.FilaAtendimentos.Find(entity.FilaAtendimentoID).Descricao,
                DataRedirecionamento = entity.DataRedirecionamento,
                UsuarioFilaID = entity.UsuarioFilaID,
                NomeUsuarioFila = entity.NomeUsuarioFila,
                LoginUsuarioFila = entity.LoginUsuarioFila,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };

            ListViewChamadoFila listChamadoFila = new ListViewChamadoFila(this.db, this.seguranca_db);
            ListViewChamadoAnotacao listChamadoAnotacao = new ListViewChamadoAnotacao(this.db, this.seguranca_db);
            ListViewChamadoAnexo listChamadoAnexo = new ListViewChamadoAnexo(this.db, this.seguranca_db);

            if (entity.ChamadoID > 0)
            {
                value.Rotas = listChamadoFila.Bind(0, 200, value.ChamadoID);
                value.Anotacoes = listChamadoAnotacao.Bind(0, 200, value.ChamadoID);
                value.Anexos = listChamadoAnexo.Bind(0, 100, value.ChamadoID);
            }
            else
            {
                #region Rotas
                value.Rotas = new List<ChamadoFilaViewModel>();
                ChamadoFilaModel ChamadoFilaModel = new ChamadoFilaModel();
                ChamadoFilaModel.Create(this.db, this.seguranca_db);
                foreach (ChamadoFila fila in entity.Rotas)
                {
                    ChamadoFilaViewModel ChamadoFilaViewModel = ChamadoFilaModel.MapToRepository(fila);
                    ((List<ChamadoFilaViewModel>)value.Rotas).Add(ChamadoFilaViewModel);
                }
                #endregion

                #region Anexos
                value.Anexos = new List<ChamadoAnexoViewModel>();
                ChamadoAnexoModel ChamadoAnexoModel = new ChamadoAnexoModel();
                ChamadoAnexoModel.Create(this.db, this.seguranca_db);
                foreach (ChamadoAnexo anexo in entity.Anexos)
                {
                    ChamadoAnexoViewModel ChamadoAnexoViewModel = ChamadoAnexoModel.MapToRepository(anexo);
                    ((List<ChamadoAnexoViewModel>)value.Anexos).Add(ChamadoAnexoViewModel);
                }
                #endregion
            }

            #region Verifica se o usuário corrente é um usuário da Fila de atendimento atual
            value.IsUsuarioFila = false;

            if (value.UsuarioFilaID.HasValue && value.UsuarioFilaID.Value == SessaoLocal.usuarioId)
                value.IsUsuarioFila = true;
            else if (value.FilaAtendimentoID != DWMSessaoLocal.FilaCondominoID(sessaoCorrente, db))
            {
                value.IsUsuarioFila = (from f in db.FilaAtendimentos
                                       join fu in db.FilaAtendimentoUsuarios on f.FilaAtendimentoID equals fu.FilaAtendimentoID
                                       where f.CondominioID == SessaoLocal.empresaId &&
                                              f.FilaAtendimentoID == value.FilaAtendimentoID &&
                                              fu.UsuarioID == SessaoLocal.usuarioId
                                       select fu.UsuarioID).Count() > 0;
            }
            #endregion

            #region Verifica se o usuário corrente é um condômino/credenciado
            value.IsCondomino = (SessaoLocal.CondominoID > 0 || SessaoLocal.CredenciadoID.HasValue);
            #endregion

            return value;
        }

        public override Chamado Find(ChamadoViewModel key)
        {
            return db.Chamados.Find(key.ChamadoID);
        }

        public override Validate Validate(ChamadoViewModel value, Crud operation)
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

            if (value.empresaId == 0)
            {
                value.mensagem.Code = 35;
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "Sua sessão expirou. Faça um novo login no sistema";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.ChamadoMotivoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Motivo").ToString();
                value.mensagem.MessageBase = "Motivo do chamado deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.ChamadoStatusID != 0 && operation == Crud.INCLUIR)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Situação").ToString();
                value.mensagem.MessageBase = "Situação do chamado deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            // Se a fila solicitante ou a fila para atendimento for a fila do condômino
            // é preciso então que o ID do condômino ou do Credenciado estejam preenchidos
            // é preciso também que a Edificação e a Unidade também estejam preenchidas
            if (DWMSessaoLocal.FilaCondominoID(sessaoCorrente, db) == value.FilaSolicitanteID ||
                DWMSessaoLocal.FilaCondominoID(sessaoCorrente, db) == value.FilaAtendimentoID)
            {
                if (value.CondominoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Condômino").ToString();
                    value.mensagem.MessageBase = "Condômino deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
                if (SessaoLocal.CredenciadoID > 0 && value.CredenciadoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Residente").ToString();
                    value.mensagem.MessageBase = "Residente deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
                if (value.EdificacaoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Edificação").ToString();
                    value.mensagem.MessageBase = "Edificação deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
                if (value.UnidadeID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Unidade").ToString();
                    value.mensagem.MessageBase = "Unidade deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            if (String.IsNullOrEmpty(value.Assunto) || value.Assunto.Trim().Length <= 10)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Assunto").ToString();
                value.mensagem.MessageBase = "Assunto deve ser informado e deve ter mais de 10 caracteres";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.UsuarioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID do Usuário").ToString();
                value.mensagem.MessageBase = "Código identificador do Usuário que abriu o chamado deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (String.IsNullOrEmpty(value.NomeUsuario))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Usuário").ToString();
                value.mensagem.MessageBase = "Nome do Usuário que abriu o chamado deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (String.IsNullOrEmpty(value.LoginUsuario))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Login do Usuário").ToString();
                value.mensagem.MessageBase = "Login do Usuário que abriu o chamado deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (String.IsNullOrEmpty(value.Prioridade))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Prioridade").ToString();
                value.mensagem.MessageBase = "Prioridade do chamado deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (String.IsNullOrEmpty(value.MensagemOriginal))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Mensagem").ToString();
                value.mensagem.MessageBase = "Mensagem do chamado deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.FilaAtendimentoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Fila de atendimento").ToString();
                value.mensagem.MessageBase = "Fila de atendimento deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.FilaAtendimentoID == value.FilaSolicitanteID)
            {
                value.mensagem.Code = 49;
                value.mensagem.Message = MensagemPadrao.Message(49, "Solicitante", "Encaminhado para").ToString();
                value.mensagem.MessageBase = "Fila do Solicitante dever ser diferente da Fila de Atendimento";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            // Valida o ChamadoFila
            if (operation == Crud.INCLUIR)
            {
                ChamadoFilaModel ChamadoFilaModel = new ChamadoFilaModel();
                ChamadoFilaModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
                value.mensagem = ChamadoFilaModel.Validate(value.ChamadoFilaViewModel, Crud.INCLUIR);
            }

            // Validar Anexos
            ChamadoAnexoModel ChamadoAnexoModel = new ChamadoAnexoModel();
            ChamadoAnexoModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
            foreach (ChamadoAnexoViewModel cha in value.Anexos)
            {
                value.mensagem = ChamadoAnexoModel.Validate(cha, Crud.INCLUIR);
                if (value.mensagem.Code > 0)
                    break;
            };

            return value.mensagem;
        }

        public override ChamadoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            ChamadoViewModel value = base.CreateRepository(Request);
            using (ApplicationContext db = new ApplicationContext())
            {
                using (SecurityContext seguranca_db = new SecurityContext())
                {
                    EmpresaSecurity <SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                    security.Create(seguranca_db);
                    Sessao sessaoCorrente = security._getSessaoCorrente(seguranca_db);
                    SessaoLocal SessaoLocal = DWMSessaoLocal.GetSessaoLocal(sessaoCorrente, db);
                    value.CondominioID = sessaoCorrente.empresaId;
                    value.CondominoID = SessaoLocal.CondominoID;
                    value.UsuarioID = SessaoLocal.usuarioId;
                    value.FilaCondominoID = DWMSessaoLocal.FilaCondominoID(sessaoCorrente, db);

                    ListViewCondominoUnidadeChamado l = new ListViewCondominoUnidadeChamado(db, seguranca_db);
                    if (SessaoLocal.CondominoID == 0)
                        value.Condominos = (PagedList<CondominoUnidadeViewModel>)l.getPagedList(0, 10, 0, 0, "");
                    else
                        value.Condominos = (PagedList<CondominoUnidadeViewModel>)l.getPagedList(0, 10, SessaoLocal.CondominoID);

                    // Anexo
                    value.ChamadoAnexoViewModel = new ChamadoAnexoViewModel();

                    if (Request["_ChamadoMotivoID"] != null && Request["_ChamadoMotivoID"] != "")
                        value.ChamadoMotivoID = int.Parse(Request["_ChamadoMotivoID"]);

                    if (Request["_FilaSolicitanteID"] != null && Request["_FilaSolicitanteID"] != "")
                        value.FilaSolicitanteID = int.Parse(Request["_FilaSolicitanteID"]);

                    if (Request["_FilaAtendimentoID"] != null && Request["_FilaAtendimentoID"] != "")
                        value.FilaAtendimentoID = int.Parse(Request["_FilaAtendimentoID"]);

                    
                    value.Assunto = Request["Assunto"] ?? "";
                    value.Prioridade = "2";
                }
            }

            return value;
        }
        #endregion

        #region Métodos customizados
        private ChamadoViewModel GetUsuario(ChamadoViewModel value)
        {
            if (value.UsuarioID == 0)
                value.UsuarioID = SessaoLocal.usuarioId;

            value.NomeUsuario = seguranca_db.Usuarios.Find(value.UsuarioID).nome; ;
            value.LoginUsuario = seguranca_db.Usuarios.Find(value.UsuarioID).login;

            return value;
        }
        #endregion
    }

    public class ListViewChamado : ListViewModelLocal<ChamadoViewModel>
    {
        #region Constructor
        public ListViewChamado() { }

        public ListViewChamado(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ChamadoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _FilaCondominoID = DWMSessaoLocal.FilaCondominoID(sessaoCorrente, this.db);

            return (from cha in db.Chamados
                    join FilaAtual in db.FilaAtendimentos on cha.FilaAtendimentoID equals FilaAtual.FilaAtendimentoID
                    join con in db.Condominos on cha.CondominoID equals con.CondominoID into CON
                    from con in CON.DefaultIfEmpty()
                    join edi in db.Edificacaos on cha.EdificacaoID equals edi.EdificacaoID into EDI
                    from edi in EDI.DefaultIfEmpty()
                    join sta in db.ChamadoStatuss on cha.ChamadoStatusID equals sta.ChamadoStatusID
                    join mot in db.ChamadoMotivos on cha.ChamadoMotivoID equals mot.ChamadoMotivoID
                    where  cha.CondominioID == SessaoLocal.empresaId &&
                           sta.CondominioID == SessaoLocal.empresaId &&
                           mot.CondominioID == SessaoLocal.empresaId && 
                           cha.ChamadoStatusID != 3 && // 3-Encerrado
                           (cha.UsuarioID == SessaoLocal.usuarioId || cha.UsuarioFilaID == SessaoLocal.usuarioId ||
                           (from usu in db.FilaAtendimentoUsuarios
                            where usu.FilaAtendimentoID == cha.FilaAtendimentoID
                            select usu.UsuarioID).Contains(SessaoLocal.usuarioId)) 
                    orderby cha.DataChamado descending
                    select new ChamadoViewModel
                    {
                        empresaId = SessaoLocal.empresaId,
                        ChamadoID = cha.ChamadoID,
                        ChamadoMotivoID = cha.ChamadoMotivoID,
                        DescricaoChamadoMotivo = mot.Descricao,
                        ChamadoStatusID = cha.ChamadoStatusID,
                        DescricaoChamadoStatus = sta.Descricao,
                        FilaSolicitanteID = cha.FilaSolicitanteID,
                        DescricaoFilaSolicitante = (from fil in db.FilaAtendimentos where fil.CondominioID == SessaoLocal.empresaId && fil.FilaAtendimentoID == cha.FilaSolicitanteID select fil.Descricao).FirstOrDefault(),
                        FilaAtendimentoID = cha.FilaAtendimentoID,
                        DescricaoFilaAtendimento = FilaAtual.Descricao,
                        FilaCondominoID = _FilaCondominoID,
                        CondominoID = cha.CondominoID,
                        CredenciadoID = cha.CredenciadoID,
                        NomeCondomino = con != null ? con.Nome : "",
                        EdificacaoID = cha.EdificacaoID,
                        DescricaoEdificacao = edi.Descricao,
                        UnidadeID = cha.UnidadeID,
                        DataChamado = cha.DataChamado,
                        Assunto = cha.Assunto,
                        UsuarioID = cha.UsuarioID,
                        NomeUsuario = cha.NomeUsuario,
                        LoginUsuario = cha.LoginUsuario,
                        Prioridade = cha.Prioridade,
                        DataUltimaAnotacao = cha.DataUltimaAnotacao,
                        DataRedirecionamento = cha.DataRedirecionamento,
                        UsuarioFilaID = cha.UsuarioFilaID,
                        NomeUsuarioFila = cha.NomeUsuarioFila,
                        LoginUsuarioFila = cha.LoginUsuarioFila,
                        PageSize = pageSize,
                        TotalCount = ((from cha1 in db.Chamados
                                       join FilaAtual1 in db.FilaAtendimentos on cha1.FilaAtendimentoID equals FilaAtual1.FilaAtendimentoID
                                       join con1 in db.Condominos on cha1.CondominoID equals con1.CondominoID into CON1
                                       from con1 in CON1.DefaultIfEmpty()
                                       join edi1 in db.Edificacaos on cha1.EdificacaoID equals edi1.EdificacaoID into EDI1
                                       from edi1 in EDI1.DefaultIfEmpty()
                                       join sta1 in db.ChamadoStatuss on cha1.ChamadoStatusID equals sta1.ChamadoStatusID
                                       join mot1 in db.ChamadoMotivos on cha1.ChamadoMotivoID equals mot1.ChamadoMotivoID
                                       where cha1.CondominioID == SessaoLocal.empresaId &&
                                              sta1.CondominioID == SessaoLocal.empresaId &&
                                              mot1.CondominioID == SessaoLocal.empresaId &&
                                              cha1.ChamadoStatusID != 3 && // 3-Encerrado
                                              (cha1.UsuarioID == SessaoLocal.usuarioId || cha1.UsuarioFilaID == SessaoLocal.usuarioId ||
                                              (from usu1 in db.FilaAtendimentoUsuarios
                                               where usu1.FilaAtendimentoID == cha1.FilaAtendimentoID
                                               select usu1.UsuarioID).Contains(SessaoLocal.usuarioId))
                                       orderby cha1.DataChamado descending
                                       select cha1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new ChamadoModel().getObject((ChamadoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-chamado";
        }
    }

    public class ListViewChamadoDetalhe : ListViewModelLocal<ChamadoViewModel>
    {
        #region Constructor
        public ListViewChamadoDetalhe() { }

        public ListViewChamadoDetalhe(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ChamadoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            #region parâmetros
            /* 0. Período:
             *      007 última semana
             *      030 últimos 30 dias
             *      060 últimos 60 dias
             *      090 Últimos 90 dias
             *      180 Últimos 180 dias
             * 
             * 1. Edificação
             * 2. Unidade
             * 3. Condômino
             * 4. Fila de atendimento
             * 5. Chamado Motivo =>> Null = Todos
             * 6. Chamado Status =>> Null = Todos             * 
             */

            #region Param1: Período
            DateTime _data1 = _data1 = Funcoes.Brasilia().Date.AddDays(-(int)param[0]);
            DateTime _data2 = Funcoes.Brasilia().Date;
            #endregion

            int? _EdificacaoID = (int?)param[1];
            int? _UnidadeID = (int?)param[2];
            int? _CondominoID = (int?)param[3];
            int? _FilaAtendimentoID = (int?)param[4];
            int? _ChamadoMotivoID = (int?)param[5];
            int? _ChamadoStatusID = (int?)param[6];
            #endregion

            return (from cha in db.Chamados
                    join FilaAtual in db.FilaAtendimentos on cha.FilaAtendimentoID equals FilaAtual.FilaAtendimentoID
                    join con in db.Condominos on cha.CondominoID equals con.CondominoID into CON
                    from con in CON.DefaultIfEmpty()
                    join edi in db.Edificacaos on cha.EdificacaoID equals edi.EdificacaoID into EDI
                    from edi in EDI.DefaultIfEmpty()
                    join sta in db.ChamadoStatuss on cha.ChamadoStatusID equals sta.ChamadoStatusID
                    join mot in db.ChamadoMotivos on cha.ChamadoMotivoID equals mot.ChamadoMotivoID
                    where cha.CondominioID == SessaoLocal.empresaId &&
                          cha.DataChamado >= _data1 && cha.DataChamado <= _data2 &&
                          sta.CondominioID == SessaoLocal.empresaId &&
                          mot.CondominioID == SessaoLocal.empresaId &&
                          (!_EdificacaoID.HasValue || (cha.EdificacaoID == _EdificacaoID && cha.UnidadeID == _UnidadeID)) &&
                          (!_CondominoID.HasValue || cha.CondominoID == _CondominoID) &&
                          (!_FilaAtendimentoID.HasValue || cha.FilaAtendimentoID == _FilaAtendimentoID) &&
                          (!_ChamadoMotivoID.HasValue || cha.ChamadoMotivoID == _ChamadoMotivoID) &&
                          (!_ChamadoStatusID.HasValue || cha.ChamadoStatusID == _ChamadoStatusID)
                    orderby cha.DataChamado descending
                    select new ChamadoViewModel
                    {
                        empresaId = SessaoLocal.empresaId,
                        ChamadoID = cha.ChamadoID,
                        ChamadoMotivoID = cha.ChamadoMotivoID,
                        DescricaoChamadoMotivo = mot.Descricao,
                        ChamadoStatusID = cha.ChamadoStatusID,
                        DescricaoChamadoStatus = sta.Descricao,
                        FilaSolicitanteID = cha.FilaSolicitanteID,
                        DescricaoFilaSolicitante = (from fil in db.FilaAtendimentos where fil.CondominioID == SessaoLocal.empresaId && fil.FilaAtendimentoID == cha.FilaSolicitanteID select fil.Descricao).FirstOrDefault(),
                        FilaAtendimentoID = cha.FilaAtendimentoID,
                        DescricaoFilaAtendimento = FilaAtual.Descricao,
                        CondominoID = cha.CondominoID,
                        CredenciadoID = cha.CredenciadoID,
                        NomeCondomino = con != null ? con.Nome : "",
                        EdificacaoID = cha.EdificacaoID,
                        DescricaoEdificacao = edi.Descricao,
                        UnidadeID = cha.UnidadeID,
                        DataChamado = cha.DataChamado,
                        Assunto = cha.Assunto,
                        UsuarioID = cha.UsuarioID,
                        NomeUsuario = cha.NomeUsuario,
                        LoginUsuario = cha.LoginUsuario,
                        Prioridade = cha.Prioridade,
                        DataUltimaAnotacao = cha.DataUltimaAnotacao,
                        DataRedirecionamento = cha.DataRedirecionamento,
                        UsuarioFilaID = cha.UsuarioFilaID,
                        NomeUsuarioFila = cha.NomeUsuarioFila,
                        LoginUsuarioFila = cha.LoginUsuarioFila,
                        PageSize = pageSize,
                        TotalCount = ((from cha1 in db.Chamados
                                       join FilaAtual1 in db.FilaAtendimentos on cha1.FilaAtendimentoID equals FilaAtual1.FilaAtendimentoID
                                       join con1 in db.Condominos on cha1.CondominoID equals con1.CondominoID into CON1
                                       from con1 in CON1.DefaultIfEmpty()
                                       join edi1 in db.Edificacaos on cha1.EdificacaoID equals edi1.EdificacaoID into EDI1
                                       from edi1 in EDI1.DefaultIfEmpty()
                                       join sta1 in db.ChamadoStatuss on cha1.ChamadoStatusID equals sta1.ChamadoStatusID
                                       join mot1 in db.ChamadoMotivos on cha1.ChamadoMotivoID equals mot1.ChamadoMotivoID
                                       where cha1.CondominioID == SessaoLocal.empresaId &&
                                             cha1.DataChamado >= _data1 && cha1.DataChamado <= _data2 &&
                                             sta1.CondominioID == SessaoLocal.empresaId &&
                                             mot1.CondominioID == SessaoLocal.empresaId &&
                                             (!_EdificacaoID.HasValue || (cha1.EdificacaoID == _EdificacaoID && cha1.UnidadeID == _UnidadeID)) &&
                                             (!_CondominoID.HasValue || cha1.CondominoID == _CondominoID) &&
                                             (!_FilaAtendimentoID.HasValue || cha1.FilaAtendimentoID == _FilaAtendimentoID) &&
                                             (!_ChamadoMotivoID.HasValue || cha1.ChamadoMotivoID == _ChamadoMotivoID) &&
                                             (!_ChamadoStatusID.HasValue || cha1.ChamadoStatusID == _ChamadoStatusID)
                                       orderby cha1.DataChamado descending
                                       select cha1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new ChamadoModel().getObject((ChamadoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-chamado-det";
        }
    }
}