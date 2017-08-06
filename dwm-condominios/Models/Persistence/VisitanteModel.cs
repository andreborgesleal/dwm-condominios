﻿using System;
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
using App_Dominio.Security;
using dwm_condominios.Models.Persistence;

namespace DWM.Models.Persistence
{
    public class VisitanteModel : CrudModelLocal<Visitante, VisitanteViewModel>
    {
        #region Constructor
        public VisitanteModel()
        {
        }

        public VisitanteModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        private bool IsUserAdm()
        {
            return DWMSessaoLocal.GetSessaoLocal(sessaoCorrente, this.db).Unidades == null;
        }

        #region Métodos da classe CrudContext
        public override VisitanteViewModel BeforeUpdate(VisitanteViewModel value)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (IsUserAdm() && value.PrestadorCondominio == "N")
            {
                var _EdificacaoID = value.EdificacaoID.GetValueOrDefault();
                var _UnidadeID = value.UnidadeID.GetValueOrDefault();

                var _condominoID = (from c in db.CondominoUnidades
                                    where c.EdificacaoID == _EdificacaoID
                                             && c.CondominioID == value.CondominioID
                                             && c.UnidadeID == _UnidadeID
                                             && c.DataFim == null
                                    select new CondominoUnidadeViewModel
                                    {
                                        CondominoID = c.CondominoID
                                    }).FirstOrDefault();

                // Valida se existe um condômino para a edificação e a unidade desejada
                if (_condominoID == null)
                {
                    value.mensagem = new Validate() { Code = 997, Message = MensagemPadrao.Message(997).ToString() };
                    value.mensagem.Code = 997;
                    value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                    value.mensagem.MessageBase = "Não existe um condômino registrado para essa edificação/unidade";
                    value.mensagem.MessageType = MsgType.WARNING;
                }
                else
                {
                    if (value.VisitanteUnidadeViewModel.Count() > 0)
                        value.VisitanteUnidadeViewModel.ElementAt(0).CondominoID = _condominoID.CondominoID;
                }
            }

            return base.BeforeUpdate(value);
        }

        public override VisitanteViewModel BeforeDelete(VisitanteViewModel value)
        {
            Visitante v = Find(value);
            VisitanteViewModel vm = MapToRepository(v);
            vm.uri = value.uri;
            return vm;
        }

        public override VisitanteViewModel BeforeInsert(VisitanteViewModel value)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (IsUserAdm() && value.PrestadorCondominio == "N")
            {
                var _EdificacaoID = value.EdificacaoID.GetValueOrDefault();
                var _UnidadeID = value.UnidadeID.GetValueOrDefault();

                var _condominoID = (from c in db.CondominoUnidades
                                    where c.EdificacaoID == _EdificacaoID
                                             && c.CondominioID == value.CondominioID
                                             && c.UnidadeID == _UnidadeID
                                             && c.DataFim == null
                                    select new CondominoUnidadeViewModel
                                    {
                                        CondominoID = c.CondominoID
                                    }).FirstOrDefault();

                // Valida se existe um condômino para a edificação e a unidade desejada
                if (_condominoID == null)
                {
                    value.mensagem = new Validate() { Code = 997, Message = MensagemPadrao.Message(997).ToString() };
                    value.mensagem.Code = 997;
                    value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                    value.mensagem.MessageBase = "Não existe um condômino registrado para essa edificação/unidade";
                    value.mensagem.MessageType = MsgType.WARNING;
                }
                else
                {
                    value.VisitanteUnidadeViewModel.ElementAt(0).CondominoID = _condominoID.CondominoID;
                }
            }
            else if (value.PrestadorCondominio == "S")
            {
                if (!value.PrestadorTipoID.HasValue)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Tipo de Prestador").ToString();
                    value.mensagem.MessageBase = "O Tipo do prestador deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                }
            }

            return base.BeforeInsert(value);
        }

        public override Visitante MapToEntity(VisitanteViewModel value)
        {
            Visitante entity = Find(value);

            if (entity == null)
                entity = new Visitante();

            entity.CondominioID = value.CondominioID;
            entity.PrestadorTipoID = value.PrestadorTipoID;
            entity.VisitanteID = value.VisitanteID;
            entity.Nome = value.Nome;
            entity.RG = value.RG;
            entity.OrgaoEmissor = value.OrgaoEmissor;
            entity.CPF = value.CPF?.Replace(".", "").Replace("-", "").Replace("/", "");
            entity.Sexo = value.Sexo;
            entity.DataInclusao = Funcoes.Brasilia();
            entity.Telefone = value.Telefone?.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            entity.Fotografia = value.Fotografia;
            entity.PrestadorCondominio = value.PrestadorCondominio;
            entity.Situacao = "A";
            entity.Placa = value.Placa;
            entity.Descricao = value.Descricao;
            entity.Marca = value.Marca;
            entity.Cor = value.Cor;

            #region VisitanteUnidadeViewModel
            if (entity.VisitanteID == 0 && value.PrestadorCondominio == "N") // Se for uma inclusão
            {
                if (IsUserAdm())
                {
                    entity.VisitanteUnidade.Add(new VisitanteUnidade()
                    {
                        CondominioID = value.CondominioID,
                        EdificacaoID = value.EdificacaoID.Value,
                        UnidadeID = value.UnidadeID.Value,
                        CondominoID = value.VisitanteUnidadeViewModel.FirstOrDefault().CondominoID,
                    });
                }
                else
                {
                    entity.VisitanteUnidade.Add(new VisitanteUnidade()
                    {
                        CondominioID = value.CondominioID,
                        EdificacaoID = value.EdificacaoID.Value,
                        UnidadeID = value.UnidadeID.Value,
                        CondominoID = DWMSessaoLocal.GetSessaoLocal().CondominoID,
                        CredenciadoID = DWMSessaoLocal.GetSessaoLocal().CredenciadoID
                    });
                }
            }
            #endregion

            return entity;
        }

        public override VisitanteViewModel MapToRepository(Visitante entity)
        {
            VisitanteViewModel v = new VisitanteViewModel()
            {
                CondominioID = entity.CondominioID,
                empresaId = entity.CondominioID,
                PrestadorTipoID = entity.PrestadorTipoID,
                VisitanteID = entity.VisitanteID,
                Nome = entity.Nome,
                RG = entity.RG,
                OrgaoEmissor = entity.OrgaoEmissor,
                CPF = entity.CPF,
                Sexo = entity.Sexo,
                DataInclusao = entity.DataInclusao,
                Telefone = entity.Telefone,
                Fotografia = entity.Fotografia,
                PrestadorCondominio = entity.PrestadorCondominio,
                Situacao = entity.Situacao,
                Placa = entity.Placa,
                Descricao = entity.Descricao,
                Cor = entity.Cor,
                Marca = entity.Marca,
                mensagem = new Validate() { Code = 0, Message = "Registro processado com sucesso", MessageBase = "Registro processado com sucesso", MessageType = MsgType.SUCCESS }
            };

            foreach (VisitanteUnidade vu in entity.VisitanteUnidade)
            {
                v.EdificacaoID = vu.EdificacaoID;
                v.UnidadeID = vu.UnidadeID;
                VisitanteUnidadeViewModel item = new VisitanteUnidadeViewModel()
                {
                    CondominioID = vu.CondominioID,
                    CondominoID = vu.CondominoID,
                    VisitanteID = vu.VisitanteID,
                    CredenciadoID = vu.CredenciadoID,
                    EdificacaoID = vu.EdificacaoID,
                    UnidadeID = vu.UnidadeID,
                    VisitanteViewModel = v
                };
                ((List<VisitanteUnidadeViewModel>)v.VisitanteUnidadeViewModel).Add(item);
            }

            return v;
        }

        public override Visitante Find(VisitanteViewModel key)
        {
            return db.Visitantes.Find(key.VisitanteID);
        }

        public override Validate Validate(VisitanteViewModel value, Crud operation)
        {
            if (value.mensagem.Code != 0)
                return value.mensagem;

            if (value.empresaId == 0)
            {
                value.mensagem.Code = 35;
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "Sua sessão expirou. Faça um novo login no sistema";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.CondominioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Condomínio").ToString();
                value.mensagem.MessageBase = "Condomínio deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.ALTERAR)
            {
                if (value.VisitanteID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Visitante").ToString();
                    value.mensagem.MessageBase = "Visitante deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Nome.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Visitante").ToString();
                    value.mensagem.MessageBase = "Nome do Condômino deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Sexo.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Sexo").ToString();
                    value.mensagem.MessageBase = "Sexo do Condômino deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (DWMSessaoLocal.GetSessaoLocal().Unidades == null)
                {
                    if (!value.EdificacaoID.HasValue && value.PrestadorCondominio == "N")
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(5, "Edificação").ToString();
                        value.mensagem.MessageBase = "A Edificação deve ser informada";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }

                    if (!value.UnidadeID.HasValue && value.PrestadorCondominio == "N")
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(5, "Unidade").ToString();
                        value.mensagem.MessageBase = "A Unidade deve ser informada";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }

                    if (String.IsNullOrEmpty(value.RG))
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(5, "RG").ToString();
                        value.mensagem.MessageBase = "O RG deve ser informado";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }

                    if (String.IsNullOrEmpty(value.OrgaoEmissor))
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(5, "Órgão Emissor").ToString();
                        value.mensagem.MessageBase = "O Órgão Emissor deve ser informado";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }

                }
            }

            if (operation == Crud.EXCLUIR)
            {
                if (value.VisitanteID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Visitante").ToString();
                    value.mensagem.MessageBase = "Visitante deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.EdificacaoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Visitante").ToString();
                    value.mensagem.MessageBase = "Edificacao deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
                if (value.UnidadeID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Visitante").ToString();
                    value.mensagem.MessageBase = "Unidade deve ser informada";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            if (operation == Crud.INCLUIR && value.PrestadorCondominio == "N")
            {
                VisitanteUnidadeModel vuModel = new VisitanteUnidadeModel();
                value.mensagem = vuModel.Validate(value.VisitanteUnidadeViewModel.FirstOrDefault(), Crud.INCLUIR);
            }

            return value.mensagem;
        }

        public override VisitanteViewModel CreateRepository(HttpRequestBase Request = null)
        {
            VisitanteViewModel u = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            u.CondominioID = security.getSessaoCorrente().empresaId;
            return u;
        }
        #endregion
    }

    public class ListViewVisitante : ListViewModelLocal<VisitanteViewModel>
    {
        #region Constructor
        public ListViewVisitante() { }
        public ListViewVisitante(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<VisitanteViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominioID = sessaoCorrente.empresaId;
            int _EdificacaoID;
            int _UnidadeID;
            DateTime dataHoje = Funcoes.Brasilia();

            if (SessaoLocal.CondominoID > 0)
            {
                _EdificacaoID = db.Edificacaos.Where(x => x.CondominioID == _CondominioID).FirstOrDefault().EdificacaoID;
                _UnidadeID = SessaoLocal.Unidades.FirstOrDefault().UnidadeID;
            }
            else
            {
                _EdificacaoID = param != null && param.Count() > 1 && param[1] != null ? int.Parse(param[1].ToString()) : 0;
                _UnidadeID = param != null && param.Count() > 2 && param[2] != null ? int.Parse(param[2].ToString()) : 0;
            }

            var q = (from v in db.Visitantes
                     join vu in db.VisitanteUnidades on v.VisitanteID equals vu.VisitanteID into vleft
                     from vu in vleft.DefaultIfEmpty()
                     join ed in db.Edificacaos on vu.EdificacaoID equals ed.EdificacaoID into vuleft
                     from ed in vuleft.DefaultIfEmpty()
                     join con in db.Condominos on vu.CondominoID equals con.CondominoID into conleft
                     from con in conleft.DefaultIfEmpty()
                     join tp in db.PrestadorTipos on v.PrestadorTipoID equals tp.PrestadorTipoID into tpleft
                     from tp in tpleft.DefaultIfEmpty()
                     where v.CondominioID == _CondominioID
                           && (_EdificacaoID == 0 || vu.EdificacaoID == _EdificacaoID)
                           && (_UnidadeID == 0 || vu.UnidadeID == _UnidadeID)
                           && v.Situacao == "A"
                     orderby v.DataInclusao, v.Nome
                     select new VisitanteViewModel
                     {
                         empresaId = sessaoCorrente.empresaId,
                         CondominioID = v.CondominioID,
                         Nome = v.Nome,
                         Sexo = v.Sexo == "M" ? "Masculino" : "Feminino",
                         RG = v.RG,
                         CPF = v.CPF,
                         DataInclusao = v.DataInclusao,
                         Fotografia = v.Fotografia,
                         OrgaoEmissor = v.OrgaoEmissor,
                         VisitanteID = v.VisitanteID,
                         PrestadorCondominio = v.PrestadorCondominio,
                         PrestadorTipoID = v.PrestadorTipoID,
                         Situacao = v.Situacao,
                         Telefone = v.Telefone,
                         EdificacaoID = vu.EdificacaoID,
                         UnidadeID = vu.UnidadeID,
                         DescricaoEdificacao = ed.Descricao,
                         Placa = v.Placa,
                         Cor = v.Cor,
                         Descricao = v.Descricao,
                         Marca = v.Marca,
                         NomeCondomino = con.Nome,
                         DescricaoTipoPrestador = tp.Descricao,
                     }).ToList();
            return q;
        }

        public override Repository getRepository(Object id)
        {
            return new FuncionarioModel().getObject((FuncionarioViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-visitantes";
        }
    }
}