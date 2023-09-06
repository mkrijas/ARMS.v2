using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.BaseModels.Inventory;
using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;

namespace ArmsServices.DataServices
{
    public class StockTransferService : IStockTransferService
    {
        IDbService Iservice;
        public StockTransferService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public IEnumerable<StockTransferInitiationModel> SelectOutGoing(int? BranchId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchId),
               new SqlParameter("@Operation", "Outgoing")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Inventory.Store.Transfer.Initiation.SelectAll]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<StockTransferInitiationModel> SelectIncoming(int? BranchId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchId),
               new SqlParameter("@Operation", "Incoming")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Inventory.Store.Transfer.Initiation.SelectAll]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<InventoryItemEntryModel> SelectItemsList(int? InvTranID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InvTranID", InvTranID),
               new SqlParameter("@Operation", "SelectByInvTranID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Inventory.Store.Transfer.Initiation.SelectAll]", parameters))
            {
                yield return new InventoryItemEntryModel()
                {
                    ItemEntryID = dr.GetInt32("ItemTransferID"),
                    ItemID = dr.GetInt32("InventoryItemID"),
                    ItemDescription = dr.GetString("InventoryItemCode"),
                    ItemQty = dr.GetDecimal("Quantity"),
                    ItemRate = dr.GetDecimal("Amount"),
                    ItemGstVal = dr.GetDecimal("EndQuantity"),
                };
            }
        }

        public StockTransferInitiationModel SelectSandB(int? InvTranID)
        {
            StockTransferInitiationModel model = null;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InvTranID", InvTranID),
               new SqlParameter("@Operation", "SelectSandB")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Inventory.Store.Transfer.Initiation.SelectAll]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public StockTransferInitiationModel Update(StockTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@InitiationStoreID",model.Store.StoreID),
               new SqlParameter("@DestinationBranchID",model.Branch.BranchID),
               new SqlParameter("@RecordStatus",model.Status),
               new SqlParameter("@Items",model.ItemsList.ToDataTable()),
               new SqlParameter("@UserID",model.UserInfo.UserID),
               new SqlParameter("@Operation", "Initiation"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Inventory.Store.Transfer.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public StockTransferInitiationModel UpdateDelivery(StockTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StockTransferID",model.StockTransferID),
               new SqlParameter("@InvTranID",model.InvTranID),
               new SqlParameter("@StoreID",model.Store.StoreID),
               new SqlParameter("@RecordStatus",model.Status),
               new SqlParameter("@Items",model.ItemsList.ToDataTable()),
               new SqlParameter("@UserID",model.UserInfo.UserID),
               new SqlParameter("@Operation", "Accept"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Inventory.Store.Transfer.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public int TransferCancel(int? InvTranID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@InvTranID",InvTranID),
               new SqlParameter("@Operation", "Cancel")
            };
            return Iservice.ExecuteNonQuery("[Inventory.Store.Transfer.Initiation.Cancel]", parameters);
        }

        public StockTransferInitiationModel RejectOrder(StockTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StockTransferID",model.StockTransferID),
               new SqlParameter("@InvTranID",model.InvTranID),
               new SqlParameter("@StoreID",model.Store.StoreID),
               new SqlParameter("@UserID",model.UserInfo.UserID),
               new SqlParameter("@Operation", "Reject")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[Inventory.Store.Transfer.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private StockTransferInitiationModel GetModel(IDataRecord dr)
        {
            return new StockTransferInitiationModel
            {
                StockTransferID = dr.GetInt32("StockTransferID"),
                InvTranID = dr.GetInt32("InvTranID"),
                Store = new StoreModel
                {
                    StoreID = dr.GetInt32("InitiationStoreID"),
                    StoreName = dr.GetString("InitiationStoreName"),
                },
                Branch = new BranchModel
                {
                    BranchID = dr.GetInt32("BranchID"),
                    BranchName = dr.GetString("BranchName"),
                },
                InitiatedDate = dr.GetDateTime("InitiatedDate"),
                Status = dr.GetByte("RecordStatus"),
                UserInfo = new UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
                EndModel = new StockTransferEndModel
                {
                    StockTransferEndID = dr.GetInt32("StockTransferEndID"),

                    //DestinationStore = new StoreModel
                    //{
                    //    StoreID = dr.GetInt32("DestinationStoreID"),
                    //    StoreName = dr.GetString("DestinationStoreName"),
                    //},
                    //DestinationBranch = new BranchModel
                    //{
                    //    BranchID = dr.GetInt32("DestinationBranchID"),
                    //    BranchName = dr.GetString("DestinationBranchName"),
                    //},
                    TransferStatus = dr.GetByte("TransferStatus"),
                    TransferEndDate = dr.GetDateTime("TransferEndDate"),
                    RecordStatus = dr.GetByte("RecordStatus"),
                }
            };
        }
    }
}