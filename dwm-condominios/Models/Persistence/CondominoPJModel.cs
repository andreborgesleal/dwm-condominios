using App_Dominio.Entidades;
using DWM.Models.Entidades;
using DWM.Models.Repositories;

namespace DWM.Models.Persistence
{
    public class CondominoPJModel : CondominoModel<CondominoPJ, CondominoPJViewModel>
    {
        #region Constructor
        public CondominoPJModel() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public CondominoPJModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override CondominoPJ MapToEntity(CondominoPJViewModel value)
        {
            CondominoPJ condomino = base.MapToEntity(value);
            condomino.Administrador = value.Administrador;
            condomino.NomeFantasia = value.NomeFantasia;
            condomino.RamoAtividadeID = value.RamoAtividadeID;

            return condomino;
        }

        public override CondominoPJViewModel MapToRepository(CondominoPJ entity)
        {
            CondominoPJViewModel condominoViewModel = base.MapToRepository(entity);

            condominoViewModel.Administrador = entity.Administrador;
            condominoViewModel.NomeFantasia = entity.NomeFantasia;
            condominoViewModel.RamoAtividadeID = entity.RamoAtividadeID;
            condominoViewModel.mensagem = new App_Dominio.Contratos.Validate() { Code = 0, Message = "Registro Incluído com sucesso!" };

            return condominoViewModel;
        }

        public override CondominoPJ Find(CondominoPJViewModel key)
        {
            return db.CondominoPJs.Find(key.CondominoID);
        }
        #endregion

    }
}