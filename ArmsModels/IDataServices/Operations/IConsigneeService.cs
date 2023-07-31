using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{

    public interface IConsigneeService
    {
        Task<ConsigneeModel> Update(ConsigneeModel model);  //Edit
        Task<ConsigneeModel> SelectByID(int? ID);
        Task<int> Delete(int? ConsigneeID, string UserID);  //Delete
        IAsyncEnumerable<ConsigneeModel> Select(int? ConsigneeID);
        IAsyncEnumerable<ConsigneeModel> SelectByOrder(int? ID);
    }
}