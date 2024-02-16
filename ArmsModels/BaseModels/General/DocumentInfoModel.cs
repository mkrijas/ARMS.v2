using System;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class DocumentInfoModel
    {
        public int? DocumentTypeID { get; set; }
        public int? DocumentID { get; set; }
        public string DocumentName { get; set; } = string.Empty;
    }
}
