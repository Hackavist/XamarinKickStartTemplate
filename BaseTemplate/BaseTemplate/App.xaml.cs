using System;
using BaseTemplate.Services.FileSystemService;
using BaseTemplate.Services.LocalDatabaseService;
using BaseTemplate.ViewModels;
using TemplateFoundation.IOCFoundation;
using TemplateFoundation.Navigation.NavigationContainers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        private void CreateDataBaseTables()
        {

        }

        private void SetDefaultLanguage()
        {
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