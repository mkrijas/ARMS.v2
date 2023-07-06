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
        IEnumerable<DriverLicenceModel> Select(int? DriverID);
        DriverLicenceModel GetActiveHeavyLicense(int? DriverID);
        DriverLicenceModel SelectByID(int? LicenceID);
        DriverLicenceModel Update(DriverLicenceModel model);
        int Delete(int? LicenceID, string UserID);
        int SaveFilePath(string link, int? id);
    }
}