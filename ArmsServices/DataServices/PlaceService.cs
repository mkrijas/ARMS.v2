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
        Task<PlaceModel> Update(PlaceModel model);
        Task<int> Delete(int PlaceID, string UserID);
        IAsyncEnumerable<PlaceModel> Select(int? PlaceID);
        Task<PlaceModel> SelectByID(int ID);
    }

    public class PlaceService : IPlaceService
    {
        IDbService Iservice;

        public PlaceService(IDbService iservice)
        {
            Iservice = iservice;
        }
        
        public async Task<PlaceModel> Update(PlaceModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PlaceID", model.PlaceID),
               new SqlParameter("@PlaceName", model.PlaceName.ToUpper()),
               //new SqlParameter("@LatLong", model.LatLong),
               new SqlParameter("@PinCode", model.PinCode),
               new SqlParameter("@DistrictID", model.DistrictID),
               new SqlParameter("@GeoFenceID", model.GeoFenceID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Place.PlacesUpdate]", parameters))
            {
                model = await GetModel(dr);
            }             
            return model;
        }
        public async Task<int> Delete(int PlaceID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PlaceID", PlaceID),               
               new SqlParameter("@UserID", UserID),
            };            
            return await Iservice.ExecuteNonQuery("[usp.Place.PlacesDelete]", parameters);
        }
        public async IAsyncEnumerable<PlaceModel> Select(int? PlaceID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PlaceID", PlaceID)               
            };

            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Place.PlacesSelect]", parameters))
            {
                yield return await GetModel(dr);            
            }
        }
        public async Task<PlaceModel> SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PlaceID", ID)
            };
            PlaceModel model = new PlaceModel();
            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Place.PlacesSelect]", parameters))
            {
                 model = await GetModel(dr);
            }
            return model;
        }
        private async Task<PlaceModel> GetModel(IDataRecord dr)
        {
            return new PlaceModel
            {
                DistrictID = dr.GetInt32("DistrictID"),
                GeoFenceID = dr.GetInt64("GeoFenceID"),
                //LatLong = dr.GetLatLongString("LatLong"),
                PinCode = dr.GetString("PinCode"),
                PlaceID = dr.GetInt32("PlaceID"),
                PlaceName = dr.GetString("PlaceName"),
                District = new DistrictModel
                {
                    DistrictName = dr.GetString("DistrictName"),
                    DistrictID = dr.GetInt32("DistrictID"),
                },
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        
    }
}
