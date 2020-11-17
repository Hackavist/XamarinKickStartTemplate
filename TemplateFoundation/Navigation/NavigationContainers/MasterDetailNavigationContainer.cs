using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TemplateFoundation.ExtensionMethods;
using TemplateFoundation.IOCFoundation;
using TemplateFoundation.Navigation.Implementations;
using TemplateFoundation.Navigation.Interfaces;
using TemplateFoundation.ViewModelFoundation;
using Xamarin.Forms;

namespace TemplateFoundation.Navigation.NavigationContainers
{
    public class MasterDetailNavigationContainer : MasterDetailPage, INavigationService
    {
        public string NavigationServiceName { get; }
        public Dictionary<string, Page> Pages { get; } = new Dictionary<string, Page>();
        protected ObservableCollection<string> PageNames { get; } = new ObservableCollection<string>();
        private readonly CollectionView collectionView = new CollectionView();
        private readonly List<Page> innerPages = new List<Page>();
        public ContentPage MenuPage;

        public MasterDetailNavigationContainer() : this(NavigationConstants.DefaultNavigationServiceName)
        {
        }

        public MasterDetailNavigationContainer(string navigationServiceName)
        {
            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
        }

        public void Init(string menuTitle, string menuIcon = null)
        {
            CreateMenuPage(menuTitle, menuIcon);
            RegisterNavigation();
        }

        public void Init<T>(string masterListName) where T : BaseViewModel
        {
            CreateMenuPage<T>(masterListName);
            RegisterNavigation();
        }

        protected virtual void RegisterNavigation()
        {
            Ioc.Container.Register<INavigationService>(this, NavigationServiceName);
        }

        internal Page CreateContainerPageSafe(Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;

            return CreateContainerPage(page);
        }

        protected virtual Page CreateContainerPage(Page page)
        {
            return new NavigationPage(page);
        }

        protected virtual void CreateMenuPage(string menuPageTitle, string menuIcon = null)
        {
            MenuPage = new ContentPage { Title = menuPageTitle };
            collectionView.SelectionMode = SelectionMode.Single;
            collectionView.ItemsSource = PageNames;
            collectionView.SelectionChanged += (sender, args) =>
            {
                if (Pages.ContainsKey((string)args.CurrentSelection[0])) Detail = Pages[(string)args.CurrentSelection[0]];
                IsPresented = false;
            };
            collectionView.ItemTemplate = new DataTemplate(() =>
            {
                Label nameLabel = new Label { Margin = new Thickness(10), VerticalTextAlignment = TextAlignment.Center, BackgroundColor = Color.Red };
                nameLabel.SetBinding(Label.TextProperty, ".");
                return nameLabel;
            });
            MenuPage.Content = collectionView;

            var navPage = new NavigationPage(MenuPage) { Title = menuPageTitle };

            if (!string.IsNullOrEmpty(menuIcon))
                navPage.IconImageSource = menuIcon;

            Master = navPage;
        }

        public void CreateMenuPage<T>(string masterListName) where T : BaseViewModel
        {
            var masterpage = ViewModelResolver.ResolveViewModel<T>();
            var pagelist = masterpage.FindByName(masterListName);
            if (pagelist is CollectionView list)
            {
                list.SelectionChanged += (sender, args) =>
                {
                    if (Pages.ContainsKey(((MenuItems)args.CurrentSelection[0]).Title))
                        Detail = Pages[((MenuItems)args.CurrentSelection[0]).Title];
                    IsPresented = false;
                };
                list.ItemTemplate = new DataTemplate(() =>
                {
                    Label nameLabel = new Label { Margin = new Thickness(10), VerticalOptions = LayoutOptions.CenterAndExpand, VerticalTextAlignment = TextAlignment.Center, BackgroundColor = Color.Red };
                    nameLabel.SetBinding(Label.TextProperty, ".");
                    return nameLabel;
                });
            }
            else throw new Exception("Master list navigation name not the same as xaml");
            Master = masterpage;
        }

        public void CreateMenuPage<T>() where T : BaseViewModel
        {
            var masterpage = ViewModelResolver.ResolveViewModel<T>();
            Master = masterpage;
        }

        public Task PushPage(Page page, BaseViewModel model, bool modal = false, bool animate = true)
        {
            return modal ? Navigation.PushModalAsync(CreateContainerPageSafe(page), animate) : (Detail as NavigationPage)?.PushAsync(page, animate);
        }

        public Task PopPage(bool modal = false, bool animate = true)
        {
            return modal ? Navigation.PopModalAsync(animate) : (Detail as NavigationPage)?.PopAsync(animate);
        }

        public Task PopToRoot(bool animate = true)
        {
            return (Detail as NavigationPage)?.PopToRootAsync(animate);
        }

        public void NotifyChildrenPageWasPopped()
        {
            (Master as NavigationPage)?.NotifyAllChildrenPopped();
            (Master as INavigationService)?.NotifyChildrenPageWasPopped();

            foreach (var page in Pages.Values)
            {
                (page as NavigationPage)?.NotifyAllChildrenPopped();
                (page as INavigationService)?.NotifyChildrenPageWasPopped();
            }

            if (Pages != null && !Pages.ContainsValue(Detail))
                (Detail as NavigationPage)?.NotifyAllChildrenPopped();
            if (Pages != null && !Pages.ContainsValue(Detail))
                (Detail as INavigationService)?.NotifyChildrenPageWasPopped();
        }

        public Task<BaseViewModel> SwitchSelectedRootPageModel<T>() where T : BaseViewModel
        {
            var tabIndex = innerPages.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            collectionView.SelectedItem = PageNames[tabIndex];

            return Task.FromResult((Detail as NavigationPage)?.CurrentPage.GetModel());
        }

        private void AddPagesToDictionary(Page page)
        {
            var viewModel = page.GetModel();
            viewModel.CurrentNavigationServiceName = NavigationServiceName;
            innerPages.Add(page);
            Page navigationContainer = CreateContainerPage(page);
            if (string.IsNullOrEmpty(viewModel.Title)) throw new Exception("no Title found for " + viewModel.GetType().Name);
            Pages.Add(viewModel.Title, navigationContainer);
            PageNames.Add(viewModel.Title);
            if (Pages.Count == 1)
                Detail = navigationContainer;
        }

        public virtual void AddPage<T>(object data = null) where T : BaseViewModel
        {
            var page = ViewModelResolver.ResolveViewModel<T>(data);
            AddPagesToDictionary(page);
        }

        public virtual void AddPage(string modelName, object data = null)
        {
            var viewModelType = Type.GetType(modelName);
            Page page = ViewModelResolver.ResolveViewModel(viewModelType, data);
            AddPagesToDictionary(page);
        }
    }

    public class MenuItems : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public bool IsSelected { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}