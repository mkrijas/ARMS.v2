using ArmsModels.BaseModels;
using ArmsModels.BaseModels.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IAssetSettingsService
    {
        IEnumerable<AssetSettingsModel> GetSettings(int? SubClassID, int? TruckID);
        IEnumerable<AssetSettingsModel> SelectByID(int? SubClassID);
        int Update(AssetSettingsModel obj, string UserID);  //Edit
    }
}