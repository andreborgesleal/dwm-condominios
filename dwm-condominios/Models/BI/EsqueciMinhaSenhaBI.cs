using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using System.Web.Mvc;
using DWM.Models.Persistence;
using App_Dominio.Enumeracoes;
using App_Dominio.Security;
using System.Linq;
using System.Data.Entity.Infrastructure;
using App_Dominio.Models;
using System.Data.Entity;
using App_Dominio.Repositories;

namespace DWM.Models.BI
{
    public class EsqueciMinhaSenhaBI : DWMContextLocal, IProcess<UsuarioViewModel, ApplicationContext>
    {
        #region Constructor
        public EsqueciMinhaSenhaBI() { }

        public override void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public EsqueciMinhaSenhaBI(ApplicationContext _db, SecurityContext _segurancaDb)
        {
            this.db = _db;
            this.seguranca_db = _segurancaDb;
        }
        #endregion

        public UsuarioViewModel Run(Repository value)
        {
            UsuarioViewModel r = (UsuarioViewModel)value;
            UsuarioViewModel result = new UsuarioViewModel()
            {
                uri = r.uri,
                empresaId = r.empresaId,
                login = r.login,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso" }
            };

            try
            {
                #region Validar LOGIN
                Usuario u = seguranca_db.Usuarios.Where(info => info.login == r.login && info.empresaId == r.empresaId).FirstOrDefault();

                if (u == null)
                {
                    result.mensagem = new Validate() { Code = 999, Message = "Login inválido ou inexistente." };
                    throw new App_DominioException(result.mensagem);
                }
                else if (u.situacao != "A")
                {
                    result.mensagem = new Validate() { Code = 998, Message = "Este login possui pendências administrativas. Favor entrar em contato com a secretaria para providenciar a atualização." };
                    throw new App_DominioException(result.mensagem);
                };
                #endregion

                #region Atualizar o cadastro do usuário com a keyword
                Random random = new Random();
                u.keyword = random.Next(9999, 99999999).ToString();
                u.dt_keyword = Funcoes.Brasilia().AddDays(1);

                seguranca_db.Entry(u).State = EntityState.Modified;
                seguranca_db.SaveChanges();
                #endregion

                #region Enviar e-mail 
                int _sistemaId = int.Parse(db.Parametros.Find(r.empresaId, (int)Enumeracoes.Enumeradores.Param.SISTEMA).Valor);
                string _URL_CONDOMINIO = db.Parametros.Find(r.empresaId, (int)Enumeracoes.Enumeradores.Param.URL_CONDOMINIO).Valor;
                int EmailTipoID = (int)Enumeracoes.Enumeradores.EmailTipo.FORGOT;
                string EmailMensagem = db.EmailTemplates.Where(info => info.CondominioID == r.empresaId && info.EmailTipoID == EmailTipoID).FirstOrDefault().EmailMensagem;
                EmailMensagem = EmailMensagem.Replace("@link_esqueci_minha_senha", "<p><a href=\"" + _URL_CONDOMINIO + "/Account/EsqueciMinhaSenha?id=" + u.usuarioId.ToString() + "&key=" + u.keyword + "\" target=\"_blank\"><span style=\"font-family: Verdana; font-size: small; color: #0094ff\">recuperar senha</span></a></p>");
                EmailMensagem = EmailMensagem.Replace("@nome", u.nome).Replace("@email", u.login);
                #region Verifica se o usuário é um condômino
                CondominoUnidade cu = null;
                int? _EdificacaoID = null;
                string _Descricao_Unidade = null;
                int? _UnidadeID = null;
                CondominoPF CondominoPF = db.CondominoPFs.Where(info => info.UsuarioID == u.usuarioId && info.CondominioID == u.empresaId && info.IndSituacao == "A").FirstOrDefault();
                if (CondominoPF != null)
                {
                    cu = (from cou in db.CondominoUnidades
                          where cou.CondominoID == CondominoPF.CondominoID
                          select cou).FirstOrDefault();

                    _EdificacaoID = cu.EdificacaoID;
                    _Descricao_Unidade = db.Edificacaos.Find(cu.EdificacaoID).Descricao;
                    _UnidadeID = cu.UnidadeID;
                }
                #endregion

                #region Envia o e-mail de renovação de senha
                EmailLogViewModel EmailLogViewModel = new EmailLogViewModel()
                {
                    uri = r.uri,
                    empresaId = u.empresaId,
                    EmailTipoID = EmailTipoID, // "Esqueci minha senha"
                    CondominioID = u.empresaId,
                    EdificacaoID = _EdificacaoID,
                    Descricao_Edificacao = _Descricao_Unidade,
                    UnidadeID = _UnidadeID,
                    GrupoCondominoID = null,
                    Descricao_GrupoCondomino = "",
                    DataEmail = Funcoes.Brasilia(),
                    Assunto = db.EmailTipos.Find(EmailTipoID, u.empresaId).Assunto,
                    EmailMensagem = EmailMensagem,
                    Nome = u.nome,
                    Email = u.login
                };

                EmailNotificacaoBI notificacaoBI = new EmailNotificacaoBI(this.db, this.seguranca_db, true);
                EmailLogViewModel = notificacaoBI.Run(EmailLogViewModel);
                if (EmailLogViewModel.mensagem.Code > 0)
                    throw new App_DominioException(EmailLogViewModel.mensagem);
                db.SaveChanges();
                #endregion
                #endregion

                result.mensagem.Code = -1; // Tem que devolver -1 porque na Superclasse, se devolver zero, vai executar novamente o SaveChanges.

            }
            catch (ArgumentException ex)
            {
                result.mensagem = new Validate() { Code = 997, Message = MensagemPadrao.Message(997).ToString(), MessageBase = ex.Message };
            }
            catch (App_DominioException ex)
            {
                result.mensagem = ex.Result;

                if (ex.InnerException != null)
                    result.mensagem.MessageBase = new App_DominioException(ex.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                else
                    result.mensagem.MessageBase = new App_DominioException(ex.Result.Message, GetType().FullName).Message;
            }
            catch (DbUpdateException ex)
            {
                result.mensagem.MessageBase = ex.InnerException.InnerException.Message ?? ex.Message;
                if (result.mensagem.MessageBase.ToUpper().Contains("REFERENCE"))
                {
                    if (result.mensagem.MessageBase.ToUpper().Contains("DELETE"))
                    {
                        result.mensagem.Code = 16;
                        result.mensagem.Message = MensagemPadrao.Message(16).ToString();
                        result.mensagem.MessageType = MsgType.ERROR;
                    }
                    else
                    {
                        result.mensagem.Code = 28;
                        result.mensagem.Message = MensagemPadrao.Message(28).ToString();
                        result.mensagem.MessageType = MsgType.ERROR;
                    }
                }
                else if (result.mensagem.MessageBase.ToUpper().Contains("PRIMARY"))
                {
                    result.mensagem.Code = 37;
                    result.mensagem.Message = MensagemPadrao.Message(37).ToString();
                    result.mensagem.MessageType = MsgType.WARNING;
                }
                else if (result.mensagem.MessageBase.ToUpper().Contains("UNIQUE KEY"))
                {
                    result.mensagem.Code = 54;
                    result.mensagem.Message = MensagemPadrao.Message(54).ToString();
                    result.mensagem.MessageType = MsgType.WARNING;
                }
                else
                {
                    result.mensagem.Code = 44;
                    result.mensagem.Message = MensagemPadrao.Message(44).ToString();
                    result.mensagem.MessageType = MsgType.ERROR;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                result.mensagem = new Validate() { Code = 42, Message = MensagemPadrao.Message(42).ToString(), MessageBase = ex.EntityValidationErrors.Select(m => m.ValidationErrors.First().ErrorMessage).First() };
            }
            catch (Exception ex)
            {
                result.mensagem.Code = 17;
                result.mensagem.Message = MensagemPadrao.Message(17).ToString();
                result.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                result.mensagem.MessageType = MsgType.ERROR;
            }
            return result;
        }

        public IEnumerable<UsuarioViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }
    }
}