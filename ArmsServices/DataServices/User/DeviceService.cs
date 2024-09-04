using ArmsModels.BaseModels;
using ArmsServices;
using Core.BaseModels.User;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Core.IDataServices.User;
using System.Threading.Tasks;

namespace DAL.DataServices.User
{
    public class DeviceService : IDeviceService
    {
        IDbService Iservice;
        public DeviceService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public IEnumerable<DeviceModel> Select(string operation)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", operation)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.User.DeviceDetails.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public async Task<int> Approve(DeviceModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "APPROVE"),
               new SqlParameter("@UserID", model.UserID),
               new SqlParameter("@DeviceID", model.DeviceID)
            };
            return await Iservice.ExecuteNonQueryAsync("[usp.User.DeviceDetails.ApproveDeny]", parameters);
        }

        public async Task<int> Deny(DeviceModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "DENY"),
               new SqlParameter("@UserID", model.UserID),
               new SqlParameter("@DeviceID", model.DeviceID)
            };
            return await Iservice.ExecuteNonQueryAsync("[usp.User.DeviceDetails.ApproveDeny]", parameters);
        }

        private DeviceModel GetModel(IDataRecord reader)
        {
            return new DeviceModel
            {
                ID = reader.GetInt32("ID"),
                UserID = reader.GetString("UserID"),
                DeviceID = reader.GetString("DeviceID"),
                TimeStamp = reader.GetDateTime("TimeStamp"),
                RecordStatus = reader.GetByte("RecordStatus"),
            };
        }
    }
}