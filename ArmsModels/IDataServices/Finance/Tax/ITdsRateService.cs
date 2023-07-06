using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITdsRateService
    {
        TdsRateModel Update(TdsRateModel model);
        TdsRateModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<TdsRateModel> SelectByIDT(int? ID);
        IEnumerable<TdsRateModel> Select(int? AssesseeType,int? TdsNPID, DateTime? EntryDate);
        IEnumerable<NatureOfPaymentModel> SelectTdsNP();
        IEnumerable<AssesseeTypeModel> SelectAssesseeTypes();
        decimal GetTdsRate(int? PartyID, int? AccountID);
    }
}