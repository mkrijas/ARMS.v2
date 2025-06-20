using ArmsModels.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace ArmsServices.DataServices.General
{
    public class MobileNotificationService : IMobileNotificationService
    {
        IDbService Iservice;

        public MobileNotificationService(IDbService iservice)
        {
            Iservice = iservice;
        }

        public MobileNotificationModel UpdateMobileNotification(MobileNotificationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@NotificationID", null),
               new SqlParameter("@UserID", model.UserID),
               new SqlParameter("@Token", model.Token),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Mobile.Notification.Update]", parameters))
            {
                model = new MobileNotificationModel()
                {
                    NotificationID = dr.GetInt32("NotificationID"),
                    UserID = dr.GetString("UserID"),
                    Token = dr.GetString("Token")
                };
            }
            return model;
        }

        public IEnumerable<MobileNotificationModel> SelectAllToken()
        {
            //List<SqlParameter> parameters = new List<SqlParameter>
            //{
            //    new SqlParameter("@UserID", 0) 
            //};

            var models = new List<MobileNotificationModel>();

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Mobile.Notification.Select]", null))
            {
                var model = new MobileNotificationModel()
                {
                    Token = dr.GetString("Token")
                };

                models.Add(model);
            }

            return models;
        }

    }
}
