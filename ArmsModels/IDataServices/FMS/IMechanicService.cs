using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IMechanicService
    {
        MechanicModel Update(MechanicModel model);  //Edit
        MechanicModel SelectByID(int? ID);
        int Delete(int? MechanicID, string UserID);  //Delete
        IEnumerable<MechanicModel> Select();
        IEnumerable<MechanicModel> SelectByWorkshop(int? WorkshopID);
        int Transfer(MechanicTransferModel model);
    }
}