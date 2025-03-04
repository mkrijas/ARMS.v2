using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;
using Core.IDataServices.Operations.ROI;
using Core.BaseModels.Operations.ROI;


namespace ArmsServices.DataServices
{
    public class TruckService : ITruckService
    {
        IDbService Iservice;
        IROITonnageService IROITonnage;

        public TruckService(IDbService iservice, IROITonnageService iROITonnage)
        {
            Iservice = iservice;
            IROITonnage = iROITonnage;
        }

        // Method to select truck body types
        public IEnumerable<string> SelectBSType()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Operation", "BSTYPE"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.ROI.Common.Select]", parameters))
            {
                yield return dr.GetString("BsType");
            }
        }

        // Method to select wheels informationv
        public IEnumerable<ROITonnageModel> SelectWheels()
        {
            List<ROITonnageModel> Model = new();
            return Model = IROITonnage.SelectWheels().ToList();
        }

        // Method to update a truck record
        public TruckModel Update(TruckModel model)
        {
            bool create = model.TruckID == null;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@HomeBranchID", model.HomeBranchID),
               new SqlParameter("@BodyType", model.BodyType),
               new SqlParameter("@ChassisNumber", model.ChassisNumber),
               new SqlParameter("@EngineNumber", model.EngineNumber),
               new SqlParameter("@FuelTankCapacity", model.FuelTankCapacity),
               new SqlParameter("@FuelType", model.FuelType),
               new SqlParameter("@GpsDeviceID", model.GpsDeviceID),
               new SqlParameter("@ManufacturedYear", model.ManufacturedYear),
               new SqlParameter("@PurchaseDate", model.PurchaseDate),
               new SqlParameter("@TruckTypeID", model.TruckTypeID),
               new SqlParameter("@TransmissionType", model.TransmissionType),
               new SqlParameter("@GrossWeight", model.GrossWeight),
               new SqlParameter("@UnladenWeight", model.UnladenWeight),
               new SqlParameter("@AssetID", model.AssetID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            model.CurrentRegistration.UserInfo = model.UserInfo;
            TruckModel cmodel = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Truck.Update]", parameters))
            {
                cmodel = GetModel(dr);
            }
            model.CurrentRegistration.TruckID = cmodel.TruckID;
            if (create)
            {
                int reg = ChangeRegistration(model.CurrentRegistration);
                if (reg > 0)
                {
                    cmodel.RegNo = model.CurrentRegistration.RegNo;
                }
            }
            return cmodel;
        }

        // Method to update truck registration
        public int UpdateRegistration(TruckRegistrationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@RegID", model.RegID),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@RegNo", model.RegNo),
               new SqlParameter("@RC", model.RC),
               new SqlParameter("@EffectFrom", model.EffectFrom),
               new SqlParameter("@EffectTo", model.EffectTo),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Truck.Registration.Update]", parameters);
        }

        // Method to validate registration date
        public int? ValidateRegistrationDate(TruckRegistrationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RegID", model.RegID),
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@EffectFrom", model.EffectFrom),
               new SqlParameter("@EffectTo", model.EffectTo)
            };
            int? result = 0;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Registration.Date.Validation]", parameters))
            {
                result = dr.GetInt32("ConflictNo");
            }
            return result;
        }

        // Method to delete a truck by its ID
        public int Delete(int? TruckID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Truck.Truck.Delete]", parameters);
        }

        // Method to select a truck by its ID
        public IEnumerable<TruckModel> Select(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Truck.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to mark a truck as sold (not implemented)
        public int Sold(int? TruckID, DateTime? SoldDate)
        {
            throw new NotImplementedException();
        }

        // Method to change the registration of a truck
        public int ChangeRegistration(TruckRegistrationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", model.TruckID),
               new SqlParameter("@RegNo", model.RegNo),
               new SqlParameter("@RC", model.RC),
               new SqlParameter("@EffectFrom", model.EffectFrom),
               new SqlParameter("@EffectTo", model.EffectTo),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Truck.Registration.Update]", parameters);
        }

        // Helper method to map data record to TruckModel
        private TruckModel GetModel(IDataRecord reader)
        {
            return new TruckModel()
            {
                RegNo = reader.GetString("RegNo"),
                HomeBranchID = reader.GetInt32("HomeBranchID"),
                CurrentBranchID = reader.GetInt32("CurrentBranchID"),
                AssetID = reader.GetInt32("AssetID"),
                BodyType = reader.GetString("BodyType"),
                ChassisNumber = reader.GetString("ChassisNumber"),
                EngineNumber = reader.GetString("EngineNumber"),
                FuelTankCapacity = reader.GetDecimal("FuelTankCapacity"),
                FuelType = reader.GetString("FuelType"),
                TruckID = reader.GetInt32("TruckID"),
                GpsDeviceID = reader.GetInt64("GpsDeviceID"),
                ManufacturedYear = reader.GetInt16("ManufacturedYear"),
                PurchaseDate = reader.GetDateTime("PurchaseDate"),
                SoldDate = reader.GetDateTime("SoldDate"),
                TransmissionType = reader.GetString("TransmissionType"),
                GrossWeight = reader.GetDecimal("GrossWeight"),
                UnladenWeight = reader.GetDecimal("UnladenWeight"),
                TruckTypeID = reader.GetInt16("TruckTypeID"),
                TruckType = reader.GetString("TruckType"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = reader.GetByte("RecordStatus"),
                    TimeStampField = reader.GetDateTime("TimeStamp"),
                    UserID = reader.GetString("UserID"),
                },
            };
        }

        // Helper method to map data record to TruckRegistrationModel
        private TruckRegistrationModel GetRegModel(IDataRecord reader)
        {
            return new TruckRegistrationModel
            {
                RegID = reader.GetInt32("RegID"),
                RegNo = reader.GetString("RegNo"),
                EffectFrom = reader.GetDateTime("EffectFrom"),
                EffectTo = reader.GetDateTime("EffectTo"),
                TruckID = reader.GetInt32("TruckID"),
                RC = reader.GetString("RC"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = reader.GetByte("RecordStatus"),
                    TimeStampField = reader.GetDateTime("TimeStamp"),
                    UserID = reader.GetString("UserID"),
                },
            };
        }

        // Method to select a truck by its ID
        public TruckModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", ID)
            };
            TruckModel model = new TruckModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Truck.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to get registration details by registration ID
        public TruckRegistrationModel GetRegistration(int? RegID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@String", RegID.ToString()),
               new SqlParameter("@Operation", "SelectByTruckID"),
            };
            TruckRegistrationModel model = new TruckRegistrationModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Registration.Select]", parameters))
            {
                model = GetRegModel(dr);
            }
            return model;
        }

        // Method to get a list of registrations for a truck
        public IEnumerable<TruckRegistrationModel> GetRegistrationList(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@String", TruckID.ToString()),
               new SqlParameter("@Operation", "SelectByTruckID"),
            };
            TruckRegistrationModel model = new TruckRegistrationModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Registration.Select]", parameters))
            {
                yield return GetRegModel(dr);
            }
        }

        // Method to get registration details by registration number
        public TruckRegistrationModel GetRegistration(string RegNo)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@String", RegNo),
               new SqlParameter("@Operation", "SelectByRegNo"),
            };
            TruckRegistrationModel model = new TruckRegistrationModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Registration.Select]", parameters))
            {
                model = GetRegModel(dr);
            }
            return model;
        }

        // Method to update the driver assignment for a truck
        public int UpdateDriver(int? TruckID, int? DriverID, bool AssignedStatus, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
               new SqlParameter("@DriverID", DriverID),
               new SqlParameter("@AssignedStatus", AssignedStatus),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Truck.Driver.Assignment.Update]", parameters);
        }

        // Method to get the assigned driver for a truck
        int? ITruckService.GetAssignedDriver(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
            };
            int? DriverID = null;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Driver.Assignment.Select]", parameters))
            {
                if (dr.GetBoolean("AssignedStatus"))
                {
                    DriverID = dr.GetInt32("DriverID");
                }
            }
            return DriverID;
        }

        // Method to get the current trip for a truck
        public long? GetCurrentTrip(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
            };
            long? TripID = null;
            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Truck.CurrentTrip.Select]", parameters))
            {
                TripID = dr.GetInt64("TripID");
            }
            return TripID;
        }

        // Method to get the last trip for a truck
        public long? GetLastTrip(int? TruckID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckID", TruckID),
            };
            long? TripID = null;
            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Truck.LastTrip.Select]", parameters))
            {
                TripID = dr.GetInt64("TripID");
            }
            return TripID;
        }

        // Method to select trucks by branch with optional filters
        public IEnumerable<TruckModel> SelectByBranch(int? BranchID, string Filer, string HomeOrOperation = "Operation")
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", Filer),
               new SqlParameter("@HomeOrOperation", HomeOrOperation),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Truck.Select]", parameters))
            {
                yield return new TruckModel()
                {
                    AssetID = dr.GetInt32("AssetID"),
                    RegNo = dr.GetString("RegNo"),
                    TruckID = dr.GetInt32("TruckID"),
                    HomeBranchID = dr.GetInt32("HomeBranchID"),
                    HomeBranchName = dr.GetString("HomeBranchName"),
                    OperatingBranchName = dr.GetString("OperatingBranchName"),
                    TruckTypeID = dr.GetInt16("TruckTypeID"),
                    TruckType = dr.GetString("TruckType"),
                    BSType = dr.GetString("BSType"),
                    wheels = dr.GetByte("wheels"),
                    TransmissionType = dr.GetString("TransmissionType"),
                    FuelType = dr.GetString("FuelType"),
                    FuelTankCapacity = dr.GetDecimal("FuelTankCapacity"),
                    BodyType = dr.GetString("BodyType"),
                    ManufacturedYear = dr.GetInt16("ManufacturedYear"),
                    EngineNumber = dr.GetString("EngineNumber"),
                    ChassisNumber = dr.GetString("ChassisNumber"),
                    GrossWeight = dr.GetDecimal("GrossWeight"),
                    UnladenWeight = dr.GetDecimal("UnladenWeight"),
                    PurchaseDate = dr.GetDateTime("PurchaseDate"),
                    DriverName = dr.GetString("DriverName"),
                    Mobile = dr.GetString("Mobile"),
                    CurrentRegistration = new()
                    {
                        RegID = dr.GetInt32("RegID"),
                        RegNo = dr.GetString("RegNo"),
                        RC = dr.GetString("RC"),
                        EffectFrom = dr.GetDateTime("EffectFrom"),
                        EffectTo = dr.GetDateTime("EffectTo"),
                    },
                    CurrentEvent = new EventModel()
                    {
                        BranchID = dr.GetInt32("BranchID"),
                        BranchName = dr.GetString("BranchName"),
                        DestinationID = dr.GetInt32("DestinationID"),
                        DriverID = dr.GetInt32("DriverID"),
                        EventReading = dr.GetInt64("EventReading"),
                        EventTime = dr.GetDateTime("EventTime"),
                        EventTypeID = dr.GetByte("EventTypeID"),
                        GcSetID = dr.GetInt64("GcSetID"),
                        OriginID = dr.GetInt32("OriginID"),
                        TripID = dr.GetInt64("TripID"),
                        TruckEventID = dr.GetInt64("EventID"),
                        TruckID = dr.GetInt32("TruckID"),
                        OriginName = dr.GetString("OriginName"),
                        DestinationName = dr.GetString("DestinationName")
                    },
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }

        // Method to select a truck by its asset ID
        public TruckModel SelectByAsset(int? AssetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", AssetID),
            };

            TruckModel model = new TruckModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Truck.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to get the truck status summary by branch ID
        public IEnumerable<TruckStatusModel> GetTruckStatus(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "Summary"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[rpt.Operation.Trucks.CurrentStatus]", parameters))
            {
                yield return new TruckStatusModel()
                {
                    DisplayOrder = dr.GetInt32("DisplayOrder"),
                    LoadStatus = dr.GetString("LoadStatus"),
                    NoOfTrucks = dr.GetInt32("NoOfTrucks"),
                    StatusText = dr.GetString("EventStatusText"),
                    Truck = dr.GetString("Truck"),
                };
            }
        }

        // Method to select all trucks by branch with optional filters
        public IEnumerable<TruckModel> SelectAllByBranch(bool IsChecked, int? BranchID = null, string Filer = "All", string HomeOrOperation = "AllOperation")
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", Filer),
               new SqlParameter("@HomeOrOperation", HomeOrOperation),
               new SqlParameter("@IsChecked", IsChecked)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Truck.Truck.Select]", parameters))
            {
                yield return new TruckModel()
                {
                    AssetID = dr.GetInt32("AssetID"),
                    RegNo = dr.GetString("RegNo"),
                    TruckID = dr.GetInt32("TruckID"),
                    TruckTypeID = dr.GetInt16("TruckTypeID"),
                    TruckType = dr.GetString("TruckType"),
                    FuelType = dr.GetString("FuelType"),
                    FuelTankCapacity = dr.GetDecimal("FuelTankCapacity"),
                    BodyType = dr.GetString("BodyType"),
                    ManufacturedYear = dr.GetInt16("ManufacturedYear"),
                    CurrentRegistration = new()
                    {
                        RegID = dr.GetInt32("RegID"),
                        RegNo = dr.GetString("RegNo"),
                        RC = dr.GetString("RC"),
                        EffectFrom = dr.GetDateTime("EffectFrom"),
                        EffectTo = dr.GetDateTime("EffectTo"),
                    },
                    CurrentEvent = new EventModel()
                    {
                        BranchID = dr.GetInt32("BranchID"),
                        BranchName = dr.GetString("BranchName"),
                        DestinationID = dr.GetInt32("DestinationID"),
                        DriverID = dr.GetInt32("DriverID"),
                        EventReading = dr.GetInt64("EventReading"),
                        EventTime = dr.GetDateTime("EventTime"),
                        EventTypeID = dr.GetByte("EventTypeID"),
                        GcSetID = dr.GetInt64("GcSetID"),
                        OriginID = dr.GetInt32("OriginID"),
                        TripID = dr.GetInt64("TripID"),
                        TruckEventID = dr.GetInt64("EventID"),
                        TruckID = dr.GetInt32("TruckID"),
                        OriginName = dr.GetString("OriginName"),
                        DestinationName = dr.GetString("DestinationName")
                    },
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }

        // Method to get the truck status summary by event
        public IEnumerable<TruckStatusModel> GetTruckStatusByEvent(int? BranchID, string SelectedValue)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "TruckDetails"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@SelectedValue", SelectedValue),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.DashBoard.ChartData.SelectOnClick]", parameters))
            {
                yield return new TruckStatusModel()
                {
                    TruckID = dr.GetInt32("TruckID"),
                    RegNo = dr.GetString("RegNo"),
                    EventTime = dr.GetDateTime("EventTime"),
                    EventDateTimeDiff = dr.GetString("EventDateTimeDiff"),
                    DriverSince = string.Format("{0} since {1}", dr.GetDateTime("DriverSince"), dr.GetString("DriverName"))
                };
            }
        }
    }
}