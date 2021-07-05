using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IBranchService
    {
        Task<BranchModel> Update(BranchModel model);
        Task<BranchModel> SelectByID(int ID);
        Task<int> Delete(int AddressID, string UserID);
        IAsyncEnumerable<BranchModel> Select();

    }
    public class BranchService: IBranchService
    {
        IDbService Iservice;
        IAddressService _addressService;
        IPlaceService _placeService;
        public BranchService(IDbService iservice,IAddressService addressService,IPlaceService placeService)
        {
            Iservice = iservice;
            _addressService = addressService;
            _placeService = placeService;
        }
        public async Task<BranchModel> Update(BranchModel model)
        {
            model.Address = await _addressService.Update(model.Address);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@BranchName", model.BranchName),
               new SqlParameter("@AddressID", model.Address.AddressID),
               new SqlParameter("@PlaceID", model.PlaceID),
               new SqlParameter("@Operate", model.Operate),
               new SqlParameter("@Active", model.Active),
               new SqlParameter("@UpwardBranchID", model.UpwardBranchID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Entity.Branch.Update]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }
        public async Task<BranchModel> SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BranchID", ID),
            };
            BranchModel model = new BranchModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Entity.Branch.Select]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }
        public async Task<int> Delete(int ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return await Iservice.ExecuteNonQueryAsync("[usp.Entity.Branch.Delete]", parameters);
        }
        public async IAsyncEnumerable<BranchModel> Select()
        {
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Entity.Branch.Select]",null))
            {
                yield return await GetModel(dr);
            }
        }

        private async Task<BranchModel> GetModel(IDataRecord dr)
        {
            return new BranchModel
            {
                Active = dr.GetBoolean("Active"),
                AddressID = dr.GetInt32("AddressID"),
                BranchName = dr.GetString("BranchName"),
                BranchID = dr.GetInt32("BranchID"),
                Operate = dr.GetBoolean("Operate"),
                PlaceID = dr.GetInt32("PlaceID"),
                UpwardBranchID = dr.GetInt32("UpwardBranchID"),
                Place = await _placeService.SelectByID(dr.GetInt32("PlaceID")),
                Address = await _addressService.SelectByID(dr.GetInt32("AddressID")),
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
