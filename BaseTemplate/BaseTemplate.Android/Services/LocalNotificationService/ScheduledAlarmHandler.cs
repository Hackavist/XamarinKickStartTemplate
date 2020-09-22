using System.IO;
using System.Xml.Serialization;
using Android.Content;
using BaseTemplate.Services.LocalNotificationService;
using Xamarin.Forms;

namespace BaseTemplate.Droid.Services.LocalNotificationService
{
    /// <summary>
    ///     Broadcast receiver
    /// </summary>
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Plugin Broadcast Receiver")]
    public class ScheduledAlarmHandler : BroadcastReceiver
    {
        /// <summary>
        /// </summary>
        public const string LocalNotificationKey = "LocalNotification";

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context context, Intent intent)
        {
            string extra = intent.GetStringExtra(LocalNotificationKey);
            LocalNotification notification = DeserializeNotification(extra);
            DependencyService.Get<ILocalNotificationService>().Notify(notification.Title, notification.Body, 12);
        }

        private static LocalNotification DeserializeNotification(string notificationString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LocalNotification));
            using StringReader stringReader = new StringReader(notificationString);
            return (LocalNotification)xmlSerializer.Deserialize(stringReader);
        }
    }
}