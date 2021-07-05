using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface ITariffService
    {       
        Task<TariffModel> Update(TariffModel model);
        Task<int> Delete(int ID, string UserID);
        IAsyncEnumerable<TariffModel> Select();
        Task<TariffModel> SelectByID(int ID);
        IAsyncEnumerable<TariffFormulaModel> SelectFormulas();
        Task<TariffFormulaModel> SelectFormulaByID(short ID);
        IAsyncEnumerable<TariffTypeModel> SelectTariffTypes();
        Task<TariffTypeModel> SelectTariffTypeByID(short ID);

    }

    public class TariffService : ITariffService
    {
        IDbService Iservice;
        public TariffService(IDbService iservice, IRouteService routeService, IOrderService orderService)
        {
            Iservice = iservice;            
        }

        public async Task<int> Delete(int ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TariffID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return await Iservice.ExecuteNonQueryAsync("[usp.Gc.Tariff.Delete]", parameters);
        }

        public async IAsyncEnumerable<TariffModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),               
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Tariff.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public async Task<TariffModel> SelectByID(int ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),              
            };
            TariffModel model = new TariffModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Tariff.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public async Task<TariffFormulaModel> SelectFormulaByID(short ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),              
            };
            TariffFormulaModel model = new TariffFormulaModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Tariff.Formula.Select]", parameters))
            {
                model = new TariffFormulaModel
                {
                    FormulaID = dr.GetInt16("FormulaID"),
                    Formula = dr.GetString("Formula"),
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

        public async IAsyncEnumerable<TariffFormulaModel> SelectFormulas()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),               
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Tariff.Formula.Select]", parameters))
            {
                yield return new TariffFormulaModel
                {
                    FormulaID = dr.GetInt16("FormulaID"),
                    Formula = dr.GetString("Formula"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }

        public async Task<TariffTypeModel> SelectTariffTypeByID(short ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            TariffTypeModel model = new TariffTypeModel();
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Tariff.Type.Select]", parameters))
            {
                model = new TariffTypeModel
                {
                    TariffTypeID = dr.GetInt16("TariffTypeID"),
                    TariffTypeName = dr.GetString("TariffTypeName"),
                    IsExpense = dr.GetBoolean("IsExpense"),
                    IsIncome = dr.GetBoolean("IsIncome"),
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

        public async IAsyncEnumerable<TariffTypeModel> SelectTariffTypes()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),              
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Tariff.Type.Select]", parameters))
            {
                yield return new TariffTypeModel
                {
                    TariffTypeID = dr.GetInt16("TariffTypeID"),
                    TariffTypeName = dr.GetString("TariffTypeName"),
                    IsExpense = dr.GetBoolean("IsExpense"),
                    IsIncome = dr.GetBoolean("IsIncome"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }

        public async Task<TariffModel> Update(TariffModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OrderID", model.OrderID),
               new SqlParameter("@RouteID", model.RouteID),
               new SqlParameter("@TariffFormulaID", model.TariffFormulaID),
               new SqlParameter("@TariffID", model.TariffID),
               new SqlParameter("@TariffName", model.TariffName),
               new SqlParameter("@TariffRate", model.TariffRate),               
               new SqlParameter("@TariffTypeID", model.TariffTypeID),
               new SqlParameter("@TruckAxles", model.TruckAxles),               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            await foreach (IDataRecord dr in Iservice.GetDataReaderAsync("[usp.Gc.Tariff.Update]", parameters))
            {
                model =  GetModel(dr);
            }
            return model;
        }

        private TariffModel GetModel(IDataRecord dr)
        {
            return new TariffModel
            {
                OrderID = dr.GetInt32("OrderID"),
                RouteID = dr.GetInt32("RouteID"),                
                TariffFormulaID = dr.GetInt16("TariffFormulaID"),
                TariffID = dr.GetInt32("TariffID"),
                TariffName = dr.GetString("TariffName"),
                TariffRate = dr.GetDecimal("TariffRate"),
                TariffTypeID = dr.GetInt16("TariffTypeID"),                
                TruckAxles = dr.GetByte("TruckAxles"),
                Formula = dr.GetString("Formula"),
                OrderName = dr.GetString("OrderName"),
                RouteName = dr.GetString("RouteName"),
                TariffTypeName = dr.GetString("TariffTypeName"),
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
