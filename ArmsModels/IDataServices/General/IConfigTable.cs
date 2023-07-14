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
        ConfigModel GetByInventoryTyreGroupID();
        ConfigModel GetAssetSubclassForTrucks();
        ConfigModel GetDefaultMileageShortageCredit();
        ConfigModel GetBaseFinanceGroupId(string groupName);
        IEnumerable<ConfigModel> GetAll();
    }
}