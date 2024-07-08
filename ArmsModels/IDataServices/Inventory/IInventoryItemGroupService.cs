
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{

    public interface IInventoryItemGroupService
    {
        InventoryItemGroupModel Update(InventoryItemGroupModel model);  //Edit
        InventoryItemGroupModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<InventoryItemGroupModel> SearchByName(string GroupName);
        IEnumerable<InventoryItemGroupModel> SelectByGroup(int? GroupID);
    }
}