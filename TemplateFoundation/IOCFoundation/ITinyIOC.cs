using System;
using TemplateFoundation.IOCFoundation.TinyIoC;

namespace TemplateFoundation.IOCFoundation
{
    public interface ITinyIoc
    {
        object Resolve(Type resolveType);
        TinyIoCContainer.RegisterOptions Register<TRegisterType>(TRegisterType instance) where TRegisterType : class;
        IRegisterOptions Register<TRegisterType>(TRegisterType instance, string name) where TRegisterType : class;
        TResolveType Resolve<TResolveType>() where TResolveType : class;
        TResolveType Resolve<TResolveType>(string name) where TResolveType : class;

        IRegisterOptions Register<TRegisterType, TRegisterImplementation>()
            where TRegisterType : class
            where TRegisterImplementation : class, TRegisterType;

        void Unregister<TRegisterType>();
        void Unregister<TRegisterType>(string name);
    }
}