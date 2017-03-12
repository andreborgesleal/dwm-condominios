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
using App_Dominio.Models;
using System.IO;
using App_Dominio.Security;

namespace DWM.Models.Persistence
{
    public class ChamadoAnexoModel : CrudModelLocal<ChamadoAnexo, ChamadoAnexoViewModel>
    {
        #region Constructor
        public ChamadoAnexoModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override ChamadoAnexoViewModel BeforeInsert(ChamadoAnexoViewModel value)
        {
            return GetUsuario(value);
        }

        public override ChamadoAnexoViewModel AfterInsert(ChamadoAnexoViewModel value)
        {
            try
            {
                #region Check if has file to transfer from Temp Folder to Users_Data Folder 
                if (!String.IsNullOrEmpty(value.FileID))
                {
                    #region Move the file from Temp Folder to Users_Data Folder
                    System.IO.FileInfo f = new System.IO.FileInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Temp"), value.FileID));
                    if (f.Exists)
                        f.MoveTo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Users_Data/Empresas/" + sessaoCorrente.empresaId.ToString() + "/Chamados"), value.FileID));
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

        public override ChamadoAnexo MapToEntity(ChamadoAnexoViewModel value)
        {
            ChamadoAnexo entity = new ChamadoAnexo();

            entity.ChamadoID = value.ChamadoID;
            entity.FileID = value.FileID;
            entity.DataAnexo = value.DataAnexo;
            entity.NomeOriginal = value.NomeOriginal;
            entity.UsuarioID = value.UsuarioID;
            entity.Nome = value.Nome;
            entity.Login = value.Login;

            return entity;
        }

        public override ChamadoAnexoViewModel MapToRepository(ChamadoAnexo entity)
        {
            return new ChamadoAnexoViewModel()
            {
                ChamadoID = entity.ChamadoID,
                FileID = entity.FileID,
                DataAnexo = entity.DataAnexo,
                NomeOriginal = entity.NomeOriginal,
                UsuarioID = entity.UsuarioID,
                Nome = entity.Nome,
                Login = entity.Login,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override ChamadoAnexo Find(ChamadoAnexoViewModel key)
        {
            return db.ChamadoAnexos.Find(key.ChamadoID, key.FileID);
        }

        public override Validate Validate(ChamadoAnexoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.ChamadoID == 0 && operation != Crud.INCLUIR)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Identificador do Chamado").ToString();
                value.mensagem.MessageBase = "Código identificador do chamado deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            if (String.IsNullOrEmpty(value.FileID))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Identificador do arquivo").ToString();
                value.mensagem.MessageBase = "Código identificador do arquivo deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            if (String.IsNullOrEmpty(value.NomeOriginal))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do arquivo").ToString();
                value.mensagem.MessageBase = "Código Nome do arquivo deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            };

            if (value.DataAnexo <= new DateTime(1980, 1, 1))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Data do anexo").ToString();
                value.mensagem.MessageBase = "Data do anexo deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.UsuarioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Usuário").ToString();
                value.mensagem.MessageBase = "Usuário deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            return value.mensagem;
        }

        public override ChamadoAnexoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            ChamadoAnexoViewModel value = base.CreateRepository(Request);
            if (Request != null && Request["ChamadoID"] != null)
                value.ChamadoID = int.Parse(Request["ChamadoID"]);
            return value;
        }
        #endregion

        #region Métodos customizados
        private ChamadoAnexoViewModel GetUsuario(ChamadoAnexoViewModel value)
        {
            if (value.UsuarioID == 0)
                value.UsuarioID = SessaoLocal.usuarioId;

            value.Nome = seguranca_db.Usuarios.Find(value.UsuarioID).nome; ;
            value.Login = seguranca_db.Usuarios.Find(value.UsuarioID).login;

            return value;
        }
        #endregion
    }

    public class ListViewChamadoAnexo : ListViewModelLocal<ChamadoAnexoViewModel>
    {
        #region Constructor
        public ListViewChamadoAnexo() { }

        public ListViewChamadoAnexo(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }

        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ChamadoAnexoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _ChamadoID = param != null && param.Count() > 0 && param[0] != null ? (int)param[0] : 0;

            if (_ChamadoID > 0)
                return (from cha in db.ChamadoAnexos
                        where cha.ChamadoID == _ChamadoID
                        orderby cha.DataAnexo descending
                        select new ChamadoAnexoViewModel
                        {
                            empresaId = SessaoLocal.empresaId,
                            ChamadoID = cha.ChamadoID,
                            FileID = cha.FileID,
                            DataAnexo = cha.DataAnexo,
                            NomeOriginal = cha.NomeOriginal,
                            UsuarioID = cha.UsuarioID,
                            Nome = cha.Nome,
                            Login = cha.Login,
                            PageSize = pageSize,
                            TotalCount = ((from cha1 in db.ChamadoAnexos
                                           where cha1.ChamadoID == _ChamadoID
                                           orderby cha1.DataAnexo descending
                                           select cha1).Count())
                        }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            else
                return new List<ChamadoAnexoViewModel>();
        }

        public override Repository getRepository(Object id)
        {
            return new ChamadoAnexoModel().getObject((ChamadoAnexoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-chamado-anexo";
        }
    }
}