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
        Task<DistrictModel> Update(DistrictModel model);
        Task<int> Delete(int DistrictID, string UserID);
        IAsyncEnumerable<DistrictModel> Select(int? DistrictID);
    }

    public class DistrictService : IDistrictService
    {
        IDbService Iservice;

        public DistrictService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public async Task<DistrictModel> Update(DistrictModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@DistrictID", model.DistrictID),
               new SqlParameter("@DistrictName", model.DistrictName),
               new SqlParameter("@StateID", model.StateID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Place.DistrictsSelect]", parameters))
                {                    
                        model =  new DistrictModel
                        {
                            DistrictID = dr.GetInt32(dr.GetOrdinal("DistrictID")),
                            DistrictName = dr.GetString(dr.GetOrdinal("DistrictName")),
                            StateID = dr.GetInt32(dr.GetOrdinal("StateID")),
                            UserInfo = new ArmsModels.SharedModels.UserInfoModel
                            {
                                RecordStatus = dr.GetByte(dr.GetOrdinal("RecordStatus")),
                                TimeStampField = dr.GetDateTime(dr.GetOrdinal("TimeStamp")),
                                UserID = dr.GetString(dr.GetOrdinal("UserID")),
                            },
                        };                    
                }
            return model;
        }
        public async Task<int> Delete(int DistrictID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DistrictID", DistrictID),               
               new SqlParameter("@UserID", UserID),
            };            
            return await Iservice.ExecuteNonQuery("[usp.Place.DistrictsDelete]", parameters);
        }
        public async IAsyncEnumerable<DistrictModel> Select(int? DistrictID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DistrictID", DistrictID)               
            };

             await foreach(IDataRecord dr in Iservice.GetDataReader("[usp.Place.DistrictsSelect]", parameters))
            {
                    yield return new DistrictModel
                    {
                        DistrictID = dr.GetInt32(dr.GetOrdinal("DistrictID")),
                        DistrictName = dr.GetString(dr.GetOrdinal("DistrictName")),
                        StateID = dr.GetInt32(dr.GetOrdinal("StateID")),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = dr.GetByte(dr.GetOrdinal("RecordStatus")),
                            TimeStampField = dr.GetDateTime(dr.GetOrdinal("TimeStamp")),
                            UserID = dr.GetString(dr.GetOrdinal("UserID")),
                        },
                    };
            }
        }

    }
}
