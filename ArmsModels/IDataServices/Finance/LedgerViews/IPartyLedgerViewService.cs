using Core.BaseModels.Finance.LedgerViews;
using System.Collections.Generic;
using System;

namespace Core.IDataServices.Finance.LedgerViews
{
    public interface IPartyLedgerViewService
    {
        IEnumerable<PartyLedgerViewModel> SelectByPartyIDAndDate(int? PartyID, DateTime? FromDate, DateTime? ToDate);
    }
}
