using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class TdsThresholdLimitService : ITdsThresholdLimitService
    {
        IDbService Iservice;

        public TdsThresholdLimitService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a TDS threshold limit by ID
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsTLID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Taxes.TDS.ThresholdLimits.Delete]", parameters);
        }

        // Method to select all TDS threshold limits
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

        // Method to select a TDS threshold limit by its ID
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

        // Method to select TDS threshold limits by Nature of Payment ID
        public IEnumerable<TdsThresholdLimitModel> SelectByNP(int TdsNPID,DateTime? EntryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByNP"),
               new SqlParameter("@TdsNPID", TdsNPID),
               new SqlParameter("@EntryDate", EntryDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.ThresholdLimits.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to update an existing TdsThresholdLimitModel record
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

        // Private method to convert an IDataRecord to a TdsThresholdLimitModel
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