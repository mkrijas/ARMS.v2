using ArmsModels.BaseModels.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace ArmsServices.DataServices.General
{
    public class PushNotificationService : IPushNotificationService
    {
        IDbService Iservice;


        public PushNotificationService(IDbService iservice,SqlTableDependencyService _tabledep)
        {
            Iservice = iservice;
            _tabledep.SubscribeTableDependency();
        }

        public string GetMessageTitle(string DocType, string DocNumber,string Varification)
        {

            return "The DocNo :- "+ DocNumber + " of "+ DocType + " requires " + Varification + ".";
        }
        public string GetMessageBody(string DocType, string DocNumber, string Varification, DateTime? DocDate)
        {

            return "The DocNo :- " + DocNumber + " of " + DocType + " requires " + Varification + ". which was requested on " + DocDate;
        }


        public PushNotificationModel UpdatePushNotification(PushNotificationModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MessageID", null),
               new SqlParameter("@InitiateBranchID", model?.InitiateBranch?.BranchID?? 0),
               new SqlParameter("@ReceivedBranchID", model?.ReceivedBranch?.BranchID?? 0),
               new SqlParameter("@MessageTitle", model.MessageTitle),
               new SqlParameter("@MessageBody", model.MessageBody),
               new SqlParameter("@Aknowledged", null),
               new SqlParameter("@AknowledgedBy", null),
               new SqlParameter("@MessageGroupID", model.MessageGroupID),
               new SqlParameter("@DocumentTypeID", model.DocumentTypeID),
               new SqlParameter("@RedirectedTo", model.RedirectedTo),
               new SqlParameter("@PageToRedirectLink", model.PageToRedirectLink),
               new SqlParameter("@ClaimValue", model.ClaimValue),
               new SqlParameter("@DocumentID", model.DocumentID),
               new SqlParameter("@ExpiredBy", model.ExpiredBy),
               new SqlParameter("@RecordStatus", model.RecordStatus)
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.General.Notification.Update]", parameters))
            {
                model = GetModel(dr);
            }
            return model;
        }        
        public IEnumerable<PushNotificationModel> SelectNotificationsBasedOnBranchDocumentIdDocumentTypeId(int? ID,int? DocumentID,int? DocumentTypeID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ID", ID),
               new SqlParameter("@DocumentID", DocumentID),
               new SqlParameter("@DocumentTypeID", DocumentTypeID),
               new SqlParameter("@Operation", "ByBranchDocIdAndDocTypeId"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.General.Notification.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public int AknowledgedSelectedItems(string MessageIDs, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MessageIDs", MessageIDs),
               new SqlParameter("@Aknowledged", 1),
               new SqlParameter("@AknowledgedBy", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.General.Notification.Aknowledge]", parameters);

        }
        public int AknowledgedCurrentItem(int? MessageID, string UserID)
        {

            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@MessageIDs", MessageID.Value.ToString()),
               new SqlParameter("@Aknowledged", 1),
               new SqlParameter("@AknowledgedBy", UserID),
            };
            return Iservice.ExecuteNonQuery("[usp.General.Notification.Aknowledge]", parameters);

        }
        public IEnumerable<PushNotificationGroupModel> GetAllGroupList()
        {

            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.General.NotificationGroups.Select]", null))
            {
                yield return new PushNotificationGroupModel()
                {
                    ID = dr.GetInt32("ID"),
                    MessageGroupID = dr.GetString("MessageGroupID"),
                    MessageGroupName = dr.GetString("MessageGroupName"),
                    MessageGroupIcon = dr.GetString("MessageGroupIcon")
                };
            }

        }
        private PushNotificationModel GetModel(IDataRecord dr)
        {
            return new PushNotificationModel
            {
                MessageID = dr.GetInt32("MessageID"),
                InitiateBranch = new ArmsModels.BaseModels.BranchModel()
                {
                    BranchID = dr.GetInt32("InitiateBranchID"),
                    BranchName = dr.GetString("InitiateBranchName"),
                },
                ReceivedBranch = new ArmsModels.BaseModels.BranchModel()
                {
                    BranchID = dr.GetInt32("ReceivedBranchID"),
                    BranchName = dr.GetString("ReceivedBranchName"),
                },
                MessageTitle = dr.GetString("MessageTitle"),
                MessageBody = dr.GetString("MessageBody"),
                Aknowledged = dr.GetBoolean("Aknowledged"),
                AknowledgedBy = dr.GetString("AknowledgedBy"),
                RedirectedTo = dr.GetInt32("RedirectedTo"),
                MessageGroupID = dr.GetString("MessageGroupID"),
                DocumentTypeID = dr.GetInt32("DocumentTypeID"),
                PageToRedirectLink = dr.GetString("PageToRedirectLink"),
                ClaimValue = dr.GetString("ClaimValue"),
                DocumentID = dr.GetInt32("DocumentID"),
                ExpiredBy = dr.GetInt32("ExpiredBy"),
                RecordStatus = dr.GetByte("RecordStatus"),
                MsgDate = dr.GetDateTime("Timestamp")
            };
        }
        public IEnumerable<PushNotificationModel> SelectActiveNotifications()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@Operation", "SelectActive"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.General.Notification.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public IEnumerable<PushNotificationModel> SelectActiveNotifications(int? BranchID, string UserID = null)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@ID", BranchID),
               new SqlParameter("@UserID", UserID),               
               new SqlParameter("@Operation", "SelectActive"),
            };
            foreach (IDataRecord dr in Iservice.GetDataReader("[usp.General.Notification.Select]", parameters))
            {
                yield return GetModel(dr);
            }
        }
        public int CreateAuthNotifications(int BranchID, int DocumentTypeID, int DocumentID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@BranchID", BranchID),
               new SqlParameter("@DocumentID", DocumentID),
               new SqlParameter("@DocumentTypeID", DocumentTypeID),
            };
            return Iservice.ExecuteNonQuery("[usp.General.Notification.CreateAuthNotification]", parameters);            
        }

        public int CancelAuthNotifications(int DocumentTypeID, int DocumentID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {               
               new SqlParameter("@DocumentID", DocumentID),
               new SqlParameter("@DocumentTypeID", DocumentTypeID),
            };
            return Iservice.ExecuteNonQuery("[usp.General.Notification.CancelAuthNotification]", parameters);
        }

        public int AcknowledgeAuthNotification(int AuthlevelID,int DocumentTypeID, int DocumentID,string UserID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
               new SqlParameter("@DocumentID", DocumentID),
               new SqlParameter("@DocumentTypeID", DocumentTypeID),
               new SqlParameter("@AuthlevelID", AuthlevelID),
               new SqlParameter("UserID",UserID)
            };
            return Iservice.ExecuteNonQuery("[usp.General.Notification.AcknowledgeAuthNotification]", parameters);
        }
    }
}
