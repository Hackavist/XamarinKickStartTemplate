using System.Threading.Tasks;
using TemplateFoundation.ViewModelFoundation;
using Xamarin.Forms;

namespace TemplateFoundation.Navigation.Interfaces
{
    public interface INavigationService
    {
        string NavigationServiceName { get; }
        Task PopToRoot(bool animate = true);

        Task PushPage(Page page, BaseViewModel model, bool modal = false, bool animate = true);

        Task PopPage(bool modal = false, bool animate = true);

        /// <summary>
        ///     This method switches the selected main page, TabbedPage the selected tab or if MasterDetail, works with custom
        ///     pages also
        /// </summary>
        /// <returns>The BasePageModel, allows you to PopToRoot, Pass Data</returns>
        Task<BaseViewModel> SwitchSelectedRootPageModel<T>() where T : BaseViewModel;

        void NotifyChildrenPageWasPopped();
    }
}