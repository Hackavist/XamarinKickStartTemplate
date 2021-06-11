using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TemplateFoundation.ExtensionMethods;
using TemplateFoundation.IOCFoundation;
using TemplateFoundation.Navigation.Implementations;
using TemplateFoundation.Navigation.Interfaces;
using TemplateFoundation.ViewModelFoundation;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace TemplateFoundation.Navigation.NavigationContainers
{
	public class FlyoutNavigationContainer : FlyoutPage, INavigationService
	{
		public void Init(string menuTitle, string menuIcon = null)
		{
			CreateMenuPage(menuTitle, menuIcon);
			RegisterNavigation();
		}

		public void Init<T>(string masterListName) where T : BaseViewModel
		{
			CreateMenuPage<T>(masterListName);
			RegisterNavigation();
		}

		private void RegisterNavigation()
		{
			Ioc.Container.Register<INavigationService>(this, NavigationServiceName);
		}

		internal Page CreateContainerPageSafe(Page page)
		{
			if (page is NavigationPage || page is FlyoutPage || page is TabbedPage)
				return page;

			return CreateContainerPage(page);
		}

		private Page CreateContainerPage(Page page)
		{
			return new NavigationPage(page);
		}

		private void CreateMenuPage(string menuPageTitle, string menuIcon = null)
		{
			this.MenuPage = new ContentPage {Title = menuPageTitle};
			this.collectionView.SelectionMode = SelectionMode.Single;
			this.collectionView.ItemsSource = PageNames;
			this.collectionView.SelectionChanged += (sender, args) =>
			{
				if (Pages.ContainsKey((string) args.CurrentSelection[0]))
					Detail = Pages[(string) args.CurrentSelection[0]];
				IsPresented = false;
			};
			this.collectionView.ItemTemplate = new DataTemplate(() =>
			{
				Label nameLabel = new Label
				{
					Margin = new Thickness(10),
					VerticalTextAlignment = TextAlignment.Center,
					BackgroundColor = Color.Red
				};
				nameLabel.SetBinding(Label.TextProperty, ".");
				return nameLabel;
			});
			this.MenuPage.Content = this.collectionView;

			NavigationPage navPage = new NavigationPage(this.MenuPage) {Title = menuPageTitle};

			if (!string.IsNullOrEmpty(menuIcon))
				navPage.IconImageSource = menuIcon;

			Flyout = navPage;
		}

		public void CreateMenuPage<T>(string masterListName) where T : BaseViewModel
		{
			Page flyoutPage = ViewModelResolver.ResolveViewModel<T>();
			object pagesList = flyoutPage.FindByName(masterListName);
			if (pagesList is CollectionView list)
			{
				list.SelectionChanged += (sender, args) =>
				{
					if (Pages.ContainsKey(((MenuItems) args.CurrentSelection[0]).Title))
						Detail = Pages[((MenuItems) args.CurrentSelection[0]).Title];
					IsPresented = false;
				};
				list.ItemTemplate = new DataTemplate(() =>
				{
					Label nameLabel = new Label
					{
						Margin = new Thickness(10),
						VerticalOptions = LayoutOptions.CenterAndExpand,
						VerticalTextAlignment = TextAlignment.Center,
						BackgroundColor = Color.Red
					};
					nameLabel.SetBinding(Label.TextProperty, ".");
					return nameLabel;
				});
			}
			else
			{
				throw new Exception("Master list navigation name not the same as xaml");
			}

			Flyout = flyoutPage;
		}

		public void CreateMenuPage<T>() where T : BaseViewModel
		{
			Flyout = ViewModelResolver.ResolveViewModel<T>();
		}

		private void AddPagesToDictionary(Page page, string pageName = null)
		{
			BaseViewModel viewModel = page.GetModel();
			viewModel.CurrentNavigationServiceName = NavigationServiceName;
			this.innerPages.Add(page);
			Page navigationContainer = CreateContainerPage(page);
			if (!string.IsNullOrEmpty(pageName))
			{
				Pages.Add(pageName, navigationContainer);
				PageNames.Add(pageName);
				if (Pages.Count == 1)
					Detail = navigationContainer;
			}
			else
			{
				if (string.IsNullOrEmpty(viewModel.Title))
					throw new Exception("no Title found for " + viewModel.GetType().Name);
				Pages.Add(viewModel.Title, navigationContainer);
				PageNames.Add(viewModel.Title);
				if (Pages.Count == 1)
					Detail = navigationContainer;
			}
		}

		public void AddPage<T>(object data = null, string pageName = null) where T : BaseViewModel
		{
			Page page = ViewModelResolver.ResolveViewModel<T>(data);
			AddPagesToDictionary(page, pageName);
		}

		public void AddPage(string modelName, object data = null, string pageName = null)
		{
			Type viewModelType = Type.GetType(modelName);
			Page page = ViewModelResolver.ResolveViewModel(viewModelType, data);
			AddPagesToDictionary(page, pageName);
		}

		#region Fields & Constants

		public Dictionary<string, Page> Pages { get; } = new Dictionary<string, Page>();
		private ObservableCollection<string> PageNames { get; } = new ObservableCollection<string>();
		public string NavigationServiceName { get; }
		private readonly CollectionView collectionView = new CollectionView();
		private readonly List<Page> innerPages = new List<Page>();
		public ContentPage MenuPage;

		#endregion

		#region Constructors

		public FlyoutNavigationContainer() : this(NavigationConstants.DefaultNavigationServiceName)
		{
		}

		public FlyoutNavigationContainer(string navigationServiceName)
		{
			NavigationServiceName = navigationServiceName;
			RegisterNavigation();
		}

		#endregion

		#region Interface Implementations

		public Task PushPage(Page page, BaseViewModel model, bool modal = false, bool animate = true)
		{
			return modal
				? Navigation.PushModalAsync(CreateContainerPageSafe(page), animate)
				: (Detail as NavigationPage)?.PushAsync(page, animate);
		}

		public Task PopPage(bool modal = false, bool animate = true)
		{
			return modal ? Navigation.PopModalAsync(animate) : (Detail as NavigationPage)?.PopAsync(animate);
		}

		public Task PopToRoot(bool animate = true)
		{
			return (Detail as NavigationPage)?.PopToRootAsync(animate);
		}

		public void NotifyChildrenPageWasPopped()
		{
			(Flyout as NavigationPage)?.NotifyAllChildrenPopped();
			(Flyout as INavigationService)?.NotifyChildrenPageWasPopped();

			foreach (Page page in Pages.Values)
			{
				(page as NavigationPage)?.NotifyAllChildrenPopped();
				(page as INavigationService)?.NotifyChildrenPageWasPopped();
			}

			if (Pages != null && !Pages.ContainsValue(Detail))
				(Detail as NavigationPage)?.NotifyAllChildrenPopped();
			if (Pages != null && !Pages.ContainsValue(Detail))
				(Detail as INavigationService)?.NotifyChildrenPageWasPopped();
		}

		public Task<BaseViewModel> SwitchSelectedRootPageModel<T>() where T : BaseViewModel
		{
			int tabIndex = this.innerPages.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

			this.collectionView.SelectedItem = PageNames[tabIndex];

			return Task.FromResult((Detail as NavigationPage)?.CurrentPage.GetModel());
		}

		#endregion
	}

	public class MenuItems : INotifyPropertyChanged
	{
		/// <summary>
		///     WeakEvent handler for the INotifyPropertyChanged Event
		/// </summary>
		private readonly DelegateWeakEventManager propertyChangedWeakEventHandler = new DelegateWeakEventManager();

		public string Title { get; set; }
		public string Image { get; set; }
		public bool IsSelected { get; set; }

		public event PropertyChangedEventHandler PropertyChanged
		{
			add => this.propertyChangedWeakEventHandler.AddEventHandler(value);
			remove => this.propertyChangedWeakEventHandler.RemoveEventHandler(value);
		}


		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null!)
		{
			this.propertyChangedWeakEventHandler.RaiseEvent(this, new PropertyChangedEventArgs(propertyName),
				nameof(PropertyChanged));
		}
	}
}