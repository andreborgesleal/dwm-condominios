using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using DWM.Models.Entidades;
using DWM.Models.Repositories;


namespace dwm_condominios.Models.Persistence
{
    public class CondominoPFModel : CrudModel<CondominoPF, CondominoPFViewModel, ApplicationContext>
    {
        #region Constructor
        public CondominoPFModel() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public CondominoPFModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override CondominoPF MapToEntity(CondominoPFViewModel value)
        {
            CondominoPF condomino = Find(value);

            if (condomino == null)
            {
                condomino = new CondominoPF();
                value.IndSituacao = "D";
            }

            condomino.CondominoID = value.CondominoID;
            condomino.CondominioID = value.CondominioID;
            condomino.Nome = value.Nome;
            condomino.IndFiscal = value.IndFiscal;
            condomino.IndProprietario = value.IndProprietario;
            condomino.TelParticular1 = value.TelParticular1 != null ? value.TelParticular1.Replace("(", "").Replace(")", "").Replace("-", "") : value.TelParticular1;
            condomino.TelParticular2 = value.TelParticular2 != null ? value.TelParticular1.Replace("(", "").Replace(")", "").Replace("-", "") : value.TelParticular2;
            condomino.TelParticular3 = value.TelParticular3 != null ? value.TelParticular3.Replace("(", "").Replace(")", "").Replace("-", "") : value.TelParticular3;
            condomino.TelParticular4 = value.TelParticular4 != null ? value.TelParticular4.Replace("(", "").Replace(")", "").Replace("-", "") : value.TelParticular4;
            condomino.Email = value.Email;
            condomino.UsuarioID = value.UsuarioID;
            condomino.Observacao = value.Observacao;
            condomino.DataCadastro = Funcoes.Brasilia();
            condomino.DataNascimento = value.DataNascimento;
            condomino.IndAnimal = value.IndAnimal;
            condomino.Avatar = value.Avatar;

            return condomino;
        }

        public override CondominoPFViewModel MapToRepository(CondominoPF entity)
        {
            CondominoPFViewModel condominoViewModel = new CondominoPFViewModel()
            {
                CondominoID = entity.CondominoID,
                Nome = entity.Nome,
                IndFiscal = entity.IndFiscal,
                IndProprietario = entity.IndProprietario,
                TelParticular1 = entity.TelParticular1,
                TelParticular2 = entity.TelParticular2,
                TelParticular3 = entity.TelParticular3,
                TelParticular4 = entity.TelParticular4,
                Email = entity.Email,
                UsuarioID = entity.UsuarioID,
                Observacao = entity.Observacao,
                DataCadastro = entity.DataCadastro,
                DataNascimento = entity.DataNascimento,
                IndAnimal = entity.IndAnimal,
                Avatar = entity.Avatar,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };

            return condominoViewModel;
        }

        public override CondominoPF Find(CondominoPFViewModel key)
        {
            return db.CondominoPFs.Find(key.CondominoID);
        }

        public override Validate Validate(CondominoPFViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation == Crud.EXCLUIR || operation == Crud.ALTERAR)
            {
                if (value.CondominoID <= 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "ID Condômino").ToString();
                    value.mensagem.MessageBase = "ID do condômino deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }

            if (value.Nome == null || value.Nome == "" || value.Nome.Length < 10)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome").ToString();
                value.mensagem.MessageBase = "Nome do condômino deve ser preenchido e ter no mínimo 10 caracteres";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.Email == null || value.Email == "" || !Funcoes.validaEmail(value.Email))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "E-mail").ToString();
                value.mensagem.MessageBase = "E-mail deve ser preenchido e ter um formato válido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Valida CPF
            if (!Funcoes.ValidaCpf(value.IndFiscal.Replace(".", "").Replace("-", "")))
            {
                value.mensagem.Code = 29;
                value.mensagem.Message = MensagemPadrao.Message(29, value.IndFiscal).ToString();
                value.mensagem.MessageBase = "IndFiscal informado está incorreto.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            #endregion

            #region Verifica se o e-mail já existe
            if (db.CondominoPFs.Where(info => info.Email == value.Email).Count() > 0)
            {
                value.mensagem.Code = 41;
                value.mensagem.Message = MensagemPadrao.Message(41, "E-mail: " + value.Email).ToString();
                value.mensagem.MessageBase = "E-mail já cadastrado.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            #endregion

            #region Verifica se existe o CPF 
            if (db.CondominoPFs.Where(info => info.IndFiscal == value.IndFiscal.Replace(".", "").Replace("-", "")).Count() > 0)
            {
                value.mensagem.Code = 41;
                value.mensagem.Message = MensagemPadrao.Message(41, "IndFiscal: " + value.IndFiscal).ToString();
                value.mensagem.MessageBase = "IndFiscal já cadastrado.";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #endregion

            return value.mensagem;
        }
        #endregion
    }

    public class ListViewMembro : ListViewModel<CondominoPFViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewMembro() { }
        public ListViewMembro(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<CondominoPFViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param[0] != null && param[0].ToString() != "" ? param[0].ToString() : null;

            IEnumerable<CondominoPFViewModel> query = (from m in db.Membros
                                                  where (_nome == null || m.Nome.Contains(_nome) || m.CPF == _nome || m.Email == _nome)
                                                  select new CondominoPFViewModel()
                                                  {
                                                      MembroID = m.MembroID,
                                                      Nome = m.Nome,
                                                      Telefone = m.Telefone,
                                                      Email = m.Email,
                                                      CPF = m.CPF,
                                                      Avatar = m.Avatar,
                                                      Banco = m.Banco,
                                                      Agencia = m.Agencia,
                                                      Conta = m.Conta,
                                                      IndSituacao = m.IndSituacao
                                                  }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            return query;
        }

        public override string action()
        {
            return "../Home/ListMembros";
        }

        public override string DivId()
        {
            return "div-membro";
        }

        public override Repository getRepository(Object id)
        {
            return new MembroModel().getObject((CondominoPFViewModel)id);
            //return new ApostaModel().getObject((ApostaViewModel)id);
        }
        #endregion
    }

}