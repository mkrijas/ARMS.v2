using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class TdsEntryService : ItdsEntryService
    {
        public int Approve(int? ID, string UserID, string Remarks)
        {
            throw new NotImplementedException();
        }

        public int Delete(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TdsTransactionEntryModel> GetEntries(int? ID)
        {
            throw new NotImplementedException();
        }

        public int RemoveFile(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }

        public int Reverse(int? ID, string UserID, string Remarks)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TdsTransactionModel> Select()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TdsTransactionModel> Select(int? BranchID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TdsTransactionModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public TdsTransactionModel SelectByID(int? ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TdsTransactionModel> SelectByParty(int? PartyID, int? PartyBranchID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TdsTransactionModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TdsTransactionModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public TdsTransactionModel Update(TdsTransactionModel model)
        {
            throw new NotImplementedException();
        }
    }
}
