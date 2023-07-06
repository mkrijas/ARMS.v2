using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class TdsRateService : ITdsRateService
    {
        IDbService Iservice;

        public TdsRateService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsRateID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.finance.taxes.TDS.Rates.Delete]", parameters);
        }

        public IEnumerable<TdsRateModel> SelectByIDT(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
              new SqlParameter("@tdsRateID", ID),
               new SqlParameter("@Operation", "ByID")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.taxes.TDS.Rates.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TdsRateModel> Select(int? AssesseeType, int? TdsNPID,DateTime? EntryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByNature"),
               new SqlParameter("AssesseeTypeID",AssesseeType),
               new SqlParameter("@TdsNPID",TdsNPID),
               new SqlParameter("@EntryDate",EntryDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.taxes.TDS.Rates.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<AssesseeTypeModel> SelectAssesseeTypes()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.AssesseeTypes.Select]", null))
            {
                yield return new AssesseeTypeModel {AssesseeTypeID = dr.GetInt32("AssesseeTypeID"),AssesseeTypeName = dr.GetString("AssesseeTypeName") };
            }
        }

        public TdsRateModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@tdsRateID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            TdsRateModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.taxes.TDS.Rates.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<NatureOfPaymentModel> SelectTdsNP()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.NatureOfPayment.Select]", null))
            {
                yield return new NatureOfPaymentModel { NatureOfPayment = dr.GetString("NatureOfPayment"),TdsNPID = dr.GetInt32("TdsNPID") };
            }
        }

        public IEnumerable<NatureOfPaymentModel> SelectTdsNPByID(int? TdsNPID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsNPID", TdsNPID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.TDS.NatureOfPayment.Select]", null))
            {
                yield return new NatureOfPaymentModel { NatureOfPayment = dr.GetString("NatureOfPayment"),TdsNPID = dr.GetInt32("TdsNPID") };
            }
        }

        public TdsRateModel Update(TdsRateModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsRateID", model.TdsRateID),
               new SqlParameter("@TdsNPID", model.TdsNP.TdsNPID),
               new SqlParameter("@AssesseeTypeID", model.AssesseeType.AssesseeTypeID),
               new SqlParameter("@PeriodFrom", model.PeriodFrom),
               new SqlParameter("@PeriodTo", model.PeriodTo),
               new SqlParameter("@TaxRate", model.TaxRate),               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.taxes.TDS.Rates.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private TdsRateModel GetModel(IDataRecord dr)
        {
            return new TdsRateModel
            {
                AssesseeType = new AssesseeTypeModel { AssesseeTypeID = dr.GetInt32("AssesseeTypeID"), AssesseeTypeName = dr.GetString("AssesseeTypeName") },                
                TdsNP = new NatureOfPaymentModel { TdsNPID = dr.GetInt32("TdsNPID"), NatureOfPayment = dr.GetString("NatureOfPayment") },
                PeriodFrom = dr.GetDateTime("PeriodFrom"),
                PeriodTo = dr.GetDateTime("PeriodTo"),
                TaxRate = dr.GetDecimal("TaxRate"),
                TdsRateID = dr.GetInt32("TdsRateID"),  
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public decimal GetTdsRate(int? PartyID, int? AccountID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "GetTaxRate"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@AccountID", AccountID),  
            };

            decimal? result = 0;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.taxes.TDS.Rates.Select]", parameters))
            {
                result = dr.GetDecimal("TaxRate");
            }
            return result??0;
        }
    }
}