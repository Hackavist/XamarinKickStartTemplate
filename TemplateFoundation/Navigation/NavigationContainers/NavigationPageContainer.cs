using System;
using System.Threading.Tasks;
using TemplateFoundation.Navigation.Interfaces;
using Xamarin.Forms;

namespace TemplateFoundation.Navigation.NavigationContainers
{
    public class NavigationPageContainer : Xamarin.Forms.NavigationPage, INavigationService
    {
        public NavigationPageContainer(Page page)
            : this(page, NavigationConstants.DefaultNavigationServiceName)
        {
        }

        public NavigationPageContainer(Page page, string navigationPageName)
            : base(page)
        {
            var pageModel = page.GetModel();
            if (pageModel == null)
                throw new InvalidCastException("BindingContext was not a BaseViewModel on this Page");

            pageModel.CurrentNavigationServiceName = navigationPageName;
            NavigationServiceName = navigationPageName;
            RegisterNavigation();
        }

        protected void RegisterNavigation()
        {
            IOCFoundation.IOC.Container.Register<INavigationService>(this, NavigationServiceName);
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

        public virtual Task PushPage(Xamarin.Forms.Page page, BaseViewModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PushModalAsync(CreateContainerPageSafe(page), animate);
            return Navigation.PushAsync(page, animate);
        }

        public virtual Task PopPage(bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PopModalAsync(animate);
            return Navigation.PopAsync(animate);
        }

        public Task PopToRoot(bool animate = true)
        {
            return Navigation.PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            this.NotifyAllChildrenPopped();
        }

        public Task<BaseViewModel> SwitchSelectedRootPageModel<T>() where T : BaseViewModel
        {
            throw new Exception("This navigation container has no selected roots, just a single root");
        }
    }
}

