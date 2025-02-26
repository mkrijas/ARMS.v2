using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using System.Security.Cryptography;


namespace ArmsServices.DataServices
{
    public class SaleService : ISaleService
    {
        IDbService Iservice;

        public SaleService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to approve a sale
        public int Approve(int? SID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SID", SID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Sales.Approve]", parameters);
        }

        // Method to delete a sale
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Sales.Delete]", parameters);
        }

        // Method to get particulars of a sale
        public IEnumerable<TaxPurchaseExpenseModel> GetParticulars(int? SID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetExp"),
               new SqlParameter("@SID", SID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Select]", parameters))
            {
                yield return new TaxPurchaseExpenseModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    CGST = dr.GetDecimal("CGST"),
                    IGST = dr.GetDecimal("IGST"),
                    SGST = dr.GetDecimal("SGST"),
                    CoaID = dr.GetInt32("CoaID"),
                    PID = dr.GetInt32("SID"),
                    TDS = dr.GetDecimal("TCS"),
                    GstRate = dr.GetDecimal("GstRate"),
                    BillReference = dr.GetString("BillReference"),
                    BranchID = dr.GetInt32("BranchID"),
                    UsageCode = dr.GetString("UsageCode"),
                    SubArdCode = dr.GetString("SubArdCode"),
                    TpeID = dr.GetInt64("SeID"),
                    CostCenter = dr.GetInt32("CostCenterID"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    Dimension = dr.GetInt32("DimensionID"),
                    DimensionVal = dr.GetString("Dimension"),
                    UsageCodeDescription = dr.GetString("UsageCodeDescription"),
                };
            }
        }

        // Method to get items associated with a sale
        public IEnumerable<TaxPurchaseItemModel> GetItems(int? PID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetItems"),
               new SqlParameter("@SID", PID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Select]", parameters))
            {
                yield return new TaxPurchaseItemModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    ItemDescription = dr.GetString("ItemDescription"),
                    CGST = dr.GetDecimal("CGST"),
                    IGST = dr.GetDecimal("IGST"),
                    SGST = dr.GetDecimal("SGST"),
                    ItemID = dr.GetInt32("InventoryItemID"),
                    CoaID = dr.GetInt32("CoaID"),
                    GstRate = dr.GetDecimal("GstRate"),
                    ItemQty = dr.GetDecimal("ItemQty"),
                    ItemRate = dr.GetDecimal("ItemRate"),
                    PID = dr.GetInt32("SID"),
                    TDS = dr.GetDecimal("TCS"),
                    TpiID = dr.GetInt64("SiID"),
                    CostCenter = dr.GetInt32("CostCenterID"),
                    CostCenterVal = dr.GetString("CostCenter"),
                    Dimension = dr.GetInt32("DimensionID"),
                    DimensionVal = dr.GetString("Dimension"),
                };
            }
        }

        // Method to get assets associated with a sale
        public IEnumerable<AssetSaleModel> GetAssets(int? PID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetAssets"),
               new SqlParameter("@SID", PID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Select]", parameters))
            {
                yield return new AssetSaleModel()
                {
                    PID = dr.GetInt32("PID"),
                    AssetID = dr.GetInt32("AssetID"),                    
                    AssetCode = dr.GetString("AssetCode"),
                    AssetName = dr.GetString("Description"),                    
                    SaleValue  = dr.GetDecimal("SaleValue"),
                    CurrentValue = dr.GetDecimal("CurrentValue"),    
                    ID = dr.GetInt64("ID"),
                    TaxableValue = dr.GetDecimal("TaxableValue"),
                    GstMechanism = dr.GetString("GstMechanism"),                   
                    SerialNumber = dr.GetString("SerialNumber"),                   
                    GSTValue = dr.GetDecimal("GSTValue"),                  
                    TaxRate = dr.GetDecimal("TaxRate"),
                    CGSTValue = dr.GetDecimal("CGSTValue"),
                    SGSTValue = dr.GetDecimal("SGSTValue"),
                    IGSTValue = dr.GetDecimal("IGSTValue"),
                    TDS = dr.GetDecimal("TDS"),
                };
            }
        }

        // Method to reverse a sale
        public int Reverse(int? PID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", PID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Status", 2)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Sales.Reverse]", parameters);
        }

        // Method to select all sales
        public IEnumerable<SaleModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select approved sales
        public IEnumerable<SaleModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
               new SqlParameter("@Type", Type)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select unapproved sales
        public IEnumerable<SaleModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm, string Type)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm),
               new SqlParameter("@Type", Type)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a sale by its ID
        public SaleModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@PID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            SaleModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to select sales by party ID
        public IEnumerable<SaleModel> SelectByParty(int? PartyID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select sales by date period
        public IEnumerable<SaleModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to update a sale
        public SaleModel Update(SaleModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SID", model.SID),
               new SqlParameter("@AdditionalTCS", model.AdditionalTCS),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@DocumentNumber", model.DocumentNumber),
               new SqlParameter("@Particulars", model.Particulars.ToDataTable()),
               new SqlParameter("@IsCredit", model.IsCredit),
               new SqlParameter("@Items", model.Items.ToDataTable()),
               new SqlParameter("@Assets", model.Assets.ToDataTable()),
               new SqlParameter("@CustomerID", model.PartyInfo.PartyID),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@CustomerCode", model.PartyInfo.PartyCode),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@SalesType", model.SalesType),
               new SqlParameter("@InvoiceNo", model.InvoiceNo),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to convert IDataRecord to SaleModel
        private SaleModel GetModel(IDataRecord dr)
        {
            return new SaleModel
            {
                SID = dr.GetInt32("SID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                AdditionalTCS = dr.GetDecimal("AdditionalTCS"),
                BranchID = dr.GetInt32("BranchID"),
                DocumentDate = dr.GetDateTime("DocDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                IsCredit = dr.GetBoolean("IsCredit"),
                MID = dr.GetInt32("MID"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                FileName = dr.GetString("FilePath"),
                AuthStatus = dr.GetString("AuthStatus"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                Narration = dr.GetString("Narration"),
                SalesType = dr.GetString("SalesType"),
                InvoiceNo = dr.GetString("InvoiceNo"),
                PartyInfo = new PartyModel()
                {
                    PartyID = dr.GetInt32("CustomerID"),
                    TradeName = dr.GetString("TradeName"),
                    PartyCode = dr.GetString("CustomerCode"),
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
}