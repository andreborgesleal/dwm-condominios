using App_Dominio.Component;
using App_Dominio.Entidades;
using App_Dominio.Security;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DWM.Models.Repositories
{
    public class GrupoCondominoUsuarioViewModel : Repository
    {
        [DisplayName("Grupo Condômino")]
        public int GrupoCondominoID { get; set; }

        [DisplayName("Condômino")]
        public int CondominoID { get; set; }

        public string DescricaoGrupo { get; set; }

        public string Situacao { get; set; }
    }
}