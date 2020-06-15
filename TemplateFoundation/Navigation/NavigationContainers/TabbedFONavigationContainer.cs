using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateFoundation.ExtensionMethods;
using TemplateFoundation.Navigation.Implementations;
using TemplateFoundation.Navigation.Interfaces;
using Xamarin.Forms;

namespace TemplateFoundation.Navigation.NavigationContainers
{
    /// <summary>
    /// This Tabbed navigation container for when you only want the tabs 
    /// to appear on the first page and then push to a second page without tabs
    /// </summary>
    public class TabbedFONavigationContainer : NavigationPage, INavigationService
    {
        private readonly TabbedPage _innerTabbedPage;
        public TabbedPage FirstTabbedPage => _innerTabbedPage;
        private readonly List<Page> _tabs = new List<Page>();
        public IEnumerable<Page> TabbedPages { get { return _tabs; } }

        public TabbedFONavigationContainer(string titleOfFirstTab) : this(titleOfFirstTab, NavigationConstants.DefaultNavigationServiceName)
        {
        }

        public TabbedFONavigationContainer(string titleOfFirstTab, string navigationServiceName) : base(new TabbedPage())
        {
            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
            _innerTabbedPage = (TabbedPage)this.CurrentPage;
            _innerTabbedPage.Title = titleOfFirstTab;
        }

        protected void RegisterNavigation()
        {
            IOCFoundation.IOC.Container.Register<INavigationService>(this, NavigationServiceName);
        }

        public virtual Page AddTab<T>(string title, string icon, object data = null) where T : BaseViewModel
        {
            var page = ViewModelResolver.ResolveViewModel<T>(data);
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            _tabs.Add(page);
            var container = CreateContainerPageSafe(page);
            container.Title = title;
            if (!string.IsNullOrWhiteSpace(icon))
                container.Icon = icon;
            _innerTabbedPage.Children.Add(container);
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

        public System.Threading.Tasks.Task PushPage(Xamarin.Forms.Page page, BaseViewModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return this.Navigation.PushModalAsync(CreateContainerPageSafe(page));
            return this.Navigation.PushAsync(page);
        }

        public System.Threading.Tasks.Task PopPage(bool modal = false, bool animate = true)
        {
            if (modal)
                return this.Navigation.PopModalAsync(animate);
            return this.Navigation.PopAsync(animate);
        }

        public Task PopToRoot(bool animate = true)
        {
            return this.Navigation.PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            foreach (var page in _innerTabbedPage.Children)
            {
                if (page is NavigationPage)
                    ((NavigationPage)page).NotifyAllChildrenPopped();
            }
        }

        public Task<BaseViewModel> SwitchSelectedRootPageModel<T>() where T : BaseViewModel
        {
            if (this.CurrentPage == _innerTabbedPage)
            {
                var page = _tabs.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);
                if (page > -1)
                {
                    _innerTabbedPage.CurrentPage = this._innerTabbedPage.Children[page];
                    return Task.FromResult(_innerTabbedPage.CurrentPage.GetModel());
                }
            }
            else
            {
                throw new Exception("Cannot switch tabs when the tab screen is not visible");
            }

            return null;
        }
    }
}

