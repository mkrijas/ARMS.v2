using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IAccountInfoService
    {  
        AccountInfoViewModel SelectByID(int? MID);        
    }

    public class AccountInfoService : IAccountInfoService
    {
        IDbService Iservice;

        public AccountInfoService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public AccountInfoViewModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MID", ID),
               new SqlParameter("@Operation", "Main"),
            };
            AccountInfoViewModel model = new AccountInfoViewModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transaction.Main.Select]", parameters))
            {
                model = GetModel(dr,ID);
            }
            return model;
        }


        public IEnumerable<AccountInfoViewSubModel> Entries(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Sub"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transaction.Main.Select]", parameters))
            {
                yield return new AccountInfoViewSubModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    AccountName =  dr.GetString("AccountName"),
                    BranchName = dr.GetString("BranchName"),
                    Reference = dr.GetString("EntryReference")
                };
            }
        }

        private AccountInfoViewModel GetModel(IDataRecord dr,int? MID)
        {
            return new AccountInfoViewModel()
            {
               CostCenter = dr.GetString("CostCenter"),
               Dimension = dr.GetString("Dimension"),
               DocumentDate = dr.GetDateTime("DocumentDate"),
               DocumentNumber = dr.GetString("DocumentNumber"),
               Narration = dr.GetString("Narration"),
               Entries = Entries(MID).ToList()
            };
        }
   
       
    }
}

