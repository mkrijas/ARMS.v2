using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;
using Microsoft.Extensions.Configuration;

namespace ArmsServices.DataServices
{
    public interface ITariffService
    {       
        TariffModel Update(TariffModel model);
        int Delete(int? ID, string UserID);
        IEnumerable<TariffModel> Select();
        IEnumerable<TariffModel> SelectByOrder(int? OrderID);
        TariffModel SelectByID(int? ID);
        IEnumerable<TariffFormulaModel> SelectFormulas();
        TariffFormulaModel SelectFormulaByID(short? ID);
        IEnumerable<TariffTypeModel> SelectTariffTypes(string Area);
        TariffTypeModel SelectTariffTypeByID(short? ID);
        TariffTypeModel UpdateTariffType(TariffTypeModel model);
        string[] TariffGroups { get; }
        IEnumerable<TariffModel> GetTariffs(string TariffGroup, int? OrderID, int? RouteID, int? Axles);
        decimal? GetTariffAmount(GcSetModel GcSet,TariffModel Tariff);
        IEnumerable<TariffModel> GeneratePendingTariffs(long? RefID);
    }


    public class TariffService : ITariffService
    {
        IDbService Iservice;
        IConfiguration Configuration;
        private const string Data = "Data";
        private IRouteService Iroute;
        public TariffService(IDbService iservice, IConfiguration configuration,IRouteService _IRoute)
        {
            Iservice = iservice;
            Configuration = configuration;
            Iroute = _IRoute;
        }
        public string[] TariffGroups { get { return Configuration.GetSection(Data).GetSection("TariffGroups").Get<string[]>(); } }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TariffID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Operation.Tariff.Delete]", parameters);
        }

        public IEnumerable<TariffModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),               
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tariff.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public IEnumerable<TariffModel> SelectByOrder(int? OrderID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OrderID", OrderID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tariff.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public TariffModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),              
            };
            TariffModel model = new TariffModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tariff.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public TariffFormulaModel SelectFormulaByID(short? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),              
            };
            TariffFormulaModel model = new TariffFormulaModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TariffFormula.Select]", parameters))
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

        public IEnumerable<TariffFormulaModel> SelectFormulas()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),               
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TariffFormula.Select]", parameters))
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

        public TariffTypeModel SelectTariffTypeByID(short? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", ID),
            };
            TariffTypeModel model = new TariffTypeModel();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TariffType.Select]", parameters))
            {
                model = GetTariffTypeModel(dr);
            }
            return model;
        }

        public IEnumerable<TariffTypeModel> SelectTariffTypes(string Area = "Operation")
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", 0),  
               new SqlParameter("@Area",Area)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TariffType.Select]", parameters))
            {
                yield return GetTariffTypeModel(dr);
            }
        }

        public TariffModel Update(TariffModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@OrderID", model.OrderID),
               new SqlParameter("@RouteID", model.RouteID),
               new SqlParameter("@TariffFormulaID", model.TariffFormulaID),
               new SqlParameter("@TariffID", model.TariffID),              
               new SqlParameter("@TariffRate", model.TariffRate),               
               new SqlParameter("@TariffTypeID", model.TariffTypeID),
               new SqlParameter("@TruckAxles", model.TruckAxles),
               new SqlParameter("@UsageID", model.UsageId),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tariff.Update]", parameters))
            {
                model =  GetModel(dr);
            }
            return model;
        }

        public TariffTypeModel UpdateTariffType(TariffTypeModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@AllowMultiple", model.AllowMultiple),
               new SqlParameter("Area",model.Area),
               new SqlParameter("@UsageCode", model.UsageCode),
               new SqlParameter("@TariffSign", model.TariffSign),               
               new SqlParameter("@TariffGroup", model.TariffGroup),
               new SqlParameter("@TariffTypeID", model.TariffTypeID),
               new SqlParameter("@TariffTypeName", model.TariffTypeName),
               new SqlParameter("@Unit", model.Unit),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.TariffType.Update]", parameters))
            {
                model = GetTariffTypeModel(dr);
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
                TariffRate = dr.GetDecimal("TariffRate"),
                TariffTypeID = dr.GetInt16("TariffTypeID"), 
                UsageId= dr.GetString("UsageID"),
                TariffSign = dr.GetInt32("TariffSign"),
                TruckAxles = dr.GetByte("TruckAxles"),
                Formula = dr.GetString("Formula"),
                OrderName = dr.GetString("OrderName"),
                RouteName = dr.GetString("RouteName"),
                TariffTypeName = dr.GetString("TariffTypeName"),
                Unit = dr.GetString("Unit"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        private TariffTypeModel GetTariffTypeModel(IDataRecord dr)
        {
            return new TariffTypeModel
            {
                TariffTypeID = dr.GetInt16("TariffTypeID"),
                TariffTypeName = dr.GetString("TariffTypeName"),
                TariffGroup = dr.GetString("TariffGroup"),
                Area = dr.GetString("Area"),
                Unit = dr.GetString("Unit"),
                AllowMultiple = dr.GetBoolean("AllowMultiple"),
                TariffSign = dr.GetInt32("TariffSign"),                
                UsageCode = dr.GetString("UsageID"),
                UserInfo = new ArmsModels.SharedModels.UserInfoModel
                {
                    RecordStatus = dr.GetByte("RecordStatus"),
                    TimeStampField = dr.GetDateTime("TimeStamp"),
                    UserID = dr.GetString("UserID"),
                },
            };
        }

        public IEnumerable<TariffModel> GetTariffs(string TariffGroup, int? OrderID, int? RouteID, int? Axles)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@TariffGroup", TariffGroup),
               new SqlParameter("@OrderID", OrderID),
               new SqlParameter("@RouteID", RouteID),
               new SqlParameter("@Axles", Axles),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Operation.Tariff.GetTariffs]", parameters))
            {
                yield return GetModel(dr);
            }
        }

        public decimal? GetTariffAmount(GcSetModel GcSet, TariffModel Tariff)
        {
            RouteModel route = Task.Run(() => Iroute.SelectByID(GcSet.RouteID)).Result;
            decimal? tariffAmount = null;
            if (Tariff?.TariffFormulaID == 1)
            {
                decimal? distance = route.Distance;
                tariffAmount = distance / Tariff.TariffRate;
            }
            else
            if (Tariff?.TariffFormulaID == 2)
            {
                decimal? distance = route.Distance;
                tariffAmount =distance* Tariff.TariffRate;
            }
            else
            if (Tariff?.TariffFormulaID == 3)
            {
                decimal? Qty = Convert.ToDecimal(GcSet.Gcs.Sum(x=> x.BillQuantity));
                tariffAmount = Qty * Tariff.TariffRate;
            }
            else
            if (Tariff?.TariffFormulaID == 4)
            {                
                tariffAmount = Tariff.TariffRate;
            }
            return tariffAmount;
        }


        public IEnumerable<TariffModel> GeneratePendingTariffs(long? RefID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@RefID", RefID),
            };           

            foreach (var dr in Iservice.GetDataReader("[usp.Operation.Transaction.Generate]", parameters))
            {
                yield return GetModel(dr);
            }
        }
    }   
}
