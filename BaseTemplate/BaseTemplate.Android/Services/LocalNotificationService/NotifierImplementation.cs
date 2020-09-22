using System;
using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using BaseTemplate.Droid.Services.LocalNotificationService;
using BaseTemplate.Services.LocalNotificationService;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;

[assembly: Dependency(typeof(NotifierImplementation))]

namespace BaseTemplate.Droid.Services.LocalNotificationService
{
    public class NotifierImplementation : ILocalNotificationService
    {
        private static NotificationManager NotificationManager =>
            (NotificationManager)AndroidApp.Context.GetSystemService(Context.NotificationService);

        private readonly string _channelId = $"{AndroidApp.Context.PackageName}.general";
        private bool _channelInitialized;

        public NotifierImplementation()
        {
            Initialize();
        }

        public event EventHandler NotificationReceived;

        public void Cancel(int id)
        {
            Intent intent = CreateIntent(id);
            PendingIntent pendingIntent =
                PendingIntent.GetBroadcast(AndroidApp.Context, 0, intent, PendingIntentFlags.CancelCurrent);

            AlarmManager alarmManager = GetAlarmManager();
            alarmManager.Cancel(pendingIntent);

            NotificationManagerCompat notificationManager = NotificationManagerCompat.From(AndroidApp.Context);
            notificationManager.Cancel(id);
        }

        public void Initialize()
        {
            if (!_channelInitialized) CreateNotificationChannel();
        }

        public void Notify(string title, string body, int id = 0)
        {
            if (!_channelInitialized) Initialize();

            Intent resultIntent = GetLauncherActivity();
            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(AndroidApp.Context);
            stackBuilder.AddNextIntent(resultIntent);
            PendingIntent resultPendingIntent =
                stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, _channelId)
                .SetContentIntent(resultPendingIntent)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources,
                    Resource.Drawable.notification_icon))
                .SetSmallIcon(Resource.Drawable.notification_icon)
                .SetDefaults((int)NotificationDefaults.All);

            Notification notification = builder.Build();
            NotificationManager.Notify(id, notification);
        }

        public void Notify(string title, string body, DateTime notificationDateTime, int id = 0)
        {
            Intent intent = CreateIntent(id);

            LocalNotification localNotification = new LocalNotification
            {
                Title = title,
                Body = body,
                Id = id,
                NotifyTime = notificationDateTime,
                IconId = Resource.Drawable.notification_icon
            };

            string serializedNotification = SerializeNotification(localNotification);
            intent.PutExtra(ScheduledAlarmHandler.LocalNotificationKey, serializedNotification);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, 0, intent, PendingIntentFlags.CancelCurrent);
            long triggerTime = NotifyTimeInMilliseconds(notificationDateTime);
            AlarmManager alarmManager = GetAlarmManager();

            alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
        }

        public void ReceiveNotification(string title, string message)
        {
            NotificationEventArgs args = new NotificationEventArgs
            {
                Title = title,
                Message = message
            };
            NotificationReceived?.Invoke(null, args);
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                const string channelName = "DefaultChannel";
                NotificationChannel channel =
                    new NotificationChannel(_channelId, channelName, NotificationImportance.Default)
                    {
                        Description = "The default channel for notifications."
                    };
                NotificationManager.CreateNotificationChannel(channel);
            }

            _channelInitialized = true;
        }

        private static Intent GetLauncherActivity()
        {
            return AndroidApp.Context.PackageManager.GetLaunchIntentForPackage(AndroidApp.Context.PackageName);
        }

        private static Intent CreateIntent(int id)
        {
            return new Intent(AndroidApp.Context, typeof(ScheduledAlarmHandler)).SetAction("LocalNotifierIntent" + id);
        }

        private static AlarmManager GetAlarmManager()
        {
            return AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
        }

        private static string SerializeNotification(LocalNotification notification)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(notification.GetType());
            using StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, notification);
            return stringWriter.ToString();
        }

        private static long NotifyTimeInMilliseconds(DateTime notifyTime)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            double epochDifference = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;

            return utcTime.AddSeconds(-epochDifference).Ticks / 10000;
        }
    }
}