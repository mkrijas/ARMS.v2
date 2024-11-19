using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROITaxAndFCExpenseService
    {
        ROITaxAndFCExpenseModel Update(ROITaxAndFCExpenseModel model);
        IEnumerable<ROITaxAndFCExpenseModel> Select();
        ROITaxAndFCExpenseModel SelectByID(int? ID);
    }
}