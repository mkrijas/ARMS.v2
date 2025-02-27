using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class PeriodicMaintenanceService : IPeriodicMaintenanceService
    {
        IDbService Iservice;

        public PeriodicMaintenanceService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to conclude a periodic maintenance entry
        public PeriodicMaintenanceConcludeModel Conclude(PeriodicMaintenanceConcludeModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PMIID", model.PMIID),
               new SqlParameter("@PMCID", model.PMCID),
               new SqlParameter("@Date", model.Date),
               new SqlParameter("@Audometer", model.Audometer),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.PeriodicMaintenanceConclude.Update]", parameters))
            {
                model.PMIID = dr.GetInt32("PMIID");
            }
            return model;
        }

        // Method to delete a periodic maintenance entry by its ID
        public int Delete(int? PMIID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PMIID", PMIID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.FMS.PeriodicMaintenanceInitiate.Delete]", parameters);
        }

        // Method to select periodic maintenance entries by their ID
        public IEnumerable<PeriodicMaintenanceInitiateModel> Select(int? PMIID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PMIID", PMIID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.PeriodicMaintenanceInitiate.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select periodic maintenance entries by truck ID
        public IEnumerable<PeriodicMaintenanceInitiateModel> SelectByTruck(int? TruckID, int? RecordStatus)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@RecordStatus", RecordStatus),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.PeriodicMaintenanceInitiate.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a periodic maintenance entry by its ID
        public PeriodicMaintenanceInitiateModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PMIID", ID),
            };
            PeriodicMaintenanceInitiateModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.PeriodicMaintenanceInitiate.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to update a periodic maintenance entr
        public PeriodicMaintenanceInitiateModel Update(PeriodicMaintenanceInitiateModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PMIID", model.PMIID),
               new SqlParameter("@NAudometer", model.NAudometer),
               new SqlParameter("@NDate", model.NDate),
               new SqlParameter("@NotificationID", model.NotificationID),
               new SqlParameter("@Remarks", model.Remarks),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.FMS.PeriodicMaintenanceInitiate.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to PeriodicMaintenanceInitiateModel
        private PeriodicMaintenanceInitiateModel GetModel(IDataRecord dr)
        {
            return new PeriodicMaintenanceInitiateModel
            {
                PMIID = dr.GetInt32("PMIID"),
                NAudometer = dr.GetInt64("NAudometer"),
                NDate = dr.GetDateTime("NDate"),
                NotificationID = dr.GetInt32("NotificationID"),
                Remarks = dr.GetString("Remarks"),
                Title = dr.GetString("Title"),
                TruckID = dr.GetInt32("TruckID"),
                JobCardID = dr.GetInt32("JobCardID"),
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

