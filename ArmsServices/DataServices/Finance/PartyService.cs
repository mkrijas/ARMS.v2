using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;


namespace ArmsServices.DataServices
{
    public class PartyService : IPartyService
    {
        IDbService Iservice;
        IAddressService _addressService;
        IBankAccountService _bankAccountService;
        IContactService _contactService;
        IVendorPostingGroupService _vendor;
        ICustomerPostingGroupService _customer;
        IRenterPostingGroupService _renter;
        ISisterPostingGroupService _sister;
        IBranchService _branch;

        public PartyService(IDbService iservice, 
            IAddressService addressService,
            IBankAccountService bankAccountService, 
            IContactService contactService,
            IVendorPostingGroupService vendor, 
            ICustomerPostingGroupService customer,
            IRenterPostingGroupService renter, 
            ISisterPostingGroupService sister,
            IBranchService branch)
        {
            Iservice = iservice;
            _addressService = addressService;
            _bankAccountService = bankAccountService;
            _contactService = contactService;
            _vendor = vendor;
            _customer = customer;
            _renter = renter;
            _sister = sister;
            _branch = branch;
        }


        public PartyModel Update(PartyModel model)
        {
            AddressModel addressModel = _addressService.Update(model.Address);
            if (model.BankAccount != null)
            {
                model.BankAccount.UserInfo = model.UserInfo;
                model.BankAccount = _bankAccountService.Update(model.BankAccount);
            }
            model.Address = addressModel;
            var ContactList = model.Contacts;
            var UserId = model.UserInfo.UserID;
            List<SqlParameter> parameters = new List<SqlParameter>
                {
                   new SqlParameter("@PartyID", model.PartyID),
                   new SqlParameter("@TradeName", model.TradeName),
                   new SqlParameter("@NatureOfFirm", model.NatureOfBusiness),
                   new SqlParameter("@AddressID", model.Address?.AddressID??null),
                   new SqlParameter("@AssesseeType", model.AssesseeType),
                   new SqlParameter("@BankAccountID", model.BankAccount?.BankAccountID??null),
                   new SqlParameter("@PAN", model.PAN),
                   new SqlParameter("@CreditLimit", model.CreditLimit),
                   new SqlParameter("@CreditPeriod", model.CreditPeriod),
                   new SqlParameter("@GstNo", model.GstNo),
                   new SqlParameter("@GstStateCode", model.GstStateCode),
                   new SqlParameter("@GstRegType", model.GstRegType),
                   new SqlParameter("@GstType", model.GstType),
                   new SqlParameter("@IcPartnerCode", model.IcPartnerCode),
                   new SqlParameter("@InterCompany", model.InterCompany),
                   new SqlParameter("@PanAvailable", model.PanAvailable),
                   //new SqlParameter("@PartyCode", model.PartyCode),
                   new SqlParameter("@PaymentMode", model.PaymentMode),
                   new SqlParameter("@VenderPostingID", model.VendorPostingGroup?.VendorPostingGroupID??null),
                   new SqlParameter("@CustomerPostingID", model.CustomerPostingGroup?.CustomerPostingGroupID??null),
                   new SqlParameter("@RentPostingID", model.RenterPostingGroup?.RenterPostingGroupID??null),
                   new SqlParameter("@SisterPostingID", model.SisterPostingGroup?.SisterPostingGroupID??null),
                   new SqlParameter("@RegName", model.RegName),
                   new SqlParameter("@TanNo", model.TanNo),
                   new SqlParameter("@TdsApplicable", model.TdsApplicable),
                   new SqlParameter("@UserID", model.UserInfo.UserID),
                };
            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Update]", parameters))
            {
                model = GetModel(reader);
            }
            List<int?> CurrentPartyContactIDs = new List<int?>();
            List<SqlParameter> parametersGetContactID = new List<SqlParameter>
            {
                 new SqlParameter("@PartyID", model.PartyID)
            };
            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Contacts.GetAllContactIDForPartyID]", parametersGetContactID))
            {
                CurrentPartyContactIDs.Add(reader.GetInt32("ContactID"));
            }
            List<IntList> ContactIDsForDetete = new List<IntList>();
            foreach (var item in CurrentPartyContactIDs)
            {
                if(item!= null && !(ContactList.Any(d=>d.ContactID == item)))
                {
                    ContactIDsForDetete.Add(new IntList() { IntField = item });
                }
            }

            List<SqlParameter> parametersDeleteContactIDs = new List<SqlParameter>
            {
                 new SqlParameter("@PartyID", model.PartyID),
                 new SqlParameter("@ContactIDs", ContactIDsForDetete.ToDataTable()),

            };
            int deleteResult = Iservice.ExecuteNonQuery("[usp.Entity.Party.Contacts.DeletePassedContactIDsForPartyID]", parametersDeleteContactIDs);
            if(deleteResult<0)
            {
                throw new Exception("Error occured while updating contact information list.");
            }
            foreach (var item in ContactList)
            {
                AddContact(model.PartyID, item, UserId);
            }
            return model;
        }

        private class IntList
        {
            public int? IntField { get; set; }
        }

        public PartyModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", ID),
               new SqlParameter("@Operation", "ByID"),
            };
            PartyModel model = new PartyModel();
            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Select]", parameters))
            {
                model = GetModel(reader);
            }
            return model;
        }


        public int Delete(int? PartyID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Entity.Party.Delete]", parameters);
        }


        public IEnumerable<PartyModel> Select(int? PartyID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Select]", parameters))
            {
                yield return GetModel(reader);

            }
        }

        private PartyModel GetModel(IDataRecord reader)
        {
            return new PartyModel()
            {
                PartyID = reader.GetInt32("PartyID"),
                TradeName = reader.GetString("TradeName"),
                Address = new AddressModel() { AddressID = reader.GetInt32("AddressID") },
                AssesseeType = reader.GetString("AssesseeTypeID"),
                BankAccount = new BankAccountModel() { BankAccountID = reader.GetInt32("BankAccountID") },
                CreditLimit = reader.GetInt32("CreditLimit"),
                CreditPeriod = reader.GetInt32("CreditPeriod"),
                GstNo = reader.GetString("GstNo"),
                GstStateCode = reader.GetInt32("GstStateCode"),
                GstRegType = reader.GetString("GstRegType"),
                GstType = reader.GetString("GstType"),
                IcPartnerCode = reader.GetString("IcPartnerCode"),
                InterCompany = reader.GetBoolean("InterCompany"),
                NatureOfBusiness = reader.GetString("NatureOfBusiness"),
                PanAvailable = reader.GetBoolean("PanAvailable"),
                PartyCode = reader.GetString("PartyCode"),
                PaymentMode = reader.GetString("PaymentMode"),
                VendorPostingGroup = new VendorPostingGroupModel() { VendorPostingGroupID = reader.GetInt32("VendorPostingGroupID") },
                CustomerPostingGroup = new CustomerPostingGroupModel() { CustomerPostingGroupID = reader.GetInt32("CustomerPostingGroupID") },
                RenterPostingGroup = new RenterPostingGroupModel() { RenterPostingGroupID = reader.GetInt32("RenterPostingGroupID") },
                SisterPostingGroup = new SisterPostingGroupModel() { SisterPostingGroupID = reader.GetInt32("SisterPostingGroupID") },
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

        public IEnumerable<PartyModel> SelectByCode(String PartyCode, string NatureOfBusiness)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyCode", PartyCode),
               new SqlParameter("@Operation", "ByCode"),
               new SqlParameter("@NatureOfBusiness",NatureOfBusiness)
            };
            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        public IEnumerable<ContactModel> GetContacts(int? PartyID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", PartyID),
               new SqlParameter("@Operation", "SelectByParty"),
            };

            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Contacts.Select]", parameters))
            {
                yield return _contactService.SelectByID(reader.GetInt32("ContactID"));
            }
        }

        public IEnumerable<PartyModel> GetCustomers(string Code)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyCode", Code),
               new SqlParameter("@NatureOfBusiness", "Customer"),
               new SqlParameter("@Operation", "ByCode"),
            };

            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        public IEnumerable<PartyModel> GetVendors(string Code)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyCode", Code),
               new SqlParameter("@NatureOfBusiness", "Supplier"),
               new SqlParameter("@Operation", "ByCode"),
            };

            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        public IEnumerable<PartyModel> GetRenters(string Code)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyCode", Code),
               new SqlParameter("@NatureOfBusiness", "Renter"),
               new SqlParameter("@Operation", "ByCode"),
            };

            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        public int AddContact(int? PartyID, ContactModel contact, string UserId)
        {
            contact.UserInfo.UserID = UserId;
            contact = _contactService.Update(contact);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@ContactID", contact?.ContactID??null)    ,
               new SqlParameter("@UserID", UserId)
            };
            return Iservice.ExecuteNonQuery("[usp.Entity.Party.Contacts.Update]", parameters);
        }

        public int? GetVendorPayableCoaID(int? VendorID)
        {
            return _vendor.GetPostingGroup(VendorID).Payable.CoaID;
        }

        public int? GetVendorDepositCoaID(int? VendorID)
        {
            return _vendor.GetPostingGroup(VendorID).Deposit.CoaID;
        }

        public int? GetVendorPrepaymentCoaID(int? VendorID)
        {
            return _vendor.GetPostingGroup(VendorID).PrePayment.CoaID;
        }

        public int? GetSisterTradeCoaID(int? ID)
        {
            return _sister.GetPostingGroup(ID).Trade.CoaID;
        }

        public int? GetSisterDepositCoaID(int? ID)
        {
            return _sister.GetPostingGroup(ID).Deposit.CoaID;
        }

        public int? GetSisterPrepaymentCoaID(int? ID)
        {           
            return _sister.GetPostingGroup(ID).PrePayment.CoaID;
        }

        public int? GetCustomerReceivableCoaID(int? CustomerID)
        {
            return _customer.GetPostingGroup(CustomerID)?.Receivable?.CoaID ?? null;
        }

        public int? GetCustomerDepositCoaID(int? CustomerID)
        {
            return _customer.GetPostingGroup(CustomerID).Deposit.CoaID;
        }

        public int? GetCustomerPrepaymentCoaID(int? CustomerID)
        {
            return _customer.GetPostingGroup(CustomerID).PrePayment.CoaID;
        }

        public int? GetRenterRentCoaID(int? RenterID)
        {
            return _renter.GetPostingGroup(RenterID).Rent.CoaID;
        }

        public int? GetRenterDepositCoaID(int? RenterID)
        {
            return _renter.GetPostingGroup(RenterID).Deposit.CoaID;
        }

        public int? GetRenterOtherCoaID(int? RenterID)
        {
            return _renter.GetPostingGroup(RenterID).Other.CoaID;
        }

        public bool IsLocal(int? PartyID, int? BranchID)
        {
            var branchModel = _branch.SelectByID(BranchID);
            var partyModel = SelectByID(PartyID);
            if( partyModel.GstType == "UnRegistered")
            {
                return true;
            }
            else if(string.IsNullOrEmpty(partyModel.GstNo))
            {
                return false;
            }

            string branchState = branchModel.GstNo?.Substring(0, 2);
            string partyState = partyModel.GstNo?.Substring(0, 2);
            return (branchState != null && partyState != null && branchState == partyState);
            // branchState.Equals(partyState);

        }

        public int? GetPartyDefaultCoaID(int? PartyID)
        {
            var BusinessNature = SelectByID(PartyID).NatureOfBusiness;
            switch (BusinessNature)
            {
                case "Supplier":
                    return GetVendorPayableCoaID(PartyID);
                case "Customer":
                    return GetCustomerReceivableCoaID(PartyID);
                case "Renter":
                    return GetRenterRentCoaID(PartyID);
                case "SisterConcern":
                    return GetSisterTradeCoaID(PartyID);
            }
            return null;
        }

        public int? GetPartyCoaID(int? PartyID, string BusinessNature, string NatureOfTransaction)
        {
            if (BusinessNature == "Supplier")
            {
                switch (NatureOfTransaction.ToLower())
                {
                    case "deposit":
                        return GetVendorDepositCoaID(PartyID);
                    case "payable":
                        return GetVendorPayableCoaID(PartyID);
                    case "prepayment":
                        return GetVendorPrepaymentCoaID(PartyID);
                }
            }

            if (BusinessNature == "Customer")
            {
                switch (NatureOfTransaction.ToLower())
                {
                    case "deposit":
                        return GetCustomerDepositCoaID(PartyID);
                    case "receivable":
                        return GetCustomerReceivableCoaID(PartyID);
                    case "prepayment":
                        return GetCustomerPrepaymentCoaID(PartyID);
                }
            }

            if (BusinessNature == "Renter")
            {
                switch (NatureOfTransaction.ToLower())
                {
                    case "deposit":
                        return GetRenterRentCoaID(PartyID);
                    case "rent":
                        return GetRenterDepositCoaID(PartyID);
                    case "other":
                        return GetRenterOtherCoaID(PartyID);
                }
            }

            if (BusinessNature == "SisterConcern")
            {
                switch (NatureOfTransaction.ToLower())
                {
                    case "deposit":
                        return GetSisterDepositCoaID(PartyID);
                    case "trade":
                        return GetSisterTradeCoaID(PartyID);
                    case "prepayment":
                        return GetSisterPrepaymentCoaID(PartyID);
                }
            }

            return null;
        }

        public IEnumerable<PartyModel> SelectByBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@BranchID",BranchID)
            };
            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.Select]", parameters))
            {
                yield return GetModel(reader);
            }
        }

        public IEnumerable<int> GetAllocatedBranches(int PartyID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID",PartyID)
            };
            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.LinkTableToBranch.Select]", parameters))
            {
                yield return reader.GetInt32("BranchID").Value;
            }            
        }

        int AllocateBranch(int? PartyID,int? BranchID, string UserID,string Operation)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@PartyID",PartyID),
               new SqlParameter("@BranchID",BranchID),
               new SqlParameter("@UserID",UserID),
               new SqlParameter("@Operation",Operation)
            };
            return Iservice.ExecuteNonQuery("[usp.Entity.Party.LinkTableToBranch.Update]", parameters);           
        }

        public int AddToBranch(int? PartyID, int? BranchID, string UserID)
        {
            return AllocateBranch(PartyID, BranchID, UserID, "ADD");
        }

        public int RemoveFromBranch(int? PartyID, int? BranchID, string UserID)
        {
            return AllocateBranch(PartyID, BranchID, UserID, "REMOVE");
        }

        public IEnumerable<string> GetPostingGroupNames()
        {
            foreach (IDataRecord reader in Iservice.GetDataReader("[usp.Entity.Party.PostingGroups.Select]", null))
            {
                yield return reader.GetString("Title");
            }
        }        
    }
}
