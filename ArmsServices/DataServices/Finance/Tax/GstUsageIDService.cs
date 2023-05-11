
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IGstUsageIDService
    {
        GstUsageCodeModel Update(GstUsageCodeModel model);
        GstUsageCodeModel SelectByCode(string Code);
        IEnumerable<GstUsageCodeModel> SelectByAccount(int AccountID , DateTime? entryDate);
        IEnumerable<GstUsageCodeModel> SelectByArea(string Area, DateTime? entryDate);
        IEnumerable<GstUsageCodeModel> SelectByTaxRate(decimal TaxRate, DateTime? entryDate);
        IEnumerable<GstUsageCodeModel> SelectBySAC(string SAC, DateTime? entryDate);
        IEnumerable<GstUsageCodeModel> FilterByText(string FilterText, DateTime? entryDate);
        int Delete(int? ID, string UserID);
        IEnumerable<GstUsageCodeModel> Select(DateTime? entryDate);
        IEnumerable<GstRateModel> GetGstRates();      
        IEnumerable<GstUsageCodeModel> SelectByTaxRateAccount(int? rateId, int? acID);
        decimal? GetGstRate(string UsageCode,DateTime? EntryDate);
    }

    public class GstUsageIDService : IGstUsageIDService
    {
        IDbService Iservice;

        public GstUsageIDService(IDbService iservice)
        {
            Iservice = iservice;
        }      

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Taxes.Gst.UsageID.Delete]", parameters);
        }

        public IEnumerable<GstUsageCodeModel> FilterByText(string FilterText, DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "AutoComplete"),
               new SqlParameter("@UsageCode",FilterText),
               new SqlParameter("@EntryDate", entryDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<GstRateModel> GetGstRates()
        {           

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.Rates.Select]", null))
            {
                yield return new GstRateModel()
                {
                    RID = dr.GetInt32("RID"),
                    TaxRate = dr.GetDecimal("TaxRate"),
                    Description = dr.GetString("Description")
                };
            }
        }

        public IEnumerable<GstUsageCodeModel> Select(DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@EntryDate", entryDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<GstUsageCodeModel> SelectByAccount(int AccountID, DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByAccount"),
               new SqlParameter("@EntryDate", entryDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<GstUsageCodeModel> SelectByArea(string Area, DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByArea"),
               new SqlParameter("@Area", Area),
               new SqlParameter("@EntryDate", entryDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public GstUsageCodeModel SelectByCode(string Code)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UsageCode", Code),
               new SqlParameter("@Operation", "ByCode")
            };
            GstUsageCodeModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }      

        public IEnumerable<GstUsageCodeModel> SelectBySAC(string SAC, DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "BySAC"),
               new SqlParameter("@SAC", SAC ),
               new SqlParameter("@EntryDate", entryDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<GstUsageCodeModel> SelectByTaxRate(decimal TaxRate, DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByRate"),
               new SqlParameter("@Taxrate", TaxRate ),
               new SqlParameter("@EntryDate", entryDate),
            };
            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public GstUsageCodeModel Update(GstUsageCodeModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.Id),
               new SqlParameter("@UsageCode", model.UsageCode),
               new SqlParameter("@Description", model.Description),
               new SqlParameter("@Area", model.Area),
               new SqlParameter("@AccountID", model.CoaID),
               new SqlParameter("@PeriodFrom", model.PeriodFrom),
               new SqlParameter("@PeriodTo", model.PeriodTo),
               new SqlParameter("@RID", model.RID),
               new SqlParameter("@SAC", model.SAC),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<GstUsageCodeModel> SelectByTaxRateAccount(int? rateId, int? acId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Compare"),
               new SqlParameter("@RID",rateId),
               new SqlParameter("@AccountID",acId),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }


        private GstUsageCodeModel GetModel(IDataRecord dr)
        {
            return new GstUsageCodeModel
            {
                Id = dr.GetInt32("ID"),
                UsageCode = dr.GetString("UsageCode"),
                Area = dr.GetString("Area"),
                Description = dr.GetString("Description"),
                PeriodFrom = dr.GetDateTime("PeriodFrom"),
                PeriodTo = dr.GetDateTime("PeriodTo"),
                CoaID = dr.GetInt32("AccountID"),
                CoaDescreption = dr.GetString("AccountName"),
                RID = dr.GetInt32("RID"),
                TaxRate = dr.GetDecimal("Taxrate"),
                SAC = dr.GetString("SAC"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public decimal? GetGstRate(string UsageCode, DateTime? EntryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetTaxRate"),
               new SqlParameter("@EntryDate", EntryDate),
               new SqlParameter("@UsageCode", UsageCode),
            };

            decimal? result = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                result = dr.GetDecimal("TaxRate");
            }
            return result;
        }
    }
}