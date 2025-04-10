using ArmsServices.DataServices;
using ArmsServices;
using Core.IDataServices.Operations;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Core.BaseModels.Operations;
using ArmsModels.BaseModels;
using System.Linq;
using System;

namespace DAL.DataServices.Operations
{
    public class TruckAvailabilityService: ITruckAvailabilityService
    {
        IDbService Iservice;
        public TruckAvailabilityService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a truck availability request by its ID
        public int DeleteRequest(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ID", ID),
                new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Operation.TruckAvailability.Request.Delete]", parameters);
        }

        // Method to select outgoing truck availability requests by branch ID
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

        // Method to get transfer information by request approval history ID
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

        // Method to get transfer information by request approval history ID
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
                    },
                    Uploads = reader.HasColumn("Uploads") && !reader.IsDBNull("Uploads") && !string.IsNullOrWhiteSpace(reader.GetString("Uploads"))
                        ? reader.GetString("Uploads").Split(";").ToList()
                        : new List<string>(),
                };
            }
            return null;
        }

        // Method to update an outgoing truck availability request
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

        // Method to select incoming truck availability requests by branch ID
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

        // Method to update the status of a truck availability request
        public RequestApprovalHistoryModel UpdateStatus(RequestApprovalHistoryModel model, List<int?> RecievedList)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("IntField", typeof(int));
            if (RecievedList == null)
                RecievedList = new List<int?>();
            foreach (int? value in RecievedList)
            {
                if (value.HasValue)
                {
                    dataTable.Rows.Add(value.Value);
                }
                else
                {
                    dataTable.Rows.Add(DBNull.Value);
                }
            }
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@RequestApprovalHistoryID", model.RequestApprovalHistoryID),
                new SqlParameter("@Uploads", string.Join(";",model.Uploads)),
                new SqlParameter("@RespondedUserID", model.RespondedUserInfo.UserID),
                new SqlParameter("@OpeningKM", model.OpeningKM),
                new SqlParameter("@ClosingKM", model.ClosingKM),
                new SqlParameter("@RequestStatus", model.RequestStatus),
                new SqlParameter("@RecordStatus", 3),
                new SqlParameter("@ReceivedList", dataTable),
                new SqlParameter("@CheckList", model.CheckList.ToDataTable()),
                new SqlParameter("@Remarks", model.Remarks)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TruckAvailability.Response.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get all truck IDs by branch ID
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

        // Helper method to map data record to RequestApprovalHistoryModel
        private RequestApprovalHistoryModel GetModel(IDataRecord dr)
        {
            return new RequestApprovalHistoryModel
            {
                RequestApprovalHistoryID = dr.GetInt32("RequestApprovalHistoryID"),
                DocNumber = dr.GetString("DocNumber"),
                TruckID = dr.GetInt32("TruckID"),
                DriverID = dr.GetInt32("DriverID"),
                OpeningKM = dr.GetInt32("OpeningKM"),
                ClosingKM = dr.GetInt32("ClosingKM"),
                //Uploads = dr.IsDBNull("Uploads") || string.IsNullOrWhiteSpace(dr.GetString("Uploads"))
                //    ? new List<string>()
                //    : dr.GetString("Uploads").Split(";").ToList(), 
                Uploads = dr.HasColumn("Uploads") && !dr.IsDBNull("Uploads") && !string.IsNullOrWhiteSpace(dr.GetString("Uploads"))
                        ? dr.GetString("Uploads").Split(";").ToList()
                        : new List<string>(),
                Remarks = dr.GetString("Remarks"),
                Truck = new TruckModel()
                {
                    AssetID = dr.GetInt32("AssetID"),
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

        public IEnumerable<AssetSettingsModel> GetCheckList(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetCheckList"),
               new SqlParameter("@ID", ID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TruckAvailability.Request.Select]", parameters))
            {
                yield return new AssetSettingsModel()
                {
                    CheckListID = dr.GetInt32("CheckListID"),
                    SettingsID = dr.GetInt32("AssetSettingsID"),
                    SettingsName = dr.GetString("SettingsName"),
                    Value = dr.GetString("Value"),
                    Condition = dr.GetString("Condition")
                };
            }
        }
    }
}
