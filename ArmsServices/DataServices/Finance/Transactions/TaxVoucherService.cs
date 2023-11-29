using ArmsModels.BaseModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using ArmsModels.BaseModels.Finance.Transactions;

namespace ArmsServices.DataServices.Finance.Transactions
{
    public class TaxVoucherService : ITaxVoucherService
    {
        IDbService Iservice;

        public TaxVoucherService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int? TaxVoucherID, string UserID, string Remark)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TaxVoucherID", TaxVoucherID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remark)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxVoucher.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxVoucher.Delete]", parameters);
        }

        public IEnumerable<TaxVoucherSubModel> GetSub(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetSub"),
               new SqlParameter("@ID", ID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxVoucher.Select]", parameters))
            {
                yield return new TaxVoucherSubModel()
                {
                    TaxVoucherSubID = dr.GetInt32("TaxVoucherSubID"),
                    TaxVoucherID = dr.GetInt32("TaxVoucherID"),
                    Amount = dr.GetDecimal("Amount"),
                    DocumentID = dr.GetInt32("DocumentID"),
                    DocumentName = dr.GetString("DocumentName"),
                    AssetName = dr.GetString("AssetName"),
                    AssetCode = dr.GetString("AssetCode"),
                    SlipNo = dr.GetString("SlipNo"),
                    UsageCode = dr.GetString("UsageCode"),
                    InvoiceDate = dr.GetDateTime("InvoiceDate"),
                    CostCenter = dr.GetInt32("CostCenterID"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    CostCenterMod = new CostCenterModel()
                    {
                        CostCenterID = dr.GetInt32("CostCenterID"),
                        CostCenter = dr.GetString("CostCenter"),

                    },
                    DimensionVal = dr.GetString("Dimension"),
                    Dimension = dr.GetInt32("DimensionID"),
                    DimensionMod = new DimensionModel()
                    {
                        DimensionID = dr.GetInt32("DimensionID"),
                        Dimension = dr.GetString("Dimension"),
                    },
                    Reference = dr.GetString("Reference"),
                };
            }
        }



        public IEnumerable<TaxVoucherSubModel> GetNotPostedSubDocuments(int? DocumentTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetNotFinancialyPosted"),
               new SqlParameter("@DocumentTypeID", DocumentTypeID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                yield return new TaxVoucherSubModel()
                {
                    TaxVoucherSubID = dr.GetInt32("TaxVoucherSubID"),
                    TaxVoucherID = dr.GetInt32("TaxVoucherID"),
                    Amount = dr.GetDecimal("Amount"),
                    DocumentID = dr.GetInt32("DocumentID"),
                    DocumentName = dr.GetString("DocumentName"),
                    AssetID = dr.GetInt32("AssetID"),
                    AssetName = dr.GetString("AssetName"),
                    AssetCode = dr.GetString("AssetCode"),
                    SlipNo = dr.GetString("ReceiptNo"),
                    UsageCode = dr.GetString("UsageCode"),
                    InvoiceDate = dr.GetDateTime("InvoiceDate"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    DimensionVal = dr.GetString("Dimension"),
                    CostCenter = dr.GetInt32("CostCenterID"),
                    Dimension = dr.GetInt32("DimensionID"),
                    Reference = dr.GetString("Refference"),
                };
            }
        }

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

        public IEnumerable<TaxVoucherModel> Select(int? BranchID)
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

        public TaxVoucherModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            TaxVoucherModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxVoucher.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<TaxVoucherModel> SelectByParty(int? PartyID, int? PartyBranchID)
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

        public IEnumerable<TaxVoucherModel> SelectByPeriod(DateTime? begin, DateTime? end)
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

        public TaxVoucherModel Update(TaxVoucherModel model)
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
               new SqlParameter("@TaxVoucherID", model.TaxVoucherID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@MID", model.MID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@InvoiceDate", model.InvoiceDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@DocumentTypeID", model.DocumentType.DocumentTypeID),
               new SqlParameter("@FromDate", model.FromDate),
               new SqlParameter("@ToDate", model.ToDate),
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
               new SqlParameter("@TaxVoucherSub", model.TaxVoucherSubList?.ToDataTable()??null),

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

        private TaxVoucherModel GetModel(IDataRecord dr)
        {
            return new TaxVoucherModel
            {
                TaxVoucherID = dr.GetInt32("TaxVoucherID"),
                BranchID = dr.GetInt32("BranchID"),
                MID = dr.GetInt32("MID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                InvoiceDate = dr.GetDateTime("InvoiceDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                Narration = dr.GetString("Narration"),
                FileName = dr.GetString("FilePath"),
                DocumentType = new AssetDocumentTypeModel()
                {
                    DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                    DocumentTypeName = dr.GetString("DocumentTypeName"),
                },
                FromDate = dr.GetDateTime("FromDate"),
                ToDate = dr.GetDateTime("ToDate"),
                IsAgent = dr.GetBooleanNullable("IsAgent"),
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
