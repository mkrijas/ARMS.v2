using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IPartyService
    {       
        PartyModel Update(PartyModel model);
        PartyModel SelectByID(int? ID);
        int Delete(int? PartyID, string UserID);
        IEnumerable<PartyModel> Select(int? PartyID);
        PartyModel SelectByCode(string PartyCode);
        bool IsCgst(int BranchID, int PartyID);
        IEnumerable<ContactModel> GetContacts(int? PartyID);
    }

    public class PartyService : IPartyService
    {
        IDbService Iservice;
        IAddressService _addressService;
        IBankAccountService _bankAccountService;

        public PartyService(IDbService iservice, IAddressService addressService, IBankAccountService bankAccountService)
        {
            Iservice = iservice;
            _addressService = addressService;
            _bankAccountService = bankAccountService;
        }


        public async Task<PartyModel> Update(PartyModel model)
        {
            AddressModel addressModel = _addressService.Update(model.Address);
            model.BankAccount.UserInfo = model.UserInfo;
            model.BankAccount = _bankAccountService.Update(model.BankAccount);
            model.Address = addressModel;

            List<SqlParameter> parameters = new List<SqlParameter>
            {

              /*  PartyID = reader.GetInt32("PartyID"),
                TradeName = reader.GetString("TradeName"),
                Address = new AddressModel() { AddressID = reader.GetInt32("AddressID") },
                AssesseeType = reader.GetString("AssesseeType"),
                BankAccount = new BankAccountModel() { BankAccountID = reader.GetInt32("BankAccountID") },
                CreditLimit = reader.GetInt32("CreditLimit"),
                CreditPeriod = reader.GetInt32("CreditPeriod"),
                GstNo = reader.GetString("GstNo"),
                GstRegType = reader.GetString("GstRegType"),
                GstType = reader.GetString("GstType"),
                IcPartnerCode = reader.GetString("IcPartnerCode"),
                InterCompany = reader.GetBoolean("InterCompany"),
                NatureOfBusiness = reader.GetString("NatureOfBusiness"),
                PanAvailable = reader.GetBoolean("PanAvailable"),
                PartyCode = reader.GetString("PartyCode"),
                PaymentMode = reader.GetString("PaymentMode"),
                PostingGroup = new VendorPostingGroup() { VendorPostingGroupID = reader.GetInt32("VendorPostingGroupID") },
                PAN = reader.GetString("PAN"),
                RegName = reader.GetString("RegName"),
                TanNo = reader.GetString("TanNo"),
                TdsApplicable = reader.GetBoolean("TdsApplicable"), */

                new SqlParameter("@PartyID", model.PartyID),
               new SqlParameter("@TradeName", model.TradeName),
               new SqlParameter("@NatureOfBusiness", model.NatureOfBusiness),
               new SqlParameter("@IsSupplier", model.),
               new SqlParameter("@NatureOfFirm", model.NatureOfFirm),
               new SqlParameter("@AssesseeTypeID", model.AssesseeTypeID),
               new SqlParameter("@PAN", model.PAN),
               new SqlParameter("@TcsApplicable", model.TcsApplicable),
               new SqlParameter("@TdsApplicable", model.TdsApplicable),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            await foreach (IDataRecord reader in Iservice.GetDataReaderAsync("[usp.Entity.PartyUpdate]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }

        public async Task<PartyModel> SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            PartyModel model = new PartyModel();
            await foreach( IDataRecord reader in Iservice.GetDataReaderAsync("[usp.Entity.PartySelect]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }


        public async Task<int> Delete(int? PartyID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", PartyID),               
               new SqlParameter("@UserID", UserID),
            };            
            return await Iservice.ExecuteNonQueryAsync("[usp.Entity.PartyDelete]", parameters);
        }


        public async IAsyncEnumerable<PartyModel> Select(int? PartyID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@Operation", "ByID"),
            };

            await foreach (IDataRecord reader in Iservice.GetDataReaderAsync("[usp.Entity.PartySelect]", parameters))
            {
                yield return GetModel(reader);
               
            }
        }

        public bool IsCgst(int BranchID, int PartyID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsCgst", BranchID){Direction = ParameterDirection.Output},
            };
            Iservice.ExecuteNonQuery("[usp.Finance.Taxes.Gst.IsCgst]", parameters);
            return bool.Parse(parameters[2].Value.ToString());
        }

        private PartyModel GetModel(IDataRecord reader)
        {
            return new PartyModel()
            {
                PartyID = reader.GetInt32("PartyID"),
                TradeName = reader.GetString("TradeName"),
                Address = new AddressModel() { AddressID = reader.GetInt32("AddressID") },
                AssesseeType = reader.GetString("AssesseeType"),
                BankAccount = new BankAccountModel() { BankAccountID = reader.GetInt32("BankAccountID") },
                CreditLimit = reader.GetInt32("CreditLimit"),
                CreditPeriod = reader.GetInt32("CreditPeriod"),
                GstNo=reader.GetString("GstNo"),
                GstRegType = reader.GetString("GstRegType"),
                GstType = reader.GetString("GstType"),
                IcPartnerCode = reader.GetString("IcPartnerCode"),
                InterCompany = reader.GetBoolean("InterCompany"),
                NatureOfBusiness = reader.GetString("NatureOfBusiness"),
                PanAvailable = reader.GetBoolean("PanAvailable"),
                PartyCode = reader.GetString("PartyCode"),
                PaymentMode = reader.GetString("PaymentMode"),
                PostingGroup = new VendorPostingGroup() { VendorPostingGroupID = reader.GetInt32("VendorPostingGroupID") },
                PAN = reader.GetString("PAN"),
                RegName = reader.GetString("RegName"),
                TanNo = reader.GetString("TanNo"),
                TdsApplicable = reader.GetBoolean("TdsApplicable"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = reader.GetByte("RecordStatus"),
                    TimeStampField = reader.GetDateTime("TimeStamp"),
                    UserID = reader.GetString("UserID"),
                },
            };
        }

        public PartyModel SelectByCode(String PartyCode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyCode", PartyCode),
               new SqlParameter("@Operation", "ByPartyCode"),
            };
            PartyModel party = null;
            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.PartySelect]", parameters))
            {
                party = GetModel(reader);
            }
            return party;
        }
    }
}
