using BaseTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TemplateFoundation.ViewModelFoundation;

namespace TemplateFoundationUnitTest.Navigation.Implementations
{
	public class ViewModelResolverData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			// get all type contains viewmodel postfix
			IEnumerable<Type> viewModelAssemblyTypes = Assembly.Load(typeof(App).Assembly.GetName()).ExportedTypes.Where(a => a.Name.ToLower().EndsWith("viewmodel"));
			// filter types to get only classes
			IEnumerable<Type> viewModelImplementations = viewModelAssemblyTypes.Where(a => a.IsClass);
			foreach (Type type in viewModelImplementations)
			{
				BaseViewModel viewModel = (BaseViewModel)Activator.CreateInstance(type);
				yield return new object[] { type, "Test data", viewModel };
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public class ViewModelResolverWithoutViewModelData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			// get all type contains viewmodel postfix
			IEnumerable<Type> viewModelAssemblyTypes = Assembly.Load(typeof(App).Assembly.GetName()).ExportedTypes.Where(a => a.Name.ToLower().EndsWith("viewmodel"));
			// filter types to get only classes
			IEnumerable<Type> viewModelImplementations = viewModelAssemblyTypes.Where(a => a.IsClass);
			foreach (Type type in viewModelImplementations)
			{
				yield return new object[] { type, "Test data" };
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
	public class ViewModelResolverWithoutTypeData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			// get all type contains viewmodel postfix
			IEnumerable<Type> viewModelAssemblyTypes = Assembly.Load(typeof(App).Assembly.GetName()).ExportedTypes.Where(a => a.Name.ToLower().EndsWith("viewmodel"));
			// filter types to get only classes
			IEnumerable<Type> viewModelImplementations = viewModelAssemblyTypes.Where(a => a.IsClass);
			foreach (Type type in viewModelImplementations)
			{
				BaseViewModel viewModel = (BaseViewModel)Activator.CreateInstance(type);
				yield return new object[] { "Test data", viewModel };
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public class ViewModelResolverGenericWithoutDataData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			// get all type contains viewmodel postfix
			IEnumerable<Type> viewModelAssemblyTypes = Assembly.Load(typeof(App).Assembly.GetName()).ExportedTypes.Where(a => a.Name.ToLower().EndsWith("viewmodel"));
			// filter types to get only classes
			IEnumerable<Type> viewModelImplementations = viewModelAssemblyTypes.Where(a => a.IsClass);
			foreach (Type type in viewModelImplementations)
			{
				yield return new object[] { type };
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public class ViewModelResolverGenericData : IEnumerable<object[]>
	{
		public IEnumerator<object[]> GetEnumerator()
		{
			// get all type contains viewmodel postfix
			IEnumerable<Type> viewModelAssemblyTypes = Assembly.Load(typeof(App).Assembly.GetName()).ExportedTypes.Where(a => a.Name.ToLower().EndsWith("viewmodel"));
			// filter types to get only classes
			IEnumerable<Type> viewModelImplementations = viewModelAssemblyTypes.Where(a => a.IsClass);
			foreach (Type type in viewModelImplementations)
			{
				yield return new object[] { type, "Test data" };
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
