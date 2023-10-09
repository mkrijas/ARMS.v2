
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class TdsAccountMappingService : ITdsAccountMappingService
    {    
    IDbService Iservice;

        public TdsAccountMappingService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsAccountMappedID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Taxes.TDS.AccountMapping.Delete]", parameters);
        }

      

        public IEnumerable<TdsAccountMappingModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.AccountMapping.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TdsAccountMappingModel> SelectByAccount(int CoaID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByAccount"),
               new SqlParameter("@CoaID", CoaID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.AccountMapping.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public TdsAccountMappingModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsAccountMappedID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            TdsAccountMappingModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.AccountMapping.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<TdsAccountMappingModel> SelectByNP(int TdsNPID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByNP"),
               new SqlParameter("@TdsNPID", TdsNPID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.AccountMapping.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public TdsAccountMappingModel CheckExist(int? CoaID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CoaID", CoaID),
               new SqlParameter("@Operation", "CheckExist")
            };
            TdsAccountMappingModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.AccountMapping.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public TdsAccountMappingModel Update(TdsAccountMappingModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsAccountMappedID", model.TdsAccountMappedID),
               new SqlParameter("@TdsNPID", model.TdsNPID),
               new SqlParameter("@CoaID", model.CoaID),               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.AccountMapping.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private TdsAccountMappingModel GetModel(IDataRecord dr)
        {
            return new TdsAccountMappingModel
            {
                CoaID = dr.GetInt32("CoaID"),
                TdsNPID = dr.GetInt32("TdsNPID"),
                TdsAccountMappedID = dr.GetInt32("TdsAccountMappedID"),
                AccountName = dr.GetString("AccountName"),
                NatureOfPayment = dr.GetString("NatureOfPayment"),
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