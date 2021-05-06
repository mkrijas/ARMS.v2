using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IPartyService
    {       
        PartyModel Update(PartyModel model);
        int Delete(int PartyID, string UserID);
        IEnumerable<PartyModel> Select(int? PartyID);
    }

    public class PartyService : IPartyService
    {
        IDbService Iservice;

        public PartyService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public PartyModel Update(PartyModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@PartyID", model.PartyID),
               new SqlParameter("@PartyName", model.PartyName),
               new SqlParameter("@NatureOfFirm", model.NatureOfFirm),
               new SqlParameter("@TcsApplicable", model.TcsApplicable),
               new SqlParameter("@TdsApplicable", model.TdsApplicable),             
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            PartyModel rmodel = new PartyModel();
            using (var reader = Iservice.GetDataReader("[usp.Entity.PartyUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new PartyModel
                    {
                        PartyID = reader.GetInt16("PartyID"),
                        PartyName = reader.GetString("PartyName"),
                        NatureOfFirm = reader.SafeGetString("NatureOfFirm"),
                        TcsApplicable = reader.GetBoolean("TcsApplicable"),
                        TdsApplicable = reader.GetBoolean("TdsApplicable"),
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
        public int Delete(int PartyID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", PartyID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Entity.PartyDelete]", parameters);
        }
        public IEnumerable<PartyModel> Select(int? PartyID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", PartyID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Entity.PartySelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new PartyModel
                    {
                        PartyID = reader.GetInt16("PartyID"),
                        PartyName = reader.GetString("PartyName"),
                        NatureOfFirm = reader.SafeGetString("NatureOfFirm"),
                        TcsApplicable = reader.GetBoolean("TcsApplicable"),
                        TdsApplicable = reader.GetBoolean("TdsApplicable"),
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
