
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITdsAccountMappingService
    {
        TdsAccountMappingModel Update(TdsAccountMappingModel model);
        TdsAccountMappingModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<TdsAccountMappingModel> Select();
        IEnumerable<TdsAccountMappingModel> SelectByNP(int TdsNPID);
        IEnumerable<TdsAccountMappingModel> SelectByAccount(int CoaID);
        TdsAccountMappingModel CheckExist(int? CoaID);

    }
}