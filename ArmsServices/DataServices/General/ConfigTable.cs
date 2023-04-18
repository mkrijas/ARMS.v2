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
        IEnumerable<ConfigModel> GetAll();
    }
    public class ConfigTable: IConfigTable
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
    }
}
