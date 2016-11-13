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

namespace DWM.Models.BI
{
    public class EditarFuncionarioBI : DWMContextLocal, IProcess<FuncionarioViewModel, ApplicationContext>
    {
        private string Operacao { get; set; }

        #region Constructor
        public EditarFuncionarioBI(string operacao)
        {
            this.Operacao = operacao;
        }

        public EditarFuncionarioBI(ApplicationContext _db, SecurityContext _segurancaDb)
        {
            this.Create(_db, _segurancaDb);
        }

        #endregion

        public FuncionarioViewModel Run(Repository value)
        {
            FuncionarioViewModel r = (FuncionarioViewModel)value;
            FuncionarioViewModel result = new FuncionarioViewModel()
            {
                uri = r.uri,
                empresaId = sessaoCorrente.empresaId,
                FuncionarioID = r.FuncionarioID,
                CondominioID = r.CondominioID,
                EdificacaoID = r.EdificacaoID,
                UnidadeID = r.UnidadeID,
                CondominoID = r.CondominoID,
                Nome = r.Nome,
                Funcao = r.Funcao,
                Sexo = r.Sexo,
                Dia = r.Dia,
                HoraInicial = r.HoraInicial,
                HoraFinal = r.HoraFinal,
                RG = r.RG,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso" }
            };

            try
            {
                int _empresaId = sessaoCorrente.empresaId;

                FuncionarioModel FuncionarioModel = new FuncionarioModel(this.db, this.seguranca_db);

                if (Operacao == "I") // Incluir veículo
                    result = FuncionarioModel.Insert(result);
                else if (Operacao == "S")
                    result = FuncionarioModel.Update(result);
                else
                    result = FuncionarioModel.Delete(result);

                if (result.mensagem.Code > 0)
                    throw new App_DominioException(result.mensagem);

                db.SaveChanges();
                seguranca_db.SaveChanges();

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
                if (result.mensagem.MessageBase.ToUpper().Contains("REFERENCE") || result.mensagem.MessageBase.ToUpper().Contains("FOREIGN"))
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

        public IEnumerable<FuncionarioViewModel> List(params object[] param)
        {
            ListViewFuncionarios ListFuncionarios = new ListViewFuncionarios(this.db, this.seguranca_db);
            return ListFuncionarios.Bind(0, 50, param);
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }
    }
}