using System;

namespace ArmsModels.BaseModels
{
    public class DashboardModel
    {
        public string Label { get; set; }
        public int? Data {  get; set; }
        public DateTime? BillDate { get; set; } = null;
        public virtual decimal? Total { get; set; }
    }
}