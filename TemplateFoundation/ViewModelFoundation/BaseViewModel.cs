using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using AutoMapper;
using TemplateFoundation.Navigation;
using TemplateFoundation.ViewModelFoundation.Interfaces;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace TemplateFoundation.ViewModelFoundation
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		public string Title { get; set; }
		public string Icon { get; set; }

		protected bool CanExecuteCommand { get; set; } = true;
		public bool IsSubscribedOnExceptionHandlers { get; set; }
		public bool IsEnabledSubscribedOnExceptionHandlers { get; set; } = true;

		/// <summary>
		///     Core methods are basic built in methods for the App including Pushing, Pop and Alert
		/// </summary>
		public IPageModelCoreMethods NavigationService { get; set; }
		protected IMapper Mapper { get; set; }

		/// <summary>
		///     This property is used by the FreshBaseContentPage and allows you to set the toolbar items on the page.
		/// </summary>
		public ObservableCollection<ToolbarItem> ToolbarItems { get; set; }

		/// <summary>
		///     The previous page model, that's automatically filled, on push
		/// </summary>
		public BaseViewModel PreviousPageModel { get; set; }

		/// <summary>
		///     A reference to the current page, that's automatically filled, on push
		/// </summary>
		public Page CurrentPage { get; set; }

		private readonly DelegateWeakEventManager pageWasPoopedWeakEventManager = new DelegateWeakEventManager();

		/// <summary>
		///     WeakEvent handler for the INotifyPropertyChanged Event
		/// </summary>
		private readonly DelegateWeakEventManager propertyChangedWeakEventHandler = new DelegateWeakEventManager();

		private bool alreadyAttached;


		/// <summary>
		///     Used when a page is shown modal and wants a new Navigation Stack
		/// </summary>
		public string CurrentNavigationServiceName = NavigationConstants.DefaultNavigationServiceName;

		/// <summary>
		///     Is true when this model is the first of a new navigation stack
		/// </summary>
		public bool IsModalFirstChild;

		private NavigationPage navigationPage;

		/// <summary>
		///     Used when a page is shown modal and wants a new Navigation Stack
		/// </summary>
		public string PreviousNavigationServiceName;

		protected BaseViewModel(IMapper mapper)
		{
			Mapper = mapper;
		}

		/// <summary>
		///     The INotifyPropertyChanged Implementation
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged
		{
			add => this.propertyChangedWeakEventHandler.AddEventHandler(value);
			remove => this.propertyChangedWeakEventHandler.RemoveEventHandler(value);
		}


		/// <summary>
		///     This method is called when the PageModel is loaded, the initData is the data that's sent from pagemodel before
		/// </summary>
		/// <param name="initData">Data that's sent to this PageModel from the pusher</param>
		public virtual void Init(object initData)
		{
			ErrorHandlerSubscribe();
		}

		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null!)
		{
			propertyChangedWeakEventHandler.RaiseEvent(this, new PropertyChangedEventArgs(propertyName),
				nameof(PropertyChanged));
		}

		/// <summary>
		///     This event is raise when a page is Popped, this might not be raise every time a page is Popped.
		///     Note* this might be raised multiple times.
		/// </summary>
		public event EventHandler PageWasPopped
		{
			add => pageWasPoopedWeakEventManager.AddEventHandler(value);
			remove => pageWasPoopedWeakEventManager.RemoveEventHandler(value);
		}

		/// <summary>
		///     This method is called when a page is Popped, it also allows for data to be returned.
		/// </summary>
		/// <param name="returnedData">This data that's returned from </param>
		public virtual void ReverseInit(object returnedData)
		{
			ErrorHandlerUnSubscribe();
		}


		internal void WireEvents(Page page)
		{
			page.Appearing += new WeakEventManager.WeakEventManager<EventArgs>(ViewIsAppearing).Handler;
			page.Disappearing += new WeakEventManager.WeakEventManager<EventArgs>(ViewIsDisappearing).Handler;
		}

		/// <summary>
		///     This method is called when the view is disappearing.
		/// </summary>
		protected virtual void ViewIsDisappearing(object sender, EventArgs e)
		{
			ErrorHandlerUnSubscribe();
		}

		/// <summary>
		///     This methods is called when the View is appearing
		/// </summary>
		protected virtual void ViewIsAppearing(object sender, EventArgs e)
		{
			ErrorHandlerSubscribe();
			if (!alreadyAttached)
				AttachPageWasPoppedEvent();
		}

		/// <summary>
		///     This is used to attach the page was popped method to a NavigationPage if available
		/// </summary>
		private void AttachPageWasPoppedEvent()
		{
			if (CurrentPage.Parent is not NavigationPage navPage) return;
			this.navigationPage = navPage;
			this.alreadyAttached = true;
			navPage.Popped += new WeakEventManager.WeakEventManager<NavigationEventArgs>(HandleNavPagePopped).Handler;
		}

		/// <summary>
		///     This means the current PageModel is shown modally and can be popped modally
		/// </summary>
		public bool IsModalAndHasPreviousNavigationStack()
		{
			return !string.IsNullOrWhiteSpace(this.PreviousNavigationServiceName) && this.PreviousNavigationServiceName != this.CurrentNavigationServiceName;
		}

		/// <summary>
		///     This is used to handle the navigation page popping a navigation page
		/// </summary>
		private void HandleNavPagePopped(object sender, NavigationEventArgs e)
		{
			if (e.Page == CurrentPage) RaisePageWasPopped();
		}

		public void RaisePageWasPopped()
		{
			this.pageWasPoopedWeakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(PageWasPopped));
			if (CurrentPage?.Parent is NavigationPage navPage) navPage.Popped -= HandleNavPagePopped;
			if (this.navigationPage != null) this.navigationPage.Popped -= HandleNavPagePopped;
			this.navigationPage = null;
			if (CurrentPage != null)
			{
				CurrentPage.Appearing -= ViewIsAppearing;
				CurrentPage.Disappearing -= ViewIsDisappearing;
			}
			CurrentPage = null;
			ViewModelCleanUp();
			GC.Collect();
		}
		public virtual void ViewModelCleanUp()
		{
			ErrorHandlerUnSubscribe();
		}

		private void ErrorHandlerSubscribe()
		{
			if (!IsEnabledSubscribedOnExceptionHandlers || IsSubscribedOnExceptionHandlers) return;
			AppDomain.CurrentDomain.UnhandledException +=
				new WeakEventManager.WeakEventManager<UnhandledExceptionEventArgs>(CurrentDomainOnUnhandledException)
					.Handler;
			AppDomain.CurrentDomain.FirstChanceException +=
				new WeakEventManager.WeakEventManager<FirstChanceExceptionEventArgs>(FirstChanceExceptionHandler)
					.Handler;
			TaskScheduler.UnobservedTaskException +=
				new WeakEventManager.WeakEventManager<UnobservedTaskExceptionEventArgs>(
						TaskSchedulerOnUnobservedTaskException)
					.Handler;
			IsSubscribedOnExceptionHandlers = true;
		}

		private void ErrorHandlerUnSubscribe()
		{
			if (!IsEnabledSubscribedOnExceptionHandlers || !IsSubscribedOnExceptionHandlers) return;
			AppDomain.CurrentDomain.UnhandledException -= CurrentDomainOnUnhandledException;
			AppDomain.CurrentDomain.FirstChanceException -= FirstChanceExceptionHandler;
			TaskScheduler.UnobservedTaskException -= TaskSchedulerOnUnobservedTaskException;
			IsSubscribedOnExceptionHandlers = false;
		}

		private void TaskSchedulerOnUnobservedTaskException(object sender,
			UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
		{
			LogUnhandledException(unobservedTaskExceptionEventArgs.Exception);
		}

		private void CurrentDomainOnUnhandledException(object sender,
			UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			LogUnhandledException(unhandledExceptionEventArgs.ExceptionObject as Exception);
		}

		private void FirstChanceExceptionHandler(object sender,
			FirstChanceExceptionEventArgs firstChanceExceptionEventArgs)
		{
			LogUnhandledException(firstChanceExceptionEventArgs.Exception);
		}

		protected void LogUnhandledException(Exception foundException)
		{
			// Crashes.TrackError(foundException);
			// Exception inner = foundException.InnerException;
			// while (inner != null)
			// {
			// 	Crashes.TrackError(inner);
			// 	inner = inner.InnerException;
			// }
			// #if DEBUG
			// NavigationService.DisplayAlert("Error Logged In App Center", foundException.Message, "Ok");
			// #else
			// NavigationService.DisplayAlert("Service Temporary Unavailable", "Please try again later", "Ok");
			// #endif
		}
	}
}