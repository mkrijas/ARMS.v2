using ArmsModels.BaseModels;
using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArmsModels.BaseModels
{
    public class CancellationReasonCodesByDocumentType : TransactionBaseModel, ICloneable
    {
        public int? ReverseEntryID { get; set; }
        public int? ReasonCodeID { get; set; }
        public int? DocumentID { get; set; }
        public int? DocumentTypeID { get; set; }
        public string OrginalDocumentNumber { get; set; }
        public string DocumentNumber { get; set; } = "New";
        public DateTime? DocumentDate { get; set; }
        public int? MID { get; set; }
        public string DocumentTypeName { get; set; }
        public List<CancellationReasonCode> CancellationReasonCodeList { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public string Remarks { get; set; }

        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CancellationReasonCodesByDocumentType>(Json);
        }
    }

    public class CancellationReasonCode
    {
        public int? ReasonCodeID { get; set; }
        public int? DocumentTypeID { get; set; }
        public string ReasonCodeDescription { get; set; }
    }
}