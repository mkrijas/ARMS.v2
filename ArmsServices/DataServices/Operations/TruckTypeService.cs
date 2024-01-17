using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
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
               new SqlParameter("@BSType", model.BSType),
               new SqlParameter("@Axles", model.Axles),
               
               new SqlParameter("@wheels", model.wheels),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            foreach (IDataReader dr in Iservice.GetDataReader("[usp.Truck.Type.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public int Delete(int? TruckTypeID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckTypeID", TruckTypeID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Truck.Type.Delete]", parameters);
        }
        public IEnumerable<TruckTypeModel> Select(short? TruckTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckTypeID", TruckTypeID)
            };

            foreach (IDataReader dr in Iservice.GetDataReader("[usp.Truck.Type.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public TruckTypeModel SelectByID(short? TruckTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckTypeID", TruckTypeID)
            };
            TruckTypeModel model = new TruckTypeModel();
            foreach (IDataReader dr in Iservice.GetDataReader("[usp.Truck.Type.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }


        private TruckTypeModel GetModel(IDataRecord reader)
        {
            return new TruckTypeModel
            {
                TruckTypeID = reader.GetInt16("TruckTypeID"),
                TruckType = reader.GetString("TruckType"),
                BSType = reader.GetString("BSType"),
                Axles = reader.GetByte("Axles"),
                
                wheels = reader.GetByte("wheels"),
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
