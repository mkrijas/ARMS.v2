using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArmsModels.BaseModels
{
    // Model representing cancellation reason codes associated with a document type
    public class CancellationReasonCodesByDocumentType : TransactionBaseModel, ICloneable
    {
        public int? ReverseEntryID { get; set; } // Unique identifier for the reverse entry
        public int? ReasonCodeID { get; set; }
        public int? DocumentID { get; set; }
        public int? DocumentTypeID { get; set; }
        public string OrginalDocumentNumber { get; set; }
        public string DocumentNumber { get; set; } = "New";
        public DateTime? DocumentDate { get; set; }
        public int? MID { get; set; }
        public string DocumentTypeName { get; set; }
        public List<CancellationReasonCode> CancellationReasonCodeList { get; set; } // List of cancellation reason codes
        public UserInfoModel UserInfo { get; set; }
        public string Remarks { get; set; }

        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CancellationReasonCodesByDocumentType>(Json);
        }
    }

    // Model representing a cancellation reason code
    public class CancellationReasonCode
    {
        public int? ReasonCodeID { get; set; }
        public int? DocumentTypeID { get; set; }
        public string ReasonCodeDescription { get; set; }
    }
}