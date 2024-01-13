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
using System.Security.Cryptography;

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
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Transfer.Initiation.SelectAll]", parameters))
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
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Transfer.Initiation.SelectAll]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<InventoryItemEntryModel> SelectItemsList(int? StockTransferID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StockTransferID", StockTransferID),
               new SqlParameter("@Operation", "GetItems")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Transfer.Initiation.SelectAll]", parameters))
            {
                yield return new InventoryItemEntryModel()
                {
                    ItemEntryID = dr.GetInt32("ItemTransferID"),
                    ItemID = dr.GetInt32("InventoryItemID"),
                    ItemDescription = dr.GetString("ItemDescription"),
                    ItemQty = dr.GetDecimal("Quantity"),
                    ItemRate = dr.GetDecimal("Amount"),
                    ItemGstVal = dr.GetDecimal("GstVal"),
                };
            }
        }

        public StockTransferInitiationModel SelectSandB(int? StockTransferID)
        {
            StockTransferInitiationModel model = null;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StockTransferID", StockTransferID),
               new SqlParameter("@Operation", "SelectSandB")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Transfer.Initiation.SelectAll]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public StockTransferInitiationModel Initiate(StockTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID",model.BranchID),
               new SqlParameter("@StoreID",model.Store.StoreID),
               new SqlParameter("@DestinationBranchID",model.OtherBranchID),
               new SqlParameter("@DocumentDate",model.DocumentDate),
               new SqlParameter("@IsLocal",model.IsLocal),
               new SqlParameter("@IsTaxable",model.IsTaxable),
               new SqlParameter("@RecordStatus",model.Status),
               new SqlParameter("@Narration",model.Narration),
               new SqlParameter("@CostCenter",model.CostCenter),
               new SqlParameter("@Dimension",model.Dimension),
               new SqlParameter("@NatureOfTransaction",model.NatureOfTransaction),
               new SqlParameter("@Items",model.ItemsList.ToDataTable()),

               new SqlParameter("@UserID",model.UserInfo.UserID),
               new SqlParameter("@Operation", "Initiation"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Transfer.Initiate]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public StockTransferEndModel UpdateDelivery(StockTransferEndModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StockTransferEndID",model.StockTransferEndID),
               new SqlParameter("@StockTransferID",model.StockTransferID),               
               new SqlParameter("@StoreID",model.Store.StoreID),
               new SqlParameter("@DocumentDate",model.DocumentDate),
               new SqlParameter("@BranchID",model.BranchID),
               new SqlParameter("@CostCenter",model.CostCenter),
               new SqlParameter("@Dimension",model.Dimension),
               new SqlParameter("@Narration",model.Narration),               
               new SqlParameter("@NatureOfTransaction",model.NatureOfTransaction),
               new SqlParameter("@UserID",model.UserInfo.UserID),
               new SqlParameter("@Operation", "Accept"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Transfer.end]", parameters))
            {
                model = GetEndModel(dr);
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
            return Iservice.ExecuteNonQuery("[usp.Inventory.Store.Transfer.Initiation.Cancel]", parameters);
        }

        public StockTransferInitiationModel RejectOrder(StockTransferInitiationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StockTransferID",model.StockTransferID),
               new SqlParameter("@InvTranID",model.InvTranID),
               new SqlParameter("@StoreID",model.Store.StoreID),
               new SqlParameter("@UserID",model.UserInfo.UserID),
               new SqlParameter("@Operation", "Reject"),
               new SqlParameter("@DestinationBranchID", model.OtherBranchID),
               new SqlParameter("@MID", model.MID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@IsLocal", model.IsLocal),
               new SqlParameter("@IsTaxable", model.IsTaxable),
               new SqlParameter("@Items",model.ItemsList.ToDataTable()),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@RecordStatus", 0)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Store.Transfer.Reject]", parameters))
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
                    StoreID = dr.GetInt32("StoreID"),
                    StoreName = dr.GetString("StoreName"),
                },
                OtherBranchID = dr.GetInt32("OtherBranchID"),
                OtherBranchName = dr.GetString("OtherBranchName"),
                DocumentDate = dr.GetDateTime("InitiatedDate"),
                Status = dr.GetByte("TransferStatus"),
                IsLocal = dr.GetBoolean("IsLocal"),
                IsTaxable = dr.GetBoolean("IsTaxable"),
                MID = dr.GetInt32("MID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                Narration= dr.GetString("Narration"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension= dr.GetInt32("Dimension"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                BranchID= dr.GetInt32("BranchID"),
                FileName = dr.GetString("FileName"),
                UserInfo = new UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },                
            };
        }


        private StockTransferEndModel GetEndModel(IDataRecord dr)
        {
            return new StockTransferEndModel
            {
                StockTransferEndID = dr.GetInt32("StockTransferEndID"),
                MID = dr.GetInt32("MID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                Narration = dr.GetString("Narration"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension = dr.GetInt32("Dimension"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                BranchID = dr.GetInt32("BranchID"),
                OtherBranchID = dr.GetInt32("OtherBranchID"),
                OtherBranchName = dr.GetString("OtherBranchName"),                
                FileName = dr.GetString("FileName"),
                Store = new StoreModel
                {
                    StoreID = dr.GetInt32("StoreID"),
                    StoreName = dr.GetString("StoreName"),
                },               
                TransferStatus = dr.GetByte("TransferStatus"),
                DocumentDate = dr.GetDateTime("TransferEndDate"),
                StockTransferID = dr.GetInt32("StockTransferID"),
                
                UserInfo = new UserInfoModel()
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                }
            };
          }


        public int Approve(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Store.Transfer.Approve]", parameters);
        }
    }
}