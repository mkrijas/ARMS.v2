
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    public interface IFreightBillingService
    {
        int? UpdateFinalInvoice(BillingModel model);
        int? UpdateProformaInvoice(ProformaInvoiceModel model);
        int? ApproveProformaInvoice(int? ProformaInvoiceID, string userID);
        int? ReverseProformaInvoice(int? ProformaInvoiceID, string userID);
        int? UpdateConsolidatedDraftBill(ConsolidatedDraftBillModel model);
        BillingModel SelectFinalInvoice(int? ID);
        ProformaInvoiceModel SelectProformaInvoice(int? ID);
        ConsolidatedDraftBillModel SelectConsolidatedDraftBill(int? ID);
        int DeleteFinalInvoice(int? ID, string UserID);
        int DeleteProformaInvoice(int? ID,string UserID);
        int DeleteConsolidatedDraftBill(int? ID, string UserID);
        IEnumerable<ConsolidatedDraftBillModel> SelectConsolidatedDraftBillList(int? ID);
        IEnumerable<ProformaInvoiceModel> SelectProformaInvoiceList(int? ID);

        IEnumerable<GcTariffModel> GetPending(int? OrderID, short? TariffTypeID);
        IEnumerable<GcTariffModel> GetPending(int? OrderID, short? TariffTypeID, DateOnly? begin, DateOnly? end);
        IEnumerable<GcTariffModel> GetBilled(int? ConsolidatedDraftBillID);
        GstModel GetGstRate(int? DraftBillID);
    }
    public class FreightBillingService: IFreightBillingService
    { 
        IDbService Iservice;

        public FreightBillingService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int DeleteFinalInvoice(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BillingID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Billing.FinalInvoice.Delete]", parameters);

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
                yield return new GcTariffModel()
                {
                    GcTariffID = dr.GetInt64("GcTariffID"),
                    ConsolidatedDraftBillID = dr.GetInt32("ConsolidatedDraftBillID"),
                    GcID = dr.GetInt64("GcID"),
                    TariffID = dr.GetInt32("TariffID"),
                    Amount = dr.GetDecimal("Amount"),
                };
            }
        }
        public IEnumerable<ProformaInvoiceModel> SelectProformaInvoiceList(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ProformaInvoiceID", ID)
            };
            
                    

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.ProformaInvoice.Select]", parameters))
            {
                yield return new ProformaInvoiceModel()
                {
                    ProformaInvoiceID = dr.GetInt32("ProformaInvoiceID"),
                    DraftBillID = dr.GetInt32("DraftBillID"),
                    OrderID = dr.GetInt32("PartyBranchID"),
                    PartyBranchCoa = dr.GetInt32("PartyBranchCoa"),
                    TariffTypeID = dr.GetInt32("TariffTypeID"),
                    TariffTypeCoa = dr.GetInt32("TariffTypeCoa"),
                    Reference = dr.GetString("Reference"),
                    BranchID = dr.GetInt32("BranchID"),
                    // BranchName = dr.GetString("BranchName"),
                    ApprovedInfo = new ArmsModels.SharedModels.UserInfoModel()
                    {
                        RecordStatus = dr.GetByte("ApprovedStatus"),
                        TimeStampField = dr.GetDateTime("ApprovedOn"),
                        UserID = dr.GetString("ApprovedBy"),
                    },
                    DocumentDate = dr.GetDateTime("DocDate"),
                    DocumentNumber = dr.GetString("DocumentNumber"),
                    MID = dr.GetInt32("MID"),
                    CostCenter = dr.GetInt32("CostCenter"),
                    Dimension = dr.GetInt32("Dimension"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                    Narration = dr.GetString("Narration"),
                    Gst = new()
                    {
                        GstRate = dr.GetDecimal("GstRate"),
                        Cgst = dr.GetDecimal("Cgst"),
                        Sgst = dr.GetDecimal("Sgst"),
                        Igst = dr.GetDecimal("Igst"),
                    },
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }

        public IEnumerable<ConsolidatedDraftBillModel> SelectConsolidatedDraftBillList(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DraftBillID", ID)
            };



            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Billing.ConsolidatedDraftBill.Select]", parameters))
            {
                yield return new ConsolidatedDraftBillModel()
                {
                    DraftBillID = dr.GetInt32("DraftBillID"),
                    BranchID = dr.GetInt32("BranchID"),
                    // BranchName = dr.GetString("BranchName"), Order=new OrderModel
                    Order = new OrderModel()
                    {
                        OrderID = dr.GetInt32("OrderID")
                    },
                    TariffType = new TariffTypeModel()
                    {
                        TariffTypeID = dr.GetInt16("TariffTypeID"),
                    },
                    DocumentDate = dr.GetDateTime("DocumentDate"),
                    DocumentNumber = dr.GetString("DocumentNumber"),
                    TotalAmount = dr.GetDecimal("TotalAmount"),
                    Narration = dr.GetString("Narration"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
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
                yield return new GcTariffModel()
                {
                   
                    ConsolidatedDraftBillID = dr.GetInt32("ConsolidatedDraftBillID"),
                    GcID = dr.GetInt32("GcID"),
                    TariffID = dr.GetInt32("TariffID"),
                    Amount = dr.GetDecimal("Amount"),
                };
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
               new SqlParameter("@BillingID", model.BillingID),               
               new SqlParameter("@ProformaInvoiceID", model.ProformaInvoiceID),
               new SqlParameter("@DraftBillID", model.DraftBillID),
               new SqlParameter("@OrderID", model.OrderID),
               new SqlParameter("@PartyBranchCoa", model.PartyBranchCoa),
               new SqlParameter("@TariffTypeID", model.TariffTypeID),
               new SqlParameter("@TariffTypeCoa", model.TariffTypeCoa),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),               
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@GstRate", model.Gst.GstRate),
               new SqlParameter("@Cgst", model.Gst.Cgst),
               new SqlParameter("@Sgst", model.Gst.Sgst),
               new SqlParameter("@Igst", model.Gst.Igst),
               new SqlParameter("@UserID", model.UserInfo.UserID),
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
                OrderID =  dr.GetInt32("PartyBranchID"),
                PartyBranchCoa = dr.GetInt32("PartyBranchCoa"),
                TariffTypeID =  dr.GetInt32("TariffTypeID"),
                TariffTypeCoa = dr.GetInt32("TariffTypeCoa"),
                Reference = dr.GetString("Reference"),
                BranchID = dr.GetInt32("BranchID"),
                // BranchName = dr.GetString("BranchName"),
                ApprovedInfo = new ArmsModels.SharedModels.UserInfoModel()
                {
                    RecordStatus = dr.GetByte("ApprovedStatus"),
                    TimeStampField = dr.GetDateTime("ApprovedOn"),
                    UserID = dr.GetString("ApprovedBy"),
                },
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                CostCenter = dr.GetInt32("CostCenter"),                
                Dimension = dr.GetInt32("Dimension"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                Gst = new GstModel()
                {
                    GstRate = dr.GetDecimal("GstRate"),
                    Cgst = dr.GetDecimal("Cgst"),
                    Sgst = dr.GetDecimal("Sgst"),
                    Igst = dr.GetDecimal("Igst"),
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
                OrderID = dr.GetInt32("PartyBranchID"),
                PartyBranchCoa = dr.GetInt32("PartyBranchCoa"),
                TariffTypeID = dr.GetInt32("TariffTypeID"),
                TariffTypeCoa = dr.GetInt32("TariffTypeCoa"),
                Reference = dr.GetString("Reference"),
                BranchID = dr.GetInt32("BranchID"),
                // BranchName = dr.GetString("BranchName"),
                ApprovedInfo = new ArmsModels.SharedModels.UserInfoModel()
                {
                    RecordStatus = dr.GetByte("ApprovedStatus"),
                    TimeStampField = dr.GetDateTime("ApprovedOn"),
                    UserID = dr.GetString("ApprovedBy"),
                },
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),
                MID = dr.GetInt32("MID"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension = dr.GetInt32("Dimension"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                Gst = new()
                {
                    GstRate = dr.GetDecimal("GstRate"),
                    Cgst = dr.GetDecimal("Cgst"),
                    Sgst = dr.GetDecimal("Sgst"),
                    Igst = dr.GetDecimal("Igst"),
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
                // BranchName = dr.GetString("BranchName"),
                Order = new OrderModel()
                {
                    OrderID = dr.GetInt32("OrderID")
                },
                TariffType = new TariffTypeModel()
                {
                    TariffTypeID = dr.GetInt16("TariffTypeID"),
                },
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocumentNumber"),               
                TotalAmount = dr.GetDecimal("TotalAmount"),
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
               new SqlParameter("@PartyBranchCoa", model.PartyBranchCoa),
               new SqlParameter("@TariffTypeID", model.TariffTypeID),
               new SqlParameter("@TariffTypeCoa", model.TariffTypeCoa),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@GstRate", model.Gst.GstRate),
               new SqlParameter("@Cgst", model.Gst.Cgst),
               new SqlParameter("@Sgst", model.Gst.Sgst),
               new SqlParameter("@Igst", model.Gst.Igst),
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
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@OrderID", model.Order.OrderID),
               new SqlParameter("@TariffTypeID", model.TariffType.TariffTypeID),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@BookedGcs", model.BookedGCs.ToDataTable()),
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

        public int DeleteProformaInvoice(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }

        public int DeleteConsolidatedDraftBill(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GcTariffModel> GetPending(int? OrderID, short? TariffTypeID, DateOnly? begin, DateOnly? end)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "Pending"),
               new SqlParameter("Begin",begin),
               new SqlParameter("End",end),
               new SqlParameter("@OrderID", OrderID),
               new SqlParameter("@TariffTypeID", TariffTypeID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.TariffEntry.Select]", parameters))
            {
                yield return new GcTariffModel()
                {
                    ConsolidatedDraftBillID = dr.GetInt32("ConsolidatedDraftBillID"),
                    GcID = dr.GetInt32("GcID"),
                    TariffID = dr.GetInt32("TariffID"),
                    Amount = dr.GetDecimal("Amount"),
                };
            }
        }
        public int? ApproveProformaInvoice(int? ProformaInvoiceID, string userID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ProformaInvoiceID", ProformaInvoiceID),
               new SqlParameter("@UserID", userID),
               new SqlParameter("@Status", 1)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Billing.ProformaInvoice.Approve]", parameters);
        }
        public int? ReverseProformaInvoice(int? ProformaInvoiceID, string userID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ProformaInvoiceID", ProformaInvoiceID),
               new SqlParameter("@UserID", userID),
               new SqlParameter("@Status", 0)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Billing.ProformaInvoice.Approve]", parameters);
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
                    Cgst = dr.GetDecimal("Cgst"),
                    Sgst = dr.GetDecimal("Cgst"),
                    Igst = dr.GetDecimal("Igst"),
                };
            }
            return new GstModel();
        }
    }

}

