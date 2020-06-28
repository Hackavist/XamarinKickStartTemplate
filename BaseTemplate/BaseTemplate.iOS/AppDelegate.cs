using System;
using System.Diagnostics;
using System.Threading.Tasks;

using BaseTemplate.Constants;

using Foundation;

using UIKit;

namespace BaseTemplate.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            DisplayCrashReport();

            return base.FinishedLaunching(app, options);
        }

        #region Error Handling
        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
                string errorFilePath = System.IO.Path.Combine(libraryPath, AppConstants.ErrorFileName);
                string errorMessage = $"Time: {DateTime.Now}\r\nError: Unhandled Exception\r\n{exception}";

                System.IO.File.WriteAllText(errorFilePath, errorMessage);

            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        // If there is an unhandled exception, the exception information is displayed 
        // on screen the next time the app is started (only in debug configuration)

        [Conditional("DEBUG")]
        private void DisplayCrashReport()
        {
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            var errorFilePath = System.IO.Path.Combine(libraryPath, AppConstants.ErrorFileName);


            if (!System.IO.File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = System.IO.File.ReadAllText(errorFilePath);
            if (string.IsNullOrEmpty(errorText))
            {
                return;
            }

            //var alertView = new UIAlertView("Crash Report", errorText, null, "Close", "Clear") { UserInteractionEnabled = true };
            //alertView.Clicked += (sender, args) =>
            //{
            //    if (args.ButtonIndex != 0)
            //    {
            //        System.IO.File.Delete(errorFilePath);
            //    }
            //};
            //alertView.Show();

            UIAlertController okAlertController = UIAlertController.Create("Crash Report", errorText, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("Dismiss", UIAlertActionStyle.Default, action => System.IO.File.Delete(errorFilePath)));
            Window.MakeKeyAndVisible();
            Window.RootViewController.PresentViewController(okAlertController, true, null);

        }
        #endregion
    }
}
