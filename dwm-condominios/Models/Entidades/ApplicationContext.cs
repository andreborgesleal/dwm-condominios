using App_Dominio.Entidades;
using System.Data.Entity;

namespace DWM.Models.Entidades
{
    public class ApplicationContext : App_DominioContext
    {
        public DbSet<Condominio> Condominios { get; set; }
        public DbSet<Condomino> Condominos { get; set; }
        public DbSet<CondominoPF> CondominoPFs { get; set; }
        public DbSet<Credenciado> Credenciados { get; set; }
        public DbSet<CondominoUnidade> CondominoUnidades { get; set; }
        public DbSet<Unidade> Unidades { get; set;  }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<Edificacao> Edificacaos { get; set; }
        public DbSet<Profissao> Profissaos { get; set; }
        public DbSet<Informativo> Informativos { get; set; }
        public DbSet<TipoCredenciado> TipoCredenciados { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<GrupoCondomino> GrupoCondominos { get; set; }
    }
}
