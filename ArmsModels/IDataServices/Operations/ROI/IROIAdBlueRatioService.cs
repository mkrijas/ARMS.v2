using ArmsModels.BaseModels;
using System.Collections.Generic;

namespace ArmsServices.DataServices
{
    public interface IROIAdBlueRatioService
    {
        ROIAdBlueStdModel Update(ROIAdBlueStdModel model);
        IEnumerable<ROIAdBlueStdModel> Select(int? BranchID);
        ROIAdBlueStdModel SelectByID(int? ID);
    }   
}
