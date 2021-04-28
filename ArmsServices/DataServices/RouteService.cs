using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IRouteService
    {       
        RouteModel Update(RouteModel model);
        int Delete(int RouteID, string UserID);
        IEnumerable<RouteModel> Select(int? RouteID);
    }

    public class RouteService : IRouteService
    {
        IDbService Iservice;

        public RouteService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public RouteModel Update(RouteModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@RouteID", model.RouteID),
               new SqlParameter("@RouteName", model.RouteName),
               new SqlParameter("@Destination", model.Destination),
               new SqlParameter("@Distance", model.Distance),
               new SqlParameter("@GpsRouteID", model.GpsRouteID),
               new SqlParameter("@MieageModifier", model.MieageModifier),
               new SqlParameter("@Origin", model.Origin),
               new SqlParameter("@RouteType", model.RouteType),
               new SqlParameter("@RunningHours", model.RunningHours),
               new SqlParameter("@SpeedLimit", model.SpeedLimit),
               new SqlParameter("@TollBooths", model.TollBooths),
               new SqlParameter("@Via", model.Via),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            RouteModel rmodel = new RouteModel();
            using (var reader = Iservice.GetDataReader("usp.Gc.RoutesUpdate", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new RouteModel
                    {
                        Destination = reader.GetInt32("Destination"),
                        Distance = reader.GetDecimal("Distance"),
                        GpsRouteID = reader.GetInt64("GpsRouteID"),
                        MieageModifier = reader.GetDecimal("MieageModifier"),
                        Origin = reader.GetInt32("Origin"),
                        RouteID = reader.GetInt32("RouteID"),
                        RouteName = reader.GetString("RouteName"),
                        RouteType = reader.SafeGetString("RouteType"),
                        RunningHours = reader.GetInt16("RunningHours"),
                        SpeedLimit = reader.GetByte("SpeedLimit"),
                        TollBooths = reader.GetByte("TollBooths"),
                        Via = reader.GetInt32("Via"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return rmodel;
        }
        public int Delete(int RouteID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RouteID", RouteID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("usp.Gc.RoutesDelete", parameters);
        }
        public IEnumerable<RouteModel> Select(int? RouteID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RouteID", RouteID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Gc.RoutesSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new RouteModel
                    {
                        Destination = reader.GetInt32("Destination"),
                        Distance = reader.GetDecimal("Distance"),
                        GpsRouteID = reader.GetInt64("GpsRouteID"),
                        MieageModifier = reader.GetDecimal("MieageModifier"),
                        Origin = reader.GetInt32("Origin"),
                        RouteID = reader.GetInt32("RouteID"),
                        RouteName = reader.GetString("RouteName"),
                        RouteType = reader.SafeGetString("RouteType"),
                        RunningHours = reader.GetInt16("RunningHours"),
                        SpeedLimit = reader.GetByte("SpeedLimit"),
                        TollBooths = reader.GetByte("TollBooths"),
                        Via = reader.GetInt32("Via"),
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
