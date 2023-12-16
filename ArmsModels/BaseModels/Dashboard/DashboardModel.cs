using System;

namespace ArmsModels.BaseModels
{
    public class DashboardModel
    {
        public string Label { get; set; }
        public int? Data {  get; set; }
        public DateTime? DateList { get; set; } = null;
        public decimal? Total { get; set; }
        public decimal? CumulativeFrequency { get; set; }
    }
}