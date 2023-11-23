using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;

namespace ArmsServices.DataServices
{
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

        public List<ChartOfAccountModel> CoaAllList { get; set; } = new();

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

        public void ClearCoaList()
        {
            CoaAllList.Clear();
        }

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
               new SqlParameter("@AccountName", model.AccountName.ToUpper()),
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
    }
}