using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsServices.DataServices
{
    public class ConsigneeService : IConsigneeService
    {
        IDbService Iservice;
        IAddressService Iaddress;

        public ConsigneeService(IDbService iservice, IAddressService addressService)
        {
            Iservice = iservice;
            Iaddress = addressService;
        }

        // Method to delete a consignee by its ID
        public async Task<int> Delete(int? ConsigneeID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ConsigneeID", ConsigneeID),
               new SqlParameter("@UserID", UserID),
            };
            return await Iservice.ExecuteNonQueryAsync("[usp.Gc.Consignee.Delete]", parameters);

        }

        // Method to select consignee details by ID
        public async IAsyncEnumerable<ConsigneeModel> Select(int? ConsigneeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ConsigneeID)
            };

            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Consignee.Select]", parameters))
            {
                yield return await GetModel(dr);
            }

        }

        // Method to update a consignee's details
        public async Task<ConsigneeModel> Update(ConsigneeModel model)
        {
            model.Address.UserInfo = model.UserInfo;
            AddressModel address = Iaddress.Update(model.Address);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ConsigneeID", model.ConsigneeID),
               new SqlParameter("@ConsigneeName", model.ConsigneeName),
               new SqlParameter("@Consignor", model.Consignor),
               new SqlParameter("@Mobile", model.Mobile),
               new SqlParameter("@OrderID", model.OrderID),
               new SqlParameter("@PlaceID", model.PlaceID),
               new SqlParameter("@AddressID", address.AddressID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Consignee.Update]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }

        // Method to select a consignee by its ID
        public async Task<ConsigneeModel> SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            ConsigneeModel model = new ConsigneeModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Consignee.Select]", parameters))
            {
                model = await GetModel(dr);
            }
            return model;
        }

        // Method to select consignee details by order ID
        public async IAsyncEnumerable<ConsigneeModel> SelectByOrder(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
               new SqlParameter("@Operation", "ByOrder"),
            };

            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Consignee.Select]", parameters))
            {
                yield return await GetModel(dr);
            }
        }

        // Helper method to map data record to ConsigneeModel
        private async Task<ConsigneeModel> GetModel(IDataRecord dr)
        {
            return new ConsigneeModel
            {
                ConsigneeID = dr.GetInt32("ConsigneeID"),
                ConsigneeName = dr.GetString("ConsigneeName"),
                ArdCode = dr.GetString("ArdCode"),
                Consignor = dr.GetBoolean("Consignor"),
                Mobile = dr.GetString("Mobile"),
                Address = Iaddress.SelectByID(dr.GetInt32("AddressID").GetValueOrDefault()),
                OrderID = dr.GetInt32("OrderID"),
                PlaceID = dr.GetInt32("PlaceID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        // Method to get stockist freight receivables for a consignee
        public IEnumerable<StockistFreightReceivableModel> GetStockistFreightReceivables(int? ConsigneeID, int? BranchID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ConsigneeID", ConsigneeID),
               new SqlParameter("@BranchID", BranchID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Gc.StockistFreight.Select]", parameters))
            {
                yield return new StockistFreightReceivableModel()
                {
                    Amount = dr.GetDecimal("Amount"),
                    EntryRef = dr.GetString("EntryRef"),
                    DocumentDate = dr.GetDateTime("DocumentDate"),
                    InvoiceNo = dr.GetString("InvoiceNo"),
                    Consignee = new ConsigneeModel()
                    {
                        ConsigneeID = dr.GetInt32("ConsigneeID"),
                        ArdCode = dr.GetString("ArdCode"),
                        Mobile = dr.GetString("Mobile"),
                        ConsigneeName = dr.GetString("ConsigneeName"),                        
                    }
                };
            }
        }
    }
}
