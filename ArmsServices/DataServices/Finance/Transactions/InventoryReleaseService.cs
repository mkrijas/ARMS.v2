using ArmsModels.BaseModels.Finance.Transactions;
using System.Collections.Generic;
using System;
using ArmsModels.BaseModels;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;

namespace ArmsServices.DataServices.Finance.Transactions
{
    public class InventoryReleaseService : IInventoryReleaseService
    {
        IDbService Iservice;

        public InventoryReleaseService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Approve(int? RID, string UserID, string Remarks)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RID", RID),
               new SqlParameter("@UserID", UserID),
               new SqlParameter("@Remarks", Remarks)
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.InventoryRelease.Approve]", parameters);
        }

        public int Delete(int? ID, bool IsUsedItem, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@IsUsedItem", IsUsedItem),
               new SqlParameter("@UserID", UserID),

            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Transactions.InventoryRelease.Delete]", parameters);
        }

        public IEnumerable<InventoryReleaseSubViewModel> GetRequstSub(int? ID, int? StoreID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetSub"),
               new SqlParameter("@ID", ID),
               new SqlParameter("@StoreID", StoreID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.InventoryRelease.Select]", parameters))
            {
                if ((dr.GetDecimal("ReleaseQty") == null) || (dr.GetDecimal("ReleaseQty") < dr.GetDecimal("RequestQty") || (dr.GetDecimal("ReleaseQty") >= 0)))
                {
                    yield return new InventoryReleaseSubViewModel()
                    {
                        ItemEntryID = dr.GetInt32("ItemEntryID"),
                        //RID = dr.GetInt32("RID"),
                        ItemID = dr.GetInt32("ItemID"),
                        ItemDescription = dr.GetString("ItemDescription"),
                        AvailableQty = dr.GetDecimal("AvailableQty"),
                        RequestQty = dr.GetDecimal("RequestQty"),
                        PendingQty = dr.GetDecimal("ReleaseQty") != null ? (dr.GetDecimal("RequestQty") - dr.GetDecimal("ReleaseQty")) : dr.GetDecimal("RequestQty"),
                        ReleaseQty = dr.GetDecimal("ReleaseQty"),
                        ItemQty = dr.GetDecimal("ReleaseQty") != null ? (dr.GetDecimal("RequestQty") - dr.GetDecimal("ReleaseQty")) : dr.GetDecimal("RequestQty"),
                        UsedQty = dr.GetDecimal("ReleaseQty"),
                    };
                }

            }
        }

        public IEnumerable<InventoryReleaseSubViewModel> GetRequstSubUsed(int? ID, int? StoreID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetSubUsed"),
               new SqlParameter("@ID", ID),
               new SqlParameter("@StoreID", StoreID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.InventoryRelease.Select]", parameters))
            {
                if ((dr.GetDecimal("ReleaseQty") == null) || (dr.GetDecimal("ReleaseQty") < dr.GetDecimal("RequestQty")))
                {
                    yield return new InventoryReleaseSubViewModel()
                    {
                        ItemEntryID = dr.GetInt32("ItemEntryID"),
                        //RID = dr.GetInt32("RID"),
                        ItemID = dr.GetInt32("ItemID"),
                        ItemDescription = dr.GetString("ItemDescription"),
                        AvailableQty = dr.GetDecimal("AvailableQty"),
                        RequestQty = dr.GetDecimal("RequestQty"),
                        PendingQty = dr.GetDecimal("ReleaseQty") != null ? (dr.GetDecimal("RequestQty") - dr.GetDecimal("ReleaseQty")) : dr.GetDecimal("RequestQty"),
                        ReleaseQty = dr.GetDecimal("ReleaseQty"),
                        ItemQty = dr.GetDecimal("ReleaseQty") != null ? (dr.GetDecimal("RequestQty") - dr.GetDecimal("ReleaseQty")) : dr.GetDecimal("RequestQty"),
                    };
                }
            }
        }

        public IEnumerable<InventoryReleaseSubViewModel> GetRequstSubReadOnly(int? ID, int? StoreID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "GetSubReadOnly"),
               new SqlParameter("@ID", ID),
               new SqlParameter("@StoreID", StoreID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.InventoryRelease.Select]", parameters))
            {
                if ((dr.GetDecimal("ReleaseQty")>=0))
                {
                    yield return new InventoryReleaseSubViewModel()
                    {
                        ItemEntryID = dr.GetInt32("ItemEntryID"),
                        //RID = dr.GetInt32("RID"),
                        ItemID = dr.GetInt32("ItemID"),
                        ItemDescription = dr.GetString("ItemDescription"),
                        AvailableQty = dr.GetDecimal("AvailableQty"),
                        RequestQty = dr.GetDecimal("RequestQty"),
                        ReleaseQty = dr.GetDecimal("ReleaseQty"),
                        ItemQty =  dr.GetDecimal("ReleaseQty"),
                    };
                }
            }
        }

        public IEnumerable<InventoryReleaseModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.InventoryRelease.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<InventoryReleaseModel> SelectByStoreID(int? StoreID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByStore"),
               new SqlParameter("@StoreID", StoreID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.InventoryRelease.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<InventoryReleaseModel> SelectByStoreAndBranchIDApproved(int? StoreID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByStoreApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@StoreID", StoreID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.InventoryRelease.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<InventoryReleaseModel> SelectByStoreAndBranchIDUnApproved(int? StoreID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByStoreUnApproved"),
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@StoreID", StoreID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.InventoryRelease.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public InventoryReleaseModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            InventoryReleaseModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.InventoryRelease.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<InventoryReleaseModel> SelectByParty(int? PartyID, int? PartyBranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByParty"),
               new SqlParameter("@PartyID", PartyID),
               new SqlParameter("@PartyBranchID", PartyBranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.InventoryRelease.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<InventoryReleaseModel> SelectByPeriod(DateTime? begin, DateTime? end)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByPeriod"),
               new SqlParameter("@begin", begin),
               new SqlParameter("@end", end),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.InventoryRelease.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public InventoryReleaseModel Update(InventoryReleaseModel model)
        {
            //List<InventoryItemEntryModel> ItemsListFormated = new();
            //foreach (var item in model.Items)
            //{
            //    ItemsListFormated.Add(new()
            //    {
            //        ItemEntryID = item.ItemEntryID,
            //        ItemID = item.ItemID,
            //        ItemRate = item.ItemRate,
            //        ItemQty = item.ItemQty
            //    });
            //}
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RID", model.RID),
               new SqlParameter("@RequestID", model.RequestID),
               new SqlParameter("@DocumentDate", model.DocumentDate),
               new SqlParameter("@RefInvoiceNo", model.RefInvoiceNo),
               new SqlParameter("@NatureOfTransaction", model.NatureOfTransaction),
               new SqlParameter("@StoreID", model.Store?.StoreID??0),
               new SqlParameter("@BranchID", model.BranchID),
               new SqlParameter("@JobcardID", model.Jobcard?.JobcardID??0),
               new SqlParameter("@TruckID", model.Truck?.TruckID??0),
               new SqlParameter("@IsUsedItem",model.IsUsedItem),
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@FilePath", model.FileName),
               new SqlParameter("@Narration", model.Narration),
               new SqlParameter("@Items", model.Items?.ToDataTable()??null),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Transactions.InventoryRelease.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        private InventoryReleaseModel GetModel(IDataRecord dr)
        {
            return new InventoryReleaseModel
            {
                RID = dr.GetInt32("RID"),
                RequestID = dr.GetInt32("RequestID"),
                DocumentDate = dr.GetDateTime("DocumentDate"),
                DocumentNumber = dr.GetString("DocNumber"),
                RefInvoiceNo = dr.GetString("RefInvoiceNo"),
                NatureOfTransaction = dr.GetString("NatureOfTransaction"),
                Store = new()
                {
                    StoreID = dr.GetInt32("StoreID"),
                    StoreName = dr.GetString("StoreName")
                },
                Jobcard = new()
                {
                    JobcardID = dr.GetInt32("JobcardID"),
                    JobcardNumber = dr.GetString("JobcardPrefix") + (dr.GetInt32("JobcardNumber")).ToString()
                },
                Truck = new()
                {
                    TruckID = dr.GetInt32("TruckID"),
                    RegNo = dr.GetString("RegNo")
                },
                IsUsedItem = dr.GetBoolean("IsUsedItem"),
                IsClosed = dr.GetBoolean("IsClosed"),
                CostCenter = dr.GetInt32("CostCenter"),
                Dimension = dr.GetInt32("Dimension"),
                Narration = dr.GetString("Narration"),
                FileName = dr.GetString("FilePath"),
                MID = dr.GetInt32("MID"),
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
