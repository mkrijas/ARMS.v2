using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Security.Cryptography;


namespace ArmsServices.DataServices
{
    public class OutstandingBillsService : IOutstandingBillsService
    {
        IDbService Iservice;

        public OutstandingBillsService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to approve an outstanding bill
        public int Approve(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AutoSettleID", ID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OutstandingBills.Autosettle.Approve]", parameters);
        }

        // Method to auto-settle an outstanding bill
        public AutoSettleModel Update(AutoSettleModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Update"),
               new SqlParameter("@AutoSettleID", model.AutoSettleID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@PartyID", model.PartyInfo.PartyID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@Bills", model.Bills.ToDataTable()),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Autosettle.Update]", parameters))
            {
                return new AutoSettleModel()
                {
                    PartyInfo = new PartyModel()
                    {
                        PartyID = dr.GetInt32("PartyID"),
                        TradeName = dr.GetString("tradeName"),
                    },
                    AutoSettleID = dr.GetInt32("AutoSettleID"),
                    BranchID = dr.GetInt32("BranchID"),
                    DocumentDate = dr.GetDateTime("DocumentDate"),
                    DocumentNumber = dr.GetString("DocumentNumber"),
                    NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                    MID = dr.GetInt32("MID"),
                    Narration = dr.GetString("Narration"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                    AuthLevelId = dr.GetInt32("AuthLevelId"),
                    AuthStatus = dr.GetString("AuthStatus"),
                    UserInfo =
                    {
                        UserID = dr.GetString("UserID"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        RecordStatus = dr?.GetByte("RecordStatus"),
                    }
                };
            }
            return null;            
        }

        // Method to delete an auto-settle entry
        public int Delete(int? ID, string userID)
        {
            //throw new NotImplementedException();
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "DELETE"),
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", userID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.OutstandingBills.Autosettle.Delete]", parameters);
        }

        // Method to delete an auto-settle entry
        public IEnumerable<BillsPaidModel> GetAutoSettledBills(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetBills"),
               new SqlParameter("@AutoSettleID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Autosettle.Select]", parameters))
            {
                yield return new BillsPaidModel()
                {
                    MID = dr.GetInt32("MID"),
                    BpID = dr.GetInt32("BpID"),
                    BranchID = dr.GetInt32("BranchID"),
                    BranchName = dr.GetString("BranchName"),
                    InvoiceDate = dr.GetDateTime("ReferenceDocDate"),
                    InvoiceNumber = dr.GetString("ReferenceDocNo"),
                    PayAmount = dr.GetDecimal("Amount"),
                    CoaID = dr.GetInt32("CoaID"),
                    IsMemo = dr.GetBoolean("IsMemo"),
                    OutstandingAmount = dr.GetDecimal("Amount"),
                    PartyID = dr.GetInt32("PartyID"),
                };
            }
        }

        // Method to select outstanding bills by Branch ID
        public IEnumerable<OutstandingBillsModel> Select(int BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }      

        // Method to select an outstanding bill by its ID
        public OutstandingBillsModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BoID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            OutstandingBillsModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to select outstanding bills by Party ID
        public IEnumerable<OutstandingBillsModel> SelectByParty(int? PartyID, int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select outstanding bills by date period
        public IEnumerable<OutstandingBillsModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
      
        // Private method to convert an IDataRecord to an OutstandingBillsModel
        private OutstandingBillsModel GetModel(IDataRecord dr)
        {
            return new OutstandingBillsModel
            {
                BoID = dr.GetInt32("BoID"),
                MID = dr.GetInt32("MID"),
                OutstandingAmount = dr.GetDecimal("OutstandingAmount"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                BranchName = dr.GetString("BranchName"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentNumber = dr.GetString("DocNumber"),
                DocumentDate = dr.GetDateTime("DocDate"),
                ReferenceDocDate = dr.GetDateTime("ReferenceDocDate"),
                ReferenceDocNo = dr.GetString("ReferenceDocNo"),
                isMemo = dr.GetBoolean("IsMemo"),
                CoaID = dr.GetInt32("CoaID"),
                SubArdCode = dr.GetString("SubArdCode"),
                PartyInfo = new PartyModel()
                {
                    PartyID = dr.GetInt32("PartyID"),
                    GstNo = dr.GetString("GstNo"),
                    TradeName = dr.GetString("TradeName")


                }

            };
        }

        // Method to search and get outstanding bill info by DocumentNumber
        public IEnumerable<OutStandingBillInfoModel> SelectByDocumentNumber(string DocumentNumber)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentNumber", DocumentNumber),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.finance.Transactions.OutstandingBills.getBillInfo]", parameters))
            {
                yield return GetInfoModel(dr);
            }
        }

        private OutStandingBillInfoModel GetInfoModel(IDataRecord dr)
        {
            return new OutStandingBillInfoModel
            {
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                PartyCode = dr.GetString("PartyCode"),
                PartyName = dr.GetString("PartyName"),
                BranchName = dr.GetString("BranchName"),
                Amount = dr.GetDecimal("Amount"),
                AccountName = dr.GetString("AccountName"),
            };
        }

        public int Reverse(int? ID, string UserID, string Remarks)
        {
            throw new NotImplementedException();
        }

        AutoSettleModel IbaseInterface<AutoSettleModel>.SelectByID(int? ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AutoSettleModel> Select(int? BranchID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AutoSettleModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AutoSettleModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public int RemoveFile(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }

        public PagedResult<AutoSettleModel> SelectAll(int? BranchID, int page, int pageSize, string search, bool _IsApproved)
        {
            List<SqlParameter> parameters = new()
            {
                new SqlParameter("@BranchID", BranchID),
                new SqlParameter("@Operation", "All"),
                new SqlParameter("@Page", page),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@Searchterm", search ?? ""),
                new SqlParameter("@IsApproved", _IsApproved),
            };
            List<AutoSettleModel> list = [];
            int? countOf = 0;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.OutstandingBills.Autosettle.Select]", parameters))
            {
                list.Add(
                    new AutoSettleModel()
                    {
                        PartyInfo = new PartyModel()
                        {
                            PartyID = dr.GetInt32("PartyID"),
                            TradeName = dr.GetString("tradeName"),
                        },
                        AutoSettleID = dr.GetInt32("AutoSettleID"),
                        BranchID = dr.GetInt32("BranchID"),
                        DocumentDate = dr.GetDateTime("DocumentDate"),
                        DocumentNumber = dr.GetString("DocumentNumber"),
                        NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                        MID = dr.GetInt32("MID"),
                        Narration = dr.GetString("Narration"),
                        TotalAmount = dr.GetDecimal("TotalAmount"),
                        AuthLevelId = dr.GetInt32("AuthLevelId"),
                        AuthStatus = dr.GetString("AuthStatus"),
                        UserInfo =
                            {
                                UserID = dr.GetString("UserID"),
                                TimeStampField = dr.GetDateTime("TimeStamp"),
                                RecordStatus = dr?.GetByte("RecordStatus"),
                            }
                    });
                if (countOf == 0)
                    countOf = dr.GetInt32("CountOf");
            }
            return new PagedResult<AutoSettleModel>
            {
                Items = list,
                TotalRecords = countOf
            };
        }
    }
}