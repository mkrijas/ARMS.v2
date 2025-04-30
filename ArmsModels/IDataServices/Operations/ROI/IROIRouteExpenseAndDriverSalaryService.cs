using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROIRouteExpenseAndDriverSalaryService
    {
        ROIRouteExpenseAndDriverSalaryModel Update(ROIRouteExpenseAndDriverSalaryModel model);
        IEnumerable<ROIRouteExpenseAndDriverSalaryModel> Select(int? ID);
        IEnumerable<ROIRouteExpenseAndDriverSalaryModel> SelectByBranch(int? ID);
    }
}