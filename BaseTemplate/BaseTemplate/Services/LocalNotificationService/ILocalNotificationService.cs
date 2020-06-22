using System;

namespace BaseTemplate.Services.LocalNotificationService
{
    public interface ILocalNotificationService
    {
        /// <summary>
        ///     The Executed Behaviour when the app receives a notification
        /// </summary>
        event EventHandler NotificationReceived;

        /// <summary>
        ///     Executes the platform Specific service initiation code
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Show a local notification now
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="id">Id of the notification</param>
        void Notify(string title, string body, int id = 0);

        /// <summary>
        ///     Show a local notification at a certain date and time
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="notificationDateTime"> The date and time the notification should be fired </param>
        /// <param name="id">Id of the notification</param>
        void Notify(string title, string body, DateTime notificationDateTime, int id = 0);

        /// <summary>
        ///     Cancel a local notification
        /// </summary>
        /// <param name="id">Id of the notification to cancel</param>
        void Cancel(int id);

        /// <summary>
        ///     Channels the parameters to the "NotificationReceived" event when a notification is fired
        /// </summary>
        void ReceiveNotification(string title, string message);
    }
}