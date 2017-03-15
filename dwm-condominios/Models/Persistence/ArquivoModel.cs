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
using System.IO;

namespace DWM.Models.Persistence
{
    public class ArquivoModel : CrudModelLocal<Arquivo, ArquivoViewModel>
    {
        #region Constructor
        public ArquivoModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override ArquivoViewModel AfterInsert(ArquivoViewModel value)
        {
            try
            {
                #region Check if has file to transfer from Temp Folder to Users_Data Folder 
                if (!String.IsNullOrEmpty(value.FileID))
                {
                    #region Move the file from Temp Folder to Users_Data Folder
                    System.IO.FileInfo f = new System.IO.FileInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Temp"), value.FileID));
                    if (f.Exists)
                        f.MoveTo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Users_Data/Empresas/" + sessaoCorrente.empresaId.ToString() + "/download"), value.FileID));
                    #endregion
                }
                #endregion
            }
            catch (DirectoryNotFoundException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Path de armazenamento do arquivo de boleto/comprovante não encontrado";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (FileNotFoundException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Arquivo de boleto/comprovante não encontrado";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (IOException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Erro referente ao arquivo de boleto/comprovante";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (Exception ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message;
                value.mensagem.MessageType = MsgType.ERROR;
            }
            return value;
        }

        public override ArquivoViewModel AfterDelete(ArquivoViewModel value)
        {
            try
            {
                #region Delete the file
                if (!String.IsNullOrEmpty(value.FileID))
                {
                    #region Delete the file in download Folder 
                    System.IO.FileInfo f = new System.IO.FileInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath(value.Path()), value.FileID));
                    if (f.Exists)
                        f.Delete(); 
                    #endregion
                }
                #endregion
            }
            catch (DirectoryNotFoundException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Path de armazenamento do arquivo não encontrado";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (FileNotFoundException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Arquivo não encontrado";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (IOException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Erro referente ao arquivo";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (Exception ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message;
                value.mensagem.MessageType = MsgType.ERROR;
            }
            return value;
        }

        public override Arquivo MapToEntity(ArquivoViewModel value)
        {
            Arquivo entity = Find(value);

            if (entity == null)
            {
                entity = new Arquivo();
                entity.Data = Funcoes.Brasilia();
            }
            else
                entity.Data = value.Data;

            entity.FileID = value.FileID;
            entity.CondominioID = sessaoCorrente.empresaId;
            entity.EdificacaoID = value.EdificacaoID;
            entity.UnidadeID = value.UnidadeID;
            entity.GrupoCondominoID = value.GrupoCondominoID;
            entity.Nome = value.Nome;
            entity.IndSempreVisivel = value.IndSempreVisivel;

            if (entity.CondominioID > 0 && entity.EdificacaoID.HasValue && entity.UnidadeID.HasValue)
            {
                entity.CondominoID = (from cu in db.CondominoUnidades
                                      where cu.CondominioID == entity.CondominioID
                                            && cu.EdificacaoID == entity.EdificacaoID
                                            && cu.UnidadeID == entity.UnidadeID
                                            && cu.DataFim == null
                                      select cu.CondominoID).FirstOrDefault();
            }
            else
                entity.CondominoID = null;

            return entity;
        }

        public override ArquivoViewModel MapToRepository(Arquivo entity)
        {
            ArquivoViewModel value = new ArquivoViewModel()
            {
                FileID = entity.FileID,
                CondominioID = entity.CondominioID,
                EdificacaoID = entity.EdificacaoID,
                UnidadeID = entity.UnidadeID,
                CondominoID = entity.CondominoID,
                GrupoCondominoID = entity.GrupoCondominoID,
                Nome = entity.Nome,
                Data = entity.Data,
                IndSempreVisivel = entity.IndSempreVisivel,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };

            if (entity.CondominoID.HasValue)
                value.CondominoNome = db.Condominos.Find(entity.CondominoID.Value).Nome;

            if (entity.EdificacaoID.HasValue)
                value.EdificacaoDescricao = db.Edificacaos.Find(entity.EdificacaoID.Value).Descricao;

            if (entity.GrupoCondominoID.HasValue)
                value.GrupoCondominoDescricao = db.GrupoCondominos.Find(entity.GrupoCondominoID).Descricao;

            return value;
        }

        public override Arquivo Find(ArquivoViewModel key)
        {
            return db.Arquivos.Find(key.FileID);
        }

        public override Validate Validate(ArquivoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && String.IsNullOrEmpty(value.FileID))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "File ID").ToString();
                value.mensagem.MessageBase = "Identificador do arquivo deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            if (operation != Crud.EXCLUIR)
            {
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

                if (value.Nome == null || value.Nome.Trim().Length <= 3)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Nome do arquivo").ToString();
                    value.mensagem.MessageBase = "Nome do arquivo deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Data == null || value.Data <= new DateTime(1980,1,1))
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Data deve ser informada").ToString();
                    value.mensagem.MessageBase = "Data do envio do arquivo deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

            }
            return value.mensagem;
        }

        public override ArquivoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            ArquivoViewModel value = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            value.CondominioID = security.getSessaoCorrente().empresaId;
            value.Data = Funcoes.Brasilia();
            return value;
        }
        #endregion
    }

    public class ListViewArquivo : ListViewModelLocal<ArquivoViewModel>
    {
        #region Constructor
        public ListViewArquivo() { }

        public ListViewArquivo(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ArquivoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            System.DateTime _data1 = (DateTime)param[0];
            System.DateTime _data2 = (DateTime)param[1];
            int? _EdificacaoID = (int?)param[2];
            int? _UnidadeID = (int?)param[3];
            int? _CondominoID = (int?)param[4];
            int? _GrupoCondominoID = (int?)param[5];

            return (from arq in db.Arquivos join edi in db.Edificacaos on arq.EdificacaoID equals edi.EdificacaoID
                    join gru in db.GrupoCondominos on arq.GrupoCondominoID equals gru.GrupoCondominoID
                    join con in db.Condominos on arq.CondominoID equals con.CondominoID
                    where arq.CondominioID == sessaoCorrente.empresaId
                          && ((arq.Data >= _data1 && arq.Data <= _data2) || arq.IndSempreVisivel == "S")
                          && (!_EdificacaoID.HasValue || arq.EdificacaoID == _EdificacaoID) 
                          && (!_UnidadeID.HasValue || arq.UnidadeID == _UnidadeID)
                          && (!_CondominoID.HasValue || arq.CondominoID == _CondominoID)
                          && (!_GrupoCondominoID.HasValue || arq.GrupoCondominoID == _GrupoCondominoID)
                    orderby arq.Data descending
                    select new ArquivoViewModel()
                    {
                        empresaId = SessaoLocal.empresaId,
                        CondominioID = SessaoLocal.empresaId,
                        FileID = arq.FileID,
                        EdificacaoID = arq.EdificacaoID,
                        EdificacaoDescricao = edi.Descricao,
                        UnidadeID = arq.UnidadeID,
                        CondominoID = arq.CondominoID,
                        CondominoNome = con.Nome,
                        GrupoCondominoID = arq.GrupoCondominoID,
                        GrupoCondominoDescricao = gru.Descricao,
                        Nome = arq.Nome,
                        Data = arq.Data,
                        IndSempreVisivel = arq.IndSempreVisivel,
                        PageSize = pageSize,
                        TotalCount = ((from arq1 in db.Arquivos join edi1 in db.Edificacaos on arq1.EdificacaoID equals edi1.EdificacaoID
                                        join gru1 in db.GrupoCondominos on arq1.GrupoCondominoID equals gru1.GrupoCondominoID
                                        join con1 in db.Condominos on arq1.CondominoID equals con1.CondominoID
                                        where arq1.CondominioID == sessaoCorrente.empresaId
                                              && ((arq1.Data >= _data1 && arq1.Data <= _data2) || arq1.IndSempreVisivel == "S")
                                              && (!_EdificacaoID.HasValue || arq1.EdificacaoID == _EdificacaoID)
                                              && (!_UnidadeID.HasValue || arq1.UnidadeID == _UnidadeID)
                                              && (!_CondominoID.HasValue || arq1.CondominoID == _CondominoID)
                                              && (!_GrupoCondominoID.HasValue || arq1.GrupoCondominoID == _GrupoCondominoID)
                                       select arq1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new ArquivoModel().getObject((ArquivoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-download";
        }
    }
}