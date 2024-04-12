using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using ArmsModels.BaseModels.Finance.Transactions;

namespace ArmsServices.DataServices
{
    public interface ISundryPaymentService : IbaseInterface<SundryPaymentModel>
    {        
        IEnumerable<SundryPaymentModel> Select();       
        IEnumerable<SundryPaymentEntryModel> GetEntries(int? SID);        
    }
}