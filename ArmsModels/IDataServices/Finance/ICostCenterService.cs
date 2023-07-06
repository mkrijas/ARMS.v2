using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ICostCenterService
    {
        CostCenterModel Update(CostCenterModel model);
        CostCenterModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);        
        IEnumerable<CostCenterModel> SelectByCategory(int? CategoryID);
        IEnumerable<CostCenterModel> Select();
        IEnumerable<CategoryModel> SelectCategory();
        CategoryModel UpdateCategory(CategoryModel model);
    }
}