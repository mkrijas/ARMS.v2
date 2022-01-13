
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;



namespace ArmsServices.DataServices
{
    public interface IContactService
    {
        ContactModel Update(ContactModel model);
        ContactModel SelectByID(int? ContactID);
        int Delete(int? ContactID, string UserID);
        IEnumerable<ContactModel> Select(int? RefKey,string  RefTable);

    }
    public class ContactService : IContactService
    {
        IDbService Iservice;
        public ContactService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public ContactModel Update(ContactModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ContactID", model.ContactID),
               new SqlParameter("@ContactName", model.ContactName),
               new SqlParameter("@AdditionalInfo", model.AdditionalInfo),
               new SqlParameter("@Phone", model.Phone),
               new SqlParameter("@Email", model.Email),
               new SqlParameter("@RefTable", model.RefTable),
               new SqlParameter("@RefKey", model.RefKey),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.Contacts.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public ContactModel SelectByID(int? ContactID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ContactID", ContactID),
            };
            ContactModel model = new ContactModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.Contacts.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }
        public int Delete(int? ContactID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ContactID", ContactID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Entity.Contacts.Delete]", parameters);
        }
        public IEnumerable<ContactModel> Select(int? RefKey,string RefTable)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RefKey", RefKey),
               new SqlParameter("@RefTable", RefTable),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Entity.Contacts.Select]", parameters))
            {
                yield return GetModel(dr);               
            }
        }
        private ContactModel GetModel(IDataRecord dr)
        {
            return new ContactModel()
            {
                ContactID = dr.GetInt32("ContactID"),
                ContactName = dr.GetString("ContactName"),
                AdditionalInfo = dr.GetString("AdditionalInfo"),
                Email = dr.GetString("Email"),
                Phone = dr.GetString("Phone"),
                RefKey = dr.GetInt32("RefKey"),
                RefTable = dr.GetString("RefTable"),
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
