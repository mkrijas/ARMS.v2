using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROITaggingService
    {
        ROITaggingModel Update(ROITaggingModel model);
        IEnumerable<ROITaggingModel> Select(int? BranchID);
        ROITaggingModel SelectByID(int? ID);
    }
}