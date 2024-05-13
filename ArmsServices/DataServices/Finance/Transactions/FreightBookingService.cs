using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public class FreightBillingService : IFreightBillingService
    {
        IDbService Iservice;

        public FreightBillingService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int ReverseFinalInvoice(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BillingID", ID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Billing.FinalInvoice.Reverse]", parameters);
        }

        public IEnumerable<GcTariffModel> GetPending(int? OrderID, short? TariffTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Pending"),
               new SqlParameter("@OrderID", OrderID),
               new SqlParameter("@TariffTypeID", TariffTypeID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.TariffEntry.Select]", parameters))
            {
                yield return GetTariffEntries(dr);
            }
        }

        public IEnumerable<ProformaInvoiceModel> SelectPendingProformaInvoiceList(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "PENDING"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.ProformaInvoice.Select]", parameters))
            {
                yield return GetProformaInvoiceModel(dr);
            }
        }

        public IEnumerable<ProformaInvoiceModel> SelectProformaInvoiceList(int? BranchID, int? ID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@ProformaInvoiceID", ID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)

            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.ProformaInvoice.Select]", parameters))
            {
                yield return GetProformaInvoiceModel(dr);
            }
        }

        public IEnumerable<ConsolidatedDraftBillModel> SelectPendingConsolidatedDraftBillList(int? ID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "PENDING"),
               new SqlParameter("@DraftBillID", ID),
               new SqlParameter("@BranchID", BranchID)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.ConsolidatedDraftBill.Select]", parameters))
            {
                yield return GetConsolidatedDraftBillModel(dr);

            }
        }

        public IEnumerable<GcTariffModel> GetBilled(int? ConsolidatedDraftBillID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Billed"),
               new SqlParameter("@ConsolidatedDraftBillID", ConsolidatedDraftBillID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.TariffEntry.Select]", parameters))
            {
                yield return GetTariffEntries(dr);
            }
        }

        public BillingModel SelectFinalInvoice(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BillingID", ID),
            };
            BillingModel model = new();

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.FinalInvoice.Select]", parameters))
            {
                model = GetFinalInvoiceModel(dr);
            }
            return model;
        }

        public int? UpdateFinalInvoice(BillingModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               //new SqlParameter("@BillingID", model.BillingID),               
               new SqlParameter("@ProformaInvoiceID", model.ProformaInvoiceID),
               //new SqlParameter("@DraftBillID", model.DraftBillID),
               //new SqlParameter("@OrderID", model.OrderID),
               //new SqlParameter("@PartyCoaID", model.PartyCoa),
               //new SqlParameter("@TariffTypeID", model.TariffType.TariffTypeID),
               //new SqlParameter("@UsageCode", model.TariffType.UsageCode),
               //new SqlParameter("@Reference", model.Reference),
               //new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@FilePath", model.FileName),
               //new SqlParameter("@CostCenter", model.CostCenter),
               //new SqlParameter("@Dimension", model.Dimension),
               //new SqlParameter("@TotalAmount", model.TotalAmount),
               //new SqlParameter("@Narration", model.Narration),
               //new SqlParameter("@GstRate", model.Gst.GstRate),
               //new SqlParameter("@Cgst", model.Gst.Cgst),
               //new SqlParameter("@Sgst", model.Gst.Sgst),
               //new SqlParameter("@Igst", model.Gst.Igst),
               //new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            int? ID = null;

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.FinalInvoice.Update]", parameters))
            {
                ID = dr.GetInt32("BillingID");
            }
            return ID;
        }

        private BillingModel GetFinalInvoiceModel(IDataRecord dr)
        {
            return new BillingModel
            {
                BillingID = dr.GetInt32("BillingID"),
                ProformaInvoiceID = dr.GetInt32("ProformaInvoiceID"),
                DraftBillID = dr.GetInt32("DraftBillID"),
                OrderID = dr.GetInt32("OrderID"),
                Party = new PartyModel()
                {
                    TradeName = dr.GetString("tradeName"),
                    PartyID = dr.GetInt32("PartyID"),
                },
                PartyCoa = dr.GetInt32("PartyCoaID"),
                TariffType = new TariffTypeModel()
                {
                    TariffTypeID = dr.GetInt16("TariffTypeID"),
                    UsageCode = dr.GetString("UsageCode"),
                },
                Reference = dr.GetString("Reference"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension = dr.GetInt32("Dimension"),
                FileName = dr.GetString("FilePath"),
                FreightAmount = dr.GetDecimal("FreightAmount"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                Gst = new GstModel()
                {
                    GstRate = dr.GetDecimal("GstRate"),
                    CGST = dr.GetDecimal("Cgst"),
                    SGST = dr.GetDecimal("Sgst"),
                    IGST = dr.GetDecimal("Igst"),
                },
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private ProformaInvoiceModel GetProformaInvoiceModel(IDataRecord dr)
        {
            return new ProformaInvoiceModel
            {
                ProformaInvoiceID = dr.GetInt32("ProformaInvoiceID"),
                DraftBillID = dr.GetInt32("DraftBillID"),
                OrderID = dr.GetInt32("OrderID"),
                PartyCoa = dr.GetInt32("PartyCoaID"),

                Party = new PartyModel()
                {
                    PartyCode = dr.GetString("PartyCode"),
                    PartyID = dr.GetInt32("PartyID"),
                    TradeName = dr.GetString("tradename")
                },

                TariffType = new TariffTypeModel()
                {
                    TariffTypeID = dr.GetInt16("TariffTypeID"),
                    UsageCode = dr.GetString("UsageCode"),
                },
                Reference = dr.GetString("Reference"),
                BranchID = dr.GetInt32("BranchID"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                FileName = dr.GetString("FilePath"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension = dr.GetInt32("Dimension"),
                FreightAmount = dr.GetDecimal("FreightAmount"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                PeriodFrom = dr.GetDateTime("DocFromDate"),
                PeriodTo = dr.GetDateTime("DocToDate"),
                Narration = dr.GetString("Narration"),
                Gst = new()
                {
                    GstRate = dr.GetDecimal("GstRate"),
                    CGST = dr.GetDecimal("Cgst"),
                    SGST = dr.GetDecimal("Sgst"),
                    IGST = dr.GetDecimal("Igst"),
                },
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }
        private ConsolidatedDraftBillModel GetConsolidatedDraftBillModel(IDataRecord dr)
        {
            return new ConsolidatedDraftBillModel
            {
                DraftBillID = dr.GetInt32("DraftBillID"),
                BranchID = dr.GetInt32("BranchID"),
                Party = new()
                {
                    PartyID = dr.GetInt32("PartyID"),
                    TradeName = dr.GetString("TradeName"),
                },
                Order = new OrderModel()
                {
                    OrderID = dr.GetInt32("OrderID"),
                    OrderName = dr.GetString("OrderName"),
                },
                TariffType = new TariffTypeModel()
                {
                    TariffTypeID = dr.GetInt16("TariffTypeID"),
                    TariffTypeName = dr.GetString("TariffTypeName"),
                    UsageCode = dr.GetString("UsageCode"),
                },
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                Narration = dr.GetString("Narration"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public int? UpdateProformaInvoice(ProformaInvoiceModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ProformaInvoiceID", model.ProformaInvoiceID),
               new SqlParameter("@DraftBillID", model.DraftBillID),
               new SqlParameter("@OrderID", model.OrderID),
               new SqlParameter("@PartyID", model.Party?.PartyID),
               new SqlParameter("@PartyCoaID", model.PartyCoa),
               new SqlParameter("@TariffTypeID", model.TariffType.TariffTypeID),
               new SqlParameter("@UsageCode", model.TariffType.UsageCode),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@FreightAmount", model.FreightAmount),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@GstRate", model.Gst.GstRate),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@Cgst", model.Gst.CGST),
               new SqlParameter("@Sgst", model.Gst.SGST),
               new SqlParameter("@Igst", model.Gst.IGST),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            int? ID = null;

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.ProformaInvoice.Update]", parameters))
            {
                ID = dr.GetInt32("ProformaInvoiceID");
            }
            return ID;
        }

        public int? UpdateConsolidatedDraftBill(ConsolidatedDraftBillModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DraftBillID", model.DraftBillID),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocFromDate", model.DocFromDate),
               new SqlParameter("@DocToDate", model.DocToDate),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@PartyID", model.Party?.PartyID),
               new SqlParameter("@OrderID", (model.Order?.OrderID)??0),
               new SqlParameter("@TariffTypeID", model.TariffType.TariffTypeID),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@FilePath", model.FileName),
               //new SqlParameter("@BookedGcs", model.BookedGCs.ToDataTable()),
               new SqlParameter("@BookedGcs", SqlDbType.Structured)
                {
                    TypeName = "dbo.GcTariffEntryTableType",
                    Value = model.BookedGCs.ToDataTable()
                },
            new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            int? ID = null;

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.ConsolidatedDraftBill.Update]", parameters))
            {
                ID = dr.GetInt32("DraftBillID");
            }
            return ID;
        }

        public ProformaInvoiceModel SelectProformaInvoice(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ProformaInvoiceID", ID)
            };
            ProformaInvoiceModel model = new();

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.ProformaInvoice.Select]", parameters))
            {
                model = GetProformaInvoiceModel(dr);
            }
            return model;
        }

        public ConsolidatedDraftBillModel SelectConsolidatedDraftBill(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DraftBillID", ID)
            };
            ConsolidatedDraftBillModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.ConsolidatedDraftBill.Select]", parameters))
            {
                model = GetConsolidatedDraftBillModel(dr);
            }
            return model;
        }

        public int DeleteProformaInvoice(int? ID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BillingID", ID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Billing.ProformaInvoice.Delete]", parameters);
        }

        public int ReverseConsolidatedDraftBill(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DraftBillID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Billing.ConsolidatedDraftBill.Reverse]", parameters);
        }

        public IEnumerable<GcTariffModel> GetPending(int? PartyID, int? OrderID, short? TariffTypeID, int? GcTypeID, DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Pending"),
               new SqlParameter("@Begin",begin),
               new SqlParameter("@End",end),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@OrderID", OrderID),
               new SqlParameter("@TariffTypeID", TariffTypeID),
               new SqlParameter("@GcTypeID", GcTypeID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.TariffEntry.Select]", parameters))
            {
                yield return GetTariffEntries(dr);
            }
        }

        public IEnumerable<GcTariffModel> GenerateTariffs(int? OrderID, short? TariffTypeID, int? GcTypeID, DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Generate"),
               new SqlParameter("@Begin", begin),
               new SqlParameter("@End", end),
               new SqlParameter("@OrderID", OrderID),
               new SqlParameter("@TariffTypeID", TariffTypeID),
               new SqlParameter("@GcTypeID", GcTypeID)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.TariffEntry.Generate]", parameters))
            {
                yield return GetTariffEntries(dr);
            }
        }

        GcTariffModel GetTariffEntries(IDataRecord dr)
        {
            return new GcTariffModel()
            {
                GcTariffID = dr.GetInt64("GcTariffID"),
                ConsolidatedDraftBillID = dr.GetInt32("ConsolidatedDraftBillID"),
                GcID = dr.GetInt64("GcID"),
                TariffID = dr.GetInt32("TariffID"),
                Amount = dr.GetDecimal("Amount"),
                BillDate = dr.GetDateTime("BillDate"),
                InvoiceDate = dr.GetDateTime("InvoiceDate"),
                BillNumber = dr.GetString("BillNumber"),
                BillQuantity = dr.GetDecimal("BillQuantity"),
                ConsigneeName = dr.GetString("ConsigneeName"),
                PassNumber = dr.GetString("PassNumber"),
                Deduction = dr.GetDecimal("Deduction"),
                PaymentMode = dr.GetString("PaymentMode"),
            };
        }

        public int? ApproveProformaInvoice(int? ProformaInvoiceID, string userID, string Remarks, string InvoiceNumber, string InvoiceRefNumber, DateTime? InvoiceDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ProformaInvoiceID", ProformaInvoiceID),
               new SqlParameter("@UserID", userID),
               new SqlParameter("@Remarks", Remarks),
               new SqlParameter("@InvoiceNumber", InvoiceNumber),
               new SqlParameter("@InvoiceRefNumber", InvoiceRefNumber),
               new SqlParameter("@InvoiceDate", InvoiceDate)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Billing.ProformaInvoice.Approve]", parameters);
        }

        public int? ReverseProformaInvoice(int? ProformaInvoiceID, string userID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ProformaInvoiceID", ProformaInvoiceID),
               new SqlParameter("@UserID", userID)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Billing.ProformaInvoice.Reverse]", parameters);
        }

        public GstModel GetGstRate(int? DraftBillID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetGst"),
               new SqlParameter("@DraftBillID",DraftBillID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.ConsolidatedDraftBill.Select]", parameters))
            {
                return new GstModel()
                {
                    GstRate = dr.GetDecimal("GstRate"),
                    CGST = dr.GetDecimal("Cgst"),
                    SGST = dr.GetDecimal("Cgst"),
                    IGST = dr.GetDecimal("Igst"),
                };
            }
            return new GstModel();
        }
    }
}