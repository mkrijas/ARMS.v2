using ArmsModels.SharedModels;

namespace ArmsModels.BaseModels
{
    public class TruckTransferInitiationModel
    {
        public int? TruckTransferInitiationID { get; set; }
        public TruckModel Truck { get; set; }
        public int? InitiationBranchID { get; set; }
        public BranchModel InitiationBranch { get; set; }
        public int? DestinationBranchID { get; set; }
        public BranchModel DestinationBranch { get; set; }
        public decimal? StartKM { get; set; }
        public string Remarks { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
    public class TruckTransferEndModel
    {
        public int? TruckTransferEndID { get; set; }
        public int? TruckTransferInitiationID { get; set; }
        public TruckModel Truck { get; set; }
        public int? InitiationBranchID { get; set; }
        public BranchModel InitiationBranch { get; set; }
        public int? DestinationBranchID { get; set; }
        public string TransferStatus { get; set; }
        public decimal? EndKM { get; set; }
        public decimal? StartKM { get; set; }
        public string Remarks { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}
