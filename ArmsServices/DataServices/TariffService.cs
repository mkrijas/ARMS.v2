using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITariffService
    {       
        TariffModel Update(TariffModel model);
        int Delete(int TariffID, string UserID);
        IEnumerable<TariffModel> Select(int? TariffID);
    }

    public class TariffService : ITariffService
    {
        IDbService Iservice;

        public TariffService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public TariffModel Update(TariffModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TariffID", model.TariffID),
               new SqlParameter("@TariffName", model.TariffName),
               new SqlParameter("@OrderID", model.OrderID),
               new SqlParameter("@TariffRate", model.TariffRate),
               new SqlParameter("@TariffTypeID", model.TariffTypeID),              
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            TariffModel rmodel = new TariffModel();
            using (var reader = Iservice.GetDataReader("[usp.Gc.TariffsUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new TariffModel
                    {
                        TariffID = reader.GetInt32("TariffID"),
                        TariffName = reader.GetString("TariffName"),
                        OrderID = reader.GetInt32("OrderID"),
                        TariffRate = reader.GetDecimal("TariffRate"),
                        TariffTypeID = reader.GetInt16("TariffTypeID"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStamp"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return rmodel;
        }
        public int Delete(int TariffID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TariffID", TariffID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Gc.TariffsDelete]", parameters);
        }
        public IEnumerable<TariffModel> Select(int? TariffID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TariffID", TariffID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Gc.TariffsSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new TariffModel
                    {
                        TariffID = reader.GetInt32("TariffID"),
                        TariffName = reader.GetString("TariffName"),
                        OrderID = reader.GetInt32("OrderID"),
                        TariffRate = reader.GetDecimal("TariffRate"),
                        TariffTypeID = reader.GetInt16("TariffTypeID"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStamp"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
        }

    }
}
