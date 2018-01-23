using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using System.Web.Mvc;
using App_Dominio.Security;
using System.Linq;
using App_Dominio.Repositories;

namespace DWM.Models.BI
{
    public class VisitanteAcessoChecarEdicaoBI : DWMContextLocal, IProcess<VisitanteAcessoViewModel, ApplicationContext>
    {
        #region Constructor
        public VisitanteAcessoChecarEdicaoBI()
        {
        }

        public VisitanteAcessoChecarEdicaoBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        private bool IsUserAdm()
        {
            return SessaoLocal.Unidades == null;
            //return DWMSessaoLocal.GetSessaoLocal(sessaoCorrente, this.db).Unidades == null;
        }

        private bool IsPortaria()
        {
            string grupo_portaria = db.Parametros.Find(SessaoLocal.empresaId, (int)Enumeracoes.Enumeradores.Param.GRUPO_PORTARIA).Valor;
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            IEnumerable<GrupoRepository> grp = security.getGrupoUsuario(SessaoLocal.usuarioId, SessaoLocal.sessaoId).AsEnumerable();

            return (from g in grp where g.grupoId == int.Parse(grupo_portaria) select g).Count() > 0;
        }


        /// <summary>
        /// Verifica se o visistante informado pertence à lista de visitantes do condômino
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public VisitanteAcessoViewModel Run(Repository value)
        {
            VisitanteAcessoViewModel r = (VisitanteAcessoViewModel)value;
            if (IsUserAdm() || IsPortaria())
                r.mensagem = new Validate() { Code = -1, Message = "Autorizado" };
            else
            {
                // Verifica se o AcessoID é da lista do CondominoID que está logado
                int x = (from q in db.VisitanteAcessoUnidades
                         where q.CondominioID == SessaoLocal.empresaId
                                && (q.CondominoID == SessaoLocal.CondominoID || q.CredenciadoID == SessaoLocal.CredenciadoID)
                                && q.AcessoID == r.AcessoID
                         select q).Count();

                if (x > 0)
                    r.mensagem = new Validate() { Code = -1, Message = "Autorizado" };
                else
                    r.mensagem = new Validate() { Code = -2, Message = "Não Autorizado" };
            }

            return r;
        }

        public IEnumerable<VisitanteAcessoViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }
    }
}