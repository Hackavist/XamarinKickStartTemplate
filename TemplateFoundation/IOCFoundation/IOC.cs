namespace TemplateFoundation.IOCFoundation
{
    public static class Ioc
    {
        private static ITinyIoc _freshIocContainer;

        public static ITinyIoc Container
        {
            get { return _freshIocContainer ??= new FreshTinyIocBuiltIn(); }
        }

        public static void OverrideContainer(ITinyIoc overrideContainer)
        {
            _freshIocContainer = overrideContainer;
        }
    }
}