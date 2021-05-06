using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IGstService
    {       
        GstModel Update(GstModel model);
        int Delete(int GstID, string UserID);
        IEnumerable<GstModel> Select(int? GstID);
    }

    public class GstService : IGstService
    {
        IDbService Iservice;

        public GstService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public GstModel Update(GstModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@GstID", model.GstID),
               new SqlParameter("@AddressID", model.AddressID),
               new SqlParameter("@Email", model.Email),
               new SqlParameter("@GstNo", model.GstNo),
               new SqlParameter("@PartyID", model.PartyID),
               new SqlParameter("@Phone", model.Phone),
               new SqlParameter("@RegName", model.RegName),
               new SqlParameter("@TanNo", model.TanNo),
               new SqlParameter("@TradeName", model.TradeName),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            GstModel rmodel = new GstModel();
            using (var reader = Iservice.GetDataReader("[usp.Entity.GstsUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new GstModel
                    {
                        GstID = reader.GetInt32("GstID"),
                        AddressID = reader.GetInt32("AddressID"),
                        Email = reader.GetString("Email"),
                        GstNo = reader.SafeGetString("GstNo"),
                        PartyID = reader.GetInt32("PartyID"),
                        Phone = reader.SafeGetString("Phone"),
                        RegName = reader.SafeGetString("RegName"),
                        TanNo = reader.SafeGetString("TanNo"),
                        TradeName = reader.SafeGetString("TradeName"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStamp"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return rmodel;
        }
        public int Delete(int GstID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GstID", GstID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Entity.GstsDelete]", parameters);
        }
        public IEnumerable<GstModel> Select(int? GstID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GstID", GstID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Entity.GstsSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new GstModel
                    {
                        GstID = reader.GetInt32("GstID"),
                        AddressID = reader.GetInt32("AddressID"),
                        Email = reader.GetString("Email"),
                        GstNo = reader.SafeGetString("GstNo"),
                        PartyID = reader.GetInt32("PartyID"),
                        Phone = reader.SafeGetString("Phone"),
                        RegName = reader.SafeGetString("RegName"),
                        TanNo = reader.SafeGetString("TanNo"),
                        TradeName = reader.SafeGetString("TradeName"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStamp"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
        }

    }
}
