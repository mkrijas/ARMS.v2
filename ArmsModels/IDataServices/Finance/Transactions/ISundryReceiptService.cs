using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface ISundryReceiptService : IbaseInterface<SundryReceiptModel>
    {        
        IEnumerable<SundryReceiptModel> Select();       
        IEnumerable<SundryReceiptEntryModel> GetEntries(int? SID);       
    }
}