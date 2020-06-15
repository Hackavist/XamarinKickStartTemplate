using System.Collections.Specialized;
using System.Linq;
using TemplateFoundation.Navigation;
using TemplateFoundation.Navigation.Interfaces;
using TemplateFoundation.ViewModelFoundation;
using Xamarin.Forms;

namespace TemplateFoundation.PageFoundation
{
    public class BaseContentPage : ContentPage
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is BaseViewModel pageModel && pageModel.ToolbarItems != null && pageModel.ToolbarItems.Count > 0)
            {
                pageModel.ToolbarItems.CollectionChanged += PageModel_ToolbarItems_CollectionChanged;

                foreach (var toolBarItem in pageModel.ToolbarItems)
                {
                    if (!ToolbarItems.Contains(toolBarItem))
                        ToolbarItems.Add(toolBarItem);
                }
            }
        }

        private void PageModel_ToolbarItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (ToolbarItem toolBarItem in e.NewItems)
            {
                if (!ToolbarItems.Contains(toolBarItem))
                    ToolbarItems.Add(toolBarItem);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
                foreach (ToolbarItem toolBarItem in e.OldItems)
                {
                    if (!ToolbarItems.Contains(toolBarItem))
                        ToolbarItems.Add(toolBarItem);
                }
        }

        protected override bool OnBackButtonPressed()
        {
            var navContainer =
                IOCFoundation.Ioc.Container.Resolve<INavigationService>(NavigationConstants.DefaultNavigationServiceName);
            if (Application.Current.MainPage.Navigation.NavigationStack.Count() == 1) return true;
            return base.OnBackButtonPressed();
        }
    }
}