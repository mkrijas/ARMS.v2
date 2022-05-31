namespace ArmsModels.BaseModels.Finance.Transactions
{
    public class PartyPaymentModel :TransactionBaseModel
    {
        public int? PaymentID { get; set; }
        public int? ChequeID { get; set; }
        public string PaymentMode { get; set; }
        public int? PartyID { get; set; }
        public int? PartyBranchID { get; set; }
    }
}
