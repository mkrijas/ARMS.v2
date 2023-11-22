using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
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
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Place.Districts.Update]", parameters))
            {
                return new DistrictModel
                {
                    DistrictID = dr.GetInt32("DistrictID"),
                    DistrictName = dr.GetString("DistrictName"),
                    StateID = dr.GetInt32("StateID"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
            return null;
        }
        public int Delete(int? DistrictID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DistrictID", DistrictID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Place.Districts.Delete]", parameters);
        }
        public IEnumerable<DistrictModel> Select(int? DistrictID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DistrictID", DistrictID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Place.Districts.Select]", parameters))
            {
                yield return new DistrictModel
                {
                    DistrictID = dr.GetInt32("DistrictID"),
                    DistrictName = dr.GetString("DistrictName"),
                    StateID = dr.GetInt32("StateID"),
                    State = new StateModel()
                    {
                        StateID = dr.GetInt32("StateID"),
                        StateName = dr.GetString("StateName"),
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

        public IEnumerable<StateModel> GetStates()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Place.States.Select]", null))
            {
                yield return new StateModel()
                {
                    StateName = dr.GetString("StateName"),
                    GstString = dr.GetString("GstString"),
                    StateID = dr.GetInt32("StateID"),
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
}
