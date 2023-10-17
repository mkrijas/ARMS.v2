
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{

    public interface IInventoryGroupService
    {
        InventoryGroupModel Update(InventoryGroupModel model);  //Edit
        InventoryGroupModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<InventoryGroupModel> SearchByName(string GroupName);
    }
}