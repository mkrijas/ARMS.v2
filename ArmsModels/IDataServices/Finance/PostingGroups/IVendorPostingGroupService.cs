using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IVendorPostingGroupService
    {
        VendorPostingGroupModel Update(VendorPostingGroupModel model);  //Edit
        VendorPostingGroupModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<VendorPostingGroupModel> Select();
        VendorPostingGroupModel GetPostingGroup(int? VendorID);
    }
}