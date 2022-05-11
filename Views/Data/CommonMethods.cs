using MudBlazor;
using System.Collections.Generic;

namespace Views.Data
{
    public static class CommonMethods
    {
        public static bool CheckPeriodOverlap(DateRange selectedRange, List<DateRange> ListOfDates)
        {
          return  ListOfDates.Exists(dateRange => dateRange.Start <= selectedRange.End && selectedRange.Start <= dateRange.End);          
        }
    }
}
