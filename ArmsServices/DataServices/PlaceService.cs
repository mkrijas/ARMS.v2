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
        PlaceModel Add(PlaceModel model);
        PlaceModel Update(PlaceModel model);
        IEnumerable<PlaceModel> Select(int? PlaceID);
    }

    public class PlaceService : IPlaceService
    {
        IDbService Iservice;

        public PlaceService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public PlaceModel Add(PlaceModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@PlaceName", model.PlaceName),
               new SqlParameter("@LatLong", model.LatLong),
               new SqlParameter("@PinCode", model.PinCode),               
               new SqlParameter("@DistrictID", model.DistrictID),
               new SqlParameter("@GeoFenceID", model.GeoFenceID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            PlaceModel place = new PlaceModel();
            using (var reader = Iservice.GetDataReader("usp.Place.PlacesInsert", parameters))
            {
                while (reader.Read())
                {
                    place = new PlaceModel
                    {
                        DistrictID = reader.GetInt32("DistrictID"),
                        GeoFenceID = reader.SafeGetInt("GeoFenceID"),
                        LatLong = reader.SafeGetString("LatLong"),
                        PinCode = reader.SafeGetString("PinCode"),
                        PlaceID = reader.GetInt32("PlaceID"),
                        PlaceName = reader.GetString("PlaceName"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return place;        
        }
        public PlaceModel Update(PlaceModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PlaceID", model.PlaceID),
               new SqlParameter("@PlaceName", model.PlaceName),
               new SqlParameter("@LatLong", model.LatLong),
               new SqlParameter("@PinCode", model.PinCode),
               new SqlParameter("@DistrictID", model.DistrictID),
               new SqlParameter("@GeoFenceID", model.GeoFenceID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            PlaceModel place = new PlaceModel();
            using (var reader = Iservice.GetDataReader("usp.Place.PlacesUpdate", parameters))
            {
                while (reader.Read())
                {
                    place = new PlaceModel
                    {
                        DistrictID = reader.GetInt32("DistrictID"),
                        GeoFenceID = reader.SafeGetInt("GeoFenceID"),
                        LatLong = reader.SafeGetString("LatLong"),
                        PinCode = reader.SafeGetString("PinCode"),
                        PlaceID = reader.GetInt32("PlaceID"),
                        PlaceName = reader.GetString("PlaceName"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return place;
        }
        public IEnumerable<PlaceModel> Select(int? PlaceID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PlaceID", PlaceID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Place.PlacesSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new PlaceModel
                    {
                        DistrictID = reader.GetInt32("DistrictID"),
                        GeoFenceID = reader.SafeGetInt("GeoFenceID"),
                        LatLong = reader.SafeGetString("LatLong"),
                        PinCode = reader.SafeGetString("PinCode"),
                        PlaceID = reader.GetInt32("PlaceID"),
                        PlaceName = reader.GetString("PlaceName"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
        }

    }
}
