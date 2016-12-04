using App_Dominio.Entidades;
using App_Dominio.Security;
using DWM.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    public class DWMContextLocal : DWMContext<ApplicationContext>
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

    }
}