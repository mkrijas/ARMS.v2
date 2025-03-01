using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class TyrePositionService : ITyrePositionService
    {
        IDbService Iservice;
        public TyrePositionService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to delete a tyre position by its ID
        public int Delete(int? ID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TyreID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.TyrePosition.Delete]", parameters);
        }

        // Method to update a tyre position
        public TyrePositionModel Update(TyrePositionModel model, int? TruckTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckType", TruckTypeID),
               new SqlParameter("@Side", model.Side),
               new SqlParameter("@Description", model.Description),
               new SqlParameter("@PositionID", model.PositionID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.TyrePosition.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }

        // Method to select tyre positions by truck type
        public IEnumerable<TyrePositionModel> Select(int? TruckType)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TruckType", TruckType),
               new SqlParameter("@Operation", "ByTrucktype"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.TyrePosition.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select all tyre positions
        public IEnumerable<TyrePositionModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Master"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.TyrePosition.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Helper method to map data record to TyrePositionModelc
        private TyrePositionModel GetModel(IDataRecord dr)
        {
            return new TyrePositionModel()
            {
                PositionID = dr.GetInt32("PositionID"),
                Description = dr.GetString("Description"),
                Side = dr.GetString("Side"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }
    }
}


