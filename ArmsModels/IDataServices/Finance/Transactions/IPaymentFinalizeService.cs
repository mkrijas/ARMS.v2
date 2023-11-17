using ArmsModels.BaseModels;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using System.Data;

namespace ArmsServices.DataServices
{
    public interface IPaymentFinalizeService
    {
        int Approve(int? PID, string UserID, string Remarks);  //Approve
        IEnumerable<PaymentFinishModel> Select(int? BranchID, int? FinalizeID, int? NumberOfRecords, string searchTerm);
        IEnumerable<PaymentFinishModel> SelectByPeriod(int? BranchID, DateTime Begin, DateTime End);
        int? Update(PaymentFinishModel model);  //Edit
    }
}