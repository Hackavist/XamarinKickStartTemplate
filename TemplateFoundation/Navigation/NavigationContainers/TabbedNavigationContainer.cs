using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateFoundation.ExtensionMethods;
using TemplateFoundation.Navigation.Implementations;
using TemplateFoundation.Navigation.Interfaces;
using Xamarin.Forms;

namespace TemplateFoundation.Navigation.NavigationContainers
{
    public class TabbedNavigationContainer : TabbedPage, INavigationService
    {
        private readonly List<Page> _tabs = new List<Page>();
        public IEnumerable<Page> TabbedPages => _tabs;

        public TabbedNavigationContainer() : this(NavigationConstants.DefaultNavigationServiceName)
        {
        }

        public TabbedNavigationContainer(string navigationServiceName)
        {
            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
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
            var navigationContainer = CreateContainerPageSafe(page);
            navigationContainer.Title = title;
            if (!string.IsNullOrWhiteSpace(icon))
                navigationContainer.Icon = icon;
            Children.Add(navigationContainer);
            return navigationContainer;
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

        public System.Threading.Tasks.Task PushPage(Xamarin.Forms.Page page, BaseViewModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return this.CurrentPage.Navigation.PushModalAsync(CreateContainerPageSafe(page));
            return this.CurrentPage.Navigation.PushAsync(page);
        }

        public System.Threading.Tasks.Task PopPage(bool modal = false, bool animate = true)
        {
            if (modal)
                return this.CurrentPage.Navigation.PopModalAsync(animate);
            return this.CurrentPage.Navigation.PopAsync(animate);
        }

        public Task PopToRoot(bool animate = true)
        {
            return this.CurrentPage.Navigation.PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            foreach (var page in this.Children)
            {
                if (page is NavigationPage)
                    ((NavigationPage)page).NotifyAllChildrenPopped();
            }
        }

        public Task<BaseViewModel> SwitchSelectedRootPageModel<T>() where T : BaseViewModel
        {
            var page = _tabs.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            if (page > -1)
            {
                CurrentPage = this.Children[page];
                var topOfStack = CurrentPage.Navigation.NavigationStack.LastOrDefault();
                if (topOfStack != null)
                    return Task.FromResult(topOfStack.GetModel());

            }
            return null;
        }
    }
}

