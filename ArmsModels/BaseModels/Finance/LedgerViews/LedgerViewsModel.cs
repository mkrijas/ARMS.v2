using System;

namespace Core.BaseModels.Finance.LedgerViews
{
    public class LedgerViewsModel
    {
        public string AccountName { get; set; }
        public DateTime? Date { get; set; }
        public string Narration { get; set; }
        public decimal? Amount { get; set; }
        public string Refference { get; set; }
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
