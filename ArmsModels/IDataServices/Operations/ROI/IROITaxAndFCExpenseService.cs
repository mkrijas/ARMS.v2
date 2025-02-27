using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROITaxAndFCExpenseService
    {
        ROITaxAndFCExpenseModel Update(ROITaxAndFCExpenseModel model);
        IEnumerable<ROITaxAndFCExpenseModel> Select(int? BranchID);
        ROITaxAndFCExpenseModel SelectByID(int? ID);
    }
}