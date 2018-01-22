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
    public class CredenciadoModel : CrudModelLocal<Credenciado, CredenciadoViewModel>
    {
        #region Constructor
        public CredenciadoModel() { }
        public CredenciadoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        public CredenciadoModel(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override Credenciado MapToEntity(CredenciadoViewModel value)
        {
            Credenciado entity = Find(value);

            if (entity == null)
                entity = new Credenciado();

            entity.CredenciadoID = value.CredenciadoID;
            entity.CondominoID = value.CondominoID;
            entity.Nome = value.Nome;
            entity.Email = !string.IsNullOrEmpty(value.Email) ? value.Email.ToLower() : null;
            entity.TipoCredenciadoID = value.TipoCredenciadoID;
            entity.Sexo = value.Sexo;
            entity.Observacao = value.Observacao;
            entity.UsuarioID = value.UsuarioID;
            entity.IndVisitantePermanente = value.IndVisitantePermanente;

            return entity;
        }

        public override CredenciadoViewModel MapToRepository(Credenciado entity)
        {
            return new CredenciadoViewModel()
            {
                CredenciadoID = entity.CredenciadoID,
                CondominoID = entity.CondominoID,
                Nome = entity.Nome,
                Email = entity.Email,
                TipoCredenciadoID = entity.TipoCredenciadoID,
                Sexo = entity.Sexo,
                Observacao = entity.Observacao,
                UsuarioID = entity.UsuarioID,
                IndVisitantePermanente = entity.IndVisitantePermanente,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Credenciado Find(CredenciadoViewModel key)
        {
            return db.Credenciados.Find(key.CredenciadoID);
        }

        public override Validate Validate(CredenciadoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation != Crud.INCLUIR && value.CredenciadoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Credenciado").ToString();
                value.mensagem.MessageBase = "Credenciado deve ser informado";
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

                if (value.CondominoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Condômino").ToString();
                    value.mensagem.MessageBase = "Condômino deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.Nome.Trim().Length == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Nome do credenciado").ToString();
                    value.mensagem.MessageBase = "Nome do credenciado deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (!Funcoes.validaEmail(value.Email))
                {
                    value.mensagem.Code = 4;
                    value.mensagem.Message = MensagemPadrao.Message(4, "E-mail", value.Email).ToString();
                    value.mensagem.MessageBase = "E-mail inválido";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (value.TipoCredenciadoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Tipo Credenciado").ToString();
                    value.mensagem.MessageBase = "Tipo de Credenciado deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                int descricaoCredenciado = (from c in db.Credenciados
                                            where c.CondominoID == value.CondominoID
                                                  && c.CredenciadoID != value.CredenciadoID
                                                  && c.Nome.Equals(value.Nome)
                                            select c.Nome).Count();
                if (descricaoCredenciado > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Nome do credenciado já existente";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (String.IsNullOrEmpty(value.IndVisitantePermanente))
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Visitante Permanente").ToString();
                    value.mensagem.MessageBase = "Informe se o dependente é visitante permanente";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }


            return value.mensagem;
        }

        public override CredenciadoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            CredenciadoViewModel value = base.CreateRepository(Request);
            value.Sexo = "M";
            value.IndVisitantePermanente = "N";
            value.TipoCredenciadoID = 0;
            return value;
        }
        #endregion
    }
    public class ListViewCredenciados : ListViewModelLocal<CredenciadoViewModel>
    {
        #region Constructor
        public ListViewCredenciados() { }
        public ListViewCredenciados(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.Create(_db, _seguranca_db);
        }
        public ListViewCredenciados(ApplicationContext _db, SecurityContext _seguranca_db, string Token = null)
        {
            this.Create(_db, _seguranca_db, Token);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<CredenciadoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _CondominoID = param != null && param.Count() > 0 && param[0] != null ? int.Parse(param[0].ToString()) : 0;

            if (_CondominoID != 0)
            {
                return (from c in db.Credenciados
                        join t in db.TipoCredenciados on c.TipoCredenciadoID equals t.TipoCredenciadoID
                        join con in db.Condominos on c.CondominoID equals con.CondominoID
                        join cu in db.CondominoUnidades on con.CondominoID equals cu.CondominoID
                        join ed in db.Edificacaos on cu.EdificacaoID equals ed.EdificacaoID
                        where c.CondominoID == _CondominoID
                        orderby c.Nome
                        select new CredenciadoViewModel
                        {
                            empresaId = sessaoCorrente.empresaId,
                            CredenciadoID = c.CredenciadoID,
                            CondominoID = c.CondominoID,
                            Nome = c.Nome,
                            Email = c.Email,
                            TipoCredenciadoID = c.TipoCredenciadoID,
                            DescricaoTipoCredenciado = t.Descricao,
                            Sexo = c.Sexo,
                            Observacao = c.Observacao,
                            UsuarioID = c.UsuarioID,
                            IndVisitantePermanente = c.IndVisitantePermanente,
                            DescricaoEdificacao = ed.Descricao,
                            UnidadeID = cu.UnidadeID,
                            PageSize = pageSize,
                            TotalCount = ((from c1 in db.Credenciados
                                           where c1.CondominoID == _CondominoID
                                           orderby c1.Nome
                                           select c1).Count())
                        }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return (from c in db.Credenciados
                        join t in db.TipoCredenciados on c.TipoCredenciadoID equals t.TipoCredenciadoID
                        join con in db.Condominos on c.CondominoID equals con.CondominoID
                        join cu in db.CondominoUnidades on con.CondominoID equals cu.CondominoID
                        join ed in db.Edificacaos on cu.EdificacaoID equals ed.EdificacaoID
                        where con.CondominioID == sessaoCorrente.empresaId
                        orderby c.Nome
                        select new CredenciadoViewModel
                        {
                            empresaId = sessaoCorrente.empresaId,
                            CredenciadoID = c.CredenciadoID,
                            CondominoID = c.CondominoID,
                            Nome = c.Nome,
                            Email = c.Email,
                            TipoCredenciadoID = c.TipoCredenciadoID,
                            DescricaoTipoCredenciado = t.Descricao,
                            Sexo = c.Sexo,
                            Observacao = c.Observacao,
                            UsuarioID = c.UsuarioID,
                            IndVisitantePermanente = c.IndVisitantePermanente,
                            DescricaoEdificacao = ed.Descricao,
                            UnidadeID = cu.UnidadeID,
                            PageSize = pageSize,
                            TotalCount = ((from c1 in db.Credenciados
                                           where c1.CondominoID == _CondominoID
                                           orderby c1.Nome
                                           select c1).Count())
                        }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            }
            
        }

        public override Repository getRepository(Object id)
        {
            return new CredenciadoModel().getObject((CredenciadoViewModel)id);
        }
        #endregion

        public override string DivId()
        {
            return "div-credenciado";
        }
    }
}