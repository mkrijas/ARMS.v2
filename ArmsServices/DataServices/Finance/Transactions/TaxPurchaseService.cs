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

        // Method to approve a tax purchase entry
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

        // Method to approve a tax purchase entry
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "DELETE"),
               new SqlParameter("@PID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxPurchase.Delete]", parameters);
        }

        // Method to remove a file associated with a tax purchase entry
        public int RemoveFile(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "REMOVEFILE"),
               new SqlParameter("@PID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.TaxPurchase.Delete]", parameters);
        }

        // Method to get expenses associated with a specific tax purchase
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
                    GstMechanism = dr.GetString("gstMechanism"),
                    TpeID = dr.GetInt64("TpeID"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    DimensionVal = dr.GetString("Dimension"),
                    CostCenter = dr.GetInt32("CostCenterID"),
                    Dimension = dr.GetInt32("DimensionID")
                };
            }
        }

        // Method to get items associated with a specific tax purchase
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
                    ItemCode = dr.GetString("InventoryItemCode"),
                    ItemDescription = dr.GetString("ItemDescription"),
                    ItemGroupDescription = dr.GetString("ItemGroupDescription"),
                    PartNumber = dr.GetString("PartNumber"),
                    CostCenter = dr.GetInt32("CostCenterID"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    Dimension = dr.GetInt32("DimensionID"),
                    DimensionVal = dr.GetString("Dimension"),
                };
            }
        }

        // Method to reverse a tax purchase entry
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

        // Method to select all tax purchase entries
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

        // Method to select approved tax purchase entries
        public IEnumerable<TaxPurchaseModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type, string TaxPurchaseType)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
               new SqlParameter("@Type", Type),
               new SqlParameter("@TaxPurchaseType", TaxPurchaseType)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select unapproved tax purchase entries
        public IEnumerable<TaxPurchaseModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type, string TaxPurchaseType)

        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
               new SqlParameter("@Type", Type),
               new SqlParameter("@TaxPurchaseType", TaxPurchaseType)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a tax purchase entry by its ID
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

        // Method to select tax purchases by party
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

        // Method to select tax purchases by period
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

        // Method to update a tax purchase entry
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
               new SqlParameter("@TaxPurchaseType", model.TaxPurchaseType ),
               new SqlParameter("@JobcardID", model.JobcardID )
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.TaxPurchase.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to TaxPurchaseModel
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
                TaxPurchaseType = dr.GetString("TaxPurchaseType"),
                TDS = dr.GetDecimal("TDS"),
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

        // Method to check for invoice duplication
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

        // Method to get assets associated with a specific tax purchase
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
                    BookValue = dr.GetDecimal("BookValue"),
                    Description = dr.GetString("Description"),
                    GstMechanism = dr.GetString("GstMechanism"),
                    GSTValue = dr.GetDecimal("GSTValue"),
                    AccountName = dr.GetString("AccountName"),
                    CoaID = dr.GetInt32("CoaID"),
                    TaxRate = dr.GetDecimal("TaxRate"),
                    CGSTValue = dr.GetDecimal("CGSTValue"),
                    SGSTValue = dr.GetDecimal("SGSTValue"),
                    IGSTValue = dr.GetDecimal("IGSTValue"),
                    TDS = dr.GetDecimal("TDS"),
                    SubARDCode = dr.GetString("SubARDCode")
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

        public IEnumerable<TaxPurchaseModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaxPurchaseModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public TaxPurchaseModel TDSReverseUpdate(TaxPurchaseModel model)
        {
            throw new NotImplementedException();
        }
    }
}