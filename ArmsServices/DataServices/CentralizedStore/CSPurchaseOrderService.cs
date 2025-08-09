using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class CSPurchaseOrderService : ICSPurchaseOrderService
    {
        IDbService Iservice;
        public CSPurchaseOrderService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to approve a purchase order
        public int Approve(int POID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", POID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@Operation", "Approve"),
            };
            return Iservice.ExecuteNonQuery("[usp.Centralized.Store.PurchaseOrder.Approve]", parameters);
        }

        // Method to reverse a purchase order    
        public int Reverse(int POID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", POID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Operation","Reverse")
            };
            return Iservice.ExecuteNonQuery("[usp.Centralized.Store.PurchaseOrder.Approve]", parameters);
        }

        // Method to cancel a purchase order
        public int CancelOrder(int POID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", POID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@Operation","CancelOrder")
            };
            return Iservice.ExecuteNonQuery("[usp.Centralized.Store.PurchaseOrder.Approve]", parameters);
        }

        // Method to delete a purchase order by its ID
        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Centralized.Store.PurchaseOrder.Delete]", parameters);
        }

        // Method to get item entries for a specific purchase order
        public IEnumerable<CSPOItemEntryModel> GetItemEntries(int POID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", POID),
               new SqlParameter("@Operation", "GetEntries")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.PurchaseOrder.Select]", parameters))
            {
                yield return new CSPOItemEntryModel()
                {
                    POItemID = dr.GetInt64("POItemID"),
                    ItemID = dr.GetInt32("ItemID"),
                    ItemRate = dr.GetDecimal("ItemRate"),
                    ItemQty = (decimal)dr.GetDecimal("ItemQty"),
                    UoM = dr.GetString("UOM"),
                    ItemGstVal = dr.GetDecimal("ItemGstVal"),
                    ItemDescription = dr.GetString("ItemDescription"),  
                    ItemCode = dr.GetString("InventoryItemCode"),
                    ItemGroupDescription = dr.GetString("ItemGroupDescription"),
                    PartNumber = dr.GetString("PartNumber"),
                    CoaID = dr.GetInt32("CoaID"),
                    BaseQty = dr.GetDecimal("BaseQty"),
                    BaseRate = dr.GetDecimal("BaseRate"),
                };
            }
        }

        // Method to get item entries for a specific purchase order (alternative method)
        public IEnumerable<CSPOItemEntryModel> GetItemEntriesPO(int POID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", POID),
               new SqlParameter("@Operation", "GetEntriesPO")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.PurchaseOrder.Select]", parameters))
            {
                yield return new CSPOItemEntryModel()
                {
                    POItemID = dr.GetInt64("POItemID"),
                    ItemID = dr.GetInt32("ItemID"),
                    ItemRate = dr.GetDecimal("ItemRate"),
                    ItemQty = (decimal)dr.GetDecimal("ItemQty"),
                    UoM = dr.GetString("UOM"),
                    ItemGstVal = dr.GetDecimal("ItemGstVal"),
                    ItemCode = dr.GetString("InventoryItemCode"),
                    ItemDescription = dr.GetString("ItemDescription"),
                    ItemGroupDescription = dr.GetString("ItemGroupDescription")
                };
            }
        }

        // Method to select pending purchase orders for a specific branch
        public IEnumerable<CSPurchaseOrderModel> SelectPending(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "All")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.PurchaseOrder.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a purchase order by its ID
        public CSPurchaseOrderModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            CSPurchaseOrderModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.PurchaseOrder.Select]", parameters))
            {
                model = GetModel(dr);
            }
            model.Entries = GetItemEntries(model.POID.Value).ToList();
            return model;
        }

        // Method to update a purchase order
        public CSPurchaseOrderModel Update(CSPurchaseOrderModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", model.POID),
               new SqlParameter("@PONo",model.PONo),
               new SqlParameter("@EntryDate",model.EntryDate),
               new SqlParameter("@PartyID",model.PartyID),
               new SqlParameter("@TotalValue",model.TotalValue),
               new SqlParameter("@Reference",model.Reference),
               new SqlParameter("@Remarks",model.Remarks),
               new SqlParameter("@BranchID",model.BranchID),
               new SqlParameter("@Items",model.Entries.ToDataTable()),
               new SqlParameter("@UserID",model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.PurchaseOrder.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to PurchaseOrderModel
        private CSPurchaseOrderModel GetModel(IDataRecord dr)
        {
            return new CSPurchaseOrderModel(
                dr.GetBoolean("GrnCreated"),
                dr.GetString("PoNo"))
            {
                POID = dr.GetInt32("POID"),
                EntryDate = dr.GetDateTime("EntryDate"),
                PartyID = dr.GetInt32("PartyID"),
                PartyName = dr.GetString("TradeName"),
                AuthLevelID = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                TotalValue = dr.GetDecimal("TotalValue"),
                Reference = dr.GetString("Reference"),
                Remarks = dr.GetString("Remarks"),               
                BranchID = dr.GetInt32("BranchID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Method to select pending purchase orders for GRN (Goods Receipt Note)
        public IEnumerable<CSPurchaseOrderModel> PendingForGrn(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "ForGrn")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.PurchaseOrder.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
    }
}
