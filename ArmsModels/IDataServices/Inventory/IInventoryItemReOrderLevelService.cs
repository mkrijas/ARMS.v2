using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IInventoryItemReOrderLevelService
    {
        InventoryItemReOrderLevelModel Update(InventoryItemReOrderLevelModel model);
        InventoryItemReOrderLevelModel SelectByID(int? ID);
        IEnumerable<InventoryItemReOrderLevelModel> SelectByItem(int? InventoryItemID, int? BranchID);
    }
}