using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    // Model representing the initiation of a truck transfer
    public class TruckTransferInitiationModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TruckTransferInitiationModel>(Json);
        }
        public int? TruckTransferInitiationID { get; set; } // Unique identifier for the truck transfer initiation
        [Required]
        public TruckModel Truck { get; set; }
        [Required]
        public BranchModel InitiationBranch { get; set; }
        public BranchModel DestinationBranch { get; set; }
        [Required]
        public EventModel TruckEvent { get; set; } = new();
        public string Remarks { get; set; }
        public TruckTransferEndModel TruckTransferEndModel { get; set; } = new(); // Model representing the end of the truck transfer
        public int IstruckReject { get; set; } = 0;
    }

    // Model representing the end of a truck transfer
    public class TruckTransferEndModel
    {
        public int? TruckTransferEndID { get; set; } // Unique identifier for the truck transfer end
        public int? BranchID { get; set; }
        public bool? TransferStatus { get; set; }
        public EventModel TruckEvent { get; set; } = new();
        public string Remarks { get; set; }

        // Property to get the status text based on TransferStatus
        public string StatusText
        {
            get
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