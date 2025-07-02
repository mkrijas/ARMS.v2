using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.BaseModels.Operations
{
    // Represents the availability status of a truck
    public class DestinationFeasibilityCheckerModel
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<DestinationFeasibilityCheckerModel>(Json);
        }
        public DestinationFeasibilityCheckerModel()
        {
            TruckType = new TruckTypeModel();
            UserInfo = new UserInfoModel();
        }
        public int? ID { get; set; }
        public ContentModel Content { get; set; }
        public string BodyType { get; set; }
        public TruckTypeModel TruckType { get; set; }
        public decimal? SystemKM { get; set; }
        public decimal? RunKM { get; set; }
        public decimal? StandardDays { get; set; }
        public decimal? FuelLitre { get; set; }
        public decimal? StandardMileage { get; set; }
        public decimal? MetricTonn { get; set; }
        public decimal? FreightRate { get; set; }
        public decimal? Incentive { get; set; }
        public decimal? PrimaryFreight { get; set; }
        public decimal? PTPK { get; set; }
        public decimal? FuelExpenses { get; set; }
        public decimal? FuelRate { get; set; }
        public decimal? FuelPercentage { get; set; }
        public decimal? AdBlueCost { get; set; }
        public decimal? DriverWages { get; set; }
        public decimal? TollExpenses { get; set; }
        public decimal? TaggingExpenses { get; set; }
        public decimal? DriversSalary { get; set; }
        public decimal? LoadingCharges { get; set; }
        public decimal? UnloadingCharges { get; set; }
        public decimal? TripOtherExpenses { get; set; }
        public decimal? TripDirectExpenses { get; set; }
        public decimal? TripPercentage { get; set; }
        public decimal? BranchAdminCost { get; set; }
        public decimal? MaintenanceCost { get; set; }
        public decimal? TyreCost { get; set; }
        public decimal? TaxAndInsurance { get; set; }
        public decimal? FcMaintenance { get; set; }
        public decimal? HOAdminCost { get; set; }
        public decimal? OtherThanTripExpenses { get; set; }
        public decimal? OtherThanTripPercentage { get; set; }
        public decimal? TotalExpense { get; set; }
        public decimal? Profit { get; set; }
        public decimal? ExpensePercentage { get; set; }
        public decimal? ProfitPercentage { get; set; }
        public int? BranchID { get; set; }
        public ArmsModels.SharedModels.UserInfoModel UserInfo { get; set; }
    }

    public class DestinationFeasibilityCheckerRatesModel
    {
        public decimal? AverageTaxAndInsurance { get; set; }
        public decimal? TyreRate { get; set; }
        public decimal? MaintenanceRate { get; set; }
        public decimal? AdBlueRatio { get; set; }
    }
}
