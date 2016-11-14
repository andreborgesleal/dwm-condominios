using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using System.Web;

namespace DWM.Models.Persistence
{
    public class FuncionarioModel : CrudModelLocal<Funcionario, FuncionarioViewModel>
    {
        #region Constructor
        public FuncionarioModel() { }
        public FuncionarioModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override Funcionario MapToEntity(FuncionarioViewModel value)
        {
            Funcionario entity = Find(value);

            if (entity == null)
                entity = new Funcionario();

            entity.FuncionarioID = value.FuncionarioID;
            entity.CondominioID = value.CondominioID;
            entity.EdificacaoID = value.EdificacaoID;
            entity.UnidadeID = value.UnidadeID;
            entity.CondominoID = value.CondominoID;
            entity.Nome = value.Nome;
            entity.Funcao = value.Funcao;
            entity.Sexo = value.Sexo;
            entity.Dia = value.Dia;
            entity.HoraInicial = value.HoraInicial;
            entity.HoraFinal = value.HoraFinal;
            entity.RG = value.RG;

            return entity;
        }

        public override FuncionarioViewModel MapToRepository(Funcionario entity)
        {
            return new FuncionarioViewModel()
            {
                FuncionarioID = entity.FuncionarioID,
                CondominioID = entity.CondominioID,
                EdificacaoID = entity.EdificacaoID,
                UnidadeID = entity.UnidadeID,
                CondominoID = entity.CondominoID,
                Nome = entity.Nome,
                Funcao = entity.Funcao,
                Sexo = entity.Sexo,
                Dia = entity.Dia,
                HoraInicial = entity.HoraInicial,
                HoraFinal = entity.HoraFinal,
                RG = entity.RG,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Funcionario Find(FuncionarioViewModel key)
        {
            return db.Funcionarios.Find(key.FuncionarioID);
        }

        public override Validate Validate(FuncionarioViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.FuncionarioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Funcionário ID").ToString();
                value.mensagem.MessageBase = "ID do Funcionário deve ser informado";
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
                if (value.CondominoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Condômino").ToString();
                    value.mensagem.MessageBase = "Condômino deve ser informada";
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
                if (value.Nome == null || value.Nome.Trim().Length < 6 )
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Nome do funcionário").ToString();
                    value.mensagem.MessageBase = "Nome do funcionário deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Sexo == null || value.Sexo.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Sexo").ToString();
                    value.mensagem.MessageBase = "Sexo do funcionário deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Dia == null || value.Dia.Trim().Length < 7)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Dia de trabalho").ToString();
                    value.mensagem.MessageBase = "Dia de trabalho do funcionário deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (!Funcoes.ValidaHora(value.HoraInicial))
                {
                    value.mensagem.Code = 4;
                    value.mensagem.Message = MensagemPadrao.Message(4, "Hora Início", value.HoraInicial).ToString();
                    value.mensagem.MessageBase = "Hora inválida";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (!Funcoes.ValidaHora(value.HoraFinal))
                {
                    value.mensagem.Code = 4;
                    value.mensagem.Message = MensagemPadrao.Message(4, "Hora Final", value.HoraFinal).ToString();
                    value.mensagem.MessageBase = "Hora inválida";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            return value.mensagem;
        }

        public override FuncionarioViewModel CreateRepository(HttpRequestBase Request = null)
        {
            FuncionarioViewModel value = base.CreateRepository(Request);
            value.Sexo = "M";
            value.HoraInicial = "08:00";
            value.HoraFinal = "18:00";
            value.Dia = "NSSSSSN";
            return value;
        }
        #endregion
    }

    public class ListViewFuncionarios : ListViewModelLocal<FuncionarioViewModel>
    {
        #region Constructor
        public ListViewFuncionarios() { }
        public ListViewFuncionarios(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<FuncionarioViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = param != null && param.Count() > 0 && param[0] != null ? int.Parse(param[0].ToString()) : 0;
            int _EdificacaoID = param != null && param.Count() > 1 && param[1] != null ? int.Parse(param[1].ToString()) : 0;
            int _UnidadeID = param != null && param.Count() > 2 && param[2] != null ? int.Parse(param[2].ToString()) : 0;
            int _CondominoID = param != null && param.Count() > 3 && param[3] != null ? int.Parse(param[3].ToString()) : 0;

            return (from f in db.Funcionarios
                    where f.CondominioID == _CondominioID
                          && f.EdificacaoID == _EdificacaoID
                          && f.UnidadeID == _UnidadeID
                          && f.CondominoID == _CondominoID
                    orderby f.Nome
                    select new FuncionarioViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        FuncionarioID = f.FuncionarioID,
                        CondominioID = f.CondominioID,
                        EdificacaoID = f.EdificacaoID,
                        UnidadeID = f.UnidadeID,
                        CondominoID = f.CondominoID,
                        Nome = f.Nome,
                        Funcao = f.Funcao,
                        Sexo = f.Sexo,
                        Dia = f.Dia,
                        HoraInicial = f.HoraInicial,
                        HoraFinal = f.HoraFinal,
                        RG = f.RG,
                        PageSize = pageSize,
                        TotalCount = ((from f1 in db.Funcionarios
                                       where f1.CondominioID == _CondominioID
                                             && f1.EdificacaoID == _EdificacaoID
                                             && f1.UnidadeID == _UnidadeID
                                             && f1.CondominoID == _CondominoID
                                       orderby f1.Nome
                                       select f1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new FuncionarioModel().getObject((FuncionarioViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-funcionarios";
        }
    }
}