using App_Dominio.Entidades;
using App_Dominio.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    public static class DWMSessaoLocal
    {
        public static SessaoLocal GetSessaoLocal(Sessao sessaoCorrente, DbContext _db)
        {
            ApplicationContext db = (ApplicationContext)_db;
            SessaoLocal SessaoLocal = new SessaoLocal()
            {
                dt_atualizacao = sessaoCorrente.dt_atualizacao,
                dt_criacao = sessaoCorrente.dt_criacao,
                dt_desativacao = sessaoCorrente.dt_desativacao,
                empresaId = sessaoCorrente.empresaId,
                ip = sessaoCorrente.ip,
                isOnline = sessaoCorrente.isOnline,
                login = sessaoCorrente.login,
                sessaoId = sessaoCorrente.sessaoId,
                sistemaId = sessaoCorrente.sistemaId,
                usuarioId = sessaoCorrente.usuarioId,
                value1 = sessaoCorrente.value1,
                value2 = sessaoCorrente.value2,
                value3 = sessaoCorrente.value3,
                value4 = sessaoCorrente.value4,
            };

            #region É Condômino?
            if ((from con in db.Condominos
                 join cre in db.Credenciados on con.CondominoID equals cre.CondominoID into CRE
                 from cre in CRE.DefaultIfEmpty()
                 where con.CondominioID == SessaoLocal.empresaId && (con.UsuarioID == SessaoLocal.usuarioId || cre.UsuarioID == SessaoLocal.usuarioId) && con.IndSituacao == "A"
                 select con.CondominoID).Count() > 0)
            {
                #region CondominoID
                SessaoLocal.CondominoID = (from con in db.Condominos
                                           join cre in db.Credenciados on con.CondominoID equals cre.CondominoID into CRE
                                           from cre in CRE.DefaultIfEmpty()
                                           where con.CondominioID == SessaoLocal.empresaId && (con.UsuarioID == SessaoLocal.usuarioId || cre.UsuarioID == SessaoLocal.usuarioId) && con.IndSituacao == "A"
                                           select con.CondominoID).FirstOrDefault();
                #endregion

                #region CredenciadoID
                SessaoLocal.CredenciadoID = (from con in db.Condominos
                                             join cre in db.Credenciados on con.CondominoID equals cre.CondominoID
                                             where con.CondominioID == SessaoLocal.empresaId && cre.UsuarioID == SessaoLocal.usuarioId && con.IndSituacao == "A"
                                             select cre.CredenciadoID).FirstOrDefault();
                #endregion

                #region Grupos do Condômino
                var _GrupoCondominoID = (from g in db.GrupoCondominoUsuarios where g.CondominoID == SessaoLocal.CondominoID select g.GrupoCondominoID).AsEnumerable();
                if (_GrupoCondominoID.Count() > 0)
                {
                    int i = 0;
                    SessaoLocal.GrupoCondominoID = new int[_GrupoCondominoID.Count()];
                    foreach (int GrupoCondominoID in _GrupoCondominoID)
                        SessaoLocal.GrupoCondominoID[i++] = GrupoCondominoID;
                }
                #endregion

                SessaoLocal.Unidades = (from uni in db.Unidades
                                        join con in db.CondominoUnidades on new { uni.CondominioID, uni.EdificacaoID, uni.UnidadeID } equals new { con.CondominioID, con.EdificacaoID, con.UnidadeID }
                                        where con.CondominoID == SessaoLocal.CondominoID
                                        select uni).ToList();
            }
            else 
            {
                #region É Fornecedor
                SessaoLocal.FilaFornecedorID = (from f in db.FilaAtendimentos
                                                join u in db.FilaAtendimentoUsuarios on f.FilaAtendimentoID equals u.FilaAtendimentoID
                                                where f.CondominioID == SessaoLocal.empresaId &&
                                                      u.UsuarioID == SessaoLocal.usuarioId &&
                                                      u.Situacao == "A" &&
                                                      f.IsFornecedor == "S"
                                                select f.FilaAtendimentoID).FirstOrDefault();
                #endregion
            }
            #endregion

            return SessaoLocal;
        }

        public static int FilaCondominoID(Sessao sessaoCorrente, ApplicationContext db)
        {
            return db.FilaAtendimentos.Where(info => info.CondominioID == sessaoCorrente.empresaId && info.Descricao.ToLower() == "condôminos").FirstOrDefault().FilaAtendimentoID;
        }

        /// <summary>
        /// Abre a conexão com o banco de dados e retorna a SessaoLocal
        /// </summary>
        /// <param name="Token"></param>
        /// <returns>Retorna Null se a sessão estiver expirada</returns>
        public static SessaoLocal GetSessaoLocal(string Token = null)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
                Sessao sessaoCorrente = security.getSessaoCorrente(Token);
                SessaoLocal SessaoLocal = null; // se a sessão estiver expirada retorna null
                if (sessaoCorrente == null)
                    return SessaoLocal;
                else
                    return GetSessaoLocal(sessaoCorrente, db);
            }
        }

    }
}