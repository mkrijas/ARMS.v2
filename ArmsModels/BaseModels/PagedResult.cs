using System.Collections.Generic;

namespace ArmsModels.BaseModels
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();

        public int? TotalRecords { get; set; }
    }
}