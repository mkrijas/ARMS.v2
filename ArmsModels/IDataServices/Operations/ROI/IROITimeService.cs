using Core.BaseModels.Operations.ROI;
using System.Collections.Generic;

namespace Core.IDataServices.Operations.ROI
{
    public interface IROITimeService
    {
        IEnumerable<ROIWheelSpeedModel> SelectWheelSpeed(int? RowNo);
        ROIWheelSpeedModel UpdateWheelSpeed(ROIWheelSpeedModel model);
        IEnumerable<ROILoadAndUnloadModel> SelectLoadUnload(int? RowNo, int? BranchID);
        ROILoadAndUnloadModel UpdateLoadUnload(ROILoadAndUnloadModel model);
    }
}