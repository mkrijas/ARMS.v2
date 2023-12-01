using Core.BaseModels.Finance.LedgerViews;
using System.Collections.Generic;
using System;

namespace Core.IDataServices.Finance.LedgerViews
{
    public interface IAssetLedgerViewService
    {
        IEnumerable<LedgerViewsModel> SelectByAssetIDAndDate(int? AssetID, DateTime? FromDate, DateTime? ToDate);
    }
}
