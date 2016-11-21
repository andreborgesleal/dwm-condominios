﻿using App_Dominio.Entidades;
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
                int i = 0;
                var _GrupoCondominoID = (from g in db.GrupoCondominoUsuarios where g.CondominoID == SessaoLocal.CondominoID select g.GrupoCondominoID).AsEnumerable();
                foreach (int GrupoCondominoID in _GrupoCondominoID)
                    SessaoLocal.GrupoCondominoID[i++] = GrupoCondominoID;
                #endregion

                SessaoLocal.Unidades = (from uni in db.Unidades
                                        join con in db.CondominoUnidades on new { uni.CondominioID, uni.EdificacaoID, uni.UnidadeID } equals new { con.CondominioID, con.EdificacaoID, con.UnidadeID }
                                        where con.CondominoID == SessaoLocal.CondominoID
                                        select uni).ToList();
            }
            #endregion

            return SessaoLocal;
        }
    }
}