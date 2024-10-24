using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Data;
using System.Data.SqlClient;
using FluentValidation;
using System.Reflection;

namespace ArmsServices.DataServices
{
    public class DriverService : IDriverService
    {
        IDbService Iservice;
        IAddressService _addressService;
        IBankAccountService _bankAccountService;

        private readonly ITruckService truckService;
        private readonly ITripService tripService;

        public DriverService(IDbService iservice, IAddressService Iaddress, ITruckService truckService, ITripService tripService, IBankAccountService bankAccountService)
        {
            Iservice = iservice;
            _addressService = Iaddress;
            this.truckService = truckService;
            this.tripService = tripService;
            _bankAccountService = bankAccountService;
        }

        public DriverModel Update(DriverModel model)
        {
            model.Address = _addressService.Update(model.Address);
            if (model.BankAccount != null && model.BankAccount.BeneficiaryName != null && model.BankAccount.BeneficiaryName != string.Empty
                && model.BankAccount.AccountNumber != null && model.BankAccount.AccountNumber != string.Empty
                && model.BankAccount.IfscCode != null && model.BankAccount.IfscCode != string.Empty)
            {
                model.BankAccount.UserInfo = model.UserInfo;
                model.BankAccount = _bankAccountService.Update(model.BankAccount);
            }
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
               new SqlParameter("@BankAccountID", model.BankAccount.BankAccountID),
               new SqlParameter("@AdditionalInfo", model.AdditionalInfo),
               new SqlParameter("@Mobile", model.Mobile),
               new SqlParameter("@Email", model.Email),
               new SqlParameter("@DriverAgentID", model.DriverAgentID),
               new SqlParameter("@FestivalBonus", model.FestivalBonus),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Driver.Update]", parameters))
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

        public int Resign(int? DriverID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@Remarks", Remarks),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Driver.Resign]", parameters);
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

        public IEnumerable<DriverModel> SelectRejoin()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID", 0),
               new SqlParameter("@Operation", "Rejoin"),
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

        public IEnumerable<DriverModel> ExcelDataCollection(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BranchID", BranchID),
                new SqlParameter("@Operation", "ForExcel"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Driver.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        private DriverModel GetModel(IDataRecord reader)
        {
            return new DriverModel
            {
                HomeBranchID = reader.GetInt32("HomeBranchID"),
                HomeBranchName = reader.GetString("HomeBranchName"),
                JoiningDate = reader.GetDateTime("JoiningDate"),
                DriverCode = reader.GetString("DriverCode"),
                DriverName = reader.GetString("DriverName"),
                DriverImage = reader.GetString("DriverImage"),
                //DriverAgentID = reader.GetInt32("DriverAgentID"),
                Mobile = reader.GetString("Mobile"),
                DriverAgent = new PartyModel()
                {
                    PartyID = reader.GetInt32("DriverAgentID"),
                    TradeName = reader.GetString("PartyName")
                },
                DateOfBirth = reader.GetDateTime("DateOfBirth"),
                AdhaarNo = reader.GetString("AdhaarNo"),
                AdhaarImage = reader.GetString("AdhaarImage"),
                Address = new AddressModel()
                {
                    Building = reader.GetString("Building"),
                    Street = reader.GetString("Street"),
                    Place = reader.GetString("Place"),
                    City = reader.GetString("City"),
                    PinCode = reader.GetString("PinCode"),
                },
                BankAccount = new BankAccountModel()
                {
                    BeneficiaryName = reader.GetString("BeneficiaryName"),
                    AccountNumber = reader.GetString("AccountNumber"),
                    IfscCode = reader.GetString("IfscCode"),
                    MicrCode = reader.GetString("MicrCode"),
                    SwiftCode = reader.GetString("SwiftCode"),
                    BankTitle = reader.GetString("BankTitle"),
                    BankBranch = reader.GetString("BankBranch"),
                },
                DriverLicence = new DriverLicenceModel()
                {
                    LicenceType = reader.GetString("LicenceType"),
                    LicenceNo = reader.GetString("LicenceNo"),
                    DLExpiryDate = reader.GetDateTime("DLExpiryDate"),
                    BadgeNo = reader.GetString("BadgeNo"),
                    BadgeExpiryDate = reader.GetDateTime("BadgeExpiryDate"),
                },
                FestivalBonus = reader.GetString("FestivalBonus"),
                DriverID = reader.GetInt32("DriverID"),
                AdditionalInfo = reader.GetString("AdditionalInfo"),
                Email = reader.GetString("Email"),
                AddressID = reader.GetInt32("AddressID"),
                BankAccountID = reader.GetInt32("BankAccountID"),
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

        //public DriverModel FindDriver(DriverModel model = null, DriverLicenceModel licence = null)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>
        //    {
        //       new SqlParameter("@Operation", "GetTracked"),
        //       new SqlParameter("@AdhaarNo", model?.AdhaarNo),
        //       new SqlParameter("@LicenceNo", licence?.LicenceNo),
        //    };
        //    foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Driver.Select]", parameters))
        //    {
        //        model = GetModel(dr);
        //    }
        //    return model;
        //}

        public DriverModel FindDriver(string Operation, DriverModel Value)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", Operation),
               new SqlParameter("@DriverID", Value.DriverID),
            };
            DriverModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Driver.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public DriverModel FindDriver(string Operation, string Value)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", Operation),
               new SqlParameter("@AdhaarNo", Value),
               new SqlParameter("@LicenceNo", Value),
            };
            DriverModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.Driver.Select]", parameters))
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
                    Driver = new DriverModel()
                    {
                        DriverID = dr.GetInt32("DriverID"),
                        DriverName = dr.GetString("DriverName"),
                    },
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
               new SqlParameter("@DriverID", LeaveModel.Driver.DriverID),
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

        public string GetWorkPeriod(int? DriverID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@DriverID", DriverID)
            };
            string workPeriod = "Unknown";
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Driver.WorkPeriod.Select]", parameters))
            {
                DateTime? startDate = dr.GetDateTime("StartDate");
                DateTime? endDate = dr.GetDateTime("EndDate");
                workPeriod = $"{startDate?.ToShortDateString()} - {endDate?.ToShortDateString()}";
                break;
            }
            return workPeriod;
        }

        public string RemoveDriverFromTruck(int? driverId, string userId)
        {
            int? assignedTruckID = GetAssignedTruck(driverId);
            if (assignedTruckID != null)
            {
                // Check if the assigned truck is on a trip
                long? currentTripID = truckService.GetCurrentTrip(assignedTruckID);
                if (currentTripID != null)
                {
                    // Check if the current trip is completed
                    bool isTripClosed = tripService.IsClosed(currentTripID);
                    if (!isTripClosed)
                    {
                        return "OnTrip";
                    }
                }
                // Assigned truck is not on a trip, remove the driver from the truck
                try
                {
                    TruckModel truck = truckService.SelectByID(assignedTruckID);
                    truckService.UpdateDriver(assignedTruckID, driverId, false, userId);
                    return "DriverRemoved";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "NoAssignedTruck";
        }

        public int ReJoin(int? DriverID, string UserID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DriverID",DriverID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@BranchID", BranchID),
            };
            return Iservice.ExecuteNonQuery("[usp.Driver.Driver.Rejoin]", parameters);
        }
    }
}