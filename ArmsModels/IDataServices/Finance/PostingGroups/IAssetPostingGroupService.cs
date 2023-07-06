using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IAssetPostingGroupService
    {
        AssetPostingGroupModel Update(AssetPostingGroupModel model);
        AssetPostingGroupModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<AssetPostingGroupModel> Select();
        AssetPostingGroupModel GetPostingGroup(int? AssetID);
    }
}