using System;

namespace ArmsModels.BaseModels
{
    // Model representing a dashboard item
    public class DashboardModel
    {
        public string Label { get; set; }
        public int? Data {  get; set; }
        public DateTime? DateList { get; set; } = null;
        public decimal? Total { get; set; }
        public decimal? CumulativeTarget { get; set; }
    }
}