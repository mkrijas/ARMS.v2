using Core.BaseModels.Finance;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace Core.IDataServices.Finance.DayOpen
{
    public interface IDayOpenService
    {
        IEnumerable<DayOpenRequestModel> Select(int? NoOfRecords, int? BranchId);  // View
        DayOpenRequestModel Update(DayOpenRequestModel model);  // Edit
        DayOpenRequestModel Approve(DayOpenRequestModel model);  // Approve
        DayOpenRequestModel RejectOrClose(DayOpenRequestModel model);  // Reject OR Close
        bool? ValidateDayOpen(DateTime? DocDate, int? DocTypeID, int? BranchID, string UserID);
    }
}