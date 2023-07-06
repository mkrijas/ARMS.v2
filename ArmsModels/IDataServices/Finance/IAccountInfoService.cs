using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IAccountInfoService
    {  
        AccountInfoViewModel SelectByID(int? MID);
        IEnumerable<AccountInfoViewSubModel> Entries(int? MID);
    }
}

