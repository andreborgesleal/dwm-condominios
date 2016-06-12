using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using System.Data.Objects.SqlClient;
using DWM.Models.Entidades;

namespace DWM.Models.Enumeracoes
{
    public class BindDropDownList
    {
        //public IEnumerable<SelectListItem> Membros(params object[] param)
        //{
        //    // params[0] -> cabeçalho (Selecione..., Todos...)
        //    // params[1] -> SelectedValue
        //    string cabecalho = param[0].ToString();
        //    string selectedValue = param[1].ToString();

        //    using (ApplicationContext db = new ApplicationContext())
        //    {
        //        IList<SelectListItem> q = new List<SelectListItem>();

        //        if (cabecalho != "")
        //            q.Add(new SelectListItem() { Value = "", Text = cabecalho });

        //        q = q.Union(from e in db.Membros.AsEnumerable()
        //                    orderby e.Nome
        //                    select new SelectListItem()
        //                    {
        //                        Value = e.MembroID.ToString(),
        //                        Text = e.Nome,
        //                        Selected = (selectedValue != "" ? e.Nome.Equals(selectedValue) : false)
        //                    }).ToList();

        //        return q;
        //    }
        //}

        //public IEnumerable<SelectListItem> Categorias(params object[] param)
        //{
        //    // params[0] -> cabeçalho (Selecione..., Todos...)
        //    // params[1] -> SelectedValue
        //    string cabecalho = param[0].ToString();
        //    string selectedValue = param[1].ToString();

        //    using (ApplicationContext db = new ApplicationContext())
        //    {
        //        IList<SelectListItem> q = new List<SelectListItem>();

        //        if (cabecalho != "")
        //            q.Add(new SelectListItem() { Value = "", Text = cabecalho });

        //        q = q.Union(from e in db.Categorias.AsEnumerable()
        //                    orderby e.descricao
        //                    select new SelectListItem()
        //                    {
        //                        Value = e.categoriaId.ToString(),
        //                        Text = e.descricao,
        //                        Selected = (selectedValue != "" ? e.descricao.Equals(selectedValue) : false)
        //                    }).ToList();

        //        return q;
        //    }
        //}
    }
}