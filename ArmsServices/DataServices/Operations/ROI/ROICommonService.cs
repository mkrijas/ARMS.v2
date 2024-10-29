using ArmsServices;
using Core.BaseModels.Operations.ROI;
using Core.IDataServices.Operations.ROI;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace DAL.DataServices.Operations.ROI
{
    public class ROICommonService : IROICommonService
    {
        IDbService Iservice;
        IROITonnageService IROITonnage;
        public ROICommonService(IDbService iservice, IROITonnageService iROITonnage)
        {
            Iservice = iservice;
            IROITonnage = iROITonnage;
        }

        public IEnumerable<ROITonnageModel> SelectBSType()
        {
            List<ROITonnageModel> Model = new();
            return Model = IROITonnage.SelectBSType().ToList();
        }
    }
}