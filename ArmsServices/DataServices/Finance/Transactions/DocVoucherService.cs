using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using ArmsModels.BaseModels.Finance.Transactions;

namespace ArmsServices.DataServices.Finance.Transactions
{
    public class DocVoucherService : IDocVoucherService
    {
        IDbService Iservice;

        public DocVoucherService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to approve a Document Voucher
        public int Approve(int? DocVoucherID, string UserID, string Remark)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TaxVoucherID", DocVoucherID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remark)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxVoucher.Approve]", parameters);
        }

        // Method to delete a Document Voucher by ID
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxVoucher.Delete]", parameters);
        }

        // Method to get sub-document vouchers by Document Voucher ID
        public IEnumerable<DocumentVoucherSubModel> GetSub(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetSub"),
               new SqlParameter("@ID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxVoucher.Select]", parameters))
            {
                yield return new DocumentVoucherSubModel()
                {
                    DocumentVoucherSubID = dr.GetInt32("TaxVoucherSubID"),
                    DocumntVoucherID = dr.GetInt32("TaxVoucherID"),
                    Amount = dr.GetDecimal("Amount"),
                    DocumentID = dr.GetInt32("DocumentID"),
                    DocumentName = dr.GetString("DocumentName"),
                    AssetName = dr.GetString("AssetName"),
                    AssetCode = dr.GetString("AssetCode"),
                    SlipNo = dr.GetString("ReceiptNo"),
                    UsageCode = dr.GetString("UsageCode"),
                    InvoiceDate = dr.GetDateTime("InvoiceDate"),
                    CostCenter = dr.GetInt32("CostCenterID"),                    
                    CostCenterMod = new CostCenterModel()
                    {
                        CostCenterID = dr.GetInt32("CostCenterID"),
                        CostCenter = dr.GetString("CostCenter"),

                    },                    
                    Dimension = dr.GetInt32("DimensionID"),
                    DimensionMod = new DimensionModel()
                    {
                        DimensionID = dr.GetInt32("DimensionID"),
                        Dimension = dr.GetString("Dimension"),
                    },
                    Reference = dr.GetString("Refference"),
                };
            }
        }

        // Method to get not posted sub-documents by Document Type I
        public IEnumerable<DocumentVoucherSubModel> GetNotPostedSubDocuments(int? DocumentTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetNotFinancialyPosted"),
               new SqlParameter("@DocumentTypeID", DocumentTypeID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                yield return new DocumentVoucherSubModel()
                {
                    DocumentVoucherSubID = dr.GetInt32("TaxVoucherSubID"),
                    DocumntVoucherID = dr.GetInt32("TaxVoucherID"),
                    Amount = dr.GetDecimal("Amount"),
                    DocumentID = dr.GetInt32("DocumentID"),
                    DocumentName = dr.GetString("DocumentName"),
                    AssetID = dr.GetInt32("AssetID"),
                    AssetName = dr.GetString("AssetName"),
                    AssetCode = dr.GetString("AssetCode"),
                    SlipNo = dr.GetString("ReceiptNo"),
                    UsageCode = dr.GetString("UsageCode"),
                    InvoiceDate = dr.GetDateTime("InvoiceDate"),
                    CostCenterMod = new CostCenterModel() { CostCenter = dr.GetString("CostCenter"),CostCenterID = dr.GetInt32("CostCenterID"), },
                    DimensionMod = new() {Dimension =  dr.GetString("Dimension"), DimensionID = dr.GetInt32("DimensionID"), },
                    CostCenter = dr.GetInt32("CostCenterID"),
                    Dimension = dr.GetInt32("DimensionID"),
                    Reference = dr.GetString("Refference"),
                };
            }
        }

        // Method to reverse a Document Voucher
        public int Reverse(int? PID, string UserID, String Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", PID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxVoucher.Reverse]", parameters);
        }

        // Method to select Document Vouchers by Branch ID
        public IEnumerable<DocumentVoucherModel> Select(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxVoucher.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a Document Voucher by its ID
        public DocumentVoucherModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            DocumentVoucherModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxVoucher.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to select Document Vouchers by Party ID and Party Branch ID
        public IEnumerable<DocumentVoucherModel> SelectByParty(int? PartyID, int? PartyBranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@PartyBranchID", PartyBranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxVoucher.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select Document Vouchers by date period
        public IEnumerable<DocumentVoucherModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxVoucher.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to update DocumentVoucherModel record
        public DocumentVoucherModel Update(DocumentVoucherModel model)
        {
            //List<TaxVoucherSubSendModel> taxVoucherSubListFormated = new();
            //foreach (var item in model.TaxVoucherSubList)
            //{
            //    taxVoucherSubListFormated.Add(new()
            //    {
            //        TaxVoucherSubID = item.TaxVoucherSubID,
            //        TaxVoucherID = item.TaxVoucherID,
            //        DocumentID = item.DocumentID,
            //        Amount = item.Amount,
            //        Reference = item.Reference
            //    });
            //}
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TaxVoucherID", model.DocumentVoucherID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@MID", model.MID),
               new SqlParameter("@DocumentDate", model.DocumentDate),               
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@DocumentTypeID", model.DocumentType.DocumentTypeID),               
               new SqlParameter("@IsAgent", model.IsAgent),
               new SqlParameter("@AgentID", model.Agent?.PartyID??null),
               new SqlParameter("@AgentCode", model.Agent?.PartyCode?? null),
               new SqlParameter("@PaymentMode", model.PaymentMode),
               new SqlParameter("@PaymentArdCode", model.PaymentArdCode),
               new SqlParameter("@PaymentCoaID", model.PaymentCoaID),
               new SqlParameter("@PaymentTool", model.PaymentTool),
               new SqlParameter("@BankCharges", model.BankCharges),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@SGST", model.SGST),
               new SqlParameter("@CGST", model.CGST),
               new SqlParameter("@IGST", model.IGST),
               new SqlParameter("@TDS", model.TDS),
               new SqlParameter("@TaxVoucherSub", model.DocVoucherSubList?.ToDataTable()??null),

               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
                foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxVoucher.Update]", parameters))
                {
                    model = GetModel(dr);
                }
            }
            catch (Exception ex)
            {

            }
            return model;
        }

        // Private method to convert an IDataRecord to a DocumentVoucherModel
        private DocumentVoucherModel GetModel(IDataRecord dr)
        {
            return new DocumentVoucherModel
            {
                DocumentVoucherID = dr.GetInt32("TaxVoucherID"),
                BranchID = dr.GetInt32("BranchID"),
                MID = dr.GetInt32("MID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
               
                DocumentNumber = dr.GetString("DocumentNumber"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                Narration = dr.GetString("Narration"),
                FileName = dr.GetString("FilePath"),
                DocumentType = new AssetDocumentTypeModel()
                {
                    DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                    DocumentTypeName = dr.GetString("DocumentTypeName"),
                },                
                IsAgent = dr.GetBoolean("IsAgent"),
                Agent = new PartyModel()
                {
                    PartyID = dr.GetInt32("AgentID"),
                    TradeName = dr.GetString("AgentName"),
                    PartyCode = dr.GetString("AgentCode"),
                },
                PaymentMode = dr.GetString("PaymentMode"),
                PaymentArdCode = dr.GetString("PaymentArdCode"),
                PaymentCoaID = dr.GetInt32("PaymentCoaID"),
                PaymentTool = dr.GetString("PaymentTool"),
                BankCharges = dr.GetDecimal("BankCharges"),
                SGST = dr.GetDecimal("SGST"),
                CGST = dr.GetDecimal("CGST"),
                IGST = dr.GetDecimal("IGST"),
                TDS = dr.GetDecimal("TDS"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
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
