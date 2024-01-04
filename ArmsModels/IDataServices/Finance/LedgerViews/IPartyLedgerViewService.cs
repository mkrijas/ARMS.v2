using Core.BaseModels.Finance.LedgerViews;
using System.Collections.Generic;
using System;
using ArmsModels.BaseModels;

namespace Core.IDataServices.Finance.LedgerViews
{
    public interface IPartyLedgerViewService
    {
        IEnumerable<LedgerViewsModel> SelectByPartyIDAndDate(PartyModel Party, int? BranchID, DateTime? FromDate, DateTime? ToDate);
    }
}