using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IAssetClassService
    {
        AssetClassModel UpdateClass(AssetClassModel model);
        AssetSubClassModel UpdateSubClass(AssetSubClassModel model);
        IEnumerable<AssetSubClassModel> SelectSubClasses(int? ID);
        IEnumerable<AssetSubClassModel> SelectSubClassesByClass(int? ID);
        IEnumerable<AssetClassModel> SelectClasses();
        int DeleteClass(int? AssetClassID, string UserID);
        int DeleteSubClass(int? AssetSubClassID, string UserID);
    }
}
   

     