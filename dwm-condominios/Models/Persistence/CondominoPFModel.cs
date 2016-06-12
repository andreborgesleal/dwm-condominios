using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using DWM.Models.Persistence;
using System.Web;

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
            condomino.DataNascimento = value.DataNascimento;
            condomino.IndAnimal = value.IndAnimal;
            condomino.Sexo = value.Sexo;

            return condomino;
        }

        public override CondominoPFViewModel MapToRepository(CondominoPF entity)
        {
            CondominoPFViewModel condominoViewModel = base.MapToRepository(entity);
            condominoViewModel.DataNascimento = entity.DataNascimento;
            condominoViewModel.Sexo = entity.Sexo;
            condominoViewModel.IndAnimal = entity.IndAnimal;

            return condominoViewModel;
        }

        public override CondominoPF Find(CondominoPFViewModel key)
        {
            return db.CondominoPFs.Find(key.CondominoID);
        }
        #endregion
    }

    public class ListViewCondominoPF : ListViewModel<CondominoPFViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewCondominoPF() { }
        public ListViewCondominoPF(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<CondominoPFViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param[0] != null && param[0].ToString() != "" ? param[0].ToString() : null;

            IEnumerable<CondominoPFViewModel> query = (from c in db.CondominoPFs
                                                       where c.CondominioID == sessaoCorrente.empresaId 
                                                             && c.IndFiscal.Length == 11
                                                             && c.IndSituacao == "A"
                                                             && (_nome == null || c.Nome.Contains(_nome) || c.IndFiscal == _nome || c.Email == _nome)
                                                       select new CondominoPFViewModel()
                                                       {
                                                          CondominioID = c.CondominioID,
                                                          CondominoID = c.CondominoID,
                                                          Nome = c.Nome,
                                                          IndFiscal = c.IndFiscal,
                                                          IndProprietario = c.IndProprietario,
                                                          TelParticular1 = c.TelParticular1,
                                                          TelParticular2 = c.TelParticular2,
                                                          Email = c.Email,
                                                          UsuarioID = c.UsuarioID,
                                                          DataCadastro = c.DataCadastro,
                                                          Avatar = c.Avatar,
                                                      }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            return query;
        }

        public override string action()
        {
            return "../Home/ListCondominosPF";
        }

        public override string DivId()
        {
            return "div-condomino-pf";
        }

        public override Repository getRepository(Object id)
        {
            return new CondominoPFModel().getObject((CondominoPFViewModel)id);
        }
        #endregion
    }

}