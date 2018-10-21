using App_Dominio.Entidades;
using System.Data.Entity;

namespace DWM.Models.Entidades
{
    public class ApplicationContext : App_DominioContext
    {
        public DbSet<Condominio> Condominios { get; set; }
        public DbSet<Condomino> Condominos { get; set; }
        public DbSet<CondominoPF> CondominoPFs { get; set; }
        public DbSet<CondominoPJ> CondominoPJs { get; set; }
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
        public DbSet<EmailTipo> EmailTipos { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<GrupoCondominoUsuario> GrupoCondominoUsuarios { get; set; }
        public DbSet<FilaAtendimento> FilaAtendimentos { get; set; }
        public DbSet<FilaAtendimentoUsuario> FilaAtendimentoUsuarios { get; set; }
        public DbSet<InformativoComentario> InformativoComentarios { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<ChamadoMotivo> ChamadoMotivos { get; set; }
        public DbSet<ChamadoStatus> ChamadoStatuss { get; set; }
        public DbSet<ChamadoFila> ChamadoFilas { get; set; }
        public DbSet<ChamadoAnexo> ChamadoAnexos { get; set; }
        public DbSet<ChamadoAnotacao> ChamadoAnotacaos { get; set; }
        public DbSet<Arquivo> Arquivos { get; set; }
        public DbSet<Balancete> Balancetes { get; set; }
        public DbSet<SaldoContabil> SaldosContabeis { get; set; }
        public DbSet<Visitante> Visitantes { get; set; }
        public DbSet<VisitanteUnidade> VisitanteUnidades { get; set; }
        public DbSet<VisitanteAcesso> VisitanteAcessos { get; set; }
        public DbSet<VisitanteAcessoUnidade> VisitanteAcessoUnidades { get; set; }
        public DbSet<PrestadorTipo> PrestadorTipos { get; set; }
        public DbSet<AluguelEspaco> AluguelEspacos { get; set; }
        public DbSet<EspacoComum> EspacoComums { get; set; }
        public DbSet<TipoEdificacao> TipoEdificacaos { get; set; }
        public DbSet<RamoAtividade> RamoAtividades { get; set; }
        public DbSet<Proprietario> Proprietarios { get; set; }
        public DbSet<ProprietarioUnidade> ProprietarioUnidades { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Empregado> Empregados { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Credor> Credores { get; set; }
        public DbSet<GrupoCredor> GrupoCredores { get; set; }
        public DbSet<Patrimonio> Patrimonios { get; set; }
        public DbSet<PatrimonioClassificacao> PatrimonioClassificacaos { get; set; }
        public DbSet<PatrimonioLocalizacao> PatrimonioLocalizacaos { get; set; }
        public DbSet<LimpezaInspecao> LimpezaInspecaos { get; set; }
        public DbSet<LimpezaRequisito> LimpezaRequisitos { get; set; }
        public DbSet<LimpezaInspecaoItem> LimpezaInspecaoItem { get; set; }
        public DbSet<CredorTipoServico> CredorTipoServicos { get; set; }
    }
}
