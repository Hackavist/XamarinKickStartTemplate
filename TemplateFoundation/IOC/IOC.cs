namespace TemplateFoundation.IOC
{
    public class IOC
    {
        private static ITinyIOC _freshIocContainer;

        public static ITinyIOC Container
        {
            get { return _freshIocContainer ??= new FreshTinyIOCBuiltIn(); }
        }

        public static void OverrideContainer(ITinyIOC overrideContainer)
        {
            _freshIocContainer = overrideContainer;
        }
    }
}