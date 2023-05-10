using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITdsThresholdLimitService
    {
        TdsThresholdLimitModel Update(TdsThresholdLimitModel model);
        TdsThresholdLimitModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<TdsThresholdLimitModel> Select();
        IEnumerable<TdsThresholdLimitModel> SelectByNP(int TdsNPID);
     
    }

    public class TdsThresholdLimitService : ITdsThresholdLimitService
    {
        IDbService Iservice;

        public TdsThresholdLimitService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsTLID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Taxes.TDS.ThresholdLimits.Delete]", parameters);
        }



        public IEnumerable<TdsThresholdLimitModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.ThresholdLimits.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
       

        public TdsThresholdLimitModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsTLID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            TdsThresholdLimitModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.ThresholdLimits.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<TdsThresholdLimitModel> SelectByNP(int TdsNPID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByNP"),
               new SqlParameter("@TdsNPID", TdsNPID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.ThresholdLimits.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public TdsThresholdLimitModel Update(TdsThresholdLimitModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsTLID", model.TdsTLID),
               new SqlParameter("@TdsNPID", model.NatureOfPayment.TdsNPID),
               new SqlParameter("@PeriodFrom", model.PeriodFrom),
               new SqlParameter("@PeriodTo", model.PeriodTo),
               new SqlParameter("@ThresholdLimitPeriod", model.ThresholdLimitPeriod),
               new SqlParameter("@ThresholdLimitSingle", model.ThresholdLimitSingle),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.ThresholdLimits.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private TdsThresholdLimitModel GetModel(IDataRecord dr)
        {
            return new TdsThresholdLimitModel
            {
                TdsTLID = dr.GetInt32("TdsTLID"),
                PeriodFrom = dr.GetDateTime("PeriodFrom"),
                PeriodTo = dr.GetDateTime("PeriodTo"),
                ThresholdLimitPeriod = dr.GetDecimal("ThresholdLimitPeriod"),
                ThresholdLimitSingle = dr.GetDecimal("ThresholdLimitSingle"),
                NatureOfPayment = new NatureOfPaymentModel { TdsNPID = dr.GetInt32("TdsNPID"), NatureOfPayment = dr.GetString("NatureOfPayment") },
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