using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TemplateFoundation.ExtensionMethods;
using TemplateFoundation.IOCFoundation;
using TemplateFoundation.Navigation.Implementations;
using TemplateFoundation.Navigation.Interfaces;
using TemplateFoundation.ViewModelFoundation;

using Xamarin.Forms;

namespace TemplateFoundation.Navigation.NavigationContainers
{
    public class TabbedNavigationContainer : TabbedPage, INavigationService
    {
        public IEnumerable<Page> TabbedPages => _tabs;
        private readonly List<Page> _tabs = new List<Page>();

        public TabbedNavigationContainer() : this(NavigationConstants.DefaultNavigationServiceName)
        {
        }

        public TabbedNavigationContainer(string navigationServiceName)
        {
            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
        }

        public Task PushPage(Page page, BaseViewModel model, bool modal = false, bool animate = true)
        {
            return modal
                ? CurrentPage.Navigation.PushModalAsync(CreateContainerPageSafe(page), animate)
                : CurrentPage.Navigation.PushAsync(page, animate);
        }

        public Task PopPage(bool modal = false, bool animate = true)
        {
            return modal ? CurrentPage.Navigation.PopModalAsync(animate) : CurrentPage.Navigation.PopAsync(animate);
        }

        public Task PopToRoot(bool animate = true)
        {
            return CurrentPage.Navigation.PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; }

        public void NotifyChildrenPageWasPopped()
        {
            foreach (Page page in Children)
            {
                (page as NavigationPage)?.NotifyAllChildrenPopped();
            }
        }

        public Task<BaseViewModel> SwitchSelectedRootPageModel<T>() where T : BaseViewModel
        {
            int page = _tabs.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            if (page > -1)
            {
                CurrentPage = Children[page];
                var topOfStack = CurrentPage.Navigation.NavigationStack.LastOrDefault();
                if (topOfStack != null)
                    return Task.FromResult(topOfStack.GetModel());
            }

            return null;
        }

        protected void RegisterNavigation()
        {
            Ioc.Container.Register<INavigationService>(this, NavigationServiceName);
        }

        public virtual Page AddTab<T>(string title, string icon, object data = null) where T : BaseViewModel
        {
            var page = ViewModelResolver.ResolveViewModel<T>(data);
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            _tabs.Add(page);
            var navigationContainer = CreateContainerPageSafe(page);
            navigationContainer.Title = title;
            if (!string.IsNullOrWhiteSpace(icon)) navigationContainer.IconImageSource = icon;
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
    }
}