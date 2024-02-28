using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class RouteService : IRouteService
    {
        IDbService Iservice;
        public RouteService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public async Task<RouteModel> Update(RouteModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RouteID", model.RouteID),
               new SqlParameter("@RouteName", model.RouteName),
               new SqlParameter("@Destination", model.Destination.PlaceID),
               new SqlParameter("@Distance", model.Distance),
               new SqlParameter("@GpsRouteID", model.GpsRouteID),
               new SqlParameter("@MieageModifier", model.MieageModifier),
               new SqlParameter("@Origin", model.Origin.PlaceID),
               new SqlParameter("@RouteType", model.RouteType),
               new SqlParameter("@RunningHours", model.RunningHours),
               new SqlParameter("@SpeedLimit", model.SpeedLimit),
               new SqlParameter("@TollBooths", model.TollBooths),
               new SqlParameter("@Via", model.Via.PlaceID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Route.Update]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }
        public async Task<RouteModel> SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByRoute"),
            };
            RouteModel model = new RouteModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Route.Select]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }
        public int Delete(int? RouteID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RouteID", RouteID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Gc.Route.Delete]", parameters);
        }
        public async IAsyncEnumerable<RouteModel> Select(int? RouteID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", RouteID) ,
               new SqlParameter("@Operation", "ByRoute"),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Route.Select]", parameters))
            {
                yield return await GetModel(dr);
            }
        }

        public async IAsyncEnumerable<RouteModel> GetDisabled(int? RouteID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", RouteID) ,
               new SqlParameter("@Operation", "GetDisabled"),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Route.Select]", parameters))
            {
                yield return await GetModel(dr);
            }
        }

        public async IAsyncEnumerable<RouteModel> SelectByOrder(int? OrderID = 0)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", OrderID),
               new SqlParameter("@Operation", "ByOrder"),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Route.Select]", parameters))
            {
                yield return await GetModel(dr);
            }
        }
        private async Task<RouteModel> GetModel(IDataRecord dr)
        {
            return new RouteModel
            {
                Destination = new PlaceModel
                {
                    PlaceID = dr.GetInt32("Destination")
                },
                Distance = dr.GetDecimal("Distance"),
                GpsRouteID = dr.GetInt64("GpsRouteID"),
                MieageModifier = dr.GetDecimal("MieageModifier"),
                Origin = new PlaceModel
                {
                    PlaceID = dr.GetInt32("Origin")
                },
                RouteID = dr.GetInt32("RouteID"),
                RouteName = dr.GetString("RouteName"),
                RouteType = dr.GetString("RouteType"),
                RunningHours = dr.GetDecimal("RunningHours"),
                SpeedLimit = dr.GetByte("SpeedLimit"),
                TollBooths = dr.GetByte("TollBooths"),
                Via = new PlaceModel
                {
                    PlaceID = dr.GetInt32("Via")
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
