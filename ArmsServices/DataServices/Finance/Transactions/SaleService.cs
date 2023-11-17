using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class SaleService : ISaleService
    {
        IDbService Iservice;

        public SaleService(IDbService iservice)
        {
            Iservice = iservice;
        }

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

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@SID", ID),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.Sales.Delete]", parameters);
        }

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
                    PID = dr.GetInt32("PID"),
                    TDS = dr.GetDecimal("TCS"),
                    GstRate = dr.GetDecimal("GstRate"),
                    BillReference = dr.GetString("BillReference"),
                    BranchID = dr.GetInt32("BranchID"),
                    UsageCode = dr.GetString("UsageCode"),
                    TpeID = dr.GetInt64("TpeID"),
                    CostCenter = dr.GetInt32("CostCenter"),
                    Dimension = dr.GetInt32("Dimension"),
                    UsageCodeDescription = dr.GetString("UsageCodeDescription"),
                };
            }
        }

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
                    ItemID = dr.GetInt32("ItemID"),
                    CoaID = dr.GetInt32("CoaID"),
                    GstRate = dr.GetDecimal("GstRate"),
                    ItemQty = dr.GetDecimal("ItemQty"),
                    ItemRate = dr.GetDecimal("ItemRate"),
                    PID = dr.GetInt32("SID"),
                    TDS = dr.GetDecimal("TCS"),
                    TpiID = dr.GetInt64("SiID"),
                    CostCenter = dr.GetInt32("CostCenter"),
                    Dimension = dr.GetInt32("Dimension"),
                };
            }
        }

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

        public IEnumerable<SaleModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<SaleModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

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
               new SqlParameter("@CustomerID", model.PartyInfo.PartyID),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@CustomerCode", model.PartyInfo.PartyCode),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.Sales.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

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