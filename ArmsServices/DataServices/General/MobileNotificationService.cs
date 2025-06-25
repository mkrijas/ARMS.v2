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

        public MobileNotificationModel UpdateMobileNotificationMessage(MobileNotificationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", null),
               new SqlParameter("@UserID", model.UserID),
               new SqlParameter("@Title", model.Title),
               new SqlParameter("@Body", model.Body),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Mobile.Notification.Message.Update]", parameters))
            {
                model = new MobileNotificationModel()
                {
                    NotificationID = dr.GetInt32("ID"),
                    UserID = dr.GetString("UserID"),
                    Title = dr.GetString("Title"),
                    Body = dr.GetString("Body"),
                    IsRead = dr.GetBoolean("IsRead"),
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
                    Token = dr.GetString("Token"),
                    UserID = dr.GetString("UserID"),
                };

                models.Add(model);
            }

            return models;
        }

        public IEnumerable<MobileNotificationModel> SelectAllMessage(string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@UserID", UserID)
            };

            var models = new List<MobileNotificationModel>();

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.Mobile.Notification.Message.Select]", null))
            {
                var model = new MobileNotificationModel()
                {
                    NotificationID = dr.GetInt32("ID"),
                    UserID = dr.GetString("UserID"),
                    Title = dr.GetString("Title"),
                    Body = dr.GetString("Body"),
                    IsRead = dr.GetBoolean("IsRead"),
                };

                models.Add(model);
            }

            return models;
        }

    }
}
