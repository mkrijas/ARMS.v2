using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ICustomerPostingGroupService
    {
        CustomerPostingGroupModel Update(CustomerPostingGroupModel model);
        CustomerPostingGroupModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);        
        IEnumerable<CustomerPostingGroupModel> Select();        
    }

    public class CustomerPostingGroupService : ICustomerPostingGroupService
    {
        IDbService Iservice;

        public CustomerPostingGroupService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CustomerPostingGroupID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.PostingGroup.Customer.Delete]", parameters);
        }


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

        public CustomerPostingGroupModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CustomerPostingGroupID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            CustomerPostingGroupModel model = new ();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Customer.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public CustomerPostingGroupModel Update(CustomerPostingGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CustomerPostingGroupID", model.CustomerPostingGroupID),
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

        private CustomerPostingGroupModel GetModel(IDataRecord dr)
        {
            return new CustomerPostingGroupModel
            {
                CustomerPostingGroupID = dr.GetInt32("CustomerPostingGroupID"),
                Title = dr.GetString("Title"),
                Receivable = new ChartOfAccountModel()
                {
                    CoaID  = dr.GetInt32("Receivable"),
                    AccountName = dr.GetString("ReceivableCoa"),
                },
                PrePayment = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("PrePayment"),
                    AccountName = dr.GetString("PrePaymentCoa"),
                },
                Deposit = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("Deposit"),
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