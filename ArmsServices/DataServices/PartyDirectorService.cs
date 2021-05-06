using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IPartyDirectorService
    {       
        PartyDirectorModel Update(PartyDirectorModel model);
        int Delete(int PartyDirectorID, string UserID);
        IEnumerable<PartyDirectorModel> Select(int? PartyDirectorID);
    }

    public class PartyDirectorService : IPartyDirectorService
    {
        IDbService Iservice;

        public PartyDirectorService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public PartyDirectorModel Update(PartyDirectorModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@PartyDirectorID", model.PartyDirectorID),
               new SqlParameter("@PersonName", model.PersonName),
               new SqlParameter("@PartyID", model.PartyID),
               new SqlParameter("@Phone", model.Phone),
               new SqlParameter("@Pan", model.Pan),             
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            PartyDirectorModel rmodel = new PartyDirectorModel();
            using (var reader = Iservice.GetDataReader("[usp.Entity.PartyDirectorsUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new PartyDirectorModel
                    {
                        PartyDirectorID = reader.GetInt32("PartyDirectorID"),
                        PersonName = reader.SafeGetString("PersonName"),
                        Pan = reader.SafeGetString("Pan"),
                        PartyID = reader.GetInt32("PartyID"),
                        Phone = reader.SafeGetString("Phone"),
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
        public int Delete(int PartyDirectorID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyDirectorID", PartyDirectorID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Entity.PartyDirectorsDelete]", parameters);
        }
        public IEnumerable<PartyDirectorModel> Select(int? PartyDirectorID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyDirectorID", PartyDirectorID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Entity.PartyDirectorsSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new PartyDirectorModel
                    {
                        PartyDirectorID = reader.GetInt32("PartyDirectorID"),
                        PersonName = reader.SafeGetString("PersonName"),
                        Pan = reader.SafeGetString("Pan"),
                        PartyID = reader.GetInt32("PartyID"),
                        Phone = reader.SafeGetString("Phone"),
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
