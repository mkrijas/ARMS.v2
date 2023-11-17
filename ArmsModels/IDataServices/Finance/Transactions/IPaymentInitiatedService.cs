using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace ArmsServices.DataServices
{
    public interface IPaymentInitiatedService
    {
        IEnumerable<PaymentInitiatedModel> PendingForCompletion(int? BranchID, string searchTerm);
        int? Update(PaymentInitiatedModel model);  //Initiate
        int? Reverse(int? ID, string UserID);  //Reverse
        IEnumerable<PaymentInitiatedModel> Select(int? BranchID);   
        IEnumerable<PaymentInitiatedModel> SelectInitiatedBetween(int? BranchID, DateTime Begin, DateTime End);
    }
}