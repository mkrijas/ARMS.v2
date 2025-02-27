using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROILoadingChargesService
    {
        ROILoadingChargesModel Update(ROILoadingChargesModel model);
        IEnumerable<ROILoadingChargesModel> Select(int? BranchID);
        ROILoadingChargesModel SelectByID(int? ID);
    }
}