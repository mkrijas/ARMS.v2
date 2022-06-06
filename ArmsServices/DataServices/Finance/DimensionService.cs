using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IDimensionService
    {
        DimensionModel Update(DimensionModel model);
        DimensionModel SelectByID(int? ID);
        int Delete(int? ID, string UserID);
        IEnumerable<DimensionModel> SelectByCategory(int? CategoryID);
        IEnumerable<DimensionModel> Select();
        IEnumerable<CategoryModel> SelectCategory();
        CategoryModel UpdateCategory(CategoryModel model);
    }

    public class DimensionService : IDimensionService
    {
        IDbService Iservice;

        public DimensionService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DimensionID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.Dimension.Delete]", parameters);
        }


        public IEnumerable<DimensionModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Dimension.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public IEnumerable<DimensionModel> SelectByCategory(int? CategoryID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByCategory"),
               new SqlParameter("@CategoryID",CategoryID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Dimension.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }


        public DimensionModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DimensionID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            DimensionModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Dimension.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<CategoryModel> SelectCategory()
        {
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Dimension.Category.Select]", null))
            {
                yield return new CategoryModel()
                {
                    CategoryID = dr.GetInt32("CategoryID"),
                    CategoryName = dr.GetString("CategoryName"),
                    UserInfo = new ArmsModels.SharedModels.UserInfoModel
                    {
                        RecordStatus = dr.GetByte("RecordStatus"),
                        TimeStampField = dr.GetDateTime("TimeStamp"),
                        UserID = dr.GetString("UserID"),
                    },
                };
            }
        }

        public DimensionModel Update(DimensionModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DimensionID", model.DimensionID),
               new SqlParameter("@Dimension", model.Dimension),
               new SqlParameter("@CategoryID", model.Category.CategoryID),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Dimension.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public CategoryModel UpdateCategory(CategoryModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CategoryID", model.CategoryID),
               new SqlParameter("@CategoryName", model.CategoryName),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.Dimension.Category.Update]", parameters))
            {
                model = new CategoryModel()
                {
                    CategoryID = dr.GetInt32("CategoryID"),
                    CategoryName = dr.GetString("CategoryName"),
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

        private DimensionModel GetModel(IDataRecord dr)
        {
            return new DimensionModel
            {
                DimensionID = dr.GetInt32("DimensionID"),
                Dimension = dr.GetString("Dimension"),
                Category = new CategoryModel()
                {
                    CategoryID = dr.GetInt32("categoryID"),
                    CategoryName = dr.GetString("CategoryName")
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