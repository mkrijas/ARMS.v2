using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IDimensionService
    {
        DimensionModel Update(DimensionModel model);
        DimensionModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<DimensionModel> SelectByCategory(int? CategoryID);
        IEnumerable<DimensionModel> Select();
        IEnumerable<CategoryModel> SelectCategory();
        CategoryModel UpdateCategory(CategoryModel model);
    }
}