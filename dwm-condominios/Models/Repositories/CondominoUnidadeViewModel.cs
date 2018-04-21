using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;

namespace DWM.Models.Repositories
{
    public class CondominoUnidadeViewModel : Repository
    {
        [DisplayName("Condomínio ID")]
        [Required(ErrorMessage = "Informe o CondominioID")]
        public int CondominioID { get; set; }

        [DisplayName("Edificação ID")]
        [Required(ErrorMessage = "Informe a EdificacaoID")]
        public int EdificacaoID { get; set; }

        [DisplayName("Unidade ID")]
        [Required(ErrorMessage = "Informe a UnidadeID")]
        public int UnidadeID { get; set; }

        public string Codigo { get; set; }

        [DisplayName("Condomino ID")]
        public int CondominoID { get; set; }

        [DisplayName("Data de Início")]
        [Required(ErrorMessage = "Informe a Data de Início")]
        public System.DateTime DataInicio { get; set; }

        [DisplayName("Data Fim")]
        public System.DateTime? DataFim { get; set; }

        public virtual CondominoViewModel CondominoViewModel { get; set; }

        public CredenciadoViewModel CredenciadoViewModel { get; set; }

        public FuncionarioViewModel FuncionarioViewModel { get; set; }

        public string EdificacaoDescricao { get; set; }

        public string Nome
        {
            get
            {
                if (CondominoViewModel != null)
                    return CondominoViewModel.Nome.ToUpper();
                else if (CredenciadoViewModel != null)
                    return CredenciadoViewModel.Nome.ToUpper();
                else if (FuncionarioViewModel != null)
                    return FuncionarioViewModel.Nome.ToUpper();
                else
                    return "Indefinido";
            }
        }

        public string DescricaoTipoCondomino
        {
            get
            {
                if (CondominoViewModel != null && CondominoViewModel.IndProprietario == "S")
                    return "Proprietário";
                else if (CondominoViewModel != null && CondominoViewModel.IndProprietario == "N")
                    return "Inquilino";
                else if (CredenciadoViewModel != null)
                    return CredenciadoViewModel.DescricaoTipoCredenciado;
                else if (FuncionarioViewModel != null)
                    return FuncionarioViewModel.Funcao;
                else
                    return "Indefinido";
            }
        }

        public string Sexo
        {
            get
            {
                if (CondominoViewModel != null)
                    return ((CondominoPFViewModel)CondominoViewModel).Sexo;
                else if (CredenciadoViewModel != null)
                    return CredenciadoViewModel.Sexo;
                else if (FuncionarioViewModel != null)
                    return FuncionarioViewModel.Sexo;
                else
                    return "Indefinido";
            }
        }

        public string Email
        {
            get
            {
                if (CondominoViewModel != null)
                    return ((CondominoPFViewModel)CondominoViewModel).Email;
                else if (CredenciadoViewModel != null)
                    return CredenciadoViewModel.Email;
                else if (FuncionarioViewModel != null)
                    return "";
                else
                    return "";
            }
        }

        public string Telefone
        {
            get
            {
                if (CondominoViewModel != null)
                    return App_Dominio.Models.Funcoes.FormataTelefone(((CondominoPFViewModel)CondominoViewModel).TelParticular1);
                else if (CredenciadoViewModel != null)
                    return "";
                else if (FuncionarioViewModel != null)
                    return "";
                else
                    return "";
            }
        }


    }
}