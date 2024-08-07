using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

        public IEnumerable<PeriodModel> GetPeriods()
        {            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Period.Select]", null))
            {
                yield return GetPeriodModel(dr);
            }
        }

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
