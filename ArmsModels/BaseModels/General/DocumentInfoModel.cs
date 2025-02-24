using System;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    // Model representing information about a document
    public class DocumentInfoModel
    {
        public int? DocumentTypeID { get; set; }
        public int? DocumentID { get; set; }
        public string DocumentName { get; set; } = string.Empty;
    }
}
