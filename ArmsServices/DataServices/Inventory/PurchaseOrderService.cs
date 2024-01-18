using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        IDbService Iservice;
        public PurchaseOrderService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int POID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", POID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@Operation", "Approve"),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.PurchaseOrder.Approve]", parameters);
        }

        public int Reverse(int POID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", POID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Operation","Reverse")
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.PurchaseOrder.Approve]", parameters);
        }

        public int CancelOrder(int POID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", POID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@Operation","CancelOrder")
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.PurchaseOrder.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.PurchaseOrder.Delete]", parameters);
        }

        public IEnumerable<InventoryItemEntryModel> GetItemEntries(int POID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", POID),
               new SqlParameter("@Operation", "GetEntries")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.PurchaseOrder.Select]", parameters))
            {
                yield return new InventoryItemEntryModel()
                {
                    ItemEntryID = dr.GetInt64("POItemID"),
                    ItemID = dr.GetInt32("ItemID"),
                    ItemRate = dr.GetDecimal("ItemRate"),
                    ItemQty = (decimal)dr.GetDecimal("ItemQty"),
                    UOM = dr.GetString("UOM"),
                    ItemGstVal = dr.GetDecimal("ItemGstVal"),
                };
            }
        }

        public IEnumerable<PurchaseOrderModel> SelectPending(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "All")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.PurchaseOrder.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public PurchaseOrderModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            PurchaseOrderModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.PurchaseOrder.Select]", parameters))
            {
                model = GetModel(dr);
            }
            model.Entries = GetItemEntries(model.POID.Value).ToList();
            return model;
        }

        public PurchaseOrderModel Update(PurchaseOrderModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", model.POID),
               new SqlParameter("@PONo",model.PONo),
               new SqlParameter("@QuoteID",model.QuoteID),
               new SqlParameter("@EntryDate",model.EntryDate),
               new SqlParameter("@PartyID",model.PartyID),
               new SqlParameter("@PartyCode",model.PartyCode),
               new SqlParameter("@TotalValue",model.TotalValue),
               new SqlParameter("@Reference",model.Reference),
               new SqlParameter("@Remarks",model.Remarks),
               new SqlParameter("@StoreID",model.StoreID),
               new SqlParameter("entries",model.Entries.ToDataTable()),
               new SqlParameter("@UserID",model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.PurchaseOrder.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private PurchaseOrderModel GetModel(IDataRecord dr)
        {
            return new PurchaseOrderModel(
                dr.GetBoolean("GrnCreated"),
                dr.GetString("PoNo"))
            {
                POID = dr.GetInt32("POID"),
                PRID = dr.GetInt32("PRID"),
                QuoteID = dr.GetInt32("QuoteID"),
                EntryDate = dr.GetDateTime("EntryDate"),
                PartyID = dr.GetInt32("PartyID"),
                PartyName = dr.GetString("TradeName"),
                AuthLevelID = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
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

        public IEnumerable<PurchaseOrderModel> SelectByStore(int StoreID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StoreID", StoreID),
               new SqlParameter("@Operation", "ByStore")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.PurchaseOrder.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<PurchaseOrderModel> PendingForGrn(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "ForGrn")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.PurchaseOrder.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
    }
}
