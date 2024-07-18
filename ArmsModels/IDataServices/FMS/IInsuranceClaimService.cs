using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public interface IInsuranceClaimService
    {
        InsuranceClaimModel Update(InsuranceClaimModel model);  //Edit
        int RemovePhoto(InsuranceClaimModel model);  //Edit
        InsuranceClaimModel SelectByID(int? ID);
        InsuranceClaimModel SelectByBreakdownID(int? ID);
        int Delete(int? InsuranceClaimID, string UserID);  //Delete
        IEnumerable<InsuranceClaimModel> Select(int? InsuranceClaimID);
        IEnumerable<InsuranceClaimEventMasterModel> GetEventList(int? limiter);
        InsuranceClaimEventMasterModel UpdateEventList(InsuranceClaimEventMasterModel model);  //UpdateList
        InsuranceClaimEventStatusModel UpdateClaimEvent(InsuranceClaimEventStatusModel model);  //UpdateList
        IEnumerable<InsuranceClaimEventStatusModel> GetEventStatusList(int? InsuranceClaimID);
        int OrderMoveUpward(int? Order);  //UpdateOrder
    }
}