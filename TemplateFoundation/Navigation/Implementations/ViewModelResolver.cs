using System;
using TemplateFoundation.IOCFoundation;
using TemplateFoundation.Navigation.Interfaces;
using TemplateFoundation.ViewModelFoundation;
using TemplateFoundation.ViewModelFoundation.Implementations;
using Xamarin.Forms;

namespace TemplateFoundation.Navigation.Implementations
{
    public static class ViewModelResolver
    {
        public static IPageModelMapper PageModelMapper { get; set; } = new PageModelMapper();

        public static Page ResolveViewModel<T>() where T : BaseViewModel
        {
            return ResolveViewModel<T>(null);
        }

        public static Page ResolveViewModel<T>(object initData) where T : BaseViewModel
        {
            var viewModel = Ioc.Container.Resolve<T>();

            return ResolveViewModel(initData, viewModel);
        }

        public static Page ResolveViewModel<T>(object data, T viewModel) where T : BaseViewModel
        {
            var type = viewModel.GetType();
            return ResolveViewModel(type, data, viewModel);
        }

        public static Page ResolveViewModel(Type type, object data)
        {
            var viewModel = Ioc.Container.Resolve(type) as BaseViewModel;
            return ResolveViewModel(type, data, viewModel);
        }

        public static Page ResolveViewModel(Type type, object data, BaseViewModel viewModel)
        {
            string name = PageModelMapper.GetPageTypeName(type);
            var pageType = Type.GetType(name);
            if (pageType == null)
                throw new Exception(name + " not found");

            var page = (Page)Ioc.Container.Resolve(pageType);

            BindingViewModel(data, page, viewModel);

            return page;
        }

        public static Page BindingViewModel(object data, Page targetPage, BaseViewModel viewModel)
        {
            viewModel.WireEvents(targetPage);
            viewModel.CurrentPage = targetPage;
            viewModel.NavigationService = new PageModelCoreMethods(targetPage, viewModel);
            viewModel.Init(data);
            targetPage.BindingContext = viewModel;
            return targetPage;
        }
    }
}