using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;
using System.Collections;

namespace ArmsServices.DataServices
{
    public class ChartOfAccountService : IChartOfAccountService
    {
        IDbService Iservice;

        public ChartOfAccountService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a chart of account entry
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TdsRateID", ID),
               new SqlParameter("@UserID", UserID),
            };

            return Iservice.ExecuteNonQuery("[usp.Finance.Coa.Delete]", parameters);
        }

        // Method to select chart of accounts by group
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

        // Method to select children of a chart of account
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

        // List to hold all chart of accounts
        public List<ChartOfAccountModel> CoaAllList { get; set; } = new();

        // Method to select all children and their sub-children for a given chart of account
        public List<ChartOfAccountModel> SelectAllChildrenAndItsSub(int? CoaID, string searchString)
        {
            ClearCoaList();
            if (searchString != null)
            {
                searchString = searchString.Trim();
            }
            var result = SelectAllChildrenAndItsSubChildren(CoaID);
            if (result.Any(d => string.IsNullOrWhiteSpace(searchString) || d.AccountName != null && d.AccountName.ToLower().Trim().Contains(searchString.ToLower().Trim())))
            {
                return result.Where(d => string.IsNullOrWhiteSpace(searchString) || d.AccountName != null && d.AccountName.ToLower().Trim().Contains(searchString.ToLower().Trim())).ToList();
            }
            else
            {
                return new List<ChartOfAccountModel>();
            }
        }

        // Method to clear the chart of accounts list
        public void ClearCoaList()
        {
            CoaAllList.Clear();
        }

        // Method to select all children and their sub-children for a given chart of account
        public List<ChartOfAccountModel> SelectAllChildrenAndItsSubChildren(int? CoaID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "children"),
               new SqlParameter("@CoaID",CoaID),
            };

            ChartOfAccountModel model = new ChartOfAccountModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.Select]", parameters))
            {
                model = GetModel(dr);
                if (model.SummaryAccount)
                {
                    SelectAllChildrenAndItsSubChildren(model.CoaID);
                }
                else
                {
                    CoaAllList.Add(model);
                }
            }
            return CoaAllList;
        }

        // Method to select base chart of accounts
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

        // Method to select a chart of account by its ID
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

        // Method to update a chart of account
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
               new SqlParameter("@LimitToPeriod",model.LimitToPeriod),
               new SqlParameter("@PeriodFrom", model.PeriodFrom),
               new SqlParameter("@PeriodTo", model.PeriodTo),
               new SqlParameter("@IsDimensionMandatory", model.IsDimensionMandatory),
               new SqlParameter("@IsCostCenterMandatory", model.IsCostCenterMandatory),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to ChartOfAccountModel
        private ChartOfAccountModel GetModel(IDataRecord dr)
        {
            return new ChartOfAccountModel
            {
                AccountDescription = dr.GetString("AccountDescription"),
                AccountName = dr.GetString("AccountName"),
                AccountCode = dr.GetString("AccountCode"),
                LimitToPeriod = dr.GetBoolean("LimitToPeriod"),
                PeriodFrom = dr.GetDateTime("PeriodFrom"),
                PeriodTo = dr.GetDateTime("PeriodTo"),
                AccountType = dr.GetString("AccountType"),
                CoaID = dr.GetInt32("CoaID"),
                ParentID = dr.GetInt32("ParentID"),
                SummaryAccount = dr.GetBoolean("SummaryAccount"),
                IsCostCenterMandatory = dr.GetBoolean("IsCostCenterMandatory"),
                IsDimensionMandatory = dr.GetBoolean("IsDimensionMandatory"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Method to filter subledgers based on a filter text
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

        // Method to get all ledgers
        public IEnumerable<ChartOfAccountModel> AllLedgers()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "All"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to get all groups of accounts
        public IEnumerable<ChartOfAccountModel> AllGroups()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "allgroups"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to get allocated branches for a specific chart of account
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

        // Method to check if cost center is mandatory for a given chart of account ID
        public bool? IsCostCenterIsMadatoryForGivenCoaID(int? CoaID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CoaID", CoaID),
            };
            bool? result = false;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.COA.CostCentor.Manadatory]", parameters))
            {
                result = dr.GetBoolean("Result");
                       
            }
            return result;
        }

        // Method to check if dimension is mandatory for a given chart of account ID
        public bool? IsDimensionIsMadatoryForGivenCoaID(int? CoaID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CoaID", CoaID),
            };
            bool? result = false;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.COA.Dimension.Manadatory]", parameters))
            {
                result = dr.GetBoolean("Result");
                       
            }
            return result;
        }

        // Method to select all accounts for a given chart of account ID
        public void SelectAll(int? CoaID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation","CHECKALL"),
               new SqlParameter("@CoaID", CoaID),
               new SqlParameter("@UserID", UserID),
            };

            Iservice.ExecuteNonQuery("[usp.finance.COA.BranchAvailability.Update]", parameters);
        }

        // Method to unselect all accounts for a given chart of account ID
        public void UnSelectAll(int? CoaID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation","UNCHECKALL"),
               new SqlParameter("@CoaID", CoaID),
               new SqlParameter("@UserID", UserID),
            };

            Iservice.ExecuteNonQuery("[usp.finance.COA.BranchAvailability.Update]", parameters);
        }

        // Method to add a branch to a chart of account
        public void AddBranch(CoaBranchAvailabilityModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation","CHECK"),
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@CoaID", model.CoaID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            Iservice.ExecuteNonQuery("[usp.finance.COA.BranchAvailability.Update]", parameters);
        }

        // Method to remove a branch from a chart of account
        public void RemoveBranch(CoaBranchAvailabilityModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation","UNCHECK"),
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@CoaID", model.CoaID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            Iservice.ExecuteNonQuery("[usp.finance.COA.BranchAvailability.Update]", parameters);
        }

        // Method to get subledgers in a specific branch
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
                    ID = dr.GetInt32("ID"),
                    CoaID = dr.GetInt32("CoaID"),
                    BranchID = dr.GetInt32("BranchID"),
                    AccountName = dr.GetString("AccountName"),
                };
            }
        }

        // Method to get payment codes for a specific branch and payment mode
        public IEnumerable<PaymentCodeModel> GetPaymentCodes(int? BranchID, string PaymentMode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByMode"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@PaymentMode",PaymentMode),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.PaymentCodes.Select]", parameters))
            {
                yield return new PaymentCodeModel()
                {
                    ID = dr.GetInt32("ID"),
                    CoaID = dr.GetInt32("CoaID"),
                    ArdCode = dr.GetString("ArdCode"),
                    Title = dr.GetString("Title"),
                    paymentMode = PaymentMode,
                };
            }
        }

        // Method to get the balance of a specific cash account
        public decimal? GetBalance( int? BranchID,int? CoaID, string ArdCode, string SubARdCode, DateTime _date)
        {

            string Query = "SELECT dbo.GetBalance(@BranchID,@CoaID,@ArdCode,@SubArdCode,@Date)";
            var _tomorrow = _date.AddDays(1);

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID ),
               new SqlParameter("@CoaID", CoaID ?? (object)DBNull.Value),
               new SqlParameter("@ArdCode", ArdCode ?? (object)DBNull.Value),
               new SqlParameter("@SubARdCode",SubARdCode ??(object) DBNull.Value),
               new SqlParameter("@Date", _tomorrow),
            };

            foreach (IDataRecord dr in Iservice.QuerySql(Query, parameters))
            {
                return (decimal)dr.GetValue(0);                
            }
            return null;

        }

        // Method to get sub-ard codes
        public IEnumerable<SubArdCodeModel> GetSubArdCodes()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "SubArdCodes"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Coa.Select]", parameters))
            {
                yield return new SubArdCodeModel()
                {
                    SubArdCode = dr.GetString("SubArdCode"),
                    ArdGroup = dr.GetString("ArdGroup"),
                    TranType = dr.GetString("TranType"),
                };
            }
        }
    }
}