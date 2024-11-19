using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROITyreAndMaintenanceExpenseService
    {
        ROITyreAndMaintenanceExpenseModel Update(ROITyreAndMaintenanceExpenseModel model);
        IEnumerable<ROITyreAndMaintenanceExpenseModel> Select();
        ROITyreAndMaintenanceExpenseModel SelectByID(int? ID);
    }
}