using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Data;
using System.Data.SqlClient;
namespace ArmsServices.DataServices
{
    public interface IDriverLicenceService
    {
      //  DriverModel Update(DriverModel model);
       // int Delete(int DriverID, string UserID);
       // IEnumerable<DriverModel> Select(int? PlaceID);
    }
    public class DriverLicenceService : IDriverLicenceService
    {
    }
}
