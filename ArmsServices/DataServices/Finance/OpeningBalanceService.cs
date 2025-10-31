using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;
using Core.IDataServices.Finance;


namespace ArmsServices.DataServices
{
    public class OpeningBalanceService : IOpeningBalanceService
    {
        IDbService Iservice;

        public OpeningBalanceService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to update an opening balance
        public OpeningBalanceModel Update(OpeningBalanceModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.OpeniongBalanceID),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.OpeningBalance.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        // Method to reset opening balance
        public int Reset(int? PeriodID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PeriodID", PeriodID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Period.OpeningBalance.Update]", parameters);            
        }

        // Method to select all opening balances
        public IEnumerable<OpeningBalanceModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.OpeningBalance.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to get opening balances for a specific period and branch
        public IEnumerable<OpeningBalanceModel> GetBalance(int? PeriodID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Period", PeriodID),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.OpeningBalance.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        IEnumerable<OpeningBalanceModel> IOpeningBalanceService.GetBalanceByArd(string BranchIDS, string ArdCode, DateTime Date)
        {
            DateTime nextDate = Date.AddDays(1);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchIDS", BranchIDS),
               new SqlParameter("@ArdCode", ArdCode),
               new SqlParameter("@Date", nextDate),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[rptFinanceLedgerBalance]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Helper method to map data record to OpeningBalanceModel
        private OpeningBalanceModel GetModel(IDataRecord dr)
        {
            return new OpeningBalanceModel()
            {
                OpeniongBalanceID = dr.GetInt32("ID"),
                BranchID = dr.GetInt32("BranchID"),
                PeriodID = dr.GetInt32("Period"),
                CoaID = dr.GetInt32("CoaID"),
                AccountName = dr.GetString("AccountName"),
                Amount = dr.GetDecimal("Amount"),
                ArdCode = dr.GetString("ArdCode"),
                SubArdCode = dr.GetString("SubArdCode"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Method to get all periods
        public IEnumerable<PeriodModel> GetPeriods()
        {            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Period.Select]", null))
            {
                yield return GetPeriodModel(dr);
            }
        }

        // Helper method to map data record to PeriodModel
        private PeriodModel GetPeriodModel(IDataRecord dr)
        {
            return new PeriodModel()
            {
                PeriodID = dr.GetInt32("PeriodID"),
                PeriodDescription = dr.GetString("PeriodDescription")
            };
        }
    }

}
