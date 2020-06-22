using System;

namespace BaseTemplate.Services.LocalNotificationService
{
    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}