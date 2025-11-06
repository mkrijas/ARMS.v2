using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
namespace ArmsServices.DataServices
{
    public interface IInventoryItemLockService
    {
        InventoryItemLockModel Update(InventoryItemLockModel model);  //Edit
        InventoryItemLockModel SelectByID(int ID);
        InventoryItemLockModel SelectByItem(int ItemID);
        IEnumerable<InventoryItemLockModel> Select();
        int Delete(int ID, string UserID);  //Delete
    }
}