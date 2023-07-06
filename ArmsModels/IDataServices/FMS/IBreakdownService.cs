using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IBreakdownService
    {
        BreakdownModel Update(BreakdownModel model);
        BreakdownModel SelectByID(int? ID);
        int Delete(int? BreakdownID, string UserID);
        IEnumerable<BreakdownModel> Select();
        IEnumerable<BreakdownModel> SelectPending(int BranchID);
    }
}