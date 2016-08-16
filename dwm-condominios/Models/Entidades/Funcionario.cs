using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Funcionario")]
    public class Funcionario
    {
        [Key]
        [DisplayName("ID")]
        public int FuncionarioID { get; set; }

        [DisplayName("CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("EdificacaoID")]
        public int EdificacaoID { get; set; }

        [DisplayName("UnidadeID")]
        public int UnidadeID { get; set; }

        [DisplayName("Condomino ID")]
        public int CondominoID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Funcao")]
        public string Funcao { get; set; }

        [DisplayName("Sexo")]
        public string Sexo { get; set; }

        [DisplayName("Dia")]
        public string Dia { get; set; }

        [DisplayName("HoraInicial")]
        public string HoraInicial { get; set; }

        [DisplayName("HoraFinal")]
        public string HoraFinal { get; set; }

        [DisplayName("RG")]
        public string RG { get; set; }
    }
}