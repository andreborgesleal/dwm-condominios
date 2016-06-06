using App_Dominio.Entidades;
using System.Data.Entity;

namespace DWM.Models.Entidades
{
    public class ApplicationContext : App_DominioContext
    {
        public DbSet<Membro> Membros { get; set; }
        public DbSet<ContaVirtual> ContaVirtuals { get; set; }
        public DbSet<TipoConta> TipoContas { get; set;  }
    }
}
