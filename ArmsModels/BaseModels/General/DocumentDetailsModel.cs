using System;
using System.ComponentModel.DataAnnotations;
using ArmsModels.SharedModels;
using Newtonsoft.Json;

namespace ArmsModels.BaseModels
{
    public class DocumentDetailsModel
    {
        public int? DocumentTypeID { get; set; }
        public int? DocumentID { get; set; }
    }
}
