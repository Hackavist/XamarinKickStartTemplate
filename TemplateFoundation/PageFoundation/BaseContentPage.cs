using System.Collections.Specialized;
using System.Linq;
using TemplateFoundation.ViewModelFoundation;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Application = Xamarin.Forms.Application;

namespace TemplateFoundation.PageFoundation
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            if (Device.RuntimePlatform == Device.iOS) BackgroundColor = Color.Transparent;
            Thickness safeInsets = On<iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Automatic).SafeAreaInsets();
            safeInsets.Bottom = 15;
            Padding = safeInsets;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (!(BindingContext is BaseViewModel pageModel) || pageModel.ToolbarItems == null ||
                pageModel.ToolbarItems.Count <= 0) return;
            pageModel.ToolbarItems.CollectionChanged += PageModel_ToolbarItems_CollectionChanged;

            foreach (ToolbarItem toolBarItem in pageModel.ToolbarItems)
                if (!ToolbarItems.Contains(toolBarItem)) ToolbarItems.Add(toolBarItem);
        }

        private void PageModel_ToolbarItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (ToolbarItem toolBarItem in e.NewItems)
                if (!ToolbarItems.Contains(toolBarItem)) ToolbarItems.Add(toolBarItem);

            if (e.Action != NotifyCollectionChangedAction.Remove &&
                e.Action != NotifyCollectionChangedAction.Replace) return;
            {
                foreach (ToolbarItem toolBarItem in e.OldItems)
                    if (!ToolbarItems.Contains(toolBarItem)) ToolbarItems.Add(toolBarItem);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            //INavigationService navContainer = Ioc.Container.Resolve<INavigationService>(NavigationConstants.DefaultNavigationServiceName);
            return Application.Current.MainPage.Navigation.NavigationStack.Count() == 1 || base.OnBackButtonPressed();
        }
    }
}