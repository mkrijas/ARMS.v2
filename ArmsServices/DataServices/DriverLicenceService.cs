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
        IEnumerable<DriverLicenceModel> Select(int DriverID);        
        DriverLicenceModel SelectByID(int LicenceID);
        DriverLicenceModel Update(DriverLicenceModel model);
        int Delete(int LicenceID,string UserID);

    }
    public class DriverLicenceService : IDriverLicenceService
    {
        IDbService Iservice;
        public DriverLicenceService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int LicenceID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LicenceID", LicenceID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Licence.Delete]", parameters);
        }

        public IEnumerable<DriverLicenceModel> Select(int DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Licence.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public DriverLicenceModel SelectByID(int LicenceID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LicenceID", LicenceID)
            };

            DriverLicenceModel model = new DriverLicenceModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Licence.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

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
