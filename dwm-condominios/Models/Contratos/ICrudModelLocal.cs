using App_Dominio.Component;
using App_Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Dominio.Contratos
{

    public interface ICrudModelLocal<R, D> : ICrudModel<R, D>
        where R : Repository
        where D : DbContext
    {
        void Open(D _db, SecurityContext _seguranca_db, string Token);
    }
}
