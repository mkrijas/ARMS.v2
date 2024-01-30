using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class CashAccountService : ICashAccountService
    {
        IDbService Iservice;

        public CashAccountService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public CashAccountModel Update(CashAccountModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CashAccountID", model.CashAccountID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@CashCode", model.CashCode),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@AccountType", model.AccountType),
               new SqlParameter("@CoaID", model.Coa.CoaID),
               new SqlParameter("@MinBalance", model.MinBalance),
               new SqlParameter("@MaxBalance", model.MaxBalance),               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CashAccount.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public int Delete(int? CashAccountID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CashAccountID", CashAccountID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.CashAccount.Delete]", parameters);
        }
        public int DisableAccount(bool? IsDisable ,int? CashAccountID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@IsDisable", IsDisable),
               new SqlParameter("@CashAccountID", CashAccountID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.CashAccount.Disable]", parameters);
        }
        public IEnumerable<CashAccountModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CashAccount.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }


        private CashAccountModel GetModel(IDataRecord dr)
        {
            return new CashAccountModel()
            {
                CashAccountID = dr.GetInt32("CashAccountID"),
                BranchID = dr.GetInt32("BranchID"),
                CashCode = dr.GetString("CashCode"),
                AccountType = dr.GetString("AccountType"),
                Coa = new ChartOfAccountModel() { AccountName = dr.GetString("AccountName"),CoaID = dr.GetInt32("CoaID") },
                MinBalance = dr.GetDecimal("MinBalance"),
                MaxBalance = dr.GetDecimal("MaxBalance"),
                Title = dr.GetString("Title"),  
                IsDisabled = (dr.GetByte("RecordStatus") == 1 ? true : false),  
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<CashAccountModel> SelectByBranch(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "ByBranch"),
            };


            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CashAccount.Select]", parameters))
            {
                yield return GetModel(dr);
            }

        }
        public IEnumerable<CashAccountModel> SelectByBranchALL(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "ByBranchAll"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CashAccount.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public CashAccountModel SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CashAccountID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            CashAccountModel model = new CashAccountModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CashAccount.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public decimal? GetBalance(int? CashAccountID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CashAccountID", CashAccountID),
               new SqlParameter("@Operation", "GetBalance"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CashAccount.Select]", parameters))
            {
                return dr.GetDecimal("Accountbalnce");
            }
            return null;
        }
    }
}
