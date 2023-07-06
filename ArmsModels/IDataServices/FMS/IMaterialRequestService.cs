using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IMaterialRequestService
    {
        MaterialRequestModel Update(MaterialRequestModel model);
        MaterialRequestModel SelectByID(int? ID);
        int Delete(int? MaterialRequestID, string UserID);
        IEnumerable<MaterialRequestModel> Select(int? MaterialRequestID);
    }
}