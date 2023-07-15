using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class InventoryGrnService : IInventoryGrnService
    {
        IDbService Iservice;
        public InventoryGrnService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int GrnID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", GrnID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@Operation","Approve")
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.GoodsReceiptNote.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.GoodsReceiptNote.Delete]", parameters);
        }

        public IEnumerable<InventoryItemEntryModel> GetItemEntries(int GrnID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", GrnID),
               new SqlParameter("@Operation", "GetEntries")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.GoodsReceiptNote.Select]", parameters))
            {
                yield return new InventoryItemEntryModel()
                {
                    ItemEntryID = dr.GetInt64("ItemEntryID"),
                    ItemID = dr.GetInt32("ItemID"),
                    ItemDescription = dr.GetString("ItemDescription"),
                    ItemRate = dr.GetDecimal("ItemRate"),
                    ItemQty = dr.GetDecimal("ItemQty"),
                    ItemGstVal = dr.GetDecimal("ItemGstVal"),
                };
            }
        }

        public IEnumerable<InventoryGrnModel> SelectPending(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "All")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.GoodsReceiptNote.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public InventoryGrnModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            InventoryGrnModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.GoodsReceiptNote.Select]", parameters))
            {
                model = GetModel(dr);
            }
            model.Entries = GetItemEntries(model.POID.Value).ToList();
            return model;
        }

        public InventoryGrnModel Update(InventoryGrnModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", model.POID),
               new SqlParameter("@GrnID",model.GrnID),
               new SqlParameter("@GrnNo",model.GrnNo),
               new SqlParameter("@EntryDate",model.EntryDate),
               new SqlParameter("@PartyID",model.PartyID),
               new SqlParameter("@TotalValue",model.TotalValue),
               new SqlParameter("@Reference",model.Reference),
               new SqlParameter("@Remarks",model.Remarks),
               new SqlParameter("@StoreID",model.StoreID),
               new SqlParameter("entries",model.Entries.ToDataTable()),
               new SqlParameter("@UserID",model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.GoodsReceiptNote.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }



        private InventoryGrnModel GetModel(IDataRecord dr)
        {
            return new InventoryGrnModel(
                dr.GetString("GrnNo"),
                dr.GetBoolean("Invoiced"))
            {
                POID = dr.GetInt32("POID"),
                GrnID = dr.GetInt32("GrnID"),
                EntryDate = dr.GetDateTime("EntryDate"),
                PartyID = dr.GetInt32("PartyID"),
                AuthLevelID = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                PartyName = dr.GetString("TradeName"),
                TotalValue = dr.GetDecimal("TotalValue"),
                Reference = dr.GetString("Reference"),
                Remarks = dr.GetString("Remarks"),
                StoreID = dr.GetInt32("StoreID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<InventoryGrnModel> SelectByStore(int StoreID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StoreID", StoreID),
               new SqlParameter("@Operation", "ByStore")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.GoodsReceiptNote.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<InventoryGrnModel> PendingToInvoice(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "ForInvoice")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.GoodsReceiptNote.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public int Reverse(int GrnID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", GrnID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Operation","Reverse")
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.GoodsReceiptNote.Approve]", parameters);
        }
    }
}
