using Core.BaseModels.Operations.ROI;
using System.Collections.Generic;

namespace Core.IDataServices.Operations.ROI
{
    public interface IROICommonService
    {
        IEnumerable<ROITonnageModel> SelectBSType();
    }
}