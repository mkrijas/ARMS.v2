using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices
{
    public interface IDriverService
    {
        DriverModel Update(DriverModel model);
        int Delete(int? DriverID, string UserID);
        IEnumerable<DriverModel> Select();
        IEnumerable<DriverModel> SelectByBranch(int BranchID);
        DriverModel SelectByID(int? DriverID);
        int UpdateBranch(int? DriverID, int? BranchID, bool availStatus, string userID);
        IEnumerable<int> GetAssignedBranches(int? DriverID);
        DriverModel FindDriver(DriverModel model = null, DriverLicenceModel licence = null);
        int AvailabilityStatus(int? DriverID);
        int Join(int? DriverID, int? BranchID, DateTime? StartDate, string UserID);
        int Resign(int? DriverID, string Remarks, string userID);
        DriverLeaveModel GetLastLeave(int? DriverID);
        int BeginLeave(DriverLeaveModel LeaveModel);
        int EndLeave(int? DriverID,string UserID);
        public int? GetAssignedTruck(int? DriverID);
        IEnumerable<DriverModel> GetDriverByAdhaarNo(string AdhaarNo);


    }

    public class DriverService : IDriverService
    {
        IDbService Iservice;
        IAddressService _addressService;
        public DriverService(IDbService iservice,IAddressService Iaddress)
        {
            Iservice = iservice;
            _addressService = Iaddress;
        }
        public DriverModel Update(DriverModel model)
        {
            model.Address =  _addressService.Update(model.Address);
            List<SqlParameter> parameters = new List<SqlParameter>
            {    
               new SqlParameter("@DriverID", model.DriverID),
               new SqlParameter("@DriverName", model.DriverName),
               new SqlParameter("@HomeBranchID", model.HomeBranchID),               
               new SqlParameter("@DriverImage", model.DriverImage),
               new SqlParameter("@DateOfBirth", model.DateOfBirth),
               new SqlParameter("@AdhaarNo", model.AdhaarNo),
               new SqlParameter("@AdhaarImage", model.AdhaarImage),  
               new SqlParameter("@AddressID", model.Address.AddressID),
               new SqlParameter("@AdditionalInfo", model.AdditionalInfo),
               new SqlParameter("@Mobile", model.Mobile),
               new SqlParameter("@Email", model.Email),
               new SqlParameter("@DriverAgentID", model.DriverAgentID),
               new SqlParameter("@FestivalBonus", model.FestivalBonus),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            
            foreach(IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Driver.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;              
        }
        public int Delete(int? DriverID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Driver.Delete]", parameters);
        }
        public IEnumerable<DriverModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", 0),
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Driver.Select]", parameters))
            {               
                    yield return GetModel(dr);               
            }
        }

        public IEnumerable<DriverModel> SelectByBranch(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
                new SqlParameter("@Operation", "ByBranch"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Driver.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public DriverModel SelectByID(int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@Operation", "ByID"),
            };

            DriverModel model = new DriverModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Driver.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private DriverModel GetModel(IDataRecord reader)
        {
            return new DriverModel
            {
                DriverName = reader.GetString("DriverName"),                
                DriverAgentID = reader.GetInt32("DriverAgentID"),
                DriverAgent = new PartyModel() {PartyID = reader.GetInt32("DriverAgentID"), TradeName = reader.GetString("PartyName") },
                HomeBranchID = reader.GetInt32("HomeBranchID"),
                DriverImage = reader.GetString("DriverImage"),
                DateOfBirth = reader.GetDateTime("DateOfBirth"),
                AdhaarNo = reader.GetString("AdhaarNo"),
                AdhaarImage = reader.GetString("AdhaarImage"),                          
                FestivalBonus = reader.GetString("FestivalBonus"),
                DriverID = reader.GetInt32("DriverID"),
                AdditionalInfo =  reader.GetString("AdditionalInfo"),
                Mobile =  reader.GetString("Mobile"),
                Email =  reader.GetString("Email"),
                AddressID = reader.GetInt32("AddressID"),
                HasValidLicense = reader.GetBoolean("HasValidLicense"),
                TruckID = reader.GetInt32("TruckID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = reader.GetByte("RecordStatus"),
                    TimeStampField = reader.GetDateTime("TimeStamp"),
                    UserID = reader.GetString("UserID"),
                },
            };
        }

        public int UpdateBranch(int? DriverID, int? BranchID, bool availStatus, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@availStatus", availStatus),
               new SqlParameter("@UserID", UserID)
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Branch.Availability]", parameters);
        }

        IEnumerable<int> IDriverService.GetAssignedBranches(int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Branch.Availability]", parameters))
            {
                yield return dr.GetInt32("BranchID").GetValueOrDefault();
            }
        }

        public DriverModel FindDriver(DriverModel model = null,DriverLicenceModel licence = null)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@DriverName", model?.DriverName), 
               new SqlParameter("@AdhaarNo", model?.AdhaarNo),               
               new SqlParameter("@LicenceNo", licence?.LicenceNo),
               new SqlParameter("@BadgeNo", licence?.BadgeNo),              
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Driver.Find]", parameters))
            {                
                model = GetModel(dr);
            }
            return model;
        }

        public int AvailabilityStatus(int? DriverID)
        {
            throw new NotImplementedException();
        }

        public int Resign(int? DriverID, string Remarks, string userID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@EndDate", DateTime.Today),
               new SqlParameter("@UserID", userID),
               new SqlParameter("@Remarks", Remarks),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.WorkPeriods.Resign]", parameters);
        }

        public DriverLeaveModel GetLastLeave(int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),              
            };

            DriverLeaveModel model = null;

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Leave.SelectLast]", parameters))
            {
                model = new DriverLeaveModel()
                {
                    BranchID = dr.GetInt32("BranchID"),
                    DriverID = dr.GetInt32("DriverID"),
                    LeaveID = dr.GetInt32("LeaveID"),
                    StartTime = dr.GetDateTime("StartTime"),
                    EndTime = dr.GetDateTime("EndTime"),
                    ExpectedReturn = dr.GetDateTime("ExpectedReturn"),
                    Reason = dr.GetString("Reason"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
            return model;
        }

        public int Join(int? DriverID, int? BranchID, DateTime? StartDate, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@StartDate", StartDate),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.WorkPeriods.Join]", parameters);
        }

        public int BeginLeave(DriverLeaveModel LeaveModel)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", LeaveModel.DriverID),
               new SqlParameter("@BranchID", LeaveModel.BranchID),
               new SqlParameter("@StartTime", LeaveModel.StartTime),
               new SqlParameter("@ExpectedReturn", LeaveModel.ExpectedReturn),
               new SqlParameter("@Reason", LeaveModel.Reason),               
               new SqlParameter("@UserID", LeaveModel.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Leave.Begin]", parameters);
        }

        public int EndLeave(int? DriverID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),               
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Leave.End]", parameters);
        }


        public int? GetAssignedTruck(int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),
            };
            int? TruckID = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Driver.Assignment.Select]", parameters))
            {
                if (dr.GetBoolean("AssignedStatus"))
                {
                    TruckID = dr.GetInt32("TruckID");
                }
            }
            return TruckID;
        }

        IEnumerable<DriverModel> IDriverService.GetDriverByAdhaarNo(string AdhaarNo)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AdhaarNo", AdhaarNo),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Driver.GetAdhaar]", parameters))
            {
                yield return GetModel(dr);
            }
        }
    }
}
