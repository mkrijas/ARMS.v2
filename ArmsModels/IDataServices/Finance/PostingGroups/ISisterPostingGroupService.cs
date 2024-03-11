using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface ISisterPostingGroupService
    {
        SisterPostingGroupModel Update(SisterPostingGroupModel model);  //Edit
        SisterPostingGroupModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        IEnumerable<SisterPostingGroupModel> Select();
        SisterPostingGroupModel GetPostingGroup(int? PartyID);
    }
}