using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Data;
using System.Data.SqlClient;
namespace ArmsServices.DataServices
{
    public interface IDriverLeaveService
    {
        IEnumerable<DriverLeaveModel> Select(int? DriverID);
        DriverLeaveModel SelectByID(int? LeaveID);
        DriverLeaveModel Update(DriverLeaveModel model);
        int Delete(int? LeaveID, string UserID);
    }
}