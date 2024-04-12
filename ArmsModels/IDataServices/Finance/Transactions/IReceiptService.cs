using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IReceiptService : IbaseInterface<ReceiptModel>
    {  
        IEnumerable<ReceiptModel> SelectByParty(int? PartyID, int? PartyBranchID, int? BranchID);
        IEnumerable<ReceiptModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID);
        IEnumerable<BillsReceiptModel> GetBills(int? ReceiptID);          
    }
}