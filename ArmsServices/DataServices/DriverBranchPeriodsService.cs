using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Data;
using System.Data.SqlClient;
namespace ArmsServices.DataServices
{
    public interface IDriverBranchPeriodsService
    {
       // DriverBranchPeriods Update(DriverBranchPeriods model);
       // int Delete(int DriverID, string UserID);
       // IEnumerable<DriverBranchPeriods> Select(int? PlaceID);
    }
    public class DriverBranchPeriodsService : IDriverBranchPeriodsService
    {
        IDbService Iservice;

        public DriverBranchPeriodsService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public DriverBranchPeriods Update(DriverBranchPeriods model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
    

               new SqlParameter("@DriverPeriodID", model.DriverPeriodID),
               new SqlParameter("@DriverID", model.DriverID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@StartDate", model.StartDate),
               new SqlParameter("@EndDate", model.EndDate),
               new SqlParameter("@Resignation", model.Resignation),
               new SqlParameter("@Remarks", model.Remarks),
               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            DriverBranchPeriods driverbranch = new DriverBranchPeriods();
            using (var reader = Iservice.GetDataReader("[usp.Driver.Branch.PeriodsUpdate]", parameters))
            {
                while (reader.Read())
                {
                    driverbranch = new DriverBranchPeriods
                    {
                        DriverPeriodID = reader.GetInt32("DriverPeriodID"),
                        DriverID = reader.GetInt32("DriverID"),
                        BranchID = (short)reader.GetInt32("BranchID"),
                        StartDate = reader.GetDateTime("StartDate"),
                        EndDate = reader.GetDateTime("EndDate"),
                        Resignation =reader.GetBoolean("Resignation"),
                        Remarks = reader.SafeGetString("Remarks"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return driverbranch;

        }
        public int Delete(int DriverPeriodID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverPeriodID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.DriversDelete]", parameters);
        }

        public IEnumerable<DriverModel> Select(int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID)
            };

            using (var reader = Iservice.GetDataReader("[usp.Driver.DriversSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new DriverModel
                    {
                        DriverName = reader.SafeGetString("DriverName"),
                        DriverImage = reader.SafeGetString("DriverImage"),
                        DateOfBirth = reader.GetDateTime("DateOfBirth"),
                        AdhaarNo = reader.SafeGetString("AdhaarNo"),
                        Mobile = reader.SafeGetString("Mobile"),
                        AddressID = reader.GetInt32("AddressID"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
        }
    }
}
