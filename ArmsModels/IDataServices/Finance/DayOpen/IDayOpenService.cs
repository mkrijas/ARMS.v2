using Core.BaseModels.Finance;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace Core.IDataServices.Finance.DayOpen
{
    public interface IDayOpenService
    {
        IEnumerable<DayOpenRequestModel> Select(int? NoOfRecords, int? BranchId);
        DayOpenRequestModel Update(DayOpenRequestModel model);
        DayOpenRequestModel Approve(DayOpenRequestModel model);
        DayOpenRequestModel RejectOrClose(DayOpenRequestModel model);
        bool? ValidateDayOpen(DateTime? DocDate, int? DocTypeID, int? BranchID);
    }
}