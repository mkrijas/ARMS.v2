
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
        GstUsageIDModel Update(GstUsageIDModel model);
        GstUsageIDModel SelectByID(string ID);
        IEnumerable<GstUsageIDModel> SelectByAccount(int AccountID , DateTime? entryDate);
        IEnumerable<GstUsageIDModel> SelectByTaxRate(decimal TaxRate, DateTime? entryDate);
        IEnumerable<GstUsageIDModel> SelectBySAC(string SAC, DateTime? entryDate);
        IEnumerable<GstUsageIDModel> FilterByText(string FilterText, DateTime? entryDate);
        int Delete(string ID, string UserID);
        IEnumerable<GstUsageIDModel> Select(DateTime? entryDate);
        IEnumerable<GstRateModel> GetGstRates();
        IEnumerable<GstUsageIDModel> SelectByIDT(int? ID);
        IEnumerable<GstUsageIDModel> SelectByTaxRateAccount(int? rateId, int? acID);
        bool UsageIDExists(string UsageID);
    }

    public class GstUsageIDService : IGstUsageIDService
    {
        IDbService Iservice;

        public GstUsageIDService(IDbService iservice)
        {
            Iservice = iservice;
        }      

        public int Delete(string ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UsageID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Taxes.Gst.UsageID.Delete]", parameters);
        }

        public IEnumerable<GstUsageIDModel> FilterByText(string FilterText, DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "AutoComplete"),
               new SqlParameter("@UsageID",FilterText),
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

        public IEnumerable<GstUsageIDModel> Select(DateTime? entryDate)
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

        public IEnumerable<GstUsageIDModel> SelectByAccount(int AccountID, DateTime? entryDate)
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

        public GstUsageIDModel SelectByID(string ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UsageID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            GstUsageIDModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<GstUsageIDModel> SelectByIDT(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
             new SqlParameter("@UsageID", ID),
               new SqlParameter("@Operation", "ByID")
            }; 

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }


        public IEnumerable<GstUsageIDModel> SelectBySAC(string SAC, DateTime? entryDate)
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

        public IEnumerable<GstUsageIDModel> SelectByTaxRate(decimal TaxRate, DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "BySAC"),
               new SqlParameter("@Taxrate", TaxRate ),
               new SqlParameter("@EntryDate", entryDate),
            };
            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.UsageID.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public GstUsageIDModel Update(GstUsageIDModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UsageID", model.UsageID),
               new SqlParameter("@AccountID", model.AccountID),
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

        public IEnumerable<GstUsageIDModel> SelectByTaxRateAccount(int? rateId, int? acId)
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


        private GstUsageIDModel GetModel(IDataRecord dr)
        {
            return new GstUsageIDModel
            {
                UsageID = dr.GetString("UsageID"),
                PeriodFrom = dr.GetDateTime("PeriodFrom"),
                PeriodTo = dr.GetDateTime("PeriodTo"),
                AccountID = dr.GetInt32("AccountID"),
                RID = dr.GetInt32("RID"),
                SAC = dr.GetString("SAC"),
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