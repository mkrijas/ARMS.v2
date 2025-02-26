using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Core.BaseModels.Finance;

namespace ArmsServices.DataServices
{
    public class AccountInfoService : IAccountInfoService
    {
        IDbService Iservice;

        public AccountInfoService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to select account information by ID
        public AccountInfoViewModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MID", ID),
               new SqlParameter("@Operation", "Main"),
            };
            AccountInfoViewModel model = new AccountInfoViewModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.finance.Transactions.Main.Select]", parameters))
            {
                model = GetModel(dr,ID);
            }
            return model;
        }

        // Method to get entries associated with a specific account information IDc
        public IEnumerable<AccountInfoViewSubModel> Entries(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Sub"),
               new SqlParameter("@MID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.finance.Transactions.Main.Select]", parameters))
            {
                yield return new AccountInfoViewSubModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    AccountName =  dr.GetString("AccountName"),
                    BranchName = dr.GetString("BranchName"),
                    Reference = dr.GetString("EntryReference"),
                    CostCenter = dr.GetString("CostCenter"),
                    Dimension = dr.GetString("Dimension")
                };
            }
        }

        // Helper method to map data record to AccountInfoViewModel
        private AccountInfoViewModel GetModel(IDataRecord dr,int? MID)
        {
            return new AccountInfoViewModel()
            {               
               DocumentDate = dr.GetDateTime("DocDate"),
               DocumentNumber = dr.GetString("DocNumber"),
               Narration = dr.GetString("Narration"),
               Entries = Entries(MID).ToList()
            };
        }

        // Method to get TDS MID associated with a specific MID
        public int? GetTdsMID(int? MID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MID", MID),
               new SqlParameter("@Operation", "ByMID"),
            };
            
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.finance.Transactions.TdsLinkTable.Select]", parameters))
            {
                return dr.GetInt32("TdsMID");
            }
            return null;
        }
    }
}

