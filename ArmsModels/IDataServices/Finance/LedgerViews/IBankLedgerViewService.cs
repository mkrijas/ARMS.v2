using Core.BaseModels.Finance.LedgerViews;
using System.Collections.Generic;
using System;

namespace Core.IDataServices.Finance.LedgerViews
{
    public interface IBankLedgerViewService
    {
        IEnumerable<LedgerViewsModel> SelectByBankIDAndDate(int? BankID, DateTime? FromDate, DateTime? ToDate);
    }
}
