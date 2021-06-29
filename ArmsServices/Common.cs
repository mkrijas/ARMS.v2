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
        public static string GetString(this IDataRecord dr, string name)
        {
            return GetFieldValue<String>(dr, name, (string)null);
        }
        public static int GetInt32(this IDataRecord dr, string name)
        {
            return GetFieldValue(dr, name, 0);
        }
        public static int? GetInt32Nullable(this IDataRecord dr, string name)
        {
            return GetFieldValue(dr, name, 0);
        }
        public static short GetInt16(this IDataRecord dr, string name)
        {
            return GetFieldValue<short>(dr, name, 0);
        }
        public static byte GetByte(this IDataRecord dr, string name)
        {
            return GetFieldValue<byte>(dr, name, 0);
        }
        public static long GetInt64(this IDataRecord dr, string name)
        {
            return GetFieldValue<long>(dr, name, 0);
        }
        public static bool GetBoolean(this IDataRecord dr, string name)
        {
            return GetFieldValue(dr, name, false);
        }
        public static decimal GetDecimal(this IDataRecord dr, string name)
        {
            return GetFieldValue<decimal>(dr, name, 0);
        }

        public static DateTime GetDateTime(this IDataRecord dr, string name)
        {
            return GetFieldValue(dr, name, DateTime.MinValue);
        }
        public static T GetFieldValue<T>(this IDataRecord dr, string fieldName, T defaultvalue = default(T))
        {
            try
            {
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
        public static string SafeGetString(this IDataRecord reader, string colName)
        {
            int colIndex = reader.GetOrdinal(colName);
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }

        public static int? SafeGetInt(this IDataRecord reader, string colName)
        {
            int colIndex = reader.GetOrdinal(colName);
            if (!reader.IsDBNull(colIndex))
                return reader.GetInt32(colIndex);
            return null;
        }

        public static DateTime? SafeGetDateTime(this IDataRecord reader, string colName)
        {
            int colIndex = reader.GetOrdinal(colName);
            if (!reader.IsDBNull(colIndex))
                return reader.GetDateTime(colIndex);
            return null;
        }


        public static DataTable GetDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
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
