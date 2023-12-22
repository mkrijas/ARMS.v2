using ArmsModels.SharedModels;
using System.Collections.Generic;

namespace Core.BaseModels.Finance
{
    public class CancellationReasonCodesByDocumentType
    {
        public int? ReasonCodeID { get; set; }
        public int? DocumentTypeID { get; set; }
        public string DocumentTypeName { get; set; }
        public List<CancellationReasonCode> CancellationReasonCodeList { get; set; }
        public UserInfoModel UserInfo { get; set; }
    }
    public class CancellationReasonCode
    {
        public int? ReasonCodeID { get; set; }
        public int? DocumentTypeID { get; set; }
        public string ReasonCodeDescription { get; set; }
    }
}
