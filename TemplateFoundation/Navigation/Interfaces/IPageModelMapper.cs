using System;

namespace TemplateFoundation.Navigation.Interfaces
{
    public interface IPageModelMapper
    {
        string GetPageTypeName(Type pageModelType);
    }
}

