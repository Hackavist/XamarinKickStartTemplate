using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateFoundation.ExtensionMethods;
using TemplateFoundation.Navigation.Implementations;
using TemplateFoundation.Navigation.Interfaces;
using TemplateFoundation.Navigation.NavigationContainers;
using TemplateFoundation.ViewModelFoundation.Interfaces;
using Xamarin.Forms;

namespace TemplateFoundation.ViewModelFoundation.Implementations
{
    public class PageModelCoreMethods : IPageModelCoreMethods
    {
        private readonly Page _currentPage;
        private readonly ViewModelFoundation.BaseViewModel _currentPageModel;

        public PageModelCoreMethods(Page currentPage, ViewModelFoundation.BaseViewModel pageModel)
        {
            _currentPage = currentPage;
            _currentPageModel = pageModel;
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            if (_currentPage != null)
                await _currentPage.DisplayAlert(title, message, cancel);
        }

        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            if (_currentPage != null)
                return await _currentPage.DisplayActionSheet(title, cancel, destruction, buttons);
            return null;
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            if (_currentPage != null)
                return await _currentPage.DisplayAlert(title, message, accept, cancel);
            return false;
        }

        public async Task PushPageModel<T>(Action<T> setPageModel, bool modal = false, bool animate = true) where T : ViewModelFoundation.BaseViewModel
        {
            T pageModel = IOCFoundation.Ioc.Container.Resolve<T>();

            setPageModel?.Invoke(pageModel);

            await PushPageModel(pageModel, null, modal, animate);
        }

        public async Task PushPageModel<T>(object data, bool modal = false, bool animate = true) where T : ViewModelFoundation.BaseViewModel
        {
            T pageModel = IOCFoundation.Ioc.Container.Resolve<T>();

            await PushPageModel(pageModel, data, modal, animate);
        }

        public async Task PushPageModel<T, TPage>(object data, bool modal = false, bool animate = true) where T : ViewModelFoundation.BaseViewModel where TPage : Page
        {
            T pageModel = IOCFoundation.Ioc.Container.Resolve<T>();
            TPage page = IOCFoundation.Ioc.Container.Resolve<TPage>();
            ViewModelResolver.BindingViewModel(data, page, pageModel);
            await PushPageModelWithPage(page, pageModel, data, modal, animate);
        }

        public Task PushPageModel(Type pageModelType, bool animate = true)
        {
            return PushPageModel(pageModelType, null, animate);
        }

        public Task PushPageModel(Type pageModelType, object data, bool modal = false, bool animate = true)
        {
            var pageModel = IOCFoundation.Ioc.Container.Resolve(pageModelType) as ViewModelFoundation.BaseViewModel;

            return PushPageModel(pageModel, data, modal, animate);
        }

        async Task PushPageModel(ViewModelFoundation.BaseViewModel pageModel, object data, bool modal = false, bool animate = true)
        {
            var page = ViewModelResolver.ResolveViewModel(data, pageModel);
            await PushPageModelWithPage(page, pageModel, data, modal, animate);
        }

        async Task PushPageModelWithPage(Page page, ViewModelFoundation.BaseViewModel pageModel, object data, bool modal = false, bool animate = true)
        {
            pageModel.PreviousPageModel = _currentPageModel; //This is the previous page model because it's push to a new one, and this is current
            pageModel.CurrentNavigationServiceName = _currentPageModel.CurrentNavigationServiceName;

            if (string.IsNullOrWhiteSpace(pageModel.PreviousNavigationServiceName))
                pageModel.PreviousNavigationServiceName = _currentPageModel.PreviousNavigationServiceName;

            if (page is MasterDetailNavigationContainer)
            {
                await this.PushNewNavigationServiceModal((MasterDetailNavigationContainer)page, pageModel, animate);
            }
            else if (page is TabbedNavigationContainer)
            {
                await this.PushNewNavigationServiceModal((TabbedNavigationContainer)page, pageModel, animate);
            }
            else
            {
                INavigationService rootNavigation = IOCFoundation.Ioc.Container.Resolve<INavigationService>(_currentPageModel.CurrentNavigationServiceName);

                await rootNavigation.PushPage(page, pageModel, modal, animate);
            }
        }

        public async Task PopPageModel(bool modal = false, bool animate = true)
        {
            string navServiceName = _currentPageModel.CurrentNavigationServiceName;
            if (_currentPageModel.IsModalFirstChild)
            {
                await PopModalNavigationService(animate);
            }
            else
            {
                if (modal)
                    this._currentPageModel.RaisePageWasPopped();

                INavigationService rootNavigation = IOCFoundation.Ioc.Container.Resolve<INavigationService>(navServiceName);
                await rootNavigation.PopPage(modal, animate);
            }
        }

        public async Task PopToRoot(bool animate)
        {
            INavigationService rootNavigation = IOCFoundation.Ioc.Container.Resolve<INavigationService>(_currentPageModel.CurrentNavigationServiceName);
            await rootNavigation.PopToRoot(animate);
        }

        public async Task PopPageModel(object data, bool modal = false, bool animate = true)
        {
            if (_currentPageModel != null && _currentPageModel.PreviousPageModel != null && data != null)
            {
                _currentPageModel.PreviousPageModel.ReverseInit(data);
            }
            await PopPageModel(modal, animate);
        }

        public Task PushPageModel<T>(bool animate = true) where T : ViewModelFoundation.BaseViewModel
        {
            return PushPageModel<T>(null, false, animate);
        }

        public Task PushPageModel<T, TPage>(bool animate = true) where T : ViewModelFoundation.BaseViewModel where TPage : Page
        {
            return PushPageModel<T, TPage>(null, animate);
        }

        public Task PushNewNavigationServiceModal(TabbedNavigationContainer tabbedNavigationContainer, ViewModelFoundation.BaseViewModel basePageModel = null, bool animate = true)
        {
            var models = tabbedNavigationContainer.TabbedPages.Select(o => o.GetModel()).ToList();
            if (basePageModel != null)
                models.Add(basePageModel);
            return PushNewNavigationServiceModal(tabbedNavigationContainer, models.ToArray(), animate);
        }

        public Task PushNewNavigationServiceModal(MasterDetailNavigationContainer masterDetailContainer, ViewModelFoundation.BaseViewModel basePageModel = null, bool animate = true)
        {
            var models = masterDetailContainer.Pages.Select(o =>
               {
                   if (o.Value is NavigationPage)
                       return ((NavigationPage)o.Value).CurrentPage.GetModel();
                   else
                       return o.Value.GetModel();
               }).ToList();

            if (basePageModel != null)
                models.Add(basePageModel);

            return PushNewNavigationServiceModal(masterDetailContainer, models.ToArray(), animate);
        }

        public Task PushNewNavigationServiceModal(INavigationService newNavigationService, ViewModelFoundation.BaseViewModel basePageModels, bool animate = true)
        {
            return PushNewNavigationServiceModal(newNavigationService, new ViewModelFoundation.BaseViewModel[] { basePageModels }, animate);
        }

        public async Task PushNewNavigationServiceModal(INavigationService newNavigationService, ViewModelFoundation.BaseViewModel[] basePageModels, bool animate = true)
        {
            var navPage = newNavigationService as Page;
            if (navPage == null)
                throw new Exception("Navigation service is not Page");

            foreach (var pageModel in basePageModels)
            {
                pageModel.CurrentNavigationServiceName = newNavigationService.NavigationServiceName;
                pageModel.PreviousNavigationServiceName = _currentPageModel.CurrentNavigationServiceName;
                pageModel.IsModalFirstChild = true;
            }

            INavigationService rootNavigation = IOCFoundation.Ioc.Container.Resolve<INavigationService>(_currentPageModel.CurrentNavigationServiceName);
            await rootNavigation.PushPage(navPage, null, true, animate);
        }

        public void SwitchOutRootNavigation(string navigationServiceName)
        {
            INavigationService rootNavigation = IOCFoundation.Ioc.Container.Resolve<INavigationService>(navigationServiceName);

            if (!(rootNavigation is Page))
                throw new Exception("Navigation service is not a page");

            Xamarin.Forms.Application.Current.MainPage = rootNavigation as Page;
        }

        public async Task PopModalNavigationService(bool animate = true)
        {
            var currentNavigationService = IOCFoundation.Ioc.Container.Resolve<INavigationService>(_currentPageModel.CurrentNavigationServiceName);
            currentNavigationService.NotifyChildrenPageWasPopped();

            IOCFoundation.Ioc.Container.Unregister<INavigationService>(_currentPageModel.CurrentNavigationServiceName);

            var navServiceName = _currentPageModel.PreviousNavigationServiceName;
            INavigationService rootNavigation = IOCFoundation.Ioc.Container.Resolve<INavigationService>(navServiceName);
            await rootNavigation.PopPage(animate);
        }

        /// <summary>
        /// This method switches the selected main page, TabbedPage the selected tab or if MasterDetail, works with custom pages also
        /// </summary>
        public Task<ViewModelFoundation.BaseViewModel> SwitchSelectedRootPageModel<T>() where T : ViewModelFoundation.BaseViewModel
        {
            var currentNavigationService = IOCFoundation.Ioc.Container.Resolve<INavigationService>(_currentPageModel.CurrentNavigationServiceName);

            return currentNavigationService.SwitchSelectedRootPageModel<T>();
        }

        /// <summary>
        /// This method is used when you want to switch the selected page, 
        /// </summary>
        public Task<ViewModelFoundation.BaseViewModel> SwitchSelectedTab<T>() where T : ViewModelFoundation.BaseViewModel
        {
            return SwitchSelectedRootPageModel<T>();
        }

        /// <summary>
        /// This method is used when you want to switch the selected page, 
        /// </summary>
        public Task<ViewModelFoundation.BaseViewModel> SwitchSelectedMaster<T>() where T : ViewModelFoundation.BaseViewModel
        {
            return SwitchSelectedRootPageModel<T>();
        }

        public async Task<string> PushPageModelWithNewNavigation<T>(object data, bool animate = true) where T : ViewModelFoundation.BaseViewModel
        {
            var page = ViewModelResolver.ResolveViewModel<T>(data);
            var navigationName = Guid.NewGuid().ToString();
            var naviationContainer = new NavigationPageContainer(page, navigationName);
            await PushNewNavigationServiceModal(naviationContainer, page.GetModel(), animate);
            return navigationName;
        }

        public void BatchBegin()
        {
            _currentPage.BatchBegin();
        }

        public void BatchCommit()
        {
            _currentPage.BatchCommit();
        }

        /// <summary>
        /// Removes current page/pagemodel from navigation
        /// </summary>
        public void RemoveFromNavigation()
        {
            this._currentPageModel.RaisePageWasPopped();
            this._currentPage.Navigation.RemovePage(_currentPage);
        }

        /// <summary>
        /// Removes specific page/pagemodel from navigation
        /// </summary>
        public void RemoveFromNavigation<TPageModel>(bool removeAll = false) where TPageModel : ViewModelFoundation.BaseViewModel =>
            RemoveFromNavigation(typeof(TPageModel), removeAll);

        /// <summary>
        /// Removes specific page/pagemodel from navigation
        /// </summary>
        public void RemoveFromNavigation(Type type, bool removeAll = false)
        {
            foreach (var page in this._currentPage.Navigation.NavigationStack.Reverse().ToList())
            {
                if (page.BindingContext?.GetType() == type)
                {
                    page.GetModel()?.RaisePageWasPopped();
                    this._currentPage.Navigation.RemovePage(page);
                    if (!removeAll)
                        break;
                }
            }
        }

        public List<Page> NavigationStack()
        {
            return _currentPage.Navigation.NavigationStack.ToList();
        }

        public List<Page> ModalStack()
        {
            return _currentPage.Navigation.ModalStack.ToList();
        }
    }
}