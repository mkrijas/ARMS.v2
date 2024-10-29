using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROIDriverIncentiveService
    {
        ROIDriverIncentiveModel Update(ROIDriverIncentiveModel model);
        IEnumerable<ROIDriverIncentiveModel> Select();
        ROIDriverIncentiveModel SelectByID(int? ID);
    }
}