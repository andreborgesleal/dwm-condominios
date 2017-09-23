using App_Dominio.Component;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using DWM.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    public abstract class CrudModelLocal<E, R> : CrudModel<E, R, ApplicationContext>, ICrudModelLocal<R, ApplicationContext>
        where E : class
        where R : Repository
    {
        protected SessaoLocal SessaoLocal { get; set; }

        public override void Create(ApplicationContext _db, SecurityContext _seguranca_db) 
        {
            base.Create(_db, _seguranca_db);
            SessaoLocal = DWMSessaoLocal.GetSessaoLocal(this.sessaoCorrente, _db);
        }

        public override void Create(ApplicationContext _db, SecurityContext _seguranca_db, string Token)
        {
            base.Create(_db, _seguranca_db, Token);
            SessaoLocal = DWMSessaoLocal.GetSessaoLocal(this.sessaoCorrente, _db);
        }

        public void Open(ApplicationContext _db, SecurityContext _seguranca_db, string Token)
        {
            Create(_db, _seguranca_db, Token);
        }

        public virtual R AfterCommit(R value)
        {
            return value;
        }

    }
}