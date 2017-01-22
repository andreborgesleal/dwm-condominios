using App_Dominio.Component;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Pattern;
using System.Data.Entity;

namespace DWM.Models.Pattern
{
    public class FactoryLocalhost<R, D> : Factory<R, D>
        where R : Repository
        where D : DbContext
    {
        public override R Execute(IProcess<R, D> proc, Repository value = null)
        {
            using (db = getContextInstance())
            {
                using (seguranca_db = new SecurityContext())
                {
                    proc.Create(db, seguranca_db);
                    R r = proc.Run(value);

                    if (r != null && r.mensagem.Code == 0)
                    {
                        db.SaveChanges();
                        seguranca_db.SaveChanges();
                    }

                    Mensagem = r != null ? r.mensagem : null;
                    return r;
                }
            }
        }
    }
}