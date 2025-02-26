using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class CustomerPostingGroupService : ICustomerPostingGroupService
    {
        IDbService Iservice;

        public CustomerPostingGroupService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Retrieves a Customer Posting Group by CustomerID
        public CustomerPostingGroupModel GetPostingGroup(int? CustomerID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", CustomerID),
               new SqlParameter("@Operation", "ByParty"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Customer.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Deletes a Customer Posting Group based on ID and UserID
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.PostingGroup.Customer.Delete]", parameters);
        }

        // Retrieves all Customer Posting Groups
        public IEnumerable<CustomerPostingGroupModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Customer.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Retrieves a specific Customer Posting Group by ID
        public CustomerPostingGroupModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            CustomerPostingGroupModel model = new ();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Customer.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Updates an existing Customer Posting Group
        public CustomerPostingGroupModel Update(CustomerPostingGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.CustomerPostingGroupID),
               new SqlParameter("@Recievable", model.Receivable.CoaID),
               new SqlParameter("@PrePayment", model.PrePayment.CoaID),
               new SqlParameter("@Deposit", model.Deposit.CoaID),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Customer.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Maps an IDataRecord to a CustomerPostingGroupModel
        private CustomerPostingGroupModel GetModel(IDataRecord dr)
        {
            return new CustomerPostingGroupModel
            {
                CustomerPostingGroupID = dr.GetInt32("ID"),
                Title = dr.GetString("Title"),
                Receivable = new ChartOfAccountModel()
                {
                    CoaID  = dr.GetInt32("Recievable"),
                    ParentID = dr.GetInt32("RecievableParentID"),
                    AccountCode = dr.GetString("RecievableCoaCode"),
                    AccountName = dr.GetString("RecievableCoa"),
                },
                PrePayment = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("PrePayment"),
                    ParentID = dr.GetInt32("PrePaymentParentID"),
                    AccountCode = dr.GetString("PrePaymentCoaCode"),
                    AccountName = dr.GetString("PrePaymentCoa"),
                },
                Deposit = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Deposit"),
                    ParentID = dr.GetInt32("DepositParentID"),
                    AccountCode = dr.GetString("DepositCoaCode"),
                    AccountName = dr.GetString("DepositCoa"),
                },
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