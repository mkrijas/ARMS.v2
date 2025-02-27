using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class BreakdownService : IBreakdownService
    {
        IDbService Iservice;

        public BreakdownService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a breakdown by its ID
        public int Delete(int? BreakdownID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BreakdownID", BreakdownID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Breakdown.Delete]", parameters);
        }

        // Method to select all breakdowns
        public IEnumerable<BreakdownModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "All")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Breakdown.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select pending breakdowns by branch ID
        public IEnumerable<BreakdownModel> SelectPending(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Pending"),
               new SqlParameter("@BranchID", BranchID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Breakdown.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a breakdown by its ID
        public BreakdownModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BreakdownID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            BreakdownModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Breakdown.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to update a breakdown
        public BreakdownModel Update(BreakdownModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BreakdownID", model.BreakdownID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@BreakdownTime", model.BreakdownTime),
               new SqlParameter("@BreakdownType", model.BreakdownType),
               new SqlParameter("@ContactNumber", model.ContactNumber),
               new SqlParameter("@Detail", model.Detail),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Breakdown.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to reject a breakdown
        public int RejectBreakdown(int? BreakdownID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BreakdownID", BreakdownID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Breakdown.Reject]", parameters);
        }

        // Method to select estimates for a specific breakdown  
        public IEnumerable<EstimateListModel> SelectEstimate(int? BreakdownID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BreakdownID", BreakdownID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Breakdown.Estimate.Select]", parameters))
            {
                yield return GetEstModel(dr);
            }
        }

        // Method to update an estimate
        public EstimateListModel UpdateEstimate(EstimateListModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BreakdownID", model.BreakdownID),
               new SqlParameter("@Description", model.Description),
               new SqlParameter("@ImagePath", model.ImagePath),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.Breakdown.Estimate.Update]", parameters))
            {
                model = GetEstModel(dr);
            }
            return model;
        }

        // Method to delete an estimate
        public int DeleteEstimate(int? EstimateID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "DELETE"),
               new SqlParameter("@ID", EstimateID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Breakdown.Estimate.ApproveOrDelete]", parameters);
        }

        // Method to approve an estimate
        public int ApproveEstimate(int? EstimateID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "APPROVE"),
               new SqlParameter("@ID", EstimateID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Breakdown.Estimate.ApproveOrDelete]", parameters);
        }

        // Method to add an image to an estimate
        public int AddImgEstimate(int? EstimateID, string ImgPath, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ADD"),
               new SqlParameter("@ID", EstimateID),
               new SqlParameter("@ImagePath", ImgPath),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Breakdown.Estimate.ApproveOrDelete]", parameters);
        }

        // Method to remove an estimate
        public int RemoveEstimate(int? EstimateID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "REMOVE"),
               new SqlParameter("@ID", EstimateID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.Breakdown.Estimate.ApproveOrDelete]", parameters);
        }

        // Helper method to map data record to BreakdownModel
        private BreakdownModel GetModel(IDataRecord dr)
        {
            return new BreakdownModel
            {
                BreakdownID = dr.GetInt32("BreakdownID"),
                BreakdownNumber = dr.GetString("BreakdownNumber"),
                BranchID = dr.GetInt32("BranchID"),
                BreakdownTime = dr.GetDateTime("BreakdownTime"),
                BreakdownType = dr.GetString("BreakdownType"),
                ContactNumber = dr.GetString("ContactNumber"),
                Detail = dr.GetString("Detail"),
                TruckID = dr.GetInt32("TruckID"),
                RegNo = dr.GetString("RegNo"),
                IsClaimInitiated = dr.GetBoolean("IsClaimInitiated"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Helper method to map data record to EstimateListModel
        private EstimateListModel GetEstModel(IDataRecord dr)
        {
            return new EstimateListModel
            {
                ID = dr.GetInt32("ID"),
                BreakdownID = dr.GetInt32("BreakdownID"),
                Description = dr.GetString("Description"),
                Amount = dr.GetDecimal("Amount"),
                ImagePath = dr.GetString("ImagePath"),
                ApprovedUserID = dr.GetString("ApprovedUserID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    UserID = dr.GetString("UserID"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                },
            };
        }
    }
}