using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArmsModels.BaseModels
{
    public class TruckTransferInitiationModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<TruckTransferInitiationModel>(Json);
        }
        public int? TruckTransferInitiationID { get; set; }
        [Required]
        public TruckModel Truck { get; set; }
        [Required]
        public BranchModel InitiationBranch { get; set; }
        public BranchModel DestinationBranch { get; set; }
        [Required]
        public EventModel TruckEvent { get; set; } = new();
        public string Remarks { get; set; }
        public TruckTransferEndModel TruckTransferEndModel { get; set; } = new();
        public int IstruckReject { get; set; } = 0;
    }

    public class TruckTransferEndModel
    {
        public int? TruckTransferEndID { get; set; }
        public int? BranchID { get; set; }
        public bool? TransferStatus { get; set; }
        public EventModel TruckEvent { get; set; } = new();
        public string Remarks { get; set; }
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