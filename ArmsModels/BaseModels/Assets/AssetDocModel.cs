using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace ArmsModels.BaseModels
{
    public class AssetDocumentRequestModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<AssetDocumentRequestModel>(Json);
        }
        public int? ID { get; set; }
        public int? BranchID { get; set; }
        public AssetDocumentTypeModel DocumentType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Remarks { get; set; }
        public int? PaymentMemoID { get; set; }
        public UserInfoModel UserInfo { get; set; }
        public List<AssetModel> Assets { get; set; }

    }

    public class AssetDocumentModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<AssetDocumentModel>(Json);
        }
        public AssetDocumentModel()
        {
            UserInfo = new();
        }
        [Required]
        public string ReceiptNo { get; set; }
        public string Refference { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        public string AttachedDocument { get; set; }
        public int? RequestID { get; set; }
        public int? DocumentID { get; set; }
        [Required]
        public AssetDocumentTypeModel DocumentType { get; set; }
        [Required]
        public AssetModel Asset { get; set; } = new();
        [Required]
        public DateTime? InvoiceDate { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        public int? NotificationID { get; set; }
        public bool IsFinanciallyPosted { get; set; }
        public UserInfoModel UserInfo { get; set; } = new();
        public DateTime? ExtendedEndDate { get; set; }
    }

    public class AssetDocumentTypeModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<AssetDocumentTypeModel>(Json);
        }
        public AssetDocumentTypeModel()
        {
            UserInfo = new();
        }
        public int? DocumentTypeID { get; set; }
        public string DocumentTypeName { get; set; }
        public int? WarnBefore { get; set; }
        public int? BlockAfter { get; set; }
        public GstUsageCodeModel UsageCode { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
}