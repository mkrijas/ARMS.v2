using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IPlaceService
    {
        Task<PlaceModel> Update(PlaceModel model);  //Edit
        Task<int> Delete(int? PlaceID, string UserID);  //Edit
        IEnumerable<PlaceModel> Select(int? PlaceID);
        Task<PlaceModel> SelectByID(int? ID);
    }
}