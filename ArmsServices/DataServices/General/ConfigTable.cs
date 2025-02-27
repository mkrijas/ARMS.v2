using ArmsModels.BaseModels.General;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices.General
{
    public class ConfigTable : IConfigTable
    {
        IDbService Iservice;
        public ConfigTable(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to get configuration settings by key string
        public ConfigModel GetByID(string KeyString)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", KeyString),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get the default cash COA ID
        public ConfigModel GetByDefaultCashCoaID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "DefaultCashCoaID"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get the finance bank group ID
        public ConfigModel GetByFinanceBankGroupID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "FinanceBankGroupID"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get the administrative expense group ID
        public ConfigModel GetByAdministrativeExpenceGroupID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "AdministrativeExpenceGroupID"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get the finance cash group ID
        public ConfigModel GetByFinanceCashGroupID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "FinanceBankGroupID"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get the inventory fuel group ID
        public ConfigModel GetByInventoryFuelGroupID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "InventoryFuelGroupID"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get the inventory AdBlue group ID
        public ConfigModel GetByInventoryAdBlueGroupID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "InventoryAdBlueGroupID"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get the inventory tyre group ID
        public ConfigModel GetByInventoryTyreGroupID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "InventoryTyreGroupID"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get all configuration settings
        public IEnumerable<ConfigModel> GetAll()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetAll")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Helper method to map data record to ConfigModel
        private ConfigModel GetModel(IDataRecord dr)
        {
            return new ConfigModel()
            {
                KeyString = dr.GetString("KeyString"),
                ValueString = dr.GetString("ValueString"),
            };
        }

        // Method to get asset subclass for trucks
        public ConfigModel GetAssetSubclassForTrucks()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "AssetSubClassTrucks"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get trip advance usage code
        public ConfigModel GetTripAdvanceUsageCode()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "TripAdvanceUsageCode"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get fast tag usage code
        public ConfigModel GetFastTagUsageCode()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "FasTagTollUsageCode"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get unloading charge usage code
        public ConfigModel GetUnloadingChargeUsageCode()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "LoadingAndUnloadingUsageCode"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get close trip event type ID 
        public ConfigModel GetCloseTripEventTypeID()
        {            
            return new ConfigModel() { KeyString= "CloseEvent",ValueString = "6"};
        }

        // Method to get finance payable group ID
        public ConfigModel GetFinancePayableGroupID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "FinancePayableGroupID"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get finance receivable group ID
        public ConfigModel GetFinanceReceivableGroupID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "FinanceReceivableGroupID"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get default mileage shortage COA ID
        public ConfigModel GetDefaultMileageShortageCoaID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "DefaultMileageShortageAccountID"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to get base finance group ID based on group name
        public ConfigModel GetBaseFinanceGroupId(string groupName)
        {
            string KeyString;
            switch (groupName)
            {
                case "Asset": KeyString = "AssetFinanceBaseGroupID"; break;
                case "Liability": KeyString = "LiabilityFinanceBaseGroupID"; break;
                case "Income": KeyString = "IncomeFinanceBaseGroupID"; break;
                case "Expense": KeyString = "ExpenceFinanceBaseGroupID"; break;
                case "Capital": KeyString = "CapitalFinanceBaseGroupID"; break;
                default: KeyString = string.Empty; break;
            }

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", KeyString),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;

        }

        // Method to get default mileage shortage receivable IDc
        public ConfigModel GetDefaultMileageShortageReceivableID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "DefaultMileageShortageUsageCode"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }
    }
}
