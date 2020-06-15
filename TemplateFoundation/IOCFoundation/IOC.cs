namespace TemplateFoundation.IOCFoundation
{
    public static class Ioc
    {
        private static ITinyIOC _freshIocContainer;

        public static ITinyIOC Container
        {
            get { return _freshIocContainer ??= new FreshTinyIocBuiltIn(); }
        }

        public static void OverrideContainer(ITinyIOC overrideContainer)
        {
            _freshIocContainer = overrideContainer;
        }
    }
}