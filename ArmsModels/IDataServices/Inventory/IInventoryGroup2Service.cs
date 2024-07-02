
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{

    public interface IInventoryGroup2Service
    {
        InventoryGroup2Model Update(InventoryGroup2Model model);  //Edit
        InventoryGroup2Model SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<InventoryGroup2Model> SearchByName(string GroupName);
    }
}