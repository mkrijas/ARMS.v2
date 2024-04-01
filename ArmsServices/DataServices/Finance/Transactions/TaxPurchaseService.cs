using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Reflection;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace ArmsServices.DataServices
{
    public class TaxPurchaseService : ITaxPurchaseService
    {
        IDbService Iservice;

        public TaxPurchaseService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int? PID, string UserID, string Remark)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", PID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remark)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxPurchase.Approve]", parameters);
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxPurchase.Delete]", parameters);
        }

        public IEnumerable<TaxPurchaseExpenseModel> GetExpenses(int? PID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetExp"),
               new SqlParameter("@PID", PID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
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
                    GstRate = dr.GetDecimal("GstRate"),
                    BillReference = dr.GetString("BillReference"),
                    BranchID = dr.GetInt32("BranchID"),
                    UsageCode = dr.GetString("UsageCode"),
                    SubArdCode = dr.GetString("SubArdCode"),
                    UsageCodeDescription = dr.GetString("UsageDescription"),
                    TpeID = dr.GetInt64("TpeID"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    DimensionVal = dr.GetString("Dimension"),
                    CostCenter = dr.GetInt32("CostCenterID"),
                    Dimension = dr.GetInt32("DimensionID")
                };
            }
        }

        public IEnumerable<TaxPurchaseItemModel> GetItems(int? PID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetItems"),
               new SqlParameter("@PID", PID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return new TaxPurchaseItemModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    CGST = dr.GetDecimal("CGST"),
                    IGST = dr.GetDecimal("IGST"),
                    SGST = dr.GetDecimal("SGST"),
                    GstRate = dr.GetDecimal("GstRate"),
                    ItemID = dr.GetInt32("ItemID"),
                    CoaID = dr.GetInt32("CoaID"),
                    ItemQty = dr.GetDecimal("ItemQty"),
                    ItemRate = dr.GetDecimal("ItemRate"),
                    PID = dr.GetInt32("PID"),
                    TDS = dr.GetDecimal("TDS"),
                    TpiID = dr.GetInt64("TpiID"),
                    ItemDescription = dr.GetString("ItemDescription"),
                    CostCenter = dr.GetInt32("CostCenterID"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    Dimension = dr.GetInt32("DimensionID"),
                    DimensionVal = dr.GetString("Dimension"),
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
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxPurchase.Reverse]", parameters);
        }

        //public int Reverse(int? PID, string UserID)
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerable<TaxPurchaseModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TaxPurchaseModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
               new SqlParameter("@Type", Type)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TaxPurchaseModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type)

        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
               new SqlParameter("@Type", Type)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public TaxPurchaseModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            TaxPurchaseModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<TaxPurchaseModel> SelectByParty(int? PartyID, int? PartyBranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@PartyBranchID", PartyBranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TaxPurchaseModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public TaxPurchaseModel Update(TaxPurchaseModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", model.PID),
               new SqlParameter("@AdditionalTDS", model.AdditionalTDS),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@Expenses", model.Expenses.ToDataTable()),
               new SqlParameter("@GRNID", model.GRNID),
               new SqlParameter("@InvoiceDate", model.InvoiceDate),
               new SqlParameter("@InvoiceNo", model.InvoiceNo),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@IsCredit", model.IsCredit),               
               new SqlParameter("@Items", model.Items.ToDataTable()),
               new SqlParameter("@NonStoreInventory", model.NonStoreInventory),
               new SqlParameter("@PartyID", model.PartyInfo.PartyID),
               new SqlParameter("@PartyCode", model.PartyInfo.PartyCode),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@Assets", model.Assets.ToDataTable()),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private TaxPurchaseModel GetModel(IDataRecord dr)
        {
            return new TaxPurchaseModel
            {
                PID = dr.GetInt32("PID"),
                AdditionalTDS = dr.GetDecimal("AdditionalTDS"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                GRNID = dr.GetInt32("GRNID"),
                InvoiceDate = dr.GetDateTime("InvoiceDate"),
                IsCredit = dr.GetBoolean("IsCredit"),
                InvoiceNo = dr.GetString("InvoiceNo"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                MID = dr.GetInt32("MID"),
                FileName = dr.GetString("FilePath"),                
                NonStoreInventory = dr.GetBoolean("NonStoreInventory"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                PartyInfo = new PartyModel()
                {
                    PartyID = dr.GetInt32("PartyID"),
                    TradeName = dr.GetString("TradeName"),
                    PartyCode = dr.GetString("PartyCode"),
                },
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public TaxPurchaseModel CheckInvoiceDuplication(TaxPurchaseModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@PID", model.PID),
                new SqlParameter("@InvoiceDate", model.InvoiceDate),
                new SqlParameter("@InvoiceNo", model.InvoiceNo),
                new SqlParameter("@PartyID", model.PartyInfo.PartyID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Validate]", parameters))
            {
                return new TaxPurchaseModel
                {
                    PID = dr.GetInt32("PID"),
                    DocumentDate = dr.GetDateTime("DocDate"),
                    DocumentNumber = dr.GetString("DocNumber"),
                };
            }
            return null;
        }

        //public TaxPurchaseModel UpdateAssetPO(TaxPurchaseModel model)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>
        //    {
        //       new SqlParameter("@PID", model.PID),
        //       new SqlParameter("@AdditionalTDS", model.AdditionalTDS),
        //       new SqlParameter("@BranchID", model.BranchID),
        //       new SqlParameter("@DocumentDate", model.DocumentDate),
        //       new SqlParameter("@DocumentNumber", model.DocumentNumber),
        //       new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
        //       new SqlParameter("@Expenses", model.Expenses.ToDataTable()),
        //       new SqlParameter("@GRNID", model.GRNID),
        //       new SqlParameter("@InvoiceDate", model.InvoiceDate),
        //       new SqlParameter("@InvoiceNo", model.InvoiceNo),
        //       new SqlParameter("@FilePath", model.FileName),
        //       new SqlParameter("@IsCredit", model.IsCredit),
        //       new SqlParameter("@Items", model.Items.ToDataTable()),
        //       new SqlParameter("@NonStoreInventory", model.NonStoreInventory),
        //       new SqlParameter("@PartyID", model.PartyInfo.PartyID),
        //       new SqlParameter("@PartyCode", model.PartyInfo.PartyCode),
        //       new SqlParameter("@TotalAmount", model.TotalAmount),
        //       new SqlParameter("@Narration", model.Narration),
        //       new SqlParameter("@UserID", model.UserInfo.UserID),
        //       new SqlParameter("@Assets", model.Assets.ToDataTable())
        //    };
        //    foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Update]", parameters))
        //    {
        //        model = GetModel(dr);
        //    }
        //    return model;
        //}
        public IEnumerable<AssetPOModel> GetAssets(int? PID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetAssets"),
               new SqlParameter("@PID", PID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return new AssetPOModel()
                {
                    AssetID = dr.GetInt32("AssetID"),
                    BranchID = dr.GetInt32("BranchID"),
                    AssetCode = dr.GetString("AssetCode"),
                    IsComplex = dr.GetBoolean("IsComplex"),
                    //ParentAssetID = dr.GetInt32("ParentAssetID"),
                    //TotalValue = dr.GetDecimal("TotalValue"),
                    Scrap = dr.GetBoolean("Scrap"),
                    BookValue = dr.GetDecimal("BookValue"),
                    DepreciationBookCode = dr.GetString("DepreciationBookCode"),
                    DepreciationEndingDate = dr.GetDateTime("DepreciationEndingDate"),
                    DepreciationStartingDate = dr.GetDateTime("DepreciationStartingDate"),
                    DepreciationMethod = dr.GetString("DepreciationMethod"),
                    Description = dr.GetString("Description"),
                    CurrentValue = dr.GetDecimal("CurrentValue"),
                    GstRateID = dr.GetInt32("GstRateID"),
                    GstMechanism = dr.GetString("GstMechanism"),
                    HsnCode = dr.GetString("HsnCode"),
                    NatureOfAsset = dr.GetString("NatureOfAsset"),
                    ProjectedDisposalDate = dr.GetDateTime("ProjectedDisposalDate"),
                    RateOfDepreciation = dr.GetDecimal("RateOfDepreciation"),
                    SalvageValue = dr.GetDecimal("SalvageValue"),
                    SerialNumber = dr.GetString("SerialNumber"),
                    SpanOfYear = dr.GetDecimal("SpanOfYear"),
                    //Status = dr.GetString("Status"),
                    WarrentyDate = dr.GetDateTime("WarrentyDate"),
                    GSTValue = dr.GetDecimal("GSTValue"),
                    GetAccountRuleDefinition = dr.GetInt32("AccountDef"),
                    AccountName = dr.GetString("AccountName"),
                    CoaID = dr.GetInt32("CoaID"),
                    TaxRate = dr.GetDecimal("TaxRate"),
                    VendorInfo = new()
                    {
                        PartyID = dr.GetInt32("PartyID"),
                        TradeName = dr.GetString("TradeName"),
                    },
                    //Description = dr.GetString("Description"),
                    //NatureOfAsset = dr.GetString("NatureOfAsset"),
                    //AssetCode = dr.GetString("AssetCode"),
                    //AccountName = dr.GetString("AccountName"),
                    //BookValue = dr.GetDecimal("BookValue"),
                    //TaxRate = dr.GetDecimal("TaxRate"),
                    //GSTValue = dr.GetDecimal("GSTValue"),
                    CGSTValue = dr.GetDecimal("CGSTValue"),
                    SGSTValue = dr.GetDecimal("SGSTValue"),
                    IGSTValue = dr.GetDecimal("IGSTValue"),
                    TDS = dr.GetDecimal("TDS"),
                };
            }
        }

        public IEnumerable<TaxPurchaseModel> Select(int? BranchID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaxPurchaseModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaxPurchaseModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}