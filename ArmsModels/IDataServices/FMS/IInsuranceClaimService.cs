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
        InsuranceClaimModel Update(InsuranceClaimModel model);
        InsuranceClaimModel SelectByID(int? ID);
        InsuranceClaimModel SelectByBreakdownID(int? ID);
        int Delete(int? InsuranceClaimID, string UserID);
        IEnumerable<InsuranceClaimModel> Select(int? InsuranceClaimID);
        IEnumerable<InsuranceClaimEventMasterModel> GetEventList(int? limiter);
        InsuranceClaimEventMasterModel UpdateEventList(InsuranceClaimEventMasterModel model);
        InsuranceClaimEventStatusModel UpdateClaimEvent(InsuranceClaimEventStatusModel model);
        IEnumerable<InsuranceClaimEventStatusModel> GetEventStatusList(int? InsuranceClaimID);
        int OrderMoveUpward(int? Order);
    }
}