using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface ICreditTransferService : IbaseInterface<CreditTransferModel>
    {          
        IEnumerable<CreditTransferModel> SelectByPeriod(DateTime? begin, DateTime? end, int? BranchID);             
    }
}