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

        [DisplayName("Condomínio ID")]
        public int CondominioID { get; set; }

        [DisplayName("Edificação ID")]
        public int EdificacaoID { get; set; }

        [DisplayName("Unidade ID")]
        public int UnidadeID { get; set; }

        [DisplayName("Condômino ID")]
        public int CondominoID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Função")]
        public string Funcao { get; set; }

        [DisplayName("Sexo")]
        public string Sexo { get; set; }

        [DisplayName("Dia")]
        public string Dia { get; set; }

        [DisplayName("Hora Inicial")]
        public string HoraInicial { get; set; }

        [DisplayName("Hora Final")]
        public string HoraFinal { get; set; }

        [DisplayName("RG")]
        public string RG { get; set; }
    }
}