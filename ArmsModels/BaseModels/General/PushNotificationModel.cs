using System;

namespace ArmsModels.BaseModels.General
{
    public class PushNotificationModel
    {
        public int MSgId { get; set; } = 0;
        public int? MessageID { get; set; }
        public BranchModel InitiateBranch { get; set; }
        public BranchModel ReceivedBranch { get; set; }
        public string MessageTitle { get; set; }
        public string MessageBody { get; set; }
        public bool? Aknowledged { get; set; }
        public string AknowledgedBy { get; set; }//UserID
        public int? RedirectedTo { get; set; }
        public string MessageGroupID { get; set; }
        public string PageToRedirectLink { get; set; }
        public string ClaimValue { get; set; }
        public int? DocumentTypeID { get; set; }
        public int? DocumentID { get; set; }
        public int? ExpiredBy { get; set; }
        public byte? RecordStatus { get; set; }
        public DateTime? TimeStamp { get; set; }
        public DateTime? MsgDate { get; set; } = DateTime.Now;
        private DateTime CurrentTime { get; set; } = DateTime.Now;
        public string MsgDatest => this.MsgDate.Value.ToString("dd-MMM-yyyy");
        public string MsgDateString { get; set; }
        public string MessageTimeSpan
        {
            get
            {     
                if (MsgDate != null)
                {
                    TimeSpan span = (CurrentTime - MsgDate.Value);
                    if (span.TotalDays/30 > 1)
                    {
                        return (span.TotalDays / 30).ToString() + " days ";
                    }

                    if (span.Days >0)
                    {
                        return span.Days.ToString()+ " days ";
                    }
                    else if(span.Hours>0)
                    {
                        return (span.Hours).ToString() + " hrs ";
                    }
                    else if(span.Minutes > 0)
                    {
                        return  (span.Minutes).ToString() + " Min.";
                    }
                    else
                    {
                        return "Now";
                    }
                }
                return string.Empty;
            }
        }
        public string ShowOrHideBody { get; set; } = "none";
    }
    public class PushNotificationGroupModel
    {
        public int? ID { get; set; }   
        public string MessageGroupID { get; set; }   
        public string MessageGroupName { get; set; }   
        public string MessageGroupIcon { get; set; }   
    }
    //public class NotificationMessage
    //{
    //    public int MSgId { get; set; } = 0;
    //    public int MessageID { get; set; } = 0;
    //    public string InitiateBranchName { get; set; }
    //    public int InitiateBranchID { get; set; }
    //    public BranchModel InitiateBranch { get; set; }
    //    public string ReceivedBranchName { get; set; }
    //    public int ReceivedBranchID { get; set; }
    //    public BranchModel ReceivedBranch { get; set; }
    //    public string MessageTitle { get; set; }
    //    public string MessageBody { get; set; }
    //    public bool Aknowledged { get; set; }
    //    public string AknowledgedBy { get; set; }
    //    public int RedirectedTo { get; set; }
    //    public int DocumentID { get; set; }
    //    public int ExpiredBy { get; set; }
    //    public DateTime MsgDate { get; set; } = DateTime.Now;
    //    public string MsgDatest => this.MsgDate.ToString("dd-MMM-yyyy");
    //    public string MsgDateString { get; set; }
    //}
}
