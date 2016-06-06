using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWM.Models.Entidades
{
    [Table("Membro")]
    public class Membro
    {
        [Key]
        [DisplayName("ID")]
        public int MembroID { get; set; }

        [DisplayName("EMail")]
        public string Email { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("CPF")]
        public string CPF { get; set; }

        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        [DisplayName("Banco")]
        public System.Nullable<int> Banco{ get; set; }

        [DisplayName("Agencia")]
        public string Agencia { get; set; }

        [DisplayName("Conta")]
        public string Conta { get; set; }

        [DisplayName("Situacao")]
        public string IndSituacao { get; set; }

        [DisplayName("Avatar")]
        public string Avatar { get; set; }
    }
}