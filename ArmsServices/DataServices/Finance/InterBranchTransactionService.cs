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
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.InterBranchAccount.Delete]", parameters);

        }

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
