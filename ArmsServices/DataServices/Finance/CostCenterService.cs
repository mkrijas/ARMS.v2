using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public class CostCenterService : ICostCenterService
    {
        IDbService Iservice;

        public CostCenterService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public int Delete(int? ID, string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CostCenterID", ID),
               new SqlParameter("@UserID", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.Finance.CostCenter.Delete]", parameters);
        }
       

        public IEnumerable<CostCenterModel> Select()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByID"),                           
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CostCenter.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public IEnumerable<CostCenterModel> SelectByCategory(int? CategoryID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@Operation", "ByCategory"),
               new SqlParameter("@CategoryID",CategoryID),
            };

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CostCenter.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }


        public CostCenterModel SelectByID(int? ID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CostCenterID", ID),
               new SqlParameter("@Operation", "ByID")
            };
            CostCenterModel model = new();
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CostCenter.Select]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }

        public IEnumerable<CategoryModel> SelectCategory()
        {  
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CostCenter.Category.Select]", null))
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

        public CostCenterModel Update(CostCenterModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@CostCenterID", model.CostCenterID),              
               new SqlParameter("@CostCenter", model.CostCenter),
               new SqlParameter("@CategoryID", model.Category.CategoryID),               
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CostCenter.Update]", parameters))
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
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Finance.CostCenter.Category.Update]", parameters))
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

        private CostCenterModel GetModel(IDataRecord dr)
        {
            return new CostCenterModel
            {  
                CostCenterID = dr.GetInt32("CostCenterID"),
                CostCenter = dr.GetString("CostCenter"),
                Category = new CategoryModel() 
                { 
                    CategoryID = dr.GetInt32("categoryID"),
                    CategoryName  =  dr.GetString("CategoryName") 
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