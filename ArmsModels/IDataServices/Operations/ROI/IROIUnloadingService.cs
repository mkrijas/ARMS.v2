using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROIUnloadingService
    {
        ROIUnloadingModel Update(ROIUnloadingModel model);
        IEnumerable<ROIUnloadingModel> Select();
        ROIUnloadingModel SelectByID(int? ID);
    }
}