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
    public class VeiculoModel : CrudModelLocal<Veiculo, VeiculoViewModel>
    {
        #region Constructor
        public VeiculoModel() { }
        public VeiculoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override Veiculo MapToEntity(VeiculoViewModel value)
        {
            Veiculo entity = Find(value);

            if (entity == null)
                entity = new Veiculo();

            entity.CondominioID = value.CondominioID;
            entity.EdificacaoID = value.EdificacaoID;
            entity.UnidadeID = value.UnidadeID;
            entity.CondominoID = value.CondominoID;
            entity.Placa = value.Placa;
            entity.Cor = value.Cor;
            entity.Descricao = value.Descricao;
            entity.Marca = value.Marca;
            entity.Condutor = value.Condutor;
            entity.NumeroSerie = value.NumeroSerie;

            return entity;
        }

        public override VeiculoViewModel MapToRepository(Veiculo entity)
        {
            return new VeiculoViewModel()
            {
                CondominioID = entity.CondominioID,
                EdificacaoID = entity.EdificacaoID,
                UnidadeID = entity.UnidadeID,
                CondominoID = entity.CondominoID,
                Placa = entity.Placa,
                Cor = entity.Cor,
                Descricao = entity.Descricao,
                Marca = entity.Marca,
                Condutor = entity.Condutor,
                NumeroSerie = entity.NumeroSerie,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Veiculo Find(VeiculoViewModel key)
        {
            return db.Veiculos.Find(key.CondominioID, key.EdificacaoID, key.UnidadeID, key.CondominoID, key.Placa );
        }

        public override Validate Validate(VeiculoViewModel value, Crud operation)
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
            else if (value.EdificacaoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Edificação").ToString();
                value.mensagem.MessageBase = "Edificação deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (value.UnidadeID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Unidade").ToString();
                value.mensagem.MessageBase = "Unidade deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (value.CondominoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condômino").ToString();
                value.mensagem.MessageBase = "Condômino deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            else if (value.Placa == null || value.Placa.Trim().Length < 7)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Placa").ToString();
                value.mensagem.MessageBase = "Placa deve ser informada";
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

                if (value.Cor == null || value.Cor.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Cor").ToString();
                    value.mensagem.MessageBase = "Cor do veículo deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Descricao == null || value.Descricao.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Descrição").ToString();
                    value.mensagem.MessageBase = "Descrição do veículo deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Marca == null || value.Marca.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Marca").ToString();
                    value.mensagem.MessageBase = "Marca do veículo deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            return value.mensagem;
        }

        public override VeiculoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            VeiculoViewModel value = base.CreateRepository(Request);
            return value;
        }
        #endregion
    }

    public class ListViewVeiculos : ListViewModelLocal<VeiculoViewModel>
    {
        #region Constructor
        public ListViewVeiculos() { }
        public ListViewVeiculos(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<VeiculoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = param != null && param.Count() > 0 && param[0] != null ? int.Parse(param[0].ToString()) : 0;
            int _EdificacaoID = param != null && param.Count() > 1 && param[1] != null ? int.Parse(param[1].ToString()) : 0;
            int _UnidadeID = param != null && param.Count() > 2 && param[2] != null ? int.Parse(param[2].ToString()) : 0;
            int _CondominoID = param != null && param.Count() > 3 && param[3] != null ? int.Parse(param[3].ToString()) : 0;

            return (from v in db.Veiculos
                    where v.CondominioID == _CondominioID
                          && v.EdificacaoID == _EdificacaoID
                          && v.UnidadeID == _UnidadeID
                          && v.CondominoID == _CondominoID
                    orderby v.Placa
                    select new VeiculoViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        CondominioID = v.CondominioID,
                        EdificacaoID = v.EdificacaoID,
                        UnidadeID = v.UnidadeID,
                        CondominoID = v.CondominoID,
                        Placa = v.Placa,
                        Marca = v.Marca,
                        Cor = v.Cor,
                        Descricao = v.Descricao,
                        Condutor = v.Condutor,
                        NumeroSerie = v.NumeroSerie,
                        PageSize = pageSize,
                        TotalCount = ((from v1 in db.Veiculos
                                       where v1.CondominioID == _CondominioID
                                             && v1.EdificacaoID == _EdificacaoID
                                             && v1.UnidadeID == _UnidadeID
                                             && v1.CondominoID == _CondominoID
                                       orderby v1.Placa
                                       select v1).Count())
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new VeiculoModel().getObject((VeiculoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-veiculos";
        }
    }
}