
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
    public interface ItdsEntryService : IbaseInterface<TdsTransactionModel>
    {
        IEnumerable<TdsTransactionModel> Select();
        IEnumerable<TdsTransactionModel> SelectByParty(int? PartyID, int? PartyBranchID);
        IEnumerable<TdsTransactionModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<TdsTransactionEntryModel> GetEntries(int? ID);
    }
}