using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;

namespace ArmsServices.DataServices
{
    // Service class for managing asset documents
    public class AssetDocumentService : IAssetDocumentService
    {
        IDbService Iservice;
        public AssetDocumentService(IDbService iservice)
        {
            Iservice = iservice;
        }

        // Updates an existing asset document and returns the updated model
        public AssetDocumentModel Update(AssetDocumentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AttachedDocument", model.AttachedDocument),
               new SqlParameter("@EndDate", model.EndDate),
               new SqlParameter("@DocumentID", model.DocumentID),
               new SqlParameter("@DocumentTypeID", model.DocumentType.DocumentTypeID),
               new SqlParameter("@StartDate", model.StartDate),
               new SqlParameter("@AssetID", model.Asset.AssetID),
               new SqlParameter("@ReferenceDate", model.InvoiceDate),
               new SqlParameter("@NotificationID", model.NotificationID),
               new SqlParameter("@ReceiptNo", model.ReceiptNo),
               new SqlParameter("@Refference", model.Refference),
               new SqlParameter("@NCBPercentage", model.NCBPercentage),
               new SqlParameter("@IDVAmount", model.IDVAmount),
               new SqlParameter("@Amount", model.Amount),
               new SqlParameter("@ExtendedEndDate", model.ExtendedEndDate),
               new SqlParameter("@UserID", model.UserInfo.UserID),
               new SqlParameter("@RecordStatus", model.UserInfo.RecordStatus),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Retrieves a list of pending asset documents for a specific branch
        public IEnumerable<AssetDocumentModel> SelectPendingByBranch(int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByBranch"),
               new SqlParameter("@ID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.FCRenewalDoc.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Validates the period of an asset document and returns a list of documents that do not comply
        public IEnumerable<AssetDocumentModel> ValidatePeriod(AssetDocumentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ValidatePeriod"),
               new SqlParameter("@DocumentID", model.DocumentID),
               new SqlParameter("@EndDate", model.EndDate),
               new SqlParameter("@DocumentTypeID", model.DocumentType.DocumentTypeID),
               new SqlParameter("@StartDate", model.StartDate),
               new SqlParameter("@AssetID", model.Asset.AssetID),
               new SqlParameter("@ExtendedEndDate", model.ExtendedEndDate)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Saves the file path for a document and returns the number of affected rows
        public int SaveFilePath(string link, int? id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@LINK",link),
               new SqlParameter("@ID", id)
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Document.FilePath]", parameters);
        }

        // Retrieves an asset document by its ID
        public AssetDocumentModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID) ,
               new SqlParameter("@Operation", "ByID"),
            };
            AssetDocumentModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        // Deletes an asset document by its ID and records the user who performed the deletion
        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Document.Delete]", parameters);
        }

        // Deletes a document type by its ID and records the user who performed the deletion
        public int DeleteType(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentTypeID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Document.Type.Delete]", parameters);
        }

        // Removes an asset document and returns the number of affected rows
        public int Remove(AssetDocumentModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", model.Asset.AssetID),
               new SqlParameter("@DocumentTypeID", model.DocumentType.DocumentTypeID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Document.RemoveType]", parameters);
        }

        // Links a document type with a tax purchase and returns the number of affected rows
        public int LinkDocumentTypeAndTaxPurchase(int? DocumentID, int? TaxPurchaseID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentID", DocumentID),
               new SqlParameter("@TaxPurchaseID", TaxPurchaseID)
            };
            return Iservice.ExecuteNonQuery("[usp.Asset.Document.TaxPurchase.Link.Update]", parameters);
        }

        // Retrieves a list of asset documents created within a specific period
        public IEnumerable<AssetDocumentModel> SelectByPeriod(DateTime? startDate, DateTime? endDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ToBeExpired"),
               new SqlParameter("@StartDate", startDate),
               new SqlParameter("@EndDate", endDate),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Retrieves a list of asset documents associated with past records for a specific asset
        public IEnumerable<AssetDocumentModel> SelectWithPast(int? AssetID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "WithPast"),
               new SqlParameter("@AssetID", AssetID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Retrieves a list of asset documents associated with a specific asset
        public IEnumerable<AssetDocumentModel> SelectByAsset(int? AssetID, bool AllDocs)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", AssetID) ,
               new SqlParameter("@AllDocs", AllDocs) ,
               new SqlParameter("@Operation", "ByAsset"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Retrieves a list of asset documents associated with a specific accident date for an asset
        public IEnumerable<AssetDocumentModel> SelectByAccidentDate(int? AssetID, bool AllDocs)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AssetID", AssetID) ,
               new SqlParameter("@AllDocs", AllDocs) ,
               new SqlParameter("@Operation", "ByAccidentDate"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        // Helper method to map data from the database to an AssetDocumentModel
        private AssetDocumentModel GetModel(IDataRecord dr)
        {
            return new AssetDocumentModel
            {
                AttachedDocument = dr.GetString("AttachedDocument"),
                EndDate = dr.GetDateTime("EndDate"),
                DocumentID = dr.GetInt32("DocumentID"),
                DocumentType = new AssetDocumentTypeModel()
                {
                    DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                    DocumentTypeName = dr.GetString("DocumentTypeName"),
                },
                InvoiceDate = dr.GetDateTime("InvoiceDate"),
                StartDate = dr.GetDateTime("StartDate"),
                Asset = new AssetModel()
                {
                    AssetID = dr.GetInt32("AssetID"),
                    Description = dr.GetString("Description"),
                },
                NotificationID = dr.GetInt32("NotificationID"),
                ReceiptNo = dr.GetString("ReceiptNo"),
                Refference = dr.GetString("Refference"),
                IDVAmount = dr.GetDecimal("IDVAmount"),
                NCBPercentage = dr.GetDecimal("NCBPercentage"),
                Amount = dr.GetDecimal("Amount"),
                IsFinanciallyPosted = dr.GetBoolean("IsFinanciallyPosted"),
                ExtendedEndDate = dr.GetDateTime("ExtendedEndDate"),
                IsFitParameter = dr.GetBoolean("IsFitParameter"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Helper method to map data from the database to an AssetDocumentModel
        public IEnumerable<AssetDocumentTypeModel> GetDocumentTypes()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentType.Select]", null))
            {
                yield return new AssetDocumentTypeModel()
                {
                    DocumentTypeName = dr.GetString("DocumentTypeName"),
                    BlockAfter = dr.GetInt32("BlockAfter"),
                    DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                    WarnBefore = dr.GetInt32("WarnBefore"),
                    UsageCode = new()
                    {
                        Id = dr.GetInt32("UsageID"),
                        GstMechanism = dr.GetString("Area"),
                        Description = dr.GetString("Description"),
                        UsageCode = dr.GetString("UsageCode")
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

        // Updates an existing document type and returns the updated model
        public AssetDocumentTypeModel UpdateDocumentType(AssetDocumentTypeModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BlockAfter", model.BlockAfter),
               new SqlParameter("@DocumentTypeID", model.DocumentTypeID),
               new SqlParameter("@DocumentTypeName", model.DocumentTypeName),
               new SqlParameter("@WarnBefore", model.WarnBefore),
               new SqlParameter("@UsageID", model.UsageCode.Id),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentType.Update]", parameters))
            {
                model = new AssetDocumentTypeModel()
                {
                    DocumentTypeName = dr.GetString("DocumentTypeName"),
                    BlockAfter = dr.GetInt32("BlockAfter"),
                    DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                    WarnBefore = dr.GetInt32("WarnBefore"),
                    UsageCode = new()
                    {
                        Id = dr.GetInt32("UsageID"),
                        GstMechanism = dr.GetString("Area"),
                        Description = dr.GetString("Description"),
                        UsageCode = dr.GetString("UsageCode")
                    },
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
            return model;
        }

        // Checks if a cost center is mandatory for a given document type ID
        public bool? IsCostCenterIsMadatoryForGivenDocumentTypeID(int? DocumentTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentTypeID", DocumentTypeID),
            };
            bool? result = false;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentType.CostCentor.Manadatory]", parameters))
            {
                result = dr.GetBoolean("Result");
            }
            return result;
        }

        // Checks if a dimension is mandatory for a given document type ID
        public bool? IsDimensionIsMadatoryForGivenDocumentTypeID(int? DocumentTypeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentTypeID", DocumentTypeID),
            };
            bool? result = false;
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.DocumentType.Dimension.Manadatory]", parameters))
            {
                result = dr.GetBoolean("Result");
            }
            return result;
        }

        // Validates if the asset document is valid for a given date
        public bool IsValid(AssetDocumentModel model, DateTime? DateToCheck)
        {
            if (!(model.StartDate?.Date <= DateToCheck?.Date && model.EndDate?.Date >= DateToCheck?.Date))
            {
                return false;
            }
            return true;
        }

        // Retrieves a list of new file names for asset documents
        public IEnumerable<AssetDocumentModel> GetNewFileName()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Asset.Document.Attachment.Select]", null))
            {
                yield return new AssetDocumentModel()
                {
                    AttachedDocument = dr.GetString("attachment"),

                };
            }
        }
    }
}