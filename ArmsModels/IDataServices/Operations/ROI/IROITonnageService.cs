using Core.BaseModels.Operations.ROI;
using System.Collections.Generic;

namespace Core.IDataServices.Operations.ROI
{
    public interface IROITonnageService
    {       
        IEnumerable<ROITonnageModel> SelectWheels();
        IEnumerable<ROITonnageModel> Select(int? RowNo, int? BranchID);
        ROITonnageModel Update(ROITonnageModel model);
        int Delete(int? ID, string UserID);
    }
}