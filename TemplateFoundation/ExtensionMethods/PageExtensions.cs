using Xamarin.Forms;
using TemplateFoundation.BaseViewModel;
namespace TemplateFoundation.ExtensionMethods
{
    public static class PageExtensions
    {
        public static BaseViewModel GetModel(this Page page)
        {
            var pageModel = page.BindingContext as BaseViewModel;
            return pageModel;
        }

        public static void NotifyAllChildrenPopped(this NavigationPage navigationPage)
        {
            foreach (var page in navigationPage.Navigation.ModalStack)
            {
                BaseViewModel pageModel = page.GetModel();
                pageModel?.RaisePageWasPopped();
            }

            foreach (var page in navigationPage.Navigation.NavigationStack)
            {
                BaseViewModel pageModel = page.GetModel();
                pageModel?.RaisePageWasPopped();
            }
        }
    }
}

