using System;
using TemplateFoundation.Navigation.Interfaces;

namespace TemplateFoundation.Navigation.Implementations
{
    public class PageModelMapper : IPageModelMapper
    {
        public string GetPageTypeName(Type pageModelType)
        {
            return pageModelType.AssemblyQualifiedName?.Replace("PageModel", "Page").Replace("ViewModel", "Page");
        }
    }
}