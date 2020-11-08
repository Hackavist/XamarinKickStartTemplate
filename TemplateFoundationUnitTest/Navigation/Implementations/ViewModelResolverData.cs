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
		IEnumerator IEnumerable.GetEnumerator()=>return GetEnumerator();
		
		public IEnumerator<object[]> GetEnumerator()
		{
			// get all type contains viewmodel postfix where type is class
			IEnumerable<Type> viewModelImplementations = Assembly.Load(typeof(App).Assembly.GetName()).ExportedTypes.Where(a => a.Name.ToLower().EndsWith("viewmodel")).Where(a => a.IsClass);
			foreach (Type type in viewModelImplementations)
			{
				BaseViewModel viewModel = (BaseViewModel)Activator.CreateInstance(type);
				yield return new object[] { type, "Test data", viewModel };
			}
		}		
	}

	public class ViewModelResolverWithoutViewModelData : IEnumerable<object[]>
	{
		IEnumerator IEnumerable.GetEnumerator()=>return GetEnumerator();
		public IEnumerator<object[]> GetEnumerator()
		{
			// get all type contains viewmodel postfix where type is class
			IEnumerable<Type> viewModelImplementations = Assembly.Load(typeof(App).Assembly.GetName()).ExportedTypes.Where(a => a.Name.ToLower().EndsWith("viewmodel")).Where(a => a.IsClass);
			foreach (Type type in viewModelImplementations)
			{
				yield return new object[] { type, "Test data" };
			}
		}
	}
	public class ViewModelResolverWithoutTypeData : IEnumerable<object[]>
	{
		IEnumerator IEnumerable.GetEnumerator()=>return GetEnumerator();
		public IEnumerator<object[]> GetEnumerator()
		{
			// get all type contains viewmodel postfix where type is class
			IEnumerable<Type> viewModelImplementations = Assembly.Load(typeof(App).Assembly.GetName()).ExportedTypes.Where(a => a.Name.ToLower().EndsWith("viewmodel")).Where(a => a.IsClass);
			foreach (Type type in viewModelImplementations)
			{
				BaseViewModel viewModel = (BaseViewModel)Activator.CreateInstance(type);
				yield return new object[] { "Test data", viewModel };
			}
		}
	}

	public class ViewModelResolverGenericWithoutDataData : IEnumerable<object[]>
	{
		IEnumerator IEnumerable.GetEnumerator()=>return GetEnumerator();
		public IEnumerator<object[]> GetEnumerator()
		{
			// get all type contains viewmodel postfix where type is class
			IEnumerable<Type> viewModelImplementations = Assembly.Load(typeof(App).Assembly.GetName()).ExportedTypes.Where(a => a.Name.ToLower().EndsWith("viewmodel")).Where(a => a.IsClass);
			foreach (Type type in viewModelImplementations)
			{
				yield return new object[] { type };
			}
		}

	}

	public class ViewModelResolverGenericData : IEnumerable<object[]>
	{
		IEnumerator IEnumerable.GetEnumerator()=>return GetEnumerator();
		public IEnumerator<object[]> GetEnumerator()
		{
			// get all type contains viewmodel postfix where type is class
			IEnumerable<Type> viewModelImplementations = Assembly.Load(typeof(App).Assembly.GetName()).ExportedTypes.Where(a => a.Name.ToLower().EndsWith("viewmodel")).Where(a => a.IsClass);
			foreach (Type type in viewModelImplementations)
			{
				yield return new object[] { type, "Test data" };
			}
		}
	}
}
