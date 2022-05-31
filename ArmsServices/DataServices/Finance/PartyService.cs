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
        Task<PartyModel> Update(PartyModel model);
        Task<PartyModel> SelectByID(int? ID);
        Task<int> Delete(int? PartyID, string UserID);
        IAsyncEnumerable<PartyModel> Select(int? PartyID);
    }

    public class PartyService : IPartyService
    {
        IDbService Iservice;

        public PartyService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public async Task<PartyModel> Update(PartyModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", model.PartyID),
               new SqlParameter("@PartyName", model.PartyName),
               new SqlParameter("@IsClient", model.IsClient),
               new SqlParameter("@IsSupplier", model.IsSupplier),
               new SqlParameter("@NatureOfFirm", model.NatureOfFirm),
               new SqlParameter("@AssesseeTypeID", model.AssesseeTypeID),
               new SqlParameter("@PAN", model.PAN),
               new SqlParameter("@TcsApplicable", model.TcsApplicable),
               new SqlParameter("@TdsApplicable", model.TdsApplicable),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            await foreach (IDataRecord reader in Iservice.GetDataReaderAsync("[usp.Entity.PartyUpdate]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }
        public async Task<PartyModel> SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", ID),               
            };
            PartyModel model = new PartyModel();
            await foreach( IDataRecord reader in Iservice.GetDataReaderAsync("[usp.Entity.PartySelect]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }
        public async Task<int> Delete(int? PartyID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", PartyID),               
               new SqlParameter("@UserID", UserID),
            };            
            return await Iservice.ExecuteNonQueryAsync("[usp.Entity.PartyDelete]", parameters);
        }
        public async IAsyncEnumerable<PartyModel> Select(int? PartyID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", PartyID)               
            };

            await foreach (IDataRecord reader in Iservice.GetDataReaderAsync("[usp.Entity.PartySelect]", parameters))
            {
                yield return GetModel(reader);
               
            }
        }

        private PartyModel GetModel(IDataRecord reader)
        {
            return new PartyModel(reader.GetString("AssesseeType"))
            {
                PartyID = reader.GetInt32("PartyID"),
                PartyName = reader.GetString("PartyName"),
                IsClient = reader.GetBoolean("IsClient"),
                IsSupplier = reader.GetBoolean("IsSupplier"),
                NatureOfFirm = reader.GetString("NatureOfFirm"),
                AssesseeTypeID = reader.GetInt32("AssesseeTypeID"), 
                PAN = reader.GetString("PAN"),
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
