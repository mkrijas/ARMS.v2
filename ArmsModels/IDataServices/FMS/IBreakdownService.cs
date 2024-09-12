using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IBreakdownService
    {
        BreakdownModel Update(BreakdownModel model);  //Edit
        BreakdownModel SelectByID(int? ID);
        int Delete(int? BreakdownID, string UserID);  //Delete
        IEnumerable<BreakdownModel> Select();
        IEnumerable<BreakdownModel> SelectPending(int BranchID);
        IEnumerable<EstimateListModel> SelectEstimate(int? BreakdownID);
        EstimateListModel UpdateEstimate(EstimateListModel model);  //Edit
        int DeleteEstimate(int? EstimateID, string UserID);  //Delete
        int ApproveEstimate(int? EstimateID, string UserID);  //Approve
        int RemoveEstimate(int? EstimateID, string UserID);  //Approve
        int AddImgEstimate(int? EstimateID, string ImgPath, string UserID);  //Approve
    }
}