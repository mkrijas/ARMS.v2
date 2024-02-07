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

        private ConfigModel GetModel(IDataRecord dr)
        {
            return new ConfigModel()
            {
                KeyString = dr.GetString("KeyString"),
                ValueString = dr.GetString("ValueString"),
            };
        }

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

        public ConfigModel GetCloseTripEventTypeID()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@KeyString", "CloseTripEventTypeID"),
               new SqlParameter("@Operation", "ByID")
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.ConfigTable.Select]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

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
    }
}
