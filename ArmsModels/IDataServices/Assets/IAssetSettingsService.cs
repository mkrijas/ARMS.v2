using ArmsModels.BaseModels;
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
       
        IEnumerable<AssetSettingsModel> SelectAssetSettings(int? ID);

    }
}
