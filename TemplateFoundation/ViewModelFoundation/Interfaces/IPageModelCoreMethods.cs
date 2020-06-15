using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateFoundation.Navigation.Interfaces;
using TemplateFoundation.Navigation.NavigationContainers;
using Xamarin.Forms;

namespace TemplateFoundation.ViewModelFoundation.Interfaces
{
    public interface IPageModelCoreMethods
    {
        Task DisplayAlert(string title, string message, string cancel);

        Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);

        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);

        Task PushPageModel<T>(object data, bool modal = false, bool animate = true) where T : ViewModelFoundation.BaseViewModel;

        Task PushPageModel<T, TPage>(object data, bool modal = false, bool animate = true)
            where T : ViewModelFoundation.BaseViewModel where TPage : Page;

        Task PopPageModel(bool modal = false, bool animate = true);

        Task PopPageModel(object data, bool modal = false, bool animate = true);

        Task PushPageModel<T>(bool animate = true) where T : ViewModelFoundation.BaseViewModel;

        Task PushPageModel<T, TPage>(bool animate = true) where T : ViewModelFoundation.BaseViewModel where TPage : Page;

        Task PushPageModel(Type pageModelType, bool animate = true);

        Task PushPageModel(Type pageModelType, object data, bool modal = false, bool animate = true);

        /// <summary>
        ///     Removes current page/ViewModel from navigation
        /// </summary>
        void RemoveFromNavigation();

        /// <summary>
        ///     Removes specific page/ViewModel from navigation
        /// </summary>
        /// <param name="removeAll">Will remove all, otherwise it will just remove first on from the top of the stack</param>
        /// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
        void RemoveFromNavigation<TPageModel>(bool removeAll = false) where TPageModel : ViewModelFoundation.BaseViewModel;

        /// <summary>
        ///     Removes specific page/ViewModel from navigation
        /// </summary>
        /// <param name="removeAll">Will remove all, otherwise it will just remove first on from the top of the stack</param>
        /// <param name="type">The 1st type parameter</param>
        void RemoveFromNavigation(Type type, bool removeAll = false);

        /// <summary>
        ///     This method pushes a new PageModel modally with a new NavigationContainer
        /// </summary>
        /// <returns>Returns the name of the new service</returns>
        Task<string> PushPageModelWithNewNavigation<T>(object data, bool animate = true) where T : ViewModelFoundation.BaseViewModel;

        Task PushNewNavigationServiceModal(INavigationService newNavigationService, ViewModelFoundation.BaseViewModel[] basePageModels,
            bool animate = true);

        Task PushNewNavigationServiceModal(TabbedNavigationContainer tabbedNavigationContainer,
            ViewModelFoundation.BaseViewModel basePageModel = null, bool animate = true);

        Task PushNewNavigationServiceModal(MasterDetailNavigationContainer masterDetailContainer,
            ViewModelFoundation.BaseViewModel basePageModel = null, bool animate = true);

        Task PushNewNavigationServiceModal(INavigationService newNavigationService, ViewModelFoundation.BaseViewModel basePageModels,
            bool animate = true);

        Task PopModalNavigationService(bool animate = true);

        void SwitchOutRootNavigation(string navigationServiceName);

        /// <summary>
        ///     This method switches the selected main page, TabbedPage the selected tab or if MasterDetail, works with custom
        ///     pages also
        /// </summary>
        /// <returns>The BasePageModel, allows you to PopToRoot, Pass Data</returns>
        /// <param name="newSelected">The ViewModel of the root you want to change</param>
        Task<ViewModelFoundation.BaseViewModel> SwitchSelectedRootPageModel<T>() where T : ViewModelFoundation.BaseViewModel;

        /// <summary>
        ///     This method is used when you want to switch the selected page,
        /// </summary>
        /// <returns>The BasePageModel, allows you to PopToRoot, Pass Data</returns>
        /// <param name="newSelectedTab">The ViewModel of the root you want to change</param>
        Task<ViewModelFoundation.BaseViewModel> SwitchSelectedTab<T>() where T : ViewModelFoundation.BaseViewModel;

        /// <summary>
        ///     This method is used when you want to switch the selected page,
        /// </summary>
        /// <returns>The BasePageModel, allows you to PopToRoot, Pass Data</returns>
        /// <param name="newSelectedMaster">The ViewModel of the root you want to change</param>
        Task<ViewModelFoundation.BaseViewModel> SwitchSelectedMaster<T>() where T : ViewModelFoundation.BaseViewModel;

        Task PopToRoot(bool animate);

        void BatchBegin();

        void BatchCommit();
        Task PushPageModel<T>(Action<T> setPageModel, bool modal = false, bool animate = true) where T : ViewModelFoundation.BaseViewModel;
        List<Page> NavigationStack();
        List<Page> ModalStack();
    }
}