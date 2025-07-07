using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class TripFuelService : ITripFuelService
    {
        IDbService Iservice;

        public TripFuelService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a trip fuel record by its ID
        public int Delete(long? TripFuelID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripFuelID", TripFuelID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Trips.Fuel.Delete]", parameters);
        }

        // Method to select a trip fuel record by its ID
        public TripFuelModel Select(int? TripFuelID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripFuelID),
               new SqlParameter("@Operation", "SelectByID"),
            };

            TripFuelModel model = null;
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Fuel.Select]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }

        // Method to select trip fuel records by trip ID
        public IEnumerable<TripFuelModel> SelectByTrip(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripID", TripID),
                new SqlParameter("@Operation", "SelectByTrip"),
            };

            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Fuel.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        // Method to select trip fuel records by asset transfer ID
        public IEnumerable<TripFuelModel> SelectByAssetTransfer(int? AssetTransferID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetTransferID", AssetTransferID),
                new SqlParameter("@Operation", "SelectByAssetTransfer"),
            };

            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Fuel.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        // Method to select a trip fuel record by tax purchase ID
        public TripFuelModel SelectByTaxPurchase(int? TaxPurchaseID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TaxPurchaseID", TaxPurchaseID),
                new SqlParameter("@Operation", "SelectByTaxPurchase"),
            };

            TripFuelModel model = null;
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Fuel.Select]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }

        // Method to update a trip fuel record
        public TripFuelModel Update(TripFuelModel model)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TripFuelID", model.TripFuelID),
               new SqlParameter("@ItemType", model.ItemType),
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@AssetTransferID", model.AssetTransferID),
               new SqlParameter("@RequestApprovalHistoryID", model.RequestApprovalHistoryID),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@BranchID", model.PurchaseModel.BranchID),
               new SqlParameter("@EntryDate", model.EntryDate),
               new SqlParameter("@FuelItemID", model.FuelItemID),
               new SqlParameter("@Quantity", model.Quantity),
               new SqlParameter("@UserID", model.PurchaseModel.UserInfo.UserID),
               new SqlParameter("@RefInvoiceNo", model.RefInvoiceNo),
               new SqlParameter("@IsPurchase", model.IsPurchase),
               new SqlParameter("@PID", model.PurchaseModel.PID),
               new SqlParameter("@RatePerLitre", model.RatePerLitre),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@IsUsedItem", model.IsUsedItem),
            };

            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Fuel.Update]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }

        // Helper method to map data record to TripFuelModel
        private TripFuelModel GetModel(IDataRecord reader)
        {
            return new TripFuelModel
            {
                TripFuelID = reader.GetInt64("TripFuelID"),
                EntryDate = reader.GetDateTime("EntryDate"),
                ItemType = reader.GetString("ItemType"),
                FuelItemID = reader.GetInt32("FuelItemID"),
                FuelItemDescription = reader.GetString("FuelItemDescription"),
                RefInvoiceNo = reader.GetString("RefInvoiceNo"),
                DocNumber = reader.GetString("DocNumber"),
                PurchaseModel = new()
                {
                    PID = reader.GetInt32("PID"),
                    BranchID = reader.GetInt32("BranchID"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = reader.GetByte("RecordStatus"),
                        TimeStampField = reader.GetDateTime("TimeStamp"),
                        UserID = reader.GetString("UserID"),
                    },
                },
                MID = reader.GetInt32 ("MID"),
                IsPurchase = reader.GetBoolean("IsPurchase"),
                Quantity = reader.GetDecimal("Quantity"),
                RatePerLitre = reader.GetDecimal("RatePerLitre"),
                Amount = reader.GetDecimal("Amount"),
                TruckID = reader.GetInt32("TruckID"),
                TripID = reader.GetInt64("TripID"),
                AssetTransferID = reader.GetInt32("AssetTransferID"),
                RequestApprovalHistoryID = reader.GetInt32("RequestApprovalHistoryID"),
            };
        }

        public IEnumerable<TripFuelModel> SelectByTransfer(int? RequestApprovalHistoryID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RequestApprovalHistoryID", RequestApprovalHistoryID),
                new SqlParameter("@Operation", "SelectByTransfer"),
            };

            foreach (var reader in Iservice.GetDataReader("[usp.Operation.Trips.Fuel.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }
    }
}
