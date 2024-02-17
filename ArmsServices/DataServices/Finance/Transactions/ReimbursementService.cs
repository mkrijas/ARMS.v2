using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace DAL.DataServices.Finance
{
    public class ReimbursementService : IReimbursementService
    {
        IDbService Iservice;


        public ReimbursementService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Reimbursement.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Reimbursement.Delete]", parameters);
        }

        public int Reverse(int? ID, string UserID, string Remarks)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InterBranchReimbursementModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Reimbursement.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<InterBranchReimbursementModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Reimbursement.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public InterBranchReimbursementModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            InterBranchReimbursementModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Reimbursement.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<InterBranchReimbursementModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Reimbursement.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public InterBranchReimbursementModel Update(InterBranchReimbursementModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@Particulars", model.Particulars.ToDataTable()),
               new SqlParameter("@InterBranchTranID", model.InterBranchTranID),
               new SqlParameter("@IsInterBranch", model.IsInterBranch),
               new SqlParameter("@OtherBranch", model.OtherBranchID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Reimbursement.Update]", parameters))
            {
                model = GetModel(dr);

            }
            return model;
        }


        private InterBranchReimbursementModel GetModel(IDataRecord dr)
        {
            InterBranchReimbursementModel model = new InterBranchReimbursementModel();
            return new InterBranchReimbursementModel()
            {
                ID = dr.GetInt32("ID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                OtherBranchName = dr.GetString("OtherBranchName"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                BranchID = dr.GetInt32("BranchID"),
                InterBranchTranID = dr.GetInt32("InterBranchTranID"),
                Narration = dr.GetString("Narration"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                OtherBranchID = dr.GetInt32("OtherBranch"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<ReimbursementSubModel> SelectParticulars (int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Reimbursement.Sub.Select]", parameters))
            {
                yield return GetSubModel(dr);
            }
        }

        private ReimbursementSubModel GetSubModel(IDataRecord dr)
        {
            return new ReimbursementSubModel()
            {
                SubID = dr.GetInt32("SubID"),
                ReimbursementID = dr.GetInt32("ReimbursementID"),
                UsageCode = dr.GetString("UsageCode"),
                CostCenterID = dr.GetInt32("CostCenterID"),
                DimensionID = dr.GetInt32("DimensionID"),
                Amount = dr.GetDecimal("Amount"),
                drcrType = dr.GetInt32("drcrType"),
                UsageCodeOther = dr.GetString("UsageCodeOther"),
                CostCenterOtherID = dr.GetInt32("CostCenterOtherID"),
                DimensionOtherID = dr.GetInt32("DimensionOtherID"),
                GstRate = dr.GetDecimal("GstRate"),
                SGST = dr.GetDecimal("SGST"),
                CGST = dr.GetDecimal("CGST"),
                IGST = dr.GetDecimal("IGST"),
                RecordStatus = (byte)dr.GetByte("RecordStatus"),
                UsageCodeDesc = dr.GetString("UsageCodeDesc"),
                UsageCodeOtherDesc = dr.GetString("UsageCodeOtherDesc"),
                CostCenterDesc = dr.GetString("CostCenterDesc"),
                DimensionDesc = dr.GetString("DimensionDesc"),
                drcrTypeDesc = dr.GetString("drcrTypeDesc"),
                CostCenterOtherDesc = dr.GetString("CostCenterOtherDesc"),
                DimensionOtherDesc = dr.GetString("DimensionOtherDesc"),
            };
        }
    }
}
