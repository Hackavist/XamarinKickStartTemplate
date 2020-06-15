﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using static System.String;

namespace TemplateFoundation.Commands.WeakEventManager
{
    /// <summary>
    /// Weak event manager that allows for garbage collection when the EventHandler is still subscribed
    /// </summary>
    /// <typeparam name="TEventArgs">Event args type.</typeparam>
    public class WeakEventManager<TEventArgs> where TEventArgs : EventArgs
    {
        readonly Dictionary<string, List<Subscription>> _eventHandlers = new Dictionary<string, List<Subscription>>();

        /// <summary>
        /// Adds the event handler
        /// </summary>
        /// <param name="handler">Handler</param>
        /// <param name="eventName">Event name</param>
        public void AddEventHandler(EventHandler<TEventArgs> handler, [CallerMemberName] string eventName = "")
        {
            if (IsNullOrWhiteSpace(eventName))
                throw new ArgumentNullException(nameof(eventName));

            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            EventManagerService.AddEventHandler(eventName, handler.Target, handler.GetMethodInfo(), _eventHandlers);
        }

        /// <summary>
        /// Removes the event handler
        /// </summary>
        /// <param name="handler">Handler</param>
        /// <param name="eventName">Event name</param>
        public void RemoveEventHandler(EventHandler<TEventArgs> handler, [CallerMemberName] string eventName = "")
        {
            if (IsNullOrWhiteSpace(eventName))
                throw new ArgumentNullException(nameof(eventName));

            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            EventManagerService.RemoveEventHandler(eventName, handler.Target, handler.GetMethodInfo(), _eventHandlers);
        }

        /// <summary>
        /// Executes the event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="eventArgs">Event arguments</param>
        /// <param name="eventName">Event name</param>
        public void HandleEvent(object sender, TEventArgs eventArgs, string eventName) =>
            EventManagerService.HandleEvent(eventName, sender, eventArgs, _eventHandlers);

        private readonly WeakReference _targetReference;
        private readonly MethodInfo _method;

        public WeakEventManager(EventHandler<TEventArgs> callback)
        {
            _method = callback.GetMethodInfo();
            _targetReference = new WeakReference(callback.Target, true);
        }

        public void Handler(object sender, TEventArgs e)
        {
            var target = _targetReference.Target;
            if (target != null)
            {
                var callback = (Action<object, TEventArgs>)_method.CreateDelegate(typeof(Action<object, TEventArgs>), target);
                callback?.Invoke(sender, e);
            }
        }
    }

    /// <summary>
    /// Weak event manager that allows for garbage collection when the EventHandler is still subscribed
    /// </summary>
    public class WeakEventManager
    {
        readonly Dictionary<string, List<Subscription>> _eventHandlers = new Dictionary<string, List<Subscription>>();

        /// <summary>
        /// Adds the event handler
        /// </summary>
        /// <param name="handler">Handler</param>
        /// <param name="eventName">Event name</param>
        public void AddEventHandler(Delegate handler, [CallerMemberName] string eventName = "")
        {
            if (IsNullOrWhiteSpace(eventName))
                throw new ArgumentNullException(nameof(eventName));

            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            EventManagerService.AddEventHandler(eventName, handler.Target, handler.GetMethodInfo(), _eventHandlers);
        }

        /// <summary>
        /// Removes the event handler.
        /// </summary>
        /// <param name="handler">Handler</param>
        /// <param name="eventName">Event name</param>
        public void RemoveEventHandler(Delegate handler, [CallerMemberName] string eventName = "")
        {
            if (IsNullOrWhiteSpace(eventName))
                throw new ArgumentNullException(nameof(eventName));

            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            EventManagerService.RemoveEventHandler(eventName, handler.Target, handler.GetMethodInfo(), _eventHandlers);
        }

        /// <summary>
        /// Executes the event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="eventArgs">Event arguments</param>
        /// <param name="eventName">Event name</param>
        public void HandleEvent(object sender, object eventArgs, string eventName) =>
            EventManagerService.HandleEvent(eventName, sender, eventArgs, _eventHandlers);
    }
}