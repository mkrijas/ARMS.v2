using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IChartOfAccountService
    {
        ChartOfAccountModel Update(ChartOfAccountModel model);
        ChartOfAccountModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<ChartOfAccountModel> SelectChildren(int? CoaID);
        IEnumerable<ChartOfAccountModel> SelectBase(); 
        IEnumerable<ChartOfAccountModel> FilterSubLedgers(string filterText);
        IEnumerable<ChartOfAccountModel> SelectByGroup(int? GroupID);
        IEnumerable<CoaBranchAvailabilityModel> GetAllocatedBranches(int? CoaID);
        void AddBranch(CoaBranchAvailabilityModel model);
        void RemoveBranch(CoaBranchAvailabilityModel model);
        IEnumerable<CoaBranchAvailabilityModel> GetSubledgersInBranch(int? BranchID, string filterText);

    }
    public class ChartOfAccountService : IChartOfAccountService
    {    
    IDbService Iservice;

        public ChartOfAccountService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsRateID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Coa.Delete]", parameters);
        }

        public IEnumerable<ChartOfAccountModel> SelectByGroup(int? CoaID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByGroup"),
               new SqlParameter("@CoaID",CoaID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<ChartOfAccountModel> SelectChildren(int? CoaID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "children"),
               new SqlParameter("@CoaID",CoaID),              
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public IEnumerable<ChartOfAccountModel> SelectBase()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Base"),               
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public ChartOfAccountModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@coaID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            ChartOfAccountModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public ChartOfAccountModel Update(ChartOfAccountModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AccountName", model.AccountName),
               new SqlParameter("@AccountDescription", model.AccountDescription),
               new SqlParameter("@CoaID", model.CoaID),
               new SqlParameter("@AccountCode", model.AccountCode),
               new SqlParameter("@ParentID", model.ParentID),
               new SqlParameter("@SummaryAccount", model.SummaryAccount),
               new SqlParameter("@AccountType", model.AccountType),
               new SqlParameter("@PeriodFrom", model.PeriodFrom),
               new SqlParameter("@PeriodTo", model.PeriodTo),               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }



        private ChartOfAccountModel GetModel(IDataRecord dr)
        {
            return new ChartOfAccountModel
            {
                AccountDescription = dr.GetString("AccountDescription"),
                AccountName = dr.GetString("AccountName"),
                AccountCode = dr.GetString("AccountCode"),
                PeriodFrom = dr.GetDateTime("PeriodFrom"),
                PeriodTo = dr.GetDateTime("PeriodTo"),
                AccountType = dr.GetString("AccountType"),
                CoaID = dr.GetInt32("CoaID"),
                ParentID = dr.GetInt32("ParentID"),
                SummaryAccount = dr.GetBoolean("SummaryAccount"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<ChartOfAccountModel> FilterSubLedgers(string filterText)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "filter"),
               new SqlParameter("@filterText",filterText),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<CoaBranchAvailabilityModel> GetAllocatedBranches(int? CoaID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByAccount"),
               new SqlParameter("@CoaID", CoaID),               
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.BranchAvailability.Select]", parameters))
            {
                yield return new CoaBranchAvailabilityModel()
                {
                    ID = dr.GetInt32("ID"),
                    CoaID = dr.GetInt32("CoaID"),
                    BranchID = dr.GetInt32("BranchID"),
                    BranchName = dr.GetString("BranchName"),
                };
            }
        }

        public void AddBranch(CoaBranchAvailabilityModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@CoaID", model.CoaID),              
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

           Iservice.ExecuteNonQuery("[usp.Finance.Coa.BranchAvailability.Update]", parameters);
            
            
        }

        public void RemoveBranch(CoaBranchAvailabilityModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@CoaID", model.CoaID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            Iservice.ExecuteNonQuery("[usp.Finance.Coa.BranchAvailability.Delete]", parameters);
        }

        public IEnumerable<CoaBranchAvailabilityModel> GetSubledgersInBranch(int? BranchID, string filterText)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@filterText",filterText),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.BranchAvailability.Select]", parameters))
            {
                yield return new CoaBranchAvailabilityModel()
                {
                    ID = dr.GetInt32 ("ID"),
                    CoaID = dr.GetInt32("CoaID"),
                    BranchID = dr.GetInt32("BranchID"),
                    AccountName = dr.GetString("AccountName"),                    
                };
            }
        }


    }


}
