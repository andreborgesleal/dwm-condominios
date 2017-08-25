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

namespace DWM.Models.BI
{
    public class GetUnidadeBI : DWMContextLocal, IProcess<UnidadeViewModel, ApplicationContext>
    {
        #region Constructor
        public GetUnidadeBI() { }

        public override void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        #endregion

        public UnidadeViewModel Run(Repository value)
        {
            UnidadeViewModel key = (UnidadeViewModel)value;

            Unidade u = this.db.Unidades.Find(key.CondominioID, key.EdificacaoID, key.UnidadeID);

            return new UnidadeViewModel()
            {
                CondominioID = u.CondominioID,
                empresaId = u.CondominioID,
                EdificacaoID = u.EdificacaoID,
                UnidadeID = u.UnidadeID,
                Codigo = u.Codigo,
                TipoCondomino = u.TipoCondomino,
                TipoUnidade = u.TipoUnidade,
                NomeCondomino = u.NomeCondomino,
                Email = u.Email,
                Validador = u.Validador
            };
        }

        public IEnumerable<UnidadeViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }

    }
}