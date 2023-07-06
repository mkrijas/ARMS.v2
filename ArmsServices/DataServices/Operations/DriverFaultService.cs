using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Data;
using System.Data.SqlClient;
namespace ArmsServices.DataServices
{
    public class DriverFaultService : IDriverFaultService
    {
        IDbService Iservice;
        public DriverFaultService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? FaultID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@FaultID", FaultID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Fault.Delete]", parameters);
        }


        public IEnumerable<DriverFaultModel> Select(int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Fault.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public DriverFaultModel SelectByID(int? FaultID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@FaultID", FaultID)
            };

            DriverFaultModel model = new DriverFaultModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Fault.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public DriverFaultModel Update(DriverFaultModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {

               new SqlParameter("@DriverID", model.DriverID),
               new SqlParameter("@FaultID", model.FaultID),
               new SqlParameter("@FaultDate", model.FaultDate),
               new SqlParameter("@Severity", model.Severity),
               new SqlParameter("@Detail", model.Detail),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Fault.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private DriverFaultModel GetModel(IDataRecord dr)
        {
            return new DriverFaultModel
            {
                FaultID = dr.GetInt32("FaultID"),
                FaultDate = dr.GetDateTime("FaultDate"),
                Severity = dr.GetByte("Severity"),
                Detail = dr.GetString("Detail"),
                BranchID = dr.GetInt32("BranchID"),
                DriverID = dr.GetInt32("DriverID"),
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
