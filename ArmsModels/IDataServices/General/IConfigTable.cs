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
        ConfigModel GetByFinanceCashGroupID();
        ConfigModel GetByInventoryFuelGroupID();
        ConfigModel GetByInventoryAdBlueGroupID();
        ConfigModel GetByInventoryTyreGroupID();
        ConfigModel GetAssetSubclassForTrucks();
        ConfigModel GetTripAdvanceUsageCode();
        ConfigModel GetDefaultMileageShortageCoaID();
        ConfigModel GetCloseTripEventTypeID();
        ConfigModel GetFinancePayableGroupID();
        ConfigModel GetFinanceReceivableGroupID();
        ConfigModel GetBaseFinanceGroupId(string groupName);
        ConfigModel GetFastTagUsageCode();
        ConfigModel GetUnloadingChargeUsageCode();
        IEnumerable<ConfigModel> GetAll();
        ConfigModel GetDefaultMileageShortageReceivableID();
    }
}