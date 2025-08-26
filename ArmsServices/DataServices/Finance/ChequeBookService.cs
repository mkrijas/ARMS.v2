using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Core.BaseModels.Finance;

namespace ArmsServices.DataServices
{
    public class ChequeBookService : IChequeBookService
    {
        IDbService Iservice;

        public ChequeBookService(IDbService iservice)
        {
            Iservice = iservice;
        }
        
        public ChequeBookModel Update(ChequeBookModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ChequeBookID", model.ChequeBookID),
               new SqlParameter("@OwnBankAccountID", model.OwnBankAccountID),
               new SqlParameter("@StartingChequeNo", model.StartingChequeNo),
               new SqlParameter("@EndingChequeNo", model.EndingChequeNo),
               new SqlParameter("@CreatedDate", model.CreatedDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.ChequeBook.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.ChequeBook.Delete]", parameters);
        }

        public int Approve(int ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.ChequeBook.Approve]", parameters);
        }

        public ChequeBookModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ChequeBookID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            ChequeBookModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.ChequeBook.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<ChequeBookModel> SelectByApproved(int? OwnBankAccountID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OwnBankAccountID", OwnBankAccountID),
               new SqlParameter("@Operation", "ByApproved")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.ChequeBook.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ChequeBookModel> SelectByUnapproved(int? OwnBankAccountID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OwnBankAccountID", OwnBankAccountID),
               new SqlParameter("@Operation", "ByUnapproved")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.ChequeBook.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ChequeBookLeavesModel LeafUpdate(ChequeBookLeavesModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LeafID", model.LeafID),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@RecordStatus", model.UserInfo.RecordStatus),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.ChequeBook.Leaves.Update]", parameters))
            {
                model = GetLeafModel(dr);
            }
            return model;
        }

        public IEnumerable<ChequeBookLeavesModel> GetAllLeaves(int? ChequeBookID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ChequeBookID", ChequeBookID),
               new SqlParameter("@Operation", "GetAll")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.ChequeBook.Leaves.Select]", parameters))
            {
                yield return GetLeafModel(dr);
            }
        }

        public IEnumerable<ChequeBookLeavesModel> GetPendingLeaves(int? ChequeBookID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ChequeBookID", ChequeBookID),
               new SqlParameter("@Operation", "GetPending")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.ChequeBook.Leaves.Select]", parameters))
            {
                yield return GetLeafModel(dr);
            }
        }

        public IEnumerable<ChequeBookLeavesModel> GetActiveLeaves(int? ChequeBookID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ChequeBookID", ChequeBookID),
               new SqlParameter("@Operation", "GetActive")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.ChequeBook.Leaves.Select]", parameters))
            {
                yield return GetLeafModel(dr);
            }
        }

        public IEnumerable<ChequeBookLeavesModel> GetCashedLeaves(int? ChequeBookID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ChequeBookID", ChequeBookID),
               new SqlParameter("@Operation", "GetCashed")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.ChequeBook.Leaves.Select]", parameters))
            {
                yield return GetLeafModel(dr);
            }
        }

        public IEnumerable<ChequeBookLeavesModel> GetCancelledLeaves(int? ChequeBookID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ChequeBookID", ChequeBookID),
               new SqlParameter("@Operation", "GetCancelled")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.ChequeBook.Leaves.Select]", parameters))
            {
                yield return GetLeafModel(dr);
            }
        }

        public IEnumerable<ChequeBookLeavesModel> GetDeletedLeaves(int? ChequeBookID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ChequeBookID", ChequeBookID),
               new SqlParameter("@Operation", "GetDeleted")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.ChequeBook.Leaves.Select]", parameters))
            {
                yield return GetLeafModel(dr);
            }
        }

        private ChequeBookModel GetModel(IDataRecord dr)
        {
            return new ChequeBookModel
            {
                ChequeBookID = dr.GetInt32("ChequeBookID"),
                OwnBankAccountID = dr.GetInt32("OwnBankAccountID"),
                StartingChequeNo = dr.GetString("StartingChequeNo"),
                EndingChequeNo = dr.GetString("EndingChequeNo"),
                CreatedDate = dr.GetDateTime("CreatedDate"),
                PendingLeaves = dr.GetInt32("PendingLeaves"),
                ActiveLeaves = dr.GetInt32("ActiveLeaves"),
                CashedLeaves = dr.GetInt32("CashedLeaves"),
                CancelledLeaves = dr.GetInt32("CancelledLeaves"),
                DeletedLeaves = dr.GetInt32("DeletedLeaves"),
                AuthLevelID = dr.GetInt32("AuthLevelID"),
                AuthStatus = dr.GetString("AuthStatus"),
                Remarks = dr.GetString("Remarks"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private ChequeBookLeavesModel GetLeafModel(IDataRecord dr)
        {
            return new ChequeBookLeavesModel
            {
                LeafID = dr.GetInt32("LeafID"),
                ChequeBookID = dr.GetInt32("ChequeBookID"),
                ChequeNo = dr.GetString("ChequeNo"),
                Amount = dr.IsDBNull("Amount") ? null : dr.GetDecimal("Amount"),
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

