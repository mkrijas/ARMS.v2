using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IInventoryItemService
    {
        InventoryItemModel Update(InventoryItemModel model);  //Edit
        InventoryItemModel SelectByID(int? ID);
        IEnumerable<InventoryItemModel> SelectListByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<InventoryItemModel> SelectByGroup(int? GroupID);
        IEnumerable<InventoryItemModel> SearchByItemCode(string itemCode);
        IEnumerable<InventoryItemModel> SearchByDescription(string itemDescription);
        IEnumerable<InventoryItemModel> SearchByHsn(string HsnCode);
    }
}