using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IExpenseMappingServices
    {
        IEnumerable<ExpenseMapping> Select();
        IEnumerable<ExpenseMapping> SelectByArea(string Area);
        ExpenseMapping Update(ExpenseMapping model);  //Edit
        ExpenseMapping SelectByID(int ID);  
        int Delete(int? ID, string UserID);  //Edit
    }
}