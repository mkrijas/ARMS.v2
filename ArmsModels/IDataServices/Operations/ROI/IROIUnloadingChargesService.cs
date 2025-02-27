using System.Collections.Generic;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROIUnloadingChargesService
    {
        ROIUnloadingChargesModel Update(ROIUnloadingChargesModel model);
        IEnumerable<ROIUnloadingChargesModel> Select(int? BranchID);
        ROIUnloadingChargesModel SelectByID(int? ID);
    }
}