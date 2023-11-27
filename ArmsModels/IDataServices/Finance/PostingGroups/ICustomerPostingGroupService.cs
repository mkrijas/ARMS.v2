using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface ICustomerPostingGroupService
    {
        CustomerPostingGroupModel Update(CustomerPostingGroupModel model);  //Edit
        CustomerPostingGroupModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<CustomerPostingGroupModel> Select();
        CustomerPostingGroupModel GetPostingGroup(int? CustomerID);
    }
}