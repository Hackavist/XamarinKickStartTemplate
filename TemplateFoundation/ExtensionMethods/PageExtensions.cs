using TemplateFoundation.ViewModelFoundation;
using Xamarin.Forms;

namespace TemplateFoundation.ExtensionMethods
{
    public static class PageExtensions
    {
        public static BaseViewModel GetModel(this Page page)
        {
            BaseViewModel pageModel = page.BindingContext as BaseViewModel;
            return pageModel;
        }

        public static void NotifyAllChildrenPopped(this NavigationPage navigationPage)
        {
            foreach (Page page in navigationPage.Navigation.ModalStack)
            {
                BaseViewModel pageModel = page.GetModel();
                pageModel?.RaisePageWasPopped();
            }

            foreach (Page page in navigationPage.Navigation.NavigationStack)
            {
                BaseViewModel pageModel = page.GetModel();
                pageModel?.RaisePageWasPopped();
            }
        }
    }
}