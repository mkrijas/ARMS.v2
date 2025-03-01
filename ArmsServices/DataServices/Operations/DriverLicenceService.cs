using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Data;
using System.Data.SqlClient;
namespace ArmsServices.DataServices
{
    public class DriverLicenceService : IDriverLicenceService
    {
        IDbService Iservice;
        public DriverLicenceService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a driver license by its ID
        public int Delete(int? LicenceID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LicenceID", LicenceID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Licence.Delete]", parameters);
        }

        // Method to get the active heavy license for a driver
        public DriverLicenceModel GetActiveHeavyLicense(int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@Operation","ActiveHeavy")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Licence.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to save the file path of a driver's license
        public int SaveFilePath(string link, int? id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LINK",link),
               new SqlParameter("@ID", id)
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Licence.FilePath]", parameters);
        }

        // Method to select driver licenses by driver ID
        public IEnumerable<DriverLicenceModel> Select(int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@Operation","ByID")
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Licence.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a driver license by its ID
        public DriverLicenceModel SelectByID(int? LicenceID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LicenceID", LicenceID),
               new SqlParameter("@Operation","ByID")
            };

            DriverLicenceModel model = new DriverLicenceModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Licence.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to update a driver's license details
        public DriverLicenceModel Update(DriverLicenceModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", model.DriverID),
               new SqlParameter("@BadgeExpiryDate", model.BadgeExpiryDate),
               new SqlParameter("@BadgeNo", model.BadgeNo),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DLExpiryDate", model.DLExpiryDate),
               new SqlParameter("@DLImage", model.DLImage),
               new SqlParameter("@LicenceID", model.LicenceID),
               new SqlParameter("@LicenceNo", model.LicenceNo),
               new SqlParameter("@LicenceType", model.LicenceType),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Licence.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to DriverLicenceModel
        private DriverLicenceModel GetModel(IDataRecord dr)
        {
            return new DriverLicenceModel
            {
                LicenceID = dr.GetInt32("LicenceID"),
                LicenceNo = dr.GetString("LicenceNo"),
                BadgeNo = dr.GetString("BadgeNo"),
                DLExpiryDate = dr.GetDateTime("DLExpiryDate"),
                BadgeExpiryDate = dr.GetDateTime("BadgeExpiryDate"),
                LicenceType = dr.GetString("LicenceType"),
                DLImage = dr.GetString("DLImage"),
                DriverID = dr.GetInt32("DriverID"),
                BranchID = dr.GetInt32("BranchID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }
    }
}
