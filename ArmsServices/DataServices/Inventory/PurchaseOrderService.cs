using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{

    public interface IPurchaseOrderService
    {
        PurchaseOrderModel Update(PurchaseOrderModel model);
        PurchaseOrderModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);   
        IEnumerable<PurchaseOrderModel> SelectPending(int BranchID);
        IEnumerable<PurchaseOrderModel> PendingForGrn(int BranchID);
        IEnumerable<PurchaseOrderModel> SelectByStore(int StoreID);
        int Approve(int POID,string UserID);
        IEnumerable<InventoryItemEntryModel> GetItemEntries(int POID);
    }
    public class PurchaseOrderService : IPurchaseOrderService
    {
        IDbService Iservice;
        public PurchaseOrderService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int POID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@POID", POID),
               new SqlParameter("@UserID", UserID),
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
                    ItemQty = dr.GetDecimal("ItemQty"),
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
               new SqlParameter("@PartyBranchID",model.PartyBranchID),
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
                dr.GetString("PoNo"), 
                dr.GetBoolean("Approved"),
                new ArmsModels.SharedModels.UserInfoModel
                {
                    TimeStampField = dr.GetDateTime("ApprovedOn"),
                    UserID = dr.GetString("ApprovedBy"),
                })
            {
                POID = dr.GetInt32("POID"),               
                PRID = dr.GetInt32("PRID"),
                QuoteID = dr.GetInt32("QuoteID"),
                 
                EntryDate = dr.GetDateTime("EntryDate"),
                PartyBranchID = dr.GetInt32("PartyBranchID"),
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
