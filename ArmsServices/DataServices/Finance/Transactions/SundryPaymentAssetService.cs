using ArmsModels.BaseModels;
using Core.BaseModels.Finance.Transactions;
using Core.IDataServices.Finance.Transactions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using ArmsServices;
using ArmsServices.DataServices;

namespace DAL.DataServices.Finance.Transactions
{
    public class SundryPaymentAssetService : ISundryPaymentAssetService
    {
        IDbService Iservice;

        public SundryPaymentAssetService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to approve a sundry payment asset entry
        public IEnumerable<SundryPaymentAssetModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryPaymentAsset.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to unapprove a sundry payment asset entry
        public IEnumerable<SundryPaymentAssetModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool IsInterBranch, string searchTerm)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryPaymentAsset.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to update a sundry payment asset entry
        public SundryPaymentAssetModel Update(SundryPaymentAssetModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
                {
                   new SqlParameter("@SundryPaymentAssetID", model?.SundryPaymentAssetID),
                   new SqlParameter("@DocDate", model.DocumentDate),
                   new SqlParameter("@DocNumber", model.DocumentNumber),
                   new SqlParameter("@MID", model.MID),
                   new SqlParameter("@BranchID", model.BranchID),
                   new SqlParameter("@AssetID", model.Asset.AssetID),
                   new SqlParameter("@AssetCoaID", model.Asset.CoaID),
                   new SqlParameter("@PaymentMode", model.PaymentMode),
                   new SqlParameter("@PaymentTool", model.PaymentTool),
                   new SqlParameter("@PaymentArdCode", model.PaymentArdCode),
                   new SqlParameter("@CoaID", model.PaymentCoaID),
                   new SqlParameter("@PayeeName", model.PayeeName),
                   new SqlParameter("@PayeeContactNo", model.PayeeContactNo),
                   new SqlParameter("@FilePath", model.FileName),
                   new SqlParameter("@Reference", model.Reference),
                   new SqlParameter("@Narration", model.Narration),
                   new SqlParameter("@Amount", model.Amount),
                   new SqlParameter("@TotalAmount", model.TotalAmount),
                   new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
                   new SqlParameter("@BankCharges", model.BankCharges),
                   new SqlParameter("@UserID", model.UserInfo.UserID),
                };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.SundryPaymentAsset.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to approve a sundry payment asset entry
        public int Approve(int? SundryPaymentAssetID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryPaymentAssetID", SundryPaymentAssetID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryPaymentAsset.Approve]", parameters);
        }

        // Method to delete a sundry payment asset entry
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SundryPaymentAssetID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.SundryPaymentAsset.Delete]", parameters);
        }

        // Helper method to map data record to SundryPaymentAssetModel
        private SundryPaymentAssetModel GetModel(IDataRecord dr)
        {
            return new SundryPaymentAssetModel
            {
                SundryPaymentAssetID = dr.GetInt32("SundryPaymentAssetID"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                MID = dr.GetInt32("MID"),
                BranchID = dr.GetInt32("BranchID"),
                PaymentMode = dr.GetString("PaymentMode"),
                PaymentTool = dr.GetString("PaymentTool"),
                PaymentArdCode = dr.GetString("ArdCode"),
                PaymentCoaID = dr.GetInt32("CoaID"),
                PayeeName = dr.GetString("PayeeName"),
                PayeeContactNo = dr.GetString("PayeeContactNo"),
                Reference = dr.GetString("Reference"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                Asset = new AssetModel
                {
                    AssetID = dr.GetInt32("AssetID"),
                    AssetCode = dr.GetString("AssetCode"),
                    Description = dr.GetString("Description"),
                    CoaID = dr.GetInt32("AssetCoaID"),
                },
                Amount = dr.GetDecimal("Amount"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                BankCharges = dr.GetDecimal("BankCharges"),
                FileName = dr.GetString("FilePath"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public int RemoveFile(int? ID, string UserID)
        {
            throw new System.NotImplementedException();
        }

        public int Reverse(int? ID, string UserID, string Remarks)
        {
            throw new System.NotImplementedException();
        }

        public SundryPaymentAssetModel SelectByID(int? ID)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<SundryPaymentAssetModel> Select(int? BranchID)
        {
            throw new System.NotImplementedException();
        }
    }
}