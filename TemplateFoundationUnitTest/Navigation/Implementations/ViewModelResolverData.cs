using BaseTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TemplateFoundation.ViewModelFoundation;

namespace TemplateFoundationUnitTest.Navigation.Implementations
{
    public class ViewModelResolverTestBase : IEnumerable<object[]>
    {
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public IEnumerator<object[]> GetEnumerator()
		{
			// get all type contains viewmodel postfix where type is class
			IEnumerable<Type> viewModelImplementations = Assembly.Load(typeof(App).Assembly.GetName()).ExportedTypes.Where(a => a.Name.ToLower().EndsWith("viewmodel")).Where(a => a.IsClass);
            foreach (Type type in viewModelImplementations)
            {
				yield return GetData(type);
			}
        }

		public virtual object[] GetData(Type type)
        {
			BaseViewModel viewModel = (BaseViewModel)Activator.CreateInstance(type);
			return new object[] { type, "Test data", viewModel };
		}
	}

	public class ViewModelResolverWithoutViewModelData : ViewModelResolverTestBase, IEnumerable<object[]>
	{
		public override object[] GetData(Type type)
		{
			return new object[] { type, "Test data" };
		}
	}

	public class ViewModelResolverWithoutTypeData : ViewModelResolverTestBase, IEnumerable<object[]>
	{
		public override object[] GetData(Type type)
		{
			BaseViewModel viewModel = (BaseViewModel)Activator.CreateInstance(type);
			return new object[] { "Test data", viewModel };
		}
	}

	public class ViewModelResolverGenericWithoutDataData : ViewModelResolverTestBase, IEnumerable<object[]>
	{
		public override object[] GetData(Type type)
		{
			return new object[] { type };
		}
	}

	public class ViewModelResolverGenericData : ViewModelResolverTestBase, IEnumerable<object[]>
	{
		public override object[] GetData(Type type)
		{
			return new object[] { type, "Test data" };
		}
	}
}
