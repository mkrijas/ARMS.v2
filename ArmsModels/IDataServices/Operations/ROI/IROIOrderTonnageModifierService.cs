using Core.BaseModels.Operations.ROI;
using System.Collections.Generic;

namespace Core.IDataServices.Operations.ROI
{
    public interface IROIOrderTonnageModifierService
    {
        IEnumerable<ROIOrderTonnageModifierModel> Select(int? RowNo, int? BranchID);
        ROIOrderTonnageModifierModel Update(ROIOrderTonnageModifierModel model);
        int Delete(int? ID, string UserID);
    }
}