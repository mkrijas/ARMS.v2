using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROITollService
    {
        ROITollModel Update(ROITollModel model);
        IEnumerable<ROITollModel> Select();
        ROITollModel SelectByID(int? ID);
    }
}