using System.ComponentModel;

namespace SMARTII.Domain.Notification
{
    public enum NotificationUIType
   {
        
        [Description("Email")]
        OnEmal = 0,

        [Description("電話")]
        OnCall = 1,
        
        [Description("Line")]
        Line = 2,
    }
}
