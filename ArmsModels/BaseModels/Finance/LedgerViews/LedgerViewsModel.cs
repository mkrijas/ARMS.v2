using System;

namespace Core.BaseModels.Finance.LedgerViews
{
    // Model representing a ledger view entry
    public class LedgerViewsModel
    {
        public string AccountName { get; set; }
        public DateTime? Date { get; set; }
        public string Narration { get; set; }
        public decimal? Amount { get; set; }
        public string Refference { get; set; }

        // Property to determine if the entry is a debit (Dr) or credit (Cr)
        public virtual string DrCrText
        {
            get
            {
                if (Amount != null && Amount >=0)
                {
                    return "Dr";
                }
                else if (Amount != null)
                {
                    return "Cr";
                }
                else
                {
                    return "";
                }
            }
        }
    }
}