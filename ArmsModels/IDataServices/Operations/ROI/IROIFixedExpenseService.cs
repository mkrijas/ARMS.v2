using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROIFixedExpenseService
    {
        ROIFixedExpenseModel Update(ROIFixedExpenseModel model);
        IEnumerable<ROIFixedExpenseModel> Select();
        ROIFixedExpenseModel SelectByID(int? ID);
    }
}