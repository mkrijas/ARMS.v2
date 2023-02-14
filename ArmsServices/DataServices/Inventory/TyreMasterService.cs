using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArmsServices.DataServices.Inventory
{
    public interface ITyreMasterService
    {

        TyreModel Update(TyreModel model);
        IEnumerable<TyreModel> SelectTyre(int? TyreID);
        int Delete(int? ID, string UserID);
    }
    public class TyreMasterService: ITyreMasterService
    {

        IDbService Iservice;

        public TyreMasterService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Inventory.Tyre.Delete]", parameters);
        }


        public IEnumerable<TyreModel> SelectTyre(int? TyreID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TyreID", TyreID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }



        public TyreModel Update(TyreModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {

               new SqlParameter("@TyreID", model.TyreID),
               new SqlParameter("@TyreSerialNumber", model.TyreSerialNumber),
               new SqlParameter("@Make", model.Make),
               new SqlParameter("@InventoryItemID", model.InventoryItemID),
               new SqlParameter("@InventoryBatchID", model.InventoryBatchID),
               new SqlParameter("@TyreType", model.TyreType),
               new SqlParameter("@TyreSize", model.TyreSize),
               new SqlParameter("@Tubeless", model.Tubeless),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@RecordStatus", 3),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Inventory.Tyre.Update]", parameters))
            {
                return GetModel(dr);
            }
            return null;
        }


        private TyreModel GetModel(IDataRecord dr)
        {
            return new TyreModel
            {
                TyreID = dr.GetInt32("TyreID"),
                TyreSerialNumber = dr.GetString("TyreSerialNumber"),
                Make = dr.GetString("Make"),
                InventoryItemID = dr.GetInt32("InventoryItemID"),
                InventoryBatchID = dr.GetInt32("InventoryBatchID"),
                TyreType = dr.GetString("TyreType"),
                TyreSize = dr.GetString("TyreSize"),
                Tubeless = dr.GetInt32("Tubeless") == 1 ? true:false,
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserId"),
                },
            };
        }
    }
}
