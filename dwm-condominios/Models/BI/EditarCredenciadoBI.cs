﻿using System;
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

namespace DWM.Models.BI
{
    public class EditarCredenciadoBI : DWMContextLocal, IProcess<CredenciadoViewModel, ApplicationContext>
    {
        private string Operacao { get; set; }

        #region Constructor
        public EditarCredenciadoBI(string operacao)
        {
            this.Operacao = operacao;
        }

        public EditarCredenciadoBI(ApplicationContext _db, SecurityContext _segurancaDb)
        {
            this.Create(_db, _segurancaDb);
        }

        #endregion

        public CredenciadoViewModel Run(Repository value)
        {
            bool EnviaEmail = false;

            CredenciadoViewModel r = (CredenciadoViewModel)value;
            CredenciadoViewModel result = new CredenciadoViewModel()
            {
                uri = r.uri,
                empresaId = sessaoCorrente.empresaId,
                CredenciadoID = r.CredenciadoID,
                CondominoID = r.CondominoID,
                Nome = r.Nome,
                Email = r.Email,
                TipoCredenciadoID = r.TipoCredenciadoID,
                Sexo = r.Sexo,
                Observacao = r.Observacao,
                UsuarioID = r.UsuarioID,
                IndVisitantePermanente = r.IndVisitantePermanente,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso" }
            };

            try
            {
                int _empresaId = SessaoLocal.empresaId;
                string _keyword = null;

                CredenciadoModel CredenciadoModel = new CredenciadoModel(this.db, this.seguranca_db);

                if (r.CredenciadoID == 0) // Incluir credenciado
                {
                    #region Validar Credenciado
                    if (CredenciadoModel.Validate(result, Crud.INCLUIR).Code > 0)
                        throw new App_DominioException(result.mensagem);
                    #endregion

                    #region Cadastrar o credenciado como um usuário em DWM-Segurança
                    
                    if (!string.IsNullOrEmpty(result.Email))
                    {
                        Random random = new Random();
                        string _senha = random.Next(9999, 999999).ToString();
                        _keyword = random.Next(9999, 99999999).ToString();
                        int _grupoId = int.Parse(db.Parametros.Find(_empresaId, (int)Enumeracoes.Enumeradores.Param.GRUPO_CREDENCIADO).Valor);

                        #region Usuario 
                        EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();

                        Usuario user = new Usuario()
                        {
                            nome = r.Nome.ToUpper(),
                            login = r.Email,
                            empresaId = _empresaId,
                            dt_cadastro = Funcoes.Brasilia(),
                            situacao = "D",
                            isAdmin = "N",
                            senha = security.Criptografar(_senha),
                            keyword = _keyword,
                            dt_keyword = Funcoes.Brasilia().AddDays(1)
                        };

                        // Verifica se o E-mail do usuário já não existe para a empresa
                        if (seguranca_db.Usuarios.Where(info => info.empresaId == _empresaId && info.login == r.Email).Count() > 0)
                            throw new ArgumentException("E-mail já cadastrado");

                        seguranca_db.Usuarios.Add(user);
                        #endregion

                        #region UsuarioGrupo
                        UsuarioGrupo ug = new UsuarioGrupo()
                        {
                            Usuario = user,
                            grupoId = _grupoId,
                            situacao = "A"
                        };

                        seguranca_db.UsuarioGrupos.Add(ug);
                        #endregion

                        seguranca_db.SaveChanges();

                        result.UsuarioID = user.usuarioId;
                        EnviaEmail = true;
                    }
                    #endregion

                    #region Incluir o credenciado
                    result = CredenciadoModel.Insert(result);
                    result.mensagem.Field = _keyword;
                    #endregion
                }
                else if (Operacao == "S") // Alterar credenciado
                {
                    #region Validar Credenciado
                    if (CredenciadoModel.Validate(result, Crud.ALTERAR).Code > 0)
                        throw new App_DominioException(result.mensagem);
                    #endregion

                    #region Atualiza o cadastro do usuário
                    if (result.UsuarioID.HasValue && result.UsuarioID > 0 && !string.IsNullOrEmpty(result.Email)) // antes existia UsuarioID e tem E-mail
                    {
                        Usuario user = seguranca_db.Usuarios.Find(result.UsuarioID.Value);

                        if (user != null)
                        {
                            user.login = result.Email;
                            user.nome = result.Nome.ToUpper();
                            user.dt_cadastro = Funcoes.Brasilia();

                            seguranca_db.Entry(user).State = EntityState.Modified;

                            seguranca_db.SaveChanges();
                        }
                    }
                    else if ((!result.UsuarioID.HasValue || result.UsuarioID.Value == 0) && !string.IsNullOrEmpty(result.Email)) // antes não existia UsuarioID e na altração passou a existir (e-mail)
                    {
                        Random random = new Random();
                        string _senha = random.Next(9999, 999999).ToString();
                        _keyword = random.Next(9999, 99999999).ToString();
                        int _grupoId = int.Parse(db.Parametros.Find(_empresaId, (int)Enumeracoes.Enumeradores.Param.GRUPO_CREDENCIADO).Valor);

                        #region Usuario 
                        EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();

                        Usuario user = new Usuario()
                        {
                            nome = r.Nome.ToUpper(),
                            login = r.Email,
                            empresaId = _empresaId,
                            dt_cadastro = Funcoes.Brasilia(),
                            situacao = "D",
                            isAdmin = "N",
                            senha = security.Criptografar(_senha),
                            keyword = _keyword,
                            dt_keyword = Funcoes.Brasilia().AddDays(1)
                        };

                        // Verifica se o E-mail do usuário já não existe para a empresa
                        if (seguranca_db.Usuarios.Where(info => info.empresaId == _empresaId && info.login == r.Email).Count() > 0)
                            throw new ArgumentException("E-mail já cadastrado");

                        seguranca_db.Usuarios.Add(user);
                        #endregion

                        #region UsuarioGrupo
                        UsuarioGrupo ug = new UsuarioGrupo()
                        {
                            Usuario = user,
                            grupoId = _grupoId,
                            situacao = "A"
                        };

                        seguranca_db.UsuarioGrupos.Add(ug);
                        #endregion

                        seguranca_db.SaveChanges();

                        result.UsuarioID = user.usuarioId;

                        EnviaEmail = true;
                    }
                    else if(result.UsuarioID.HasValue && result.UsuarioID > 0 && string.IsNullOrEmpty(result.Email)) // antes existia usuário e agora não existe mais => Exclui o usuário em dwm-segurança
                    {
                        #region Exclui o cadastro do usuário
                        int _grupoId = int.Parse(db.Parametros.Find(_empresaId, (int)Enumeracoes.Enumeradores.Param.GRUPO_CREDENCIADO).Valor);

                        // Exclui o usuário do Grupo
                        UsuarioGrupo ug = seguranca_db.UsuarioGrupos.Find(result.UsuarioID, _grupoId);
                        seguranca_db.Set<UsuarioGrupo>().Remove(ug);

                        // Exclui o usuário da tabela Sessao
                        seguranca_db.Database.ExecuteSqlCommand("delete from Sessao where usuarioId = " + result.UsuarioID.ToString() + " and empresaId = " + sessaoCorrente.empresaId.ToString());

                        // Exclui o usuário 
                        Usuario user = seguranca_db.Usuarios.Find(result.UsuarioID);
                        seguranca_db.Set<Usuario>().Remove(user);

                        seguranca_db.SaveChanges();
                        #endregion

                        result.UsuarioID = null;
                    }
                    #endregion

                    #region Alterar credenciado
                    result = CredenciadoModel.Update(result);
                    #endregion
                }
                else // Excluir credenciado
                {
                    #region Validar Credenciado
                    if (CredenciadoModel.Validate(result, Crud.EXCLUIR).Code > 0)
                        throw new App_DominioException(result.mensagem);
                    #endregion

                    #region Exclui o cadastro do usuário
                    if (result.UsuarioID.HasValue && result.UsuarioID.Value > 0)
                    {
                        int _grupoId = int.Parse(db.Parametros.Find(_empresaId, (int)Enumeracoes.Enumeradores.Param.GRUPO_CREDENCIADO).Valor);

                        // Exclui o usuário do Grupo
                        UsuarioGrupo ug = seguranca_db.UsuarioGrupos.Find(result.UsuarioID, _grupoId);
                        seguranca_db.Set<UsuarioGrupo>().Remove(ug);

                        // Exclui o usuário da tabela Sessao
                        seguranca_db.Database.ExecuteSqlCommand("delete from Sessao where usuarioId = " + result.UsuarioID.ToString() + " and empresaId = " + sessaoCorrente.empresaId.ToString());

                        // Exclui o usuário 
                        Usuario user = seguranca_db.Usuarios.Find(result.UsuarioID);
                        seguranca_db.Set<Usuario>().Remove(user);

                        seguranca_db.SaveChanges();
                    }
                    #endregion

                    #region Alterar credenciado
                    result = CredenciadoModel.Delete(result);
                    #endregion
                }

                if (result.mensagem.Code > 0)
                    throw new App_DominioException(result.mensagem);

                db.SaveChanges();

                if (EnviaEmail)
                {
                    int _sistemaId = int.Parse(db.Parametros.Find(SessaoLocal.empresaId, (int)Enumeracoes.Enumeradores.Param.SISTEMA).Valor);
                    string _URL_CONDOMINIO = db.Parametros.Find(SessaoLocal.empresaId, (int)Enumeracoes.Enumeradores.Param.URL_CONDOMINIO).Valor;
                    #region envio de e-mail ao credenciado para ativação
                    int EmailTipoID = (int)DWM.Models.Enumeracoes.Enumeradores.EmailTipo.CADASTRO_CREDENCIADO;
                    string EmailMensagem = db.EmailTemplates.Where(info => info.CondominioID == SessaoLocal.empresaId && info.EmailTipoID == EmailTipoID).FirstOrDefault().EmailMensagem;
                    EmailMensagem = EmailMensagem.Replace("@link_credenciado", "<p><a href=\"" + _URL_CONDOMINIO + "/Account/AtivarCredenciado?id=" + result.UsuarioID.ToString() + "&key=" + _keyword + "\" target=\"_blank\"><span style=\"font-family: Verdana; font-size: small; color: #0094ff\">Acesso ao " + seguranca_db.Sistemas.Find(_sistemaId).descricao + "</span></a></p>");

                    CondominoUnidade cu = (from cou in db.CondominoUnidades
                                           where cou.CondominioID == SessaoLocal.empresaId
                                                 && cou.CondominoID == r.CondominoID
                                           select cou).FirstOrDefault();

                    EmailLogViewModel EmailLogViewModel = new EmailLogViewModel()
                    {
                        uri = r.uri,
                        empresaId = SessaoLocal.empresaId,
                        EmailTipoID = EmailTipoID, // "Cadastro Credenciado"
                        CondominioID = SessaoLocal.empresaId,
                        EdificacaoID = cu.EdificacaoID,
                        Descricao_Edificacao = db.Edificacaos.Find(cu.EdificacaoID).Descricao,
                        UnidadeID = cu.UnidadeID,
                        GrupoCondominoID = null,
                        Descricao_GrupoCondomino = "",
                        DataEmail = Funcoes.Brasilia(),
                        Assunto = db.EmailTipos.Find(EmailTipoID, SessaoLocal.empresaId).Assunto,
                        EmailMensagem = EmailMensagem,
                        Nome = r.Nome,
                        Email = r.Email
                    };

                    EmailNotificacaoBI notificacaoBI = new EmailNotificacaoBI(this.db, this.seguranca_db);
                    EmailLogViewModel = notificacaoBI.Run(EmailLogViewModel);
                    if (EmailLogViewModel.mensagem.Code > 0)
                        throw new App_DominioException(EmailLogViewModel.mensagem);


                    //result.CredenciadoViewModel.mensagem.Field = factory.Mensagem.Field; // senha do credenciado
                    //EnviarEmailCredenciadoBI EnviarEmailCredenciadoBI = new EnviarEmailCredenciadoBI(this.db, this.seguranca_db);
                    //CredenciadoViewModel repository = EnviarEmailCredenciadoBI.Run(result);
                    //if (repository.mensagem.Code > 0)
                    //    throw new ArgumentException(repository.mensagem.MessageBase);
                    #endregion
                }
                else
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

        public IEnumerable<CredenciadoViewModel> List(params object[] param)
        {
            ListViewCredenciados ListCredenciados = new ListViewCredenciados(this.db, this.seguranca_db);
            return ListCredenciados.Bind(0, 50, param);
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}