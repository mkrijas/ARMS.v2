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
        AssetClassModel UpdateClass(AssetClassModel model);  //Edit
        AssetSubClassModel UpdateSubClass(AssetSubClassModel model);  //Edit
        IEnumerable<AssetSubClassModel> SelectSubClasses(int? ID);
        IEnumerable<AssetSubClassModel> SelectSubClassesByClass(int? ID);
        IEnumerable<AssetClassModel> SelectClasses();
        int DeleteClass(int? AssetClassID, string UserID);  //Delete
        int DeleteSubClass(int? AssetSubClassID, string UserID);  //Delete
    }
}