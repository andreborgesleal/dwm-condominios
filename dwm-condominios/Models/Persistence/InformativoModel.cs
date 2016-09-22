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

        public override InformativoViewModel AfterInsert(InformativoViewModel value)
        {
            try
            {
                #region Check if has file to transfer from Temp Folder to Users_Data Folder 
                if (value.Midia1 != null && value.Midia1 != "")
                {
                    #region Move the file from Temp Folder to Users_Data/Informativos Folder
                    FileInfo f = new FileInfo(Path.Combine(HttpContext.Current.Server.MapPath("~/Temp"), value.Midia1));
                    if (f.Exists)
                        f.MoveTo(Path.Combine(HttpContext.Current.Server.MapPath("~/Users_Data/Empresas/" + sessaoCorrente.empresaId.ToString() + "/Informativos"), value.Midia1));
                    #endregion
                }
                if (value.Midia2 != null && value.Midia2 != "")
                {
                    #region Move the file from Temp Folder to Users_Data/Informativos Folder
                    FileInfo f = new FileInfo(Path.Combine(HttpContext.Current.Server.MapPath("~/Temp"), value.Midia2));
                    if (f.Exists)
                        f.MoveTo(Path.Combine(HttpContext.Current.Server.MapPath("~/Users_Data/Empresas/" + sessaoCorrente.empresaId.ToString() + "/Informativos"), value.Midia2));
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

        public override Informativo MapToEntity(InformativoViewModel value)
        {
            Informativo entity = Find(value);

            if (entity == null)
                entity = new Informativo();

            entity.InformativoID = value.InformativoID;
            entity.CondominioID = value.CondominioID;
            entity.EdificacaoID = value.EdificacaoID;
            entity.GrupoCondominoID = value.GrupoCondominoID;
            entity.DataInformativo = Funcoes.Brasilia();
            entity.DataPublicacao = value.DataPublicacao;
            entity.DataExpiracao = value.DataExpiracao;
            entity.Cabecalho = value.Cabecalho;
            entity.Resumo = value.Resumo;
            entity.MensagemDetalhada = value.MensagemDetalhada;
            entity.Midia1 = value.Midia1;
            entity.Midia2 = value.Midia2;
            entity.InformativoAnuncio = "N";

            return entity;
        }

        public override InformativoViewModel MapToRepository(Informativo entity)
        {
            return new InformativoViewModel()
            {
                InformativoID = entity.InformativoID,
                CondominioID = entity.CondominioID,
                EdificacaoID = entity.EdificacaoID,
                GrupoCondominoID = entity.GrupoCondominoID,
                DataInformativo = entity.DataInformativo,
                DataPublicacao = entity.DataPublicacao,
                DataExpiracao = entity.DataExpiracao,
                Cabecalho = entity.Cabecalho,
                Resumo = entity.Resumo,
                MensagemDetalhada = entity.MensagemDetalhada,
                Midia1 = entity.Midia1,
                Midia2 = entity.Midia2,
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

            if (operation != Crud.INCLUIR && value.InformativoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Credenciado").ToString();
                value.mensagem.MessageBase = "Código identificador do informativo deve ser informado";
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

                if (value.DataPublicacao == null || value.DataPublicacao <= new DateTime(1980,1,1))
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Data da Publicação").ToString();
                    value.mensagem.MessageBase = "Data da Publicação deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.DataExpiracao != null && value.DataExpiracao < value.DataPublicacao)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "DataExpiracao").ToString();
                    value.mensagem.MessageBase = "A Data de Expiração deve ser maior que a Data de Publicação";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Cabecalho == null || value.Cabecalho.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Cabeçalho").ToString();
                    value.mensagem.MessageBase = "Cabeçalho deve ser preenchido";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Resumo == null || value.Resumo.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Resumo").ToString();
                    value.mensagem.MessageBase = "Resumo deve ser preenchido";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
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

            DateTime data1 = param.Count() > 0 && param[0] != null ? (DateTime)param[0] : new DateTime(1980, 1, 1);
            DateTime data2 = param.Count() > 1 && param[1] != null ? (DateTime)param[1] : Funcoes.Brasilia().Date.AddDays(30);

            return (from info in db.Informativos
                    join gru in db.GrupoCondominos on info.GrupoCondominoID equals gru.GrupoCondominoID into GRU
                    from gru in GRU.DefaultIfEmpty()
                    join edi in db.Edificacaos on info.EdificacaoID equals edi.EdificacaoID into EDI
                    from edi in EDI.DefaultIfEmpty()
                    where info.DataPublicacao >= data1 && info.DataPublicacao <= data2
                            && info.CondominioID == sessaoCorrente.empresaId
                    orderby info.DataPublicacao
                    select new InformativoViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        InformativoID = info.InformativoID,
                        CondominioID = info.CondominioID,
                        EdificacaoID = info.EdificacaoID,
                        descricao_edificacao = edi.Descricao,
                        GrupoCondominoID = gru.GrupoCondominoID,
                        descricao_GrupoCondomino = gru.Descricao,
                        DataExpiracao = info.DataExpiracao,
                        DataInformativo = info.DataInformativo,
                        DataPublicacao = info.DataPublicacao,
                        Cabecalho = info.Cabecalho,
                        Resumo = info.Resumo,
                        MensagemDetalhada = info.MensagemDetalhada,
                        Midia1 = info.Midia1,
                        Midia2 = info.Midia2,
                        InformativoAnuncio = info.InformativoAnuncio,
                        PageSize = pageSize,
                        TotalCount = (from info1 in db.Informativos
                                      join gru1 in db.GrupoCondominos on info1.GrupoCondominoID equals gru1.GrupoCondominoID into GRU1
                                      from gru1 in GRU1.DefaultIfEmpty()
                                      join edi1 in db.Edificacaos on info1.EdificacaoID equals edi1.EdificacaoID into EDI1
                                      from edi1 in EDI.DefaultIfEmpty()
                                      where info1.DataPublicacao >= data1 && info1.DataPublicacao <= data2
                                              && info1.CondominioID == sessaoCorrente.empresaId
                                      orderby info1.DataPublicacao
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