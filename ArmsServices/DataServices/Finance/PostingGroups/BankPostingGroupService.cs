using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IBankPostingGroupService
    {
        BankPostingGroupModel Update(BankPostingGroupModel model);
        BankPostingGroupModel SelectByID(int? ID);
        BankPostingGroupModel SelectByBank(int? BankID);
        int Delete(int? ID, string UserID);        
        IEnumerable<BankPostingGroupModel> Select();        
    }

    public class BankPostingGroupService : IBankPostingGroupService
    {
        IDbService Iservice;

        public BankPostingGroupService(IDbService iservice)
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
            return Iservice.ExecuteNonQuery("[usp.Finance.PostingGroup.Bank.Delete]", parameters);
        }


        public IEnumerable<BankPostingGroupModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Bank.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public BankPostingGroupModel SelectByBank(int? BankID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", BankID),
               new SqlParameter("@Operation", "ByBank"),
            };
            BankPostingGroupModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Bank.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public BankPostingGroupModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            BankPostingGroupModel model = new ();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Bank.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public BankPostingGroupModel Update(BankPostingGroupModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@BankAccount", model.BankAccount.CoaID),
               new SqlParameter("@BankCharges", model.BankCharges.CoaID),
               new SqlParameter("@ProcessingFee", model.ProcessingFee.CoaID),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.PostingGroup.Bank.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private BankPostingGroupModel GetModel(IDataRecord dr)
        {
            return new BankPostingGroupModel
            {
                ID = dr.GetInt32("ID"),
                Title = dr.GetString("Title"),
                BankAccount = new ChartOfAccountModel()
                {
                    CoaID  = dr.GetInt32("BankAccount"),
                    AccountName = dr.GetString("BankAccountCoa"),
                },
                ProcessingFee = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("ProcessingFee"),
                    AccountName = dr.GetString("ProcessingFeeCoa"),
                },
                BankCharges = new ChartOfAccountModel()
                {
                    CoaID = dr.GetInt32("BankCharges"),
                    AccountName = dr.GetString("BankChargesCoa"),
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