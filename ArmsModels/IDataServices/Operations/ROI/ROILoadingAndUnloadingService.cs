using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROILoadingAndUnloadingService
    {
        ROILoadingAndUnloadingModel Update(ROILoadingAndUnloadingModel model);
        IEnumerable<ROILoadingAndUnloadingModel> Select();
        ROILoadingAndUnloadingModel SelectByID(int? ID);
    }
}