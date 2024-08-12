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

        public RequestApprovalHistoryModel GetTransferInfo(long? RequestApprovalHistoryID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", RequestApprovalHistoryID),
               new SqlParameter("@Operation", "GetTransferInfo"),
            };
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.TruckAvailability.Request.Select]", parameters))
            {
                return new RequestApprovalHistoryModel()
                {
                    RequestApprovalHistoryID = reader.GetInt32("RequestApprovalHistoryID"),
                    DocNumber = reader.GetString("DocNumber"),
                    OpeningKM = reader.GetInt32("OpeningKM"),
                    ClosingKM = reader.GetInt32("ClosingKM"),
                    Fuel = reader.GetDecimal("Fuel"),
                    Truck = new TruckModel()
                    {
                        TruckID = reader.GetInt32("TruckID"),
                        RegNo = reader.GetString("RegNo"),
                    }
                };
            }
            return null;
        }

        public RequestApprovalHistoryModel GetTransferByID(long? RequestApprovalHistoryID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", RequestApprovalHistoryID),
               new SqlParameter("@Operation", "ByID"),
            };
            foreach (var reader in Iservice.GetDataReader("[usp.Operation.TruckAvailability.Request.Select]", parameters))
            {
                return new RequestApprovalHistoryModel()
                {
                    RequestApprovalHistoryID = reader.GetInt32("RequestApprovalHistoryID"),
                    DocNumber = reader.GetString("DocNumber"),
                    Truck = new TruckModel()
                    {
                        TruckID = reader.GetInt32("TruckID"),
                        RegNo = reader.GetString("RegNo"),
                    }
                };
            }
            return null;
        }

        public RequestApprovalHistoryModel UpdateOutgoing(RequestApprovalHistoryModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@RequestApprovalHistoryID", model.RequestApprovalHistoryID),
                new SqlParameter("@TruckID", model.Truck.TruckID),
                new SqlParameter("@RequestedBranchID", model.RequestedBranchID),
                new SqlParameter("@RequestedUserID", model.RequestedUserInfo.UserID),
                new SqlParameter("@RequestStatus", model.RequestStatus),
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
                new SqlParameter("@OpeningKM", model.OpeningKM),
                new SqlParameter("@ClosingKM", model.ClosingKM),
                new SqlParameter("@RequestStatus", model.RequestStatus),
                new SqlParameter("@RecordStatus", 3),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TruckAvailability.Response.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        public IEnumerable<int?> GetAllTruckIdsByBranchID(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BranchID", BranchID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TruckAvailability.Request.TrickIds.Select]", parameters))
            {
                yield return dr.GetInt32("TruckID");
            }
        }

        private RequestApprovalHistoryModel GetModel(IDataRecord dr)
        {
            return new RequestApprovalHistoryModel
            {
                RequestApprovalHistoryID = dr.GetInt32("RequestApprovalHistoryID"),
                DocNumber = dr.GetString("DocNumber"),
                TruckID = dr.GetInt32("TruckID"),
                DriverID = dr.GetInt32("DriverID"),
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
                RequestStatus = dr.GetByte("RequestStatus"),
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
