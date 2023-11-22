using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IAssetDocumentService
    {
        AssetDocumentModel Update(AssetDocumentModel model);  //Edit
        int SaveFilePath(string link, int? id);  //Edit
        AssetDocumentModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);  //Delete
        int DeleteType(int? ID, string UserID);  //DeleteType
        int Remove(AssetDocumentModel model);  //Remove
        IEnumerable<AssetDocumentModel> SelectByPeriod(DateTime? startDate, DateTime? endDate);
        IEnumerable<AssetDocumentModel> SelectWithPast(int? AssetID);
        IEnumerable<AssetDocumentModel> SelectByAsset(int? AssetID);
        IEnumerable<AssetDocumentTypeModel> GetDocumentTypes();
        AssetDocumentTypeModel UpdateDocumentType(AssetDocumentTypeModel model);  //EditType
        IEnumerable<AssetDocumentModel> ValidatePeriod(AssetDocumentModel model);
        bool IsValid(AssetDocumentModel model, DateTime? DateToCheck);
    }
}