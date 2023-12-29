using System;

namespace ArmsModels.BaseModels
{
    public class FinanceDashboardModel
    {
        public int? CoaID { get; set; }
        public DateTime? Date { get; set; } = DateTime.Today;
        public decimal? TotalAmount { get; set; }
    }
}