using System;
using System.Reflection;

namespace TemplateFoundation.WeakEventManager
{
    internal readonly struct Subscription
    {
        public Subscription(WeakReference subscriber, MethodInfo handler)
        {
            Subscriber = subscriber;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public WeakReference Subscriber { get; }
        public MethodInfo Handler { get; }
    }
}
