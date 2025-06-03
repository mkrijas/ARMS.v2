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

        // Method to approve a GRN
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

        // Method to delete a GRN
        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.GoodsReceiptNote.Delete]", parameters);
        }

        // Method to cancel a GRN to invoice
        public int ToInvoiceCancel(int? ID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", ID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.GoodsReceiptNote.ToInvoice.Cancel]", parameters);
        }

        // Method to get item entries for a specific GRN
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
                    ItemCode = dr.GetString("InventoryItemCode"),
                    ItemGroupDescription = dr.GetString("ItemGroupDescription"),
                    ItemDescription = dr.GetString("ItemDescription"),
                    PartNumber = dr.GetString("PartNumber"),
                    ItemRate = dr.GetDecimal("ItemRate"),
                    ItemQty = (decimal)dr.GetDecimal("ItemQty"),
                    UOM = dr.GetString("UOM"),
                    CoaID = dr.GetInt32("CoaID"),
                    ItemGstVal = dr.GetDecimal("ItemGstVal"),
                };
            }
        }

        // Method to select pending GRNs for a specific branch
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

        // Method to select a GRN by its ID
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

        // Method to check the quantity of a purchase order (PO)
        public string CheckPOQty(int POID)
        {
            string result = "";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", POID),
            };
            InventoryGrnModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.GoodsReceiptNote.Select]", parameters))
            {
                model = GetModel(dr);
            }
            model.Entries = GetItemEntries(model.POID.Value).ToList();
            return result;
        }

        // Method to update a GRN
        public InventoryGrnModel Update(InventoryGrnModel model)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@POID", model.POID),
               new SqlParameter("@GrnID",model.GrnID),
               new SqlParameter("@GrnNo",model.GrnNo),
               new SqlParameter("@EntryDate",model.EntryDate),
               new SqlParameter("@PartyID",model.PartyID),
               new SqlParameter("@TruckID",model.TruckID),
               new SqlParameter("@UsedInventory",model.UsedInventory),
               new SqlParameter("@TotalValue",model.TotalValue),
               new SqlParameter("@Reference",model.Reference),
               new SqlParameter("@Remarks",model.Remarks),
               new SqlParameter("@StoreID",model.StoreID),
               new SqlParameter("entries",model.Entries.ToDataTable()),
               new SqlParameter("@UserID",model.UserInfo.UserID),
            };
            //foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.GoodsReceiptNote.Update]", parameters))
            foreach(IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.GoodsReceiptNoteCheckQty.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to InventoryGrnModel
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
                TruckID = dr.GetInt32("TruckID"),
                UsedInventory = dr.GetByte("UsedInventory"),
                AuthLevelID = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                PartyName = dr.GetString("TradeName"),
                TotalValue = dr.GetDecimal("TotalValue"),
                Reference = dr.GetString("Reference"),
                Remarks = dr.GetString("Remarks"),
                StoreID = dr.GetInt32("StoreID"),
                Store = new StoreModel
                {
                   StoreName = dr.GetString("StoreName"),
                },
                IssuedQty = dr.GetDecimal("IssuedQty"),
                NoOfGR = dr.GetInt32("NoOfGR"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Method to select GRNs by store ID
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

        // Method to get pending GRNs for invoicing
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

        // Method to reverse a GRN
        public int Reverse(DateTime DocDate, int GrnID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocDate", DocDate),
               new SqlParameter("@GrnID", GrnID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.GoodsReceiptNote.Reverse]", parameters);
        }
    }
}
