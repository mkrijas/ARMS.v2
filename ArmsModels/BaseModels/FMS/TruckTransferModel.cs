using ArmsModels.SharedModels;

namespace ArmsModels.BaseModels
{
    public class TruckTransferInitiationModel
    {
        public int? TruckTransferInitiationID { get; set; }
        public TruckModel Truck { get; set; }
        public BranchModel InitiationBranch { get; set; }        
        public BranchModel DestinationBranch { get; set; }
        public EventModel TruckEvent { get; set; }
        public string Remarks { get; set; }
        public TruckTransferEndModel TruckTransferEndModel { get; set; }
        public int IstruckReject { get; set; } = 0;
    }
    public class TruckTransferEndModel
    {
        public int? TruckTransferEndID { get; set; }        
        public int? BranchID { get; set; }              
        public bool? TransferStatus { get; set; }
        public EventModel TruckEvent { get; set; }
        public string Remarks { get; set; }
        public string StatusText { get
            {
                if (TransferStatus == null)
                    return "Being Transferred";
                else if (TransferStatus.Value)
                    return "Completed";
                else
                    return "Rejected";
            }
        }
    }
}
