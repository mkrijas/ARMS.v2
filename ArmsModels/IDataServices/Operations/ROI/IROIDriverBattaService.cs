using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IROIDriverBattaService
    {
        ROIDriverBattaModel Update(ROIDriverBattaModel model);
        IEnumerable<ROIDriverBattaModel> Select(int? BranchID);
        ROIDriverBattaModel SelectByID(int? ID);
        ROIDriverBattaInFrtPercentageModel UpdateInFrtPercentage(ROIDriverBattaInFrtPercentageModel model);
        IEnumerable<ROIDriverBattaInFrtPercentageModel> SelectInFrtPercentage(int? BranchID);
        ROIDriverBattaInFrtPercentageModel SelectByIDInFrtPercentage(int? ID);
    }
}