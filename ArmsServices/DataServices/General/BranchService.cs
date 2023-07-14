using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class BranchService : IBranchService
    {
        IDbService Iservice;
        IAddressService _addressService;
        IPlaceService _placeService;
        IContactService _contactService;
        public BranchService(IDbService iservice, IAddressService addressService, IPlaceService placeService, IContactService contactService)
        {
            Iservice = iservice;
            _addressService = addressService;
            _placeService = placeService;
            _contactService = contactService;
        }
        public BranchModel Update(BranchModel model)
        {
            model.Address = _addressService.Update(model.Address);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@BranchName", model.BranchName),
               new SqlParameter("@BranchCode", model.BranchCode),
               new SqlParameter("@BranchAbbrev", model.BranchAbbrev),
               new SqlParameter("@CoaID", model.Coa.CoaID),
               new SqlParameter("@GstNo", model.GstNo),
               new SqlParameter("@AddressID", model.Address.AddressID),
               new SqlParameter("@PlaceID", model.PlaceID),
               new SqlParameter("@Operate", model.Operate),
               new SqlParameter("@Active", model.Active),
               new SqlParameter("@UpwardBranchID", model.UpwardBranchID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.Branch.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public BranchModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@BranchID", ID),
            };
            BranchModel model = new BranchModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.Branch.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Entity.Branch.Delete]", parameters);
        }
        public IEnumerable<BranchModel> Select()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.Branch.Select]", null))
            {
                yield return GetModel(dr);
            }
        }

        private BranchModel GetModel(IDataRecord dr)
        {
            return new BranchModel
            {
                Active = dr.GetBoolean("Active"),
                AddressID = dr.GetInt32("AddressID"),
                BranchName = dr.GetString("BranchName"),
                BranchCode = dr.GetString("BranchCode"),
                BranchAbbrev = dr.GetString("BranchAbbrev"),
                Coa = new ChartOfAccountModel() { CoaID = dr.GetInt32("CoaID") },
                BranchID = dr.GetInt32("BranchID"),
                Operate = dr.GetBoolean("Operate"),
                PlaceID = dr.GetInt32("PlaceID"),
                GstNo = dr.GetString("GstNo"),
                UpwardBranchID = dr.GetInt32("UpwardBranchID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }
        public int AddContact(int? BranchID, ContactModel contact)
        {
            contact = _contactService.Update(contact);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@UserID", contact.UserInfo.UserID),
               new SqlParameter("@contactID", contact?.ContactID??0)
            };
            return Iservice.ExecuteNonQuery("[usp.Entity.Branch.Contacts.Update]", parameters);
        }
        public IEnumerable<ContactModel> GetContacts(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "GetContacts"),
            };

            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Branch.Select]", parameters))
            {
                yield return _contactService.SelectByID(reader.GetInt32("ContactID"));
            }
        }
        public string GetBranchName(int? BranchID)
        {
            var result = SelectByID(BranchID);
            return result.BranchName;
        }
    }
}
