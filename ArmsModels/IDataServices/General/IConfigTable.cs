using ArmsModels.BaseModels.General;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices.General
{
    public interface IConfigTable
    {
        ConfigModel GetByID(string KeyString);
        ConfigModel GetByDefaultCashCoaID();
        ConfigModel GetByFinanceBankGroupID();
        ConfigModel GetByAdministrativeExpenceGroupID();
        ConfigModel GetByFinanceCashGroupID();
        ConfigModel GetByInventoryFuelGroupID();
        ConfigModel GetByInventoryTyreGroupID();
        ConfigModel GetAssetSubclassForTrucks();
        ConfigModel GetTripAdvanceUsageCode();
        ConfigModel GetDefaultMileageShortageCredit();
        ConfigModel GetCloseTripEventTypeID();
        ConfigModel GetFinancePayableGroupID();
        ConfigModel GetFinanceReceivableGroupID();
        ConfigModel GetBaseFinanceGroupId(string groupName);
        IEnumerable<ConfigModel> GetAll();
    }
}