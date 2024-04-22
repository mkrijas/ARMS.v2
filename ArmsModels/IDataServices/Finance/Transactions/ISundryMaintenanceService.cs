
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;


namespace ArmsServices.DataServices
{
    public interface ISundryMaintenanceService : IbaseInterface<SundryMaintenanceModel>
    {
        IEnumerable<SundryMaintenanceModel> Select();        
        IEnumerable<SundryMaintenanceModel> SelectByParty(int? PartyID, int? PartyBranchID);
        IEnumerable<SundryMaintenanceModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<SundryMaintenanceEntryModel> GetEntries(int? ID);   
    }
}