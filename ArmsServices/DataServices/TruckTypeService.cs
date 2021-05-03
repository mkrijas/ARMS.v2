using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITruckTypeService
    {       
        TruckTypeModel Update(TruckTypeModel model);
        int Delete(int TruckTypeID, string UserID);
        IEnumerable<TruckTypeModel> Select(int? TruckTypeID);
    }

    public class TruckTypeService : ITruckTypeService
    {
        IDbService Iservice;

        public TruckTypeService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public TruckTypeModel Update(TruckTypeModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TruckTypeID", model.TruckTypeID),
               new SqlParameter("@TruckType", model.TruckType),
               new SqlParameter("@Axles", model.Axles),
               new SqlParameter("@GrossWeight", model.GrossWeight),
               new SqlParameter("@UnladenWeight", model.UnladenWeight),
               new SqlParameter("@wheels", model.wheels),              
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            TruckTypeModel rmodel = new TruckTypeModel();
            using (var reader = Iservice.GetDataReader("[usp.Truck.TruckTypesUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new TruckTypeModel
                    {
                        TruckTypeID = reader.GetInt16("TruckTypeID"),
                        TruckType = reader.GetString("TruckType"),
                        Axles = reader.GetByte("Axles"),
                        GrossWeight = reader.GetDecimal("GrossWeight"),
                        UnladenWeight = reader.GetDecimal("UnladenWeight"),
                        wheels = reader.GetByte("wheels"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return rmodel;
        }
        public int Delete(int TruckTypeID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckTypeID", TruckTypeID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Gc.TruckTypesDelete]", parameters);
        }
        public IEnumerable<TruckTypeModel> Select(int? TruckTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckTypeID", TruckTypeID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Truck.TruckTypesSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new TruckTypeModel
                    {
                        TruckTypeID = reader.GetInt16("TruckTypeID"),
                        TruckType = reader.GetString("TruckType"),
                        Axles = reader.GetByte("Axles"),
                        GrossWeight = reader.GetDecimal("GrossWeight"),
                        UnladenWeight = reader.GetDecimal("UnladenWeight"),
                        wheels = reader.GetByte("wheels"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
        }

    }
}
