using ArmsModels.BaseModels;
using System.Collections.Generic;
using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ArmsServices.DataServices
{
    public interface IPaymentFinalizeService :IbaseInterface<PaymentFinishModel>
    {       
        IEnumerable<PaymentFinishModel> SelectByPeriod(int? BranchID, DateTime Begin, DateTime End);       
    }
}