namespace ArmsModels.BaseModels
{
    public class InterBranchMappingModel
    {
        public int? ID { get; set; }
        public int? DebitBranchID { get; set; }
        public int? CreditBranchID { get; set; }
        public int? CoaID { get; set; }
        public string TransactionType { get; set; }
        public SharedModels.UserInfoModel UserInfo { get; set; } = new();
    }
}
