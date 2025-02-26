using ArmsModels.BaseModels;
using ArmsServices;
using Core.IDataServices.Finance.DayOpen;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Core.BaseModels.Finance;
using System.Reflection;
using System;

namespace DAL.DataServices.Finance.DayOpen
{
    public class DayOpenService : IDayOpenService
    {
        IDbService Iservice;

        public DayOpenService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Retrieves a list of DayOpenRequestModel records based on the given number of records and branch ID
        public IEnumerable<DayOpenRequestModel> Select(int? NoOfRecords, int? BranchId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", ""),
               new SqlParameter("@BranchId", BranchId),
               new SqlParameter("@NoOfRecords", NoOfRecords),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.entity.DayOpen.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Updates an existing DayOpenRequestModel record in the database
        public DayOpenRequestModel Update(DayOpenRequestModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.Branch.BranchID),
               new SqlParameter("@DocTypeID", model.DocType.ID),
               new SqlParameter("@DateFrom", model.FromDate),
               new SqlParameter("@DateUpto", model.ToDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.entity.DayOpen.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Approves a day open request by updating its status in the database.
        public DayOpenRequestModel Approve(DayOpenRequestModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "APPROVE"),
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.entity.DayOpen.ApproveReject]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Rejects or closes a day open request by modifying its status in the database.
        public DayOpenRequestModel RejectOrClose(DayOpenRequestModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "REJECTCLOSE"),
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.entity.DayOpen.ApproveReject]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Validates whether a day open request can be processed for a given date, document type, and branch.
        public bool? ValidateDayOpen(DateTime? DocDate, int? DocTypeID, int? BranchID)
        {
            bool? result = false;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocDate", DocDate),
               new SqlParameter("@DocTypeID", DocTypeID),
               new SqlParameter("@BranchID", BranchID),
            };
            //int? result = Iservice.ExecuteNonQuery("[usp.entity.DayOpen.Validate]", parameters);
            //return result;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.entity.DayOpen.Validate]", parameters))
            {
                result = dr.GetBoolean("ReturnValue");
            }
            return result;
        }

        // Maps database records to the DayOpenRequestModel class.
        private DayOpenRequestModel GetModel(IDataRecord dr)
        {
            return new DayOpenRequestModel
            {
                ID = dr.GetInt32("ID"),
                Branch = new BranchModel
                {
                    BranchID = dr.GetInt32("BranchID"),
                    BranchName = dr.GetString("BranchName"),
                },
                DocType = new DocTypeModel
                {
                    ID = dr.GetInt32("DocumentTypeID"),
                    Description = dr.GetString("Description"),
                },
                FromDate = dr.GetDateTime("DateFrom"),
                ToDate = dr.GetDateTime("DateUpto"),
                IsOpen = dr.GetBoolean("IsOpen"),
                OpenByUsedID = dr.GetString("OpenedBy"),
                OpenDate = dr.GetDateTime("OpenedOn"),
                CloseByUsedID = dr.GetString("ClosedBy"),
                CloseDate = dr.GetDateTime("ClosedOn"),
                RecordStatus = dr.GetByte("RecordStatus"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    UserID = dr.GetString("UserID"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                },
            };
        }
    }
}