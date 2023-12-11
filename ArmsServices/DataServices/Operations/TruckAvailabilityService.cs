using ArmsServices.DataServices;
using ArmsServices;
using Core.IDataServices.Operations;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Core.BaseModels.Operations;
using ArmsModels.BaseModels;

namespace DAL.DataServices.Operations
{
    public class TruckAvailabilityService: ITruckAvailabilityService
    {
        IDbService Iservice;
        public TruckAvailabilityService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int DeleteRequest(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
                    {
                       new SqlParameter("@ID", ID),
                       new SqlParameter("@UserID", UserID),
                    };
            return Iservice.ExecuteNonQuery("[usp.Operation.TruckAvailability.Request.Delete]", parameters);
        }

        public IEnumerable<RequestApprovalHistoryModel> SelectOutgoingRequests(int? ID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
                    {
                       new SqlParameter("@Operation", "Outgoing"),
                       new SqlParameter("@ID", ID),
                       new SqlParameter("@BranchID", BranchID),
                    };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TruckAvailability.Request.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public RequestApprovalHistoryModel UpdateOutgoing(RequestApprovalHistoryModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
                    {
                       new SqlParameter("@RequestApprovalHistoryID", model.RequestApprovalHistoryID),
                       new SqlParameter("@TruckID", model.Truck.TruckID),
                       new SqlParameter("@RequestedBranchID", model.RequestedBranchID),
                       new SqlParameter("@RequestedUserID", model.RequestedUserInfo.UserID),
                       new SqlParameter("@RecordStatus", 3),
                    };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TruckAvailability.Request.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        public IEnumerable<RequestApprovalHistoryModel> SelectIncomingTrucks(int? ID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
                    {
                       new SqlParameter("@Operation", "Incoming"),
                       new SqlParameter("@ID", ID),
                       new SqlParameter("@BranchID", BranchID),
                    };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TruckAvailability.Request.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public RequestApprovalHistoryModel UpdateStatus(RequestApprovalHistoryModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
                    {
                       new SqlParameter("@RequestApprovalHistoryID", model.RequestApprovalHistoryID),
                       new SqlParameter("@RespondedUserID", model.RespondedUserInfo.UserID),
                       new SqlParameter("@IsApproved", model.IsApproved),
                       new SqlParameter("@RecordStatus", 3),
                    };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TruckAvailability.Response.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        private RequestApprovalHistoryModel GetModel(IDataRecord dr)
        {
            return new RequestApprovalHistoryModel
            {
                RequestApprovalHistoryID = dr.GetInt32("RequestApprovalHistoryID"),
                TruckID = dr.GetInt32("TruckID"),
                Truck = new TruckModel()
                {
                    TruckID = dr.GetInt32("TruckID"),
                    RegNo = dr.GetString("RegNo"),
                },
                RequestedBranchID = dr.GetInt32("RequestedBranchID"),
                RequestedBranch = new BranchModel()
                {
                    BranchID = dr.GetInt32("RequestedBranchID"),
                    BranchName = dr.GetString("RequestedBranchName"),
                },
                RespondedBranchID = dr.GetInt32("RespondedBranchID"),
                RespondedBranch = new BranchModel()
                {
                    BranchID = dr.GetInt32("RespondedBranchID"),
                    BranchName = dr.GetString("RespondedBranchName"),
                },
                RequestedUserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("RequestedTimeStamp"),
                    UserID = dr.GetString("RequestedUserID"),
                },
                RespondedUserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("RespondedTimeStamp"),
                    UserID = dr.GetString("RespondedUserID"),
                },
            };
        }
    }
}
