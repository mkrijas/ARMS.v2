using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IPartyBranchService
    {       
        PartyBranchModel Update(PartyBranchModel model);
        int Delete(int? GstID, string UserID);
        IEnumerable<PartyBranchModel> Select(int? GstID);
        IEnumerable<PartyBranchModel> SelectByParty(int? PartyID = 0);
        bool IsCgst(int BranchID, int PartyBranchID);
    }

    public class PartyBranchService : IPartyBranchService
    {
        IDbService Iservice;
        IAddressService _addressService;
        IBankAccountService _bankAccountService;

        public PartyBranchService(IDbService iservice,IAddressService addressService,IBankAccountService bankAccountService)
        {
            Iservice = iservice;
            _addressService = addressService;
            _bankAccountService = bankAccountService;
        }
        public PartyBranchModel Update(PartyBranchModel model)
        {
            AddressModel addressModel = _addressService.Update(model.Address);
            model.BankAccount.UserInfo = model.UserInfo;
            model.BankAccount = _bankAccountService.Update(model.BankAccount);
            model.AddressID = addressModel.AddressID;

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyBranchID", model.GstID),
               new SqlParameter("@AddressID", model.AddressID),              
               new SqlParameter("@GstNo", model.GstNo),
               new SqlParameter("@PartyID", model.PartyID),
               new SqlParameter("@AccountID", model.Coa.CoaID),
               new SqlParameter("@BankAccountID", model.BankAccount.BankAccountID),
               new SqlParameter("@RegName", model.RegName),
               new SqlParameter("@TanNo", model.TanNo),
               new SqlParameter("@TradeName", model.TradeName),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            
            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Branch.Update]", parameters))
            {               
                    model = GetModel(reader);
            }
            return model;
        }
        public int Delete(int? GstID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GstID", GstID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Entity.Party.Branch.Delete]", parameters);
        }
        public IEnumerable<PartyBranchModel> Select(int? GstID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", GstID),
               new SqlParameter("@Operation", "ByID")
            };

            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Branch.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        public IEnumerable<PartyBranchModel> SelectByParty(int? PartyID = 0)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", PartyID),
               new SqlParameter("@Operation", "SelectByParty"),
            };

            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Branch.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        public bool IsCgst(int BranchID, int PartyBranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyBranchID", PartyBranchID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@IsCgst", BranchID){Direction = ParameterDirection.Output},
            };
            Iservice.ExecuteNonQuery("[usp.Finance.Taxes.Gst.IsCgst]", parameters);
            return bool.Parse(parameters[2].Value.ToString());
        }

        private PartyBranchModel GetModel(IDataRecord reader)
        {
            return new PartyBranchModel()
            {
                GstID = reader.GetInt32("PartyBranchID"),
                AddressID = reader.GetInt32("AddressID"),
                GstNo = reader.GetString("GstNo"),
                PartyID = reader.GetInt32("PartyID"),
                Coa = new ChartOfAccountModel() { CoaID = reader.GetInt32("CoaID")},
                BankAccount = new BankAccountModel() { BankAccountID = reader.GetInt32("BankAccountID")},
                RegName = reader.GetString("RegName"),
                TanNo = reader.GetString("TanNo"),
                TradeName = reader.GetString("TradeName"),
                Address = _addressService.SelectByID(reader.GetInt32("AddressID").GetValueOrDefault()),
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
