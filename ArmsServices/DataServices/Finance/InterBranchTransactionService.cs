using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class InterBranchTransactionService : IInterBranchMappingService
    {
        IDbService Iservice;

        public InterBranchTransactionService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete an inter-branch transaction mapping by its ID
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.InterBranchAccount.Delete]", parameters);

        }

        // Method to get all inter-branch transaction types
        public IEnumerable<InterBranchTransactionTypeModel> GetTypes()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetTypes"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.InterBranchAccount.Select]", parameters))
            {
                yield return new InterBranchTransactionTypeModel()
                {
                    ID = dr.GetInt32("ID"),
                    TransactionTypeName = dr.GetString("TransactionTypeName"),
                };
            }
        }

        // Method to select inter-branch mappings with optional filtering
        public IEnumerable<InterBranchMappingModel> Select(int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.InterBranchAccount.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a specific inter-branch mapping by branch and transaction type IDs
        public InterBranchMappingModel Select(int? BranchID, int? TransactionTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Specific"),
               new SqlParameter("@TransactionTypeID", TransactionTypeID),
               new SqlParameter("@BranchID", BranchID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.InterBranchAccount.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to select an inter-branch mapping by its ID 
        public InterBranchMappingModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@ID", ID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.InterBranchAccount.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to check if entries already exist for a given inter-branch mapping
        public InterBranchMappingModel IsEntriesAlreadyExistOrNot(InterBranchMappingModel model)
        {
            InterBranchMappingModel ExistModel = null;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@TransactionTypeID", model.TransactionTypeID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.InterBranchAccount.Entries.Exist]", parameters))
            {
                ExistModel = GetModel(dr);
            }
            return ExistModel;
        }

        // Method to update an inter-branch mapping
        public InterBranchMappingModel Update(InterBranchMappingModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@InterBranchArdCode", model.InterBranchArdCode),
               new SqlParameter("@TransactionTypeID", model.TransactionTypeID),
               new SqlParameter("@CoaID", model.CoaID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.InterBranchAccount.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to InterBranchMappingModel
        private InterBranchMappingModel GetModel(IDataRecord dr)
        {
            return new InterBranchMappingModel
            {
                ID = dr.GetInt32("ID"),
                BranchID = dr.GetInt32("BranchID"),
                BranchName = dr.GetString("BranchName"),
                CoaID = dr.GetInt32("CoaID"),
                InterBranchArdCode = dr.GetString("InterBranchArdCode"),
                TransactionTypeID = dr.GetInt32("TransactionTypeID"),
                TransactionTypeName = dr.GetString("TransactionTypeName"),
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
