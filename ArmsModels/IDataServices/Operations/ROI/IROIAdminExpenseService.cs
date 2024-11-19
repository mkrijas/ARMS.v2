using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROIAdminExpenseService
    {
        ROIAdminExpenseModel Update(ROIAdminExpenseModel model);
        IEnumerable<ROIAdminExpenseModel> Select();
        ROIAdminExpenseModel SelectByID(int? ID);
    }
}