using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Core.BaseModels.Operations;


namespace ArmsServices.DataServices
{
    public class DestinationFeasibilityCheckerService : IDestinationFeasibilityCheckerService
    {
        IDbService Iservice;

        public DestinationFeasibilityCheckerService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to update a place
        public async Task<DestinationFeasibilityCheckerModel> Update(DestinationFeasibilityCheckerModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", model.ID),
               new SqlParameter("@ContentID", model.Content.ContentID),
               new SqlParameter("@BodyType", model.BodyType),
               new SqlParameter("@TruckTypeID", model.TruckType.TruckTypeID),
               new SqlParameter("@SystemKM", model.SystemKM),
               new SqlParameter("@RunKM", model.RunKM),
               new SqlParameter("@StandardDays", model.StandardDays),
               new SqlParameter("@FuelLitre", model.FuelLitre),
               new SqlParameter("@StandardMileage", model.StandardMileage),
               new SqlParameter("@MetricTonn", model.MetricTonn),
               new SqlParameter("@FreightRate", model.FreightRate),
               new SqlParameter("@Incentive", model.Incentive),
               new SqlParameter("@PrimaryFreight", model.PrimaryFreight),
               new SqlParameter("@PTPK", model.PTPK),
               new SqlParameter("@FuelExpenses", model.FuelExpenses),
               new SqlParameter("@FuelRate", model.FuelRate),
               new SqlParameter("@FuelPercentage", model.FuelPercentage),
               new SqlParameter("@AdBlueCost", model.AdBlueCost),
               new SqlParameter("@DriverWages", model.DriverWages),
               new SqlParameter("@TollExpenses", model.TollExpenses),
               new SqlParameter("@TaggingExpenses", model.TaggingExpenses),
               new SqlParameter("@DriversSalary", model.DriversSalary),
               new SqlParameter("@LoadingCharges", model.LoadingCharges),
               new SqlParameter("@UnloadingCharges", model.UnloadingCharges),
               new SqlParameter("@TripOtherExpenses", model.TripOtherExpenses),
               new SqlParameter("@TripDirectExpenses", model.TripDirectExpenses),
               new SqlParameter("@TripPercentage", model.TripPercentage),
               new SqlParameter("@BranchAdminCost", model.BranchAdminCost),
               new SqlParameter("@MaintenanceCost", model.MaintenanceCost),
               new SqlParameter("@TyreCost", model.TyreCost),
               new SqlParameter("@TaxAndInsurance", model.TaxAndInsurance),
               new SqlParameter("@FcMaintenance", model.FcMaintenance),
               new SqlParameter("@HOAdminCost", model.HOAdminCost),
               new SqlParameter("@OtherThanTripExpenses", model.OtherThanTripExpenses),
               new SqlParameter("@OtherThanTripPercentage", model.OtherThanTripPercentage),
               new SqlParameter("@TotalExpense", model.TotalExpense),
               new SqlParameter("@Profit", model.Profit),
               new SqlParameter("@ExpensePercentage", model.ExpensePercentage),
               new SqlParameter("@ProfitPercentage", model.ProfitPercentage),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Operation.DestinationFeasibilityChecker.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to delete a place by its ID
        public async Task<int> Delete(int? PlaceID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", PlaceID),
               new SqlParameter("@UserID", UserID),
            };
            return await Iservice.ExecuteNonQueryAsync("[usp.Operation.DestinationFeasibilityChecker.Delete]", parameters);
        }

        // Method to select places based on ID and a search string
        public IEnumerable<DestinationFeasibilityCheckerModel> Select(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.DestinationFeasibilityChecker.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a place by its ID
        public DestinationFeasibilityCheckerRatesModel SelectRates(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.DestinationFeasibilityChecker.Rates.Select]", parameters))
            {
                return new DestinationFeasibilityCheckerRatesModel()
                {
                    AverageTaxAndInsurance = dr.GetDecimal("AverageTaxAndInsurance"),
                    TyreRate = dr.GetDecimal("TyreRate"),
                    MaintenanceRate = dr.GetDecimal("MaintenanceRate"),
                    AdBlueRatio = dr.GetDecimal("AdBlueRatio")
                };
            }
            return null;
        }

        // Helper method to map data record to PlaceModel
        private DestinationFeasibilityCheckerModel GetModel(IDataRecord dr)
        {
            return new DestinationFeasibilityCheckerModel
            {
                ID = dr.GetInt32("ID"),
                Content = new ContentModel
                {
                    ContentID = dr.GetByte("ContentID"),
                    ContentName = dr.GetString("ContentName"),
                },
                BodyType = dr.GetString("BodyType"),
                TruckType = new TruckTypeModel
                {
                    TruckTypeID = dr.GetByte("TruckTypeID"),
                    TruckType = dr.GetString("TruckType"),
                    wheels = dr.GetByte("wheels"),
                    BSType = dr.GetString("BSType"),
                },
                SystemKM = dr.GetDecimal("SystemKM"),
                RunKM = dr.GetDecimal("RunKM"),
                StandardDays = dr.GetDecimal("StandardDays"),
                FuelLitre = dr.GetDecimal("FuelLitre"),
                StandardMileage = dr.GetDecimal("StandardMileage"),
                MetricTonn = dr.GetDecimal("MetricTonn"),
                FreightRate = dr.GetDecimal("FreightRate"),
                Incentive = dr.GetDecimal("Incentive"),
                PrimaryFreight = dr.GetDecimal("PrimaryFreight"),
                PTPK = dr.GetDecimal("PTPK"),
                FuelExpenses = dr.GetDecimal("FuelExpenses"),
                FuelRate = dr.GetDecimal("FuelRate"),
                FuelPercentage = dr.GetDecimal("FuelPercentage"),
                AdBlueCost = dr.GetDecimal("AdBlueCost"),
                DriverWages = dr.GetDecimal("DriverWages"),
                TollExpenses = dr.GetDecimal("TollExpenses"),
                TaggingExpenses = dr.GetDecimal("TaggingExpenses"),
                DriversSalary = dr.GetDecimal("DriversSalary"),
                LoadingCharges = dr.GetDecimal("LoadingCharges"),
                UnloadingCharges = dr.GetDecimal("UnloadingCharges"),
                TripOtherExpenses = dr.GetDecimal("TripOtherExpenses"),
                TripDirectExpenses = dr.GetDecimal("TripDirectExpenses"),
                TripPercentage = dr.GetDecimal("TripPercentage"),
                BranchAdminCost = dr.GetDecimal("BranchAdminCost"),
                MaintenanceCost = dr.GetDecimal("MaintenanceCost"),
                TyreCost = dr.GetDecimal("TyreCost"),
                TaxAndInsurance = dr.GetDecimal("TaxAndInsurance"),
                FcMaintenance = dr.GetDecimal("FcMaintenance"),
                HOAdminCost = dr.GetDecimal("HOAdminCost"),
                OtherThanTripExpenses = dr.GetDecimal("OtherThanTripExpenses"),
                OtherThanTripPercentage = dr.GetDecimal("OtherThanTripPercentage"),
                TotalExpense = dr.GetDecimal("TotalExpense"),
                Profit = dr.GetDecimal("Profit"),
                ExpensePercentage = dr.GetDecimal("ExpensePercentage"),
                ProfitPercentage = dr.GetDecimal("ProfitPercentage"),
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
