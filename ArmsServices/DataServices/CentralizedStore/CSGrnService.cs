using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ArmsModels.BaseModels;
namespace ArmsServices.DataServices
{
    public class CSGrnService : ICSGrnService
    {
        IDbService Iservice;

        public CSGrnService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Method to approve a tax purchase entry
         public int Approve(int? GrnID, string UserID, string Remark)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", GrnID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remark)
            };
            return Iservice.ExecuteNonQuery("[usp.Centralized.Store.GoodsReceiptNote.Approve]", parameters);
        }

        // Method to approve a tax purchase entry
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Centralized.Store.GoodsReceiptNote.Delete]", parameters);
        }       

        // Method to get items associated with a specific tax purchase
        public IEnumerable<CSGrnItemModel> GetItems(int? GrnID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetItems"),
               new SqlParameter("@GrnID", GrnID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.GoodsReceiptNote.Select]", parameters))
            {
                yield return new CSGrnItemModel()
                {
                    GrnItemID = dr.GetInt64("GrnItemID"),
                    GrnID = dr.GetInt32("GrnID"),
                    POItemID = dr.GetInt64("POItemID"),
                    ItemID = dr.GetInt32("ItemID"),
                    ItemCode = dr.GetString("InventoryItemCode"),
                    ItemDescription = dr.GetString("ItemDescription"),
                    ItemGroupDescription = dr.GetString("ItemGroupDescription"),
                    UoM = dr.GetString("UoM"),
                    PartNumber = dr.GetString("PartNumber"),
                    CoaID = dr.GetInt32("CoaID"),
                    ItemQty = dr.GetDecimal("ItemQty"),
                    ItemRate = dr.GetDecimal("ItemRate"),
                    GstRate = dr.GetDecimal("GstRate"),
                    SGST = dr.GetDecimal("SGST"),
                    CGST = dr.GetDecimal("CGST"),
                    IGST = dr.GetDecimal("IGST"),
                    TDS = dr.GetDecimal("TDS"),
                    ItemGstVal = dr.GetDecimal("ItemGstVal"),
                    Amount = dr.GetDecimal("Amount"),
                    MRP = dr.GetDecimal("MRP"),
                    BatchNo = dr.GetString("BatchNo"),
                };
            }
        }

        // Method to reverse a tax purchase entry
        public int Reverse(int? GrnID, string UserID, String Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", GrnID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Centralized.Store.GoodsReceiptNote.Reverse]", parameters);
        }

        // Method to select all tax purchase entries
        public IEnumerable<CSGrnModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.GoodsReceiptNote.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select approved tax purchase entries
        public IEnumerable<CSGrnModel> SelectByApproved(int? BranchID, int? NumberOfRecords, string searchTerm)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.GoodsReceiptNote.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select unapproved tax purchase entries
        public IEnumerable<CSGrnModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, string searchTerm)

        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByUnapproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@numberOfRecords", NumberOfRecords),
               new SqlParameter("@searchTerm", searchTerm)
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.GoodsReceiptNote.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Method to select a tax purchase entry by its ID
        public CSGrnModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            CSGrnModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.GoodsReceiptNote.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Method to update a tax purchase entry
        public CSGrnModel Update(CSGrnModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@GrnID", model.GrnID),
               new SqlParameter("@DocDate", model.DocumentDate),
               new SqlParameter("@DocNo", model.DocumentNumber),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@POID", model.POID),
               new SqlParameter("@InvoiceNo", model.InvoiceNo),
               new SqlParameter("@InvoiceDate", model.InvoiceDate),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@PartyID", model.PartyInfo.PartyID),
               new SqlParameter("@PartyCode", model.PartyInfo.PartyCode),
               new SqlParameter("@Reference", model.Reference),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@IsCredit", model.IsCredit),
               new SqlParameter("@TotalAmount", model.TotalAmount),
               new SqlParameter("@RoundOff", model.RoundOff),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@Items", model.Items.ToDataTable())
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Centralized.Store.GoodsReceiptNote.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Helper method to map data record to TaxPurchaseModel
        private CSGrnModel GetModel(IDataRecord dr)
        {
            return new CSGrnModel
            {
                GrnID = dr.GetInt32("GrnID"),
                DocumentNumber = dr.GetString("DocNo"),
                DocumentDate = dr.GetDateTime("DocDate"),
                MID = dr.GetInt32("MID"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                POID = dr.GetInt32("POID"),
                InvoiceNo = dr.GetString("InvoiceNo"),
                InvoiceDate = dr.GetDateTime("InvoiceDate"),
                BranchID = dr.GetInt32("BranchID"),
                PartyInfo = new PartyModel()
                {
                    PartyID = dr.GetInt32("PartyID"),
                    PartyCode = dr.GetString("PartyCode"),
                    TradeName = dr.GetString("TradeName"),
                },
                Reference = dr.GetString("Reference"),
                Narration = dr.GetString("Narration"),
                IsCredit = dr.GetBoolean("IsCredit"),
                TotalAmount = dr.GetDecimal("TotalAmount"),
                RoundOff = dr.GetDecimal("RoundOff"),
                AuthLevelId = dr.GetInt32("AuthLevelId"),
                AuthStatus = dr.GetString("AuthStatus"),
                IssuedQty = dr.GetDecimal("IssuedQty"),
                FileName = dr.GetString("FilePath"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<CSGrnModel> Select(int? BranchID)
        {
            throw new NotImplementedException();
        }
        CSGrnModel ICSGrnService.SelectByID(int? ID)
        {
            throw new NotImplementedException();
        }

        CSGrnModel IbaseInterface<CSGrnModel>.SelectByID(int? ID)
        {
            throw new NotImplementedException();
        }

        // Method to remove a file associated with a tax purchase entry
        public int RemoveFile(int? ID, string UserID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CSGrnModel> SelectByApproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CSGrnModel> SelectByUnapproved(int? BranchID, int? NumberOfRecords, bool InterBranch, string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}