using ArmsModels.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;

namespace ArmsModels.BaseModels
{
    // Model representing a request for asset documents
    public class AssetDocumentRequestModel : ICloneable
    {
        // Method to create a deep copy of the object using JSON serialization
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this); // Serialize the object to JSON
            return JsonConvert.DeserializeObject<AssetDocumentRequestModel>(Json); // Deserialize back to a new object
        }
        public int? ID { get; set; } // Unique identifier for the request
        public int? BranchID { get; set; }
        public AssetDocumentTypeModel DocumentType { get; set; } // Type of the document
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Remarks { get; set; }
        public int? PaymentMemoID { get; set; }
        public UserInfoModel UserInfo { get; set; } // Information about the user making the request
        public List<AssetModel> Assets { get; set; } // List of assets associated with the request

    }

    // Model representing an asset document
    public class AssetDocumentModel : ICloneable
    {
        public object Clone()
        {
            string Json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<AssetDocumentModel>(Json);
        }

        // Constructor to initialize the UserInfo property
        public AssetDocumentModel()
        {
            UserInfo = new(); // Initialize UserInfo
        }
        [Required] // Validation attribute to ensure this field is required
        public string ReceiptNo { get; set; }
        public string Refference { get; set; }
        [RequiredIfDocumentType(19, 20)] // Custom validation attribute to require this field based on document type
        public decimal? IDVAmount { get; set; }
        [RequiredIfDocumentType(19, 20)]
        public decimal? NCBPercentage { get; set; }
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
        public bool IsFitParameter { get; set; }
    }

    // Model representing the type of an asset document
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
        public int? DocumentTypeID { get; set; } // Unique identifier for the document type
        public string DocumentTypeName { get; set; }
        public int? WarnBefore { get; set; }
        public int? BlockAfter { get; set; }
        public GstUsageCodeModel UsageCode { get; set; } // Usage code associated with the document type
        public UserInfoModel UserInfo { get; set; }
    }

    // Custom validation attribute to require a field based on document type
    public class RequiredIfDocumentTypeAttribute : ValidationAttribute
    {
        private readonly int[] _documentTypeIds;

        public RequiredIfDocumentTypeAttribute(params int[] documentTypeIds)
        {
            _documentTypeIds = documentTypeIds; // Set the document type IDs
        }

        // Method to validate the field
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (AssetDocumentModel)validationContext.ObjectInstance; // Get the current model instance
            // Check if the document type ID is in the specified list and the value is null
            if (_documentTypeIds.Contains(model.DocumentType.DocumentTypeID.GetValueOrDefault()) && value == null)
            {
                return new ValidationResult($"{validationContext.DisplayName} is required for the specified document types.");
            }

            return ValidationResult.Success;
        }
    }
}