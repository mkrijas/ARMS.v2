using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IGstItemService
    {
        GstItemModel Update(GstItemModel model);
        GstItemModel SelectByID(int ID);
        IEnumerable<GstItemModel> SelectByItem(int ItemID, DateTime? entryDate);
        IEnumerable<GstItemModel> SelectByTaxRate(decimal TaxRate, DateTime? entryDate);        
        IEnumerable<GstItemModel> FilterByText(string FilterText, DateTime? entryDate);
        int Delete(int ID, string UserID);
        IEnumerable<GstItemModel> SelectByItem(int ItemID);
        IEnumerable<GstItemModel> Select(DateTime? entryDate);
        
    }

    public class GstItemService : IGstItemService
    {
        IDbService Iservice;

        public GstItemService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@HsnID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Taxes.Gst.HSN.Delete]", parameters);
        }

        public IEnumerable<GstItemModel> FilterByText(string FilterText, DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "AutoComplete"),
               new SqlParameter("@ItemCode",FilterText),
               new SqlParameter("@EntryDate", entryDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.HSN.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }        

        public IEnumerable<GstItemModel> Select(DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@EntryDate", entryDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.HSN.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<GstItemModel> SelectByItem(int ItemID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ItemID", ItemID),
               new SqlParameter("@Operation", "ByItem")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.HSN.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<GstItemModel> SelectByItem(int ItemID, DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByItem"),
               new SqlParameter("@EntryDate", entryDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.HSN.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public GstItemModel SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@HsnID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            GstItemModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.HSN.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
               

        public IEnumerable<GstItemModel> SelectByTaxRate(decimal TaxRate, DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByRate"),
               new SqlParameter("@Taxrate", TaxRate ),
               new SqlParameter("@EntryDate", entryDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.HSN.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public GstItemModel Update(GstItemModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@HsnID", model.HsnID),
               new SqlParameter("@ItemID", model.ItemID),
               new SqlParameter("@PeriodFrom", model.PeriodFrom),
               new SqlParameter("@PeriodTo", model.PeriodTo),
               new SqlParameter("@RID", model.RID),              
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.HSN.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
      


        private GstItemModel GetModel(IDataRecord dr)
        {
            return new GstItemModel(dr.GetString("ItemCode"), dr.GetDecimal("Taxrate"))
            {
                HsnID = dr.GetInt32("HsnID"),
                ItemID = dr.GetInt32("ItemID"),
                PeriodFrom = dr.GetDateTime("PeriodFrom"),
                PeriodTo = dr.GetDateTime("PeriodTo"),                         
                RID = dr.GetInt32("RID"),                        
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