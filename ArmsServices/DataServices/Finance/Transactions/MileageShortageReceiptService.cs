using ArmsModels.BaseModels.Finance.Transactions;
using ArmsServices;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsServices.DataServices;
using Core.BaseModels.Finance.Transactions;
using ArmsModels.BaseModels.General;
using ArmsServices.DataServices.General;


namespace DAL.DataServices.Finance.Transactions
{
    public class MileageShortageReceiptService: IMileageShortageReceiptService
    {
        IDbService Iservice;
        IConfigTable configTable;
        public MileageShortageReceiptService(IDbService iservice, IConfigTable IconfigTable)
        {
            Iservice = iservice;
            configTable= IconfigTable;
        }

        // Method to delete a mileage shortage receipt
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MileageShortageReceiptID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.MileageShortageReceipt.Delete]", parameters);

        }

        // Method to select all mileage shortage receipts
        public IEnumerable<MileageShortageReceiptModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select mileage shortage receipts by Trip ID
        public IEnumerable<MileageShortageReceiptModel> SelectByTripID(long? TripID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByTripID"),
               new SqlParameter("@TripID", TripID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select mileage shortage receipts by Transfer ID
        public IEnumerable<MileageShortageReceiptModel> SelectByTransferID(int? RequestApprovalHistoryID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByTransferID"),
               new SqlParameter("@RequestApprovalHistoryID", RequestApprovalHistoryID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select approved mileage shortage receipts
        public IEnumerable<MileageShortageReceiptModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select unapproved mileage shortage receipts
        public IEnumerable<MileageShortageReceiptModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a mileage shortage receipt by its ID
        public MileageShortageReceiptModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MileageShortageReceiptID", ID),
               new SqlParameter("@Operation", "GetEntries")
            };
            MileageShortageReceiptModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to approve a mileage shortage receipt
        public int Approve(int? MileageShortageReceiptID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MileageShortageReceiptID", MileageShortageReceiptID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.MileageShortageReceipt.Approve]", parameters);
        }

        // Method to reverse a mileage shortage receipt
        public int Reverse(int? MileageShortageReceiptID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MileageShortageReceiptID", MileageShortageReceiptID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.MileageShortageReceipt.Reverse]", parameters);
        }

        // Method to update MileageShortageReceiptModel record
        public MileageShortageReceiptModel Update(MileageShortageReceiptModel model)
        {
            ConfigModel MileageShortageAccount;
            ConfigModel CashAccount;

            if (model.AllowanceAmount == 0 || model.AllowanceAmount == null)
            {
                MileageShortageAccount = configTable.GetDefaultMileageShortageCoaID();
                CashAccount = configTable.GetByDefaultCashCoaID();
            }
            else
            {
                MileageShortageAccount = configTable.GetDefaultMileageShortageCoaID();
                //CashAccount = configTable.GetByDefaultCashCoaID();
                CashAccount = configTable.GetDefaultMileageShortageReceivableID();
            }
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MileageShortageReceiptID", model.MileageShortageReceiptID),
               new SqlParameter("@CreditCoaID", MileageShortageAccount.ValueString),
               new SqlParameter("@DebitCoaID", CashAccount.ValueString),
               new SqlParameter("@ReceiptMode", model.ReceiptMode),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNumber", model.DocumentNumber),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@TripID", model.TripID),
               new SqlParameter("@DriverID", model.DriverID),
               new SqlParameter("@AssetTransferID", model.AssetTransferID),
               new SqlParameter("@RequestApprovalHistoryID", model.RequestApprovalHistoryID),
               new SqlParameter("@RunningKM", model.RunningKM),
               new SqlParameter("@ConsumedFuel", model.ConsumedFuel),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@AllottedMileage", model.AllottedMileage),
               new SqlParameter("@AllottedDistance", model.AllottedDistance),
               new SqlParameter("@FuelPrice", model.FuelPrice),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@AllowanceAmount", model.AllowanceAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.MileageShortageReceipt.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Private method to convert an IDataRecord to a MileageShortageReceiptModel
        private MileageShortageReceiptModel GetModel(IDataRecord dr)
        {
            return new MileageShortageReceiptModel
            {
                MileageShortageReceiptID = dr.GetInt32("MileageShortageReceiptID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                ReceiptMode = dr.GetString("ReceiptMode"),
                TripID = dr.GetInt32("TripID"),
                TripNo = dr.GetString("TripNo"),
                AssetTransferID = dr.GetInt32("AssetTransferID"),
                RequestApprovalHistoryID = dr.GetInt32("RequestApprovalHistoryID"),
                DriverID = dr.GetInt32("DriverID"),
                RunningKM = dr.GetDecimal("RunningKM"),
                ConsumedFuel = dr.GetDecimal("ConsumedFuel"),
                AllottedMileage = dr.GetDecimal("AllottedMileage"),
                AllottedDistance = dr.GetDecimal("AllottedDistance"),
                FuelPrice = dr.GetDecimal("FuelPrice"),
                Reference = dr.GetString("Reference"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                BranchID = dr.GetInt32("BranchID"),
                FileName = dr.GetString("FilePath"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                MID = dr.GetInt32("MID"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension = dr.GetInt32("Dimension"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                AllowanceAmount = dr.GetDecimal("AllowanceAmount"),
                Narration = dr.GetString("Narration"),

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
