using App_Dominio.Component;
using App_Dominio.Entidades;
using DWM.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    public abstract class ListViewModelLocal<R> : ListViewModel<R, ApplicationContext>
        where R : Repository
    {
        protected SessaoLocal SessaoLocal { get; set; }

        public override void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
            SessaoLocal = DWMSessaoLocal.GetSessaoLocal(this.sessaoCorrente, _db);
        }
    }
}