using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;


namespace ArmsServices.DataServices
{
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

        public IEnumerable<GstItemModel> SelectByDate(DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByDate"),
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

        public GstItemModel SelectByItem(int ItemID, DateTime? entryDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByItem"),
               new SqlParameter("@ItemID", ItemID),
               new SqlParameter("@EntryDate", entryDate),
            };
            GstItemModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.HSN.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
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
               new SqlParameter("@GstMechanism", model.GstMechanism),
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
                GstMechanism = dr.GetString("GstMechanism"),
                ItemDescription = dr.GetString("ItemDescription"),
                HsnCode = dr.GetString("HsnCode"),
                ItemGroupDescription = dr.GetString("ItemGroupDescription"),
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




        public IEnumerable<GstInOutModel> GetInOut()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Taxes.Gst.Master.Select]", null))
            {
                yield return new GstInOutModel
                {
                    GstTypeID = dr.GetInt32("GstTypeID"),
                    GstType = dr.GetString("GstType"),
                    InputAccount = dr.GetString("InputAccount"),
                    OutputAccount = dr.GetString("OutputAccount"),
                };
            }
        }
    }
}