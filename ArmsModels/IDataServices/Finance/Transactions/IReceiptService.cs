using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
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
        public PagedResult<ReceiptModel> SelectAll(int? BranchID, int page, int pageSize, string search, bool _IsApproved, bool IsInterBranch = false);
    }
}