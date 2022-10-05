using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;



namespace ArmsServices.DataServices
{
    public interface IDrCrNoteService
    {
        DrCrNoteModel Update(DrCrNoteModel model);
        DrCrNoteModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<DrCrNoteModel> Select();
        IEnumerable<DrCrNoteModel> SelectByParty(int? PartyID, int? PartyBranchID);
        IEnumerable<DrCrNoteModel> SelectByPeriod(DateTime? begin, DateTime? end);
        IEnumerable<TaxPurchaseExpenseModel> GetExpenses(int? ID);
        IEnumerable<TaxPurchaseItemModel> GetItems(int? ID);
        IEnumerable<BillInfoModel> GetBillInfo(int? BranchID,string DrCrType,int? PartyBranchID,string  DocumentNumberSearchKey);
        int Approve(int? ID, string UserID);
        int Reverse(int? ID, string UserID);

    }

    public class DrCrNoteService : IDrCrNoteService
    {
        IDbService Iservice;

        public DrCrNoteService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Status", 1)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.DrCrNote.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxPurchase.Delete]", parameters);
        }

        public IEnumerable<BillInfoModel> GetBillInfo(int? BranchID,string DrCrType, int? PartyBranchID, string DocumentNumberSearchKey)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@Operation", "GetBillInfo"),
               new SqlParameter("@PartyBranchID", PartyBranchID),
               new SqlParameter("@DrCrType", DrCrType),
               new SqlParameter("@DocumentNumberSearchKey", DocumentNumberSearchKey),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.BillInfo.Select]", parameters))
            {
                yield return new BillInfoModel()
                {
                    TotalAmount = dr.GetDecimal("TotalAmount"),                 
                    BillID = dr.GetInt32("ID"),                  
                    DocumentNumber = dr.GetString("DocumentNumber"),
                    DocumentDate = dr.GetDateTime("DocumentDate")
                };
            }
        }

        public IEnumerable<TaxPurchaseExpenseModel> GetExpenses(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetParticulars"),
               new SqlParameter("@ID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.DrCrNote.Select]", parameters))
            {
                yield return new TaxPurchaseExpenseModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    CGST = dr.GetDecimal("CGST"),
                    IGST = dr.GetDecimal("IGST"),
                    SGST = dr.GetDecimal("SGST"),
                    CoaID = dr.GetInt32("CoaID"),
                    PID = dr.GetInt32("PID"),
                    TDS = dr.GetDecimal("TDS"),
                    BillReference = dr.GetString("BillReference"),
                    BranchID = dr.GetInt32("BranchID"),
                    UsageID = dr.GetString("UsageID"),
                    TpeID = dr.GetInt64("TpeID"),
                };
            }
        }

        public IEnumerable<TaxPurchaseItemModel> GetItems(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetItems"),
               new SqlParameter("@ID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.DrCrNote.Select]", parameters))
            {
                yield return new TaxPurchaseItemModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    CGST = dr.GetDecimal("CGST"),
                    IGST = dr.GetDecimal("IGST"),
                    SGST = dr.GetDecimal("SGST"),
                    ItemID = dr.GetInt32("ItemID"),
                    CoaID = dr.GetInt32("CoaID"),
                    ItemQty = dr.GetDecimal("ItemQty"),
                    ItemRate = dr.GetDecimal("ItemRate"),
                    PID = dr.GetInt32("PID"),
                    TDS = dr.GetDecimal("TDS"),
                    TpiID = dr.GetInt64("TpiID"),
                };
            }
        }

        public int Reverse(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Status", 2)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.DrCrNote.Approve]", parameters);
        }

        public IEnumerable<DrCrNoteModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.DrCrNote.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public DrCrNoteModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            DrCrNoteModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.DrCrNote.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<DrCrNoteModel> SelectByParty(int? PartyID, int? PartyBranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@PartyBranchID", PartyBranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.DrCrNote.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<DrCrNoteModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.DrCrNote.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public DrCrNoteModel Update(DrCrNoteModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DrCrNoteID", model.DrCrNoteID),
               new SqlParameter("@DrCrType", model.DrCrType),
               new SqlParameter("@ReasonCode", model.ReasonCode),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@OriginalTransactionID", model.OriginalTransactionID),
               new SqlParameter("@OriginalTranDocDate", model.OriginalTranDocDate),
               new SqlParameter("@OriginalTranDocNumber", model.OriginalTranDocNumber),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@MID", model.MID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@Particulars", model.Particulars.ToDataTable()),               
               new SqlParameter("@CostCenter", model.CostCenter),
                new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@Items", model.Items.ToDataTable()),
               new SqlParameter("@PartyBranchCoaID", model.PartyBranchCoaID),
               new SqlParameter("@PartyBranchID", model.PartyBranch.GstID),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.DrCrNote.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private DrCrNoteModel GetModel(IDataRecord dr)
        {
            return new DrCrNoteModel
            {
                DrCrNoteID = dr.GetInt32("DrCrNoteID"),
                DrCrType = dr.GetString("DrCrType"),
                ReasonCode = dr.GetString("ReasonCode"),
                Reference =   dr.GetString("Reference"),
                OriginalTranDocDate  = dr.GetDateTime("OriginalTranDocDate"),
                OriginalTranDocNumber = dr.GetString("OriginalTranDocNumber"),
                OriginalTransactionID = dr.GetInt32("OriginalTransactionID"),                
                BranchID = dr.GetInt32("BranchID"),
                ApprovedInfo = new ArmsModels.SharedModels.UserInfoModel()
                {
                    RecordStatus = dr.GetByte("ApprovedStatus"),
                    TimeStampField = dr.GetDateTime("ApprovedOn"),
                    UserID = dr.GetString("ApprovedBy"),
                },
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),                
                MID = dr.GetInt32("MID"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension = dr.GetInt32("Dimension"),                
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                PartyBranch = new PartyBranchModel()
                {
                    GstID = dr.GetInt32("PartyBranchID"),
                    PartyID = dr.GetInt32("PartyID"),
                    Party = new PartyModel()
                    {
                        PartyID = dr.GetInt32("PartyID"),
                        PartyName = dr.GetString("PartyName")
                    },
                },
                PartyBranchCoaID = dr.GetInt32("PartyBranchCoaID"),
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