using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IRenterPostingGroupService
    {
        RenterPostingGroupModel Update(RenterPostingGroupModel model);  //Edit
        RenterPostingGroupModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<RenterPostingGroupModel> Select();
        RenterPostingGroupModel GetPostingGroup(int? RenterID);
    }
}