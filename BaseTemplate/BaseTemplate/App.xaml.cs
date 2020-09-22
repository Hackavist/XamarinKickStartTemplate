using System.Globalization;
using System.Threading;
using BaseTemplate.Resources;
using BaseTemplate.Services.FileSystemService;
using BaseTemplate.Services.LocalDatabaseService;
using BaseTemplate.ViewModels;
using TemplateFoundation.IOCFoundation;
using TemplateFoundation.Navigation.NavigationContainers;
using Xamarin.Forms;

namespace BaseTemplate
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetupDependencyInjection();
            CreateDataBaseTables();
            SetDefaultLanguage();
            SetStartPage();
        }

        private void SetupDependencyInjection()
        {
            Ioc.Container.Register<ILocalDatabaseService, LocalDatabaseService>().AsSingleton();
            Ioc.Container.Register<IFileSystemService, FileSystemService>().AsSingleton();
        }
        /// <summary>
        /// Create your database tables that you need
        /// </summary>
        private void CreateDataBaseTables()
        {
            // Ioc.Container.Resolve<LocalDatabaseService>().CreateDatabaseTables(Send List of tabels);
        }
        /// <summary>
        /// Set your default language for the entire app
        /// Just change culture info ar,en,fr,es
        /// </summary>
        private void SetDefaultLanguage()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            AppResources.Culture = new CultureInfo("en");
        }

        private void SetStartPage()
        {
            var masterDetailNav = new MasterDetailNavigationContainer();
            masterDetailNav.Init("Menu");
            masterDetailNav.AddPage<MainViewModel>("Home");
            MainPage = masterDetailNav;
        }
    }
}