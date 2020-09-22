using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TemplateFoundation.ExtensionMethods;
using TemplateFoundation.IOCFoundation;
using TemplateFoundation.Navigation.Implementations;
using TemplateFoundation.Navigation.Interfaces;
using TemplateFoundation.ViewModelFoundation;

using Xamarin.Forms;

namespace TemplateFoundation.Navigation.NavigationContainers
{
    /// <summary>
    ///     This Tabbed navigation container for when you only want the tabs
    ///     to appear on the first page and then push to a second page without tabs
    /// </summary>
    public class TabbedFONavigationContainer : NavigationPage, INavigationService
    {
        public TabbedPage FirstTabbedPage { get; }

        public IEnumerable<Page> TabbedPages => _tabs;
        private readonly List<Page> _tabs = new List<Page>();

        public TabbedFONavigationContainer(string titleOfFirstTab) : this(titleOfFirstTab,
            NavigationConstants.DefaultNavigationServiceName)
        {
        }

        public TabbedFONavigationContainer(string titleOfFirstTab, string navigationServiceName) : base(new TabbedPage())
        {
            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
            FirstTabbedPage = (TabbedPage)CurrentPage;
            FirstTabbedPage.Title = titleOfFirstTab;
        }

        public Task PushPage(Page page, BaseViewModel model, bool modal = false, bool animate = true)
        {
            return modal ? Navigation.PushModalAsync(CreateContainerPageSafe(page), animate) : Navigation.PushAsync(page, animate);
        }

        public Task PopPage(bool modal = false, bool animate = true)
        {
            return modal ? Navigation.PopModalAsync(animate) : Navigation.PopAsync(animate);
        }

        public Task PopToRoot(bool animate = true)
        {
            return Navigation.PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; }

        public void NotifyChildrenPageWasPopped()
        {
            foreach (Page page in FirstTabbedPage.Children) (page as NavigationPage)?.NotifyAllChildrenPopped();
        }

        public Task<BaseViewModel> SwitchSelectedRootPageModel<T>() where T : BaseViewModel
        {
            if (CurrentPage == FirstTabbedPage)
            {
                var page = _tabs.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);
                if (page > -1)
                {
                    FirstTabbedPage.CurrentPage = FirstTabbedPage.Children[page];
                    return Task.FromResult(FirstTabbedPage.CurrentPage.GetModel());
                }
            }
            else
            {
                throw new Exception("Cannot switch tabs when the tab screen is not visible");
            }

            return null;
        }

        protected void RegisterNavigation()
        {
            Ioc.Container.Register<INavigationService>(this, NavigationServiceName);
        }

        public virtual Page AddTab<T>(string title, string icon, object data = null) where T : BaseViewModel
        {
            Page page = ViewModelResolver.ResolveViewModel<T>(data);
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            _tabs.Add(page);
            Page container = CreateContainerPageSafe(page);
            container.Title = title;
            if (!string.IsNullOrWhiteSpace(icon))
                container.IconImageSource = icon;
            FirstTabbedPage.Children.Add(container);
            return container;
        }

        internal Page CreateContainerPageSafe(Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;

            return CreateContainerPage(page);
        }

        protected virtual Page CreateContainerPage(Page page)
        {
            return page;
        }
    }
}