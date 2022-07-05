using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;

namespace ArmsServices
{
    public static class ext
    {
        public static string GetLatLongString(this IDataRecord dr, string name)
        {            
            SqlGeography geo = GetFieldValue<SqlGeography>(dr, name, null);
            return string.Empty;
        }
        public static bool IsDBNull(this IDataRecord dr, string name)
        {
            return dr.IsDBNull(dr.GetOrdinal(name));            
        }

        public static string GetString(this IDataRecord dr, string name)
        {
            return GetFieldValue<String>(dr, name, (string)null);
        }
        public static int? GetInt32(this IDataRecord dr, string name)
        {
            return GetFieldValue<int?>(dr, name, null);
        }    
        
        public static short? GetInt16(this IDataRecord dr, string name)
        {
            return GetFieldValue<short?>(dr, name, null);
        }
        public static byte? GetByte(this IDataRecord dr, string name)
        {
            return GetFieldValue<byte?>(dr, name, null);
        }
        public static long? GetInt64(this IDataRecord dr, string name)
        {
            return GetFieldValue<long?>(dr, name, null);
        }
        public static bool GetBoolean(this IDataRecord dr, string name)
        {
            return GetFieldValue(dr, name, false);
        }
        public static decimal? GetDecimal(this IDataRecord dr, string name)
        {
            return GetFieldValue<decimal?>(dr, name, null);
        }

        public static DateTime? GetDateTime(this IDataRecord dr, string name)
        {
            return GetFieldValue<DateTime?>(dr, name, null);
        }
        public static T GetFieldValue<T>(this IDataRecord dr, string fieldName, T defaultvalue = default(T))
        {            
            try
            {
                if (!dr.HasColumn(fieldName))
                    return defaultvalue;
                var value = dr[fieldName];
                if (value == DBNull.Value || value == null)
                    return defaultvalue;
                return (T)value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }      

        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = (typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)).Where(x => x.GetGetMethod().IsVirtual == false).ToArray();
            foreach (PropertyInfo prop in Props)
            {
                if(!prop.GetGetMethod().IsVirtual)
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            int length = Props.Where(x => x.GetGetMethod().IsVirtual == false).ToArray().Length;
            foreach (T item in items)
            {
                var values = new object[length];
                for (int i = 0; i < length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
