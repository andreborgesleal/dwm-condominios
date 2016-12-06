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

namespace DWM.Models.Persistence
{
    public class ChamadoModel : CrudModelLocal<Chamado, ChamadoViewModel>
    {
        #region Constructor
        public ChamadoModel() { }
        #endregion

        #region Métodos da classe CrudContext
        public override ChamadoViewModel BeforeInsert(ChamadoViewModel value)
        {
            value = GetUsuario(value);
            value.empresaId = SessaoLocal.empresaId;
            value.DataChamado = Funcoes.Brasilia();
            value.CondominioID = SessaoLocal.empresaId;
            value.DataRedirecionamento = Funcoes.Brasilia(); // Este atributo será atualizado pela Trigger

            #region Executa o método BeforeInsert do ChamadoFila
            ChamadoFilaModel ChamadoFilaModel = new ChamadoFilaModel();
            ChamadoFilaModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
            value.ChamadoFilaViewModel = ChamadoFilaModel.BeforeInsert(value.ChamadoFilaViewModel);
            #endregion

            #region Executa o método BeforeInsert do ChamadoAnexo para cada elemento da coleção
            ChamadoAnexoModel ChamadoAnexoModel = new ChamadoAnexoModel();
            ChamadoAnexoModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
            int index = 0;
            while (index <= value.Anexos.Count()-1)
            {
                ChamadoAnexoViewModel ChamadoAnexoViewModel = ChamadoAnexoModel.BeforeInsert(value.Anexos.ElementAt(index));
                value.Anexos.ElementAt(index).UsuarioID = ChamadoAnexoViewModel.UsuarioID;
                value.Anexos.ElementAt(index).Nome = ChamadoAnexoViewModel.Nome;
                value.Anexos.ElementAt(index).Login = ChamadoAnexoViewModel.Login;
                index++;
            }
            #endregion
            return value;
        }

        public override Chamado MapToEntity(ChamadoViewModel value)
        {
            Chamado entity = Find(value);

            if (entity == null)
                entity = new Chamado();

            entity.ChamadoID = value.ChamadoID;
            entity.ChamadoMotivoID = value.ChamadoMotivoID;
            entity.ChamadoStatusID = value.ChamadoStatusID;
            entity.FilaSolicitanteID = value.FilaSolicitanteID;
            entity.CondominioID = value.CondominioID;
            entity.CondominoID = value.CondominoID;
            entity.CredenciadoID = value.CredenciadoID;
            entity.EdificacaoID = value.EdificacaoID;
            entity.UnidadeID = value.UnidadeID;
            entity.DataChamado = value.DataChamado;
            entity.Assunto = value.Assunto;
            entity.UsuarioID = value.UsuarioID;
            entity.NomeUsuario = value.NomeUsuario;
            entity.LoginUsuario = value.LoginUsuario;
            entity.Prioridade = value.Prioridade;
            entity.DataUltimaAnotacao = value.DataUltimaAnotacao;
            entity.FilaAtendimentoID = value.FilaAtendimentoID;
            entity.DataRedirecionamento = value.DataRedirecionamento;
            entity.UsuarioFilaID = value.UsuarioFilaID;
            entity.NomeUsuarioFila = value.NomeUsuarioFila;
            entity.LoginUsuarioFila = value.LoginUsuarioFila;

            // Executa o MapToEntity do ChamadoFila
            ChamadoFilaModel ChamadoFilaModel = new ChamadoFilaModel();
            ChamadoFilaModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
            entity.ChamadoFila = ChamadoFilaModel.MapToEntity(value.ChamadoFilaViewModel);

            // Mapear anexos
            ChamadoAnexoModel ChamadoAnexoModel = new ChamadoAnexoModel();
            ChamadoAnexoModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
            entity.Anexos = new List<ChamadoAnexo>();
            foreach (ChamadoAnexoViewModel cha in value.Anexos)
            {
                ChamadoAnexo ChamadoAnexo = ChamadoAnexoModel.MapToEntity(cha);
                entity.Anexos.Add(ChamadoAnexo);
            };
            return entity;
        }

        public override ChamadoViewModel MapToRepository(Chamado entity)
        {
            ChamadoViewModel value = new ChamadoViewModel()
            {
                ChamadoID = entity.ChamadoID,
                ChamadoMotivoID = entity.ChamadoMotivoID,
                ChamadoStatusID = entity.ChamadoStatusID,
                FilaSolicitanteID = entity.FilaSolicitanteID,
                CondominioID = entity.CondominioID,
                CondominoID = entity.CondominoID,
                CredenciadoID = entity.CredenciadoID,
                EdificacaoID = entity.EdificacaoID,
                UnidadeID = entity.UnidadeID,
                DataChamado = entity.DataChamado,
                Assunto = entity.Assunto,
                UsuarioID = entity.UsuarioID,
                NomeUsuario = entity.NomeUsuario,
                LoginUsuario = entity.LoginUsuario,
                Prioridade = entity.Prioridade,
                DataUltimaAnotacao = entity.DataUltimaAnotacao,
                MensagemOriginal = entity.MensagemOriginal,
                FilaAtendimentoID = entity.FilaAtendimentoID,
                DataRedirecionamento = entity.DataRedirecionamento,
                UsuarioFilaID = entity.UsuarioFilaID,
                NomeUsuarioFila = entity.NomeUsuarioFila,
                LoginUsuarioFila = entity.LoginUsuarioFila,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };

            ListViewChamadoFila listChamadoFila = new ListViewChamadoFila(this.db, this.seguranca_db);
            value.Rotas = listChamadoFila.Bind(0, 200, value.ChamadoID);

            // Observação: incluir aqui as Anotações do chamado (ChamadoAnotacao) (IEnumerable<CahamdoAnotacaoViewModel>) 

            // Observação: incluir aqui os anexos do chamado (ChamadoAnotacao) (IEnumerable<CahamdoAnexoViewModel>) 
            return value;
        }

        public override Chamado Find(ChamadoViewModel key)
        {
            return db.Chamados.Find(key.ChamadoID);
        }

        public override Validate Validate(ChamadoViewModel value, Crud operation)
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

            if (value.empresaId == 0)
            {
                value.mensagem.Code = 35;
                value.mensagem.Message = MensagemPadrao.Message(35).ToString();
                value.mensagem.MessageBase = "Sua sessão expirou. Faça um novo login no sistema";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.ChamadoMotivoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Motivo").ToString();
                value.mensagem.MessageBase = "Motivo do chamado deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.ChamadoStatusID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Situação").ToString();
                value.mensagem.MessageBase = "Situação do chamado deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            // Se a fila solicitante ou a fila para atendimento for a fila do condômino
            // é preciso então que o ID do condômino ou do Credenciado estejam preenchidos
            // é preciso também que a Edificação e a Unidade também estejam preenchidas
            if (DWMSessaoLocal.FilaCondominoID(sessaoCorrente, db) == value.FilaSolicitanteID ||
                DWMSessaoLocal.FilaCondominoID(sessaoCorrente, db) == value.FilaAtendimentoID)
            {
                if (value.CondominoID == 0 && value.CredenciadoID == 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Condômino/Residente").ToString();
                    value.mensagem.MessageBase = "Condômino/Residente deve ser informado";
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
            }

            if (String.IsNullOrEmpty(value.Assunto) || value.Assunto.Trim().Length <= 10)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Assunto").ToString();
                value.mensagem.MessageBase = "Assunto deve ser informado e deve ter mais de 10 caracteres";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.UsuarioID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "ID do Usuário").ToString();
                value.mensagem.MessageBase = "Código identificador do Usuário que abriu o chamado deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (String.IsNullOrEmpty(value.NomeUsuario))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Usuário").ToString();
                value.mensagem.MessageBase = "Nome do Usuário que abriu o chamado deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (String.IsNullOrEmpty(value.LoginUsuario))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Login do Usuário").ToString();
                value.mensagem.MessageBase = "Login do Usuário que abriu o chamado deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (String.IsNullOrEmpty(value.Prioridade))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Prioridade").ToString();
                value.mensagem.MessageBase = "Prioridade do chamado deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (String.IsNullOrEmpty(value.MensagemOriginal))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Mensagem").ToString();
                value.mensagem.MessageBase = "Mensagem do chamado deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.FilaAtendimentoID == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Fila de atendimento").ToString();
                value.mensagem.MessageBase = "Fila de atendimento deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            // Valida o ChamadoFila
            ChamadoFilaModel ChamadoFilaModel = new ChamadoFilaModel();
            ChamadoFilaModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
            value.mensagem = ChamadoFilaModel.Validate(value.ChamadoFilaViewModel, Crud.INCLUIR);

            // Validar Anexos
            ChamadoAnexoModel ChamadoAnexoModel = new ChamadoAnexoModel();
            ChamadoAnexoModel.Create(this.db, this.seguranca_db, SessaoLocal.sessaoId);
            foreach (ChamadoAnexoViewModel cha in value.Anexos)
            {
                value.mensagem = ChamadoAnexoModel.Validate(cha, Crud.INCLUIR);
                if (value.mensagem.Code > 0)
                    break;
            };

            return value.mensagem;
        }

        public override ChamadoViewModel CreateRepository(HttpRequestBase Request = null)
        {
            ChamadoViewModel value = base.CreateRepository(Request);
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            value.CondominioID = security.getSessaoCorrente().empresaId;
            return value;
        }
        #endregion

        #region Métodos customizados
        private ChamadoViewModel GetUsuario(ChamadoViewModel value)
        {
            if (value.UsuarioID == 0)
                value.UsuarioID = SessaoLocal.usuarioId;

            value.NomeUsuario = seguranca_db.Usuarios.Find(value.UsuarioID).nome; ;
            value.LoginUsuario = seguranca_db.Usuarios.Find(value.UsuarioID).login;

            return value;
        }
        #endregion

    }
}