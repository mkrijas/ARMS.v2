using System;

namespace ArmsModels.BaseModels
{
    public class DashboardModel
    {
        public DateTime? BillDate { get; set; } = null;
        public virtual decimal? TotalBillQuantity { get; set; }
    }
}