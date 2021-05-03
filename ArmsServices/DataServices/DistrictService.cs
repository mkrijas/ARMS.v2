using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IDistrictService
    {       
        DistrictModel Update(DistrictModel model);
        int Delete(int DistrictID, string UserID);
        IEnumerable<DistrictModel> Select(int? DistrictID);
    }

    public class DistrictService : IDistrictService
    {
        IDbService Iservice;

        public DistrictService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public DistrictModel Update(DistrictModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@DistrictID", model.DistrictID),
               new SqlParameter("@DistrictName", model.DistrictName),
               new SqlParameter("@StateID", model.StateID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            DistrictModel rmodel = new DistrictModel();
            using (var reader = Iservice.GetDataReader("[usp.Place.DistrictsUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new DistrictModel
                    {
                        DistrictID = reader.GetInt16("DistrictID"),
                        DistrictName = reader.GetString("DistrictName"),
                        StateID = reader.GetInt32("StateID"),                        
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
        public int Delete(int DistrictID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DistrictID", DistrictID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Place.DistrictsDelete]", parameters);
        }
        public IEnumerable<DistrictModel> Select(int? DistrictID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DistrictID", DistrictID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Place.DistrictsSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new DistrictModel
                    {
                        DistrictID = reader.GetInt16("DistrictID"),
                        DistrictName = reader.GetString("DistrictName"),
                        StateID = reader.GetInt32("StateID"),
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
