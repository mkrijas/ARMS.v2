using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ArmsModels.BaseModels;


namespace ArmsServices.DataServices
{
    public interface IStateService
    {       
        StateModel Update(StateModel model);
        int Delete(int StateID, string UserID);
        IEnumerable<StateModel> Select(int? StateID);
    }

    public class StateService : IStateService
    {
        IDbService Iservice;

        public StateService(IDbService iservice)
        {
            Iservice = iservice;
        }
        public StateModel Update(StateModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StateID", model.StateID),
               new SqlParameter("@StateName", model.StateName),
               new SqlParameter("@UserID", model.UserInfo.UserID),
            };

            StateModel rmodel = new StateModel();
            using (var reader = Iservice.GetDataReader("[usp.Place.StatesUpdate]", parameters))
            {
                while (reader.Read())
                {
                    rmodel = new StateModel
                    {
                        StateID = reader.GetInt16("StateID"),
                        StateName = reader.GetString("StateName"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
            return rmodel;
        }
        public int Delete(int StateID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StateID", StateID),               
               new SqlParameter("@UserID", UserID),
            };            
            return Iservice.ExecuteNonQuery("[usp.Place.StatesDelete]", parameters);
        }
        public IEnumerable<StateModel> Select(int? StateID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@StateID", StateID)               
            };

            using (var reader = Iservice.GetDataReader("[usp.Place.StatesSelect]", parameters))
            {
                while (reader.Read())
                {
                    yield return new StateModel
                    {
                        StateID = reader.GetInt16("StateID"),
                        StateName = reader.GetString("StateName"),
                        UserInfo = new ArmsModels.SharedModels.UserInfoModel
                        {
                            RecordStatus = reader.GetByte("RecordStatus"),
                            TimeStampField = reader.GetDateTime("TimeStampField"),
                            UserID = reader.GetString("UserID"),
                        },
                    };
                }
            }
        }

    }
}
