using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Entidades;
using DWM.Models.Repositories;

namespace DWM.Models.Persistence
{
    public class CondominoPFModel : CondominoModel<CondominoPF, CondominoPFViewModel>
    {
        #region Constructor
        public CondominoPFModel() { }

        public void Create(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            this.db = _db;
            this.seguranca_db = _seguranca_db;
        }

        public CondominoPFModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override CondominoPF MapToEntity(CondominoPFViewModel value)
        {
            CondominoPF condomino = base.MapToEntity(value);
            condomino.ProfissaoID = value.ProfissaoID;
            condomino.DataNascimento = value.DataNascimento;
            condomino.IndAnimal = value.IndAnimal;
            condomino.Sexo = value.Sexo;

            return condomino;
        }

        public override CondominoPFViewModel MapToRepository(CondominoPF entity)
        {
            CondominoPFViewModel condominoViewModel = base.MapToRepository(entity);

            condominoViewModel.ProfissaoID = entity.ProfissaoID;
            condominoViewModel.DataNascimento = entity.DataNascimento;
            condominoViewModel.Sexo = entity.Sexo;
            condominoViewModel.IndAnimal = entity.IndAnimal;
            condominoViewModel.mensagem = new App_Dominio.Contratos.Validate() { Code = 0, Message = "Registro Incluído com sucesso!" };

            return condominoViewModel;
        }

        public override CondominoPF Find(CondominoPFViewModel key)
        {
            return db.CondominoPFs.Find(key.CondominoID);
        }
        #endregion
    }


}