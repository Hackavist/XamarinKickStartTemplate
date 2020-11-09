using System;
using TemplateFoundation.Navigation.Implementations;
using TemplateFoundation.ViewModelFoundation;
using Xamarin.Forms;
using Xunit;

namespace TemplateFoundationUnitTest.Navigation.Implementations
{
	public class ViewModelResolverUnitTest
	{
		[Theory]
		[ClassData(typeof(ViewModelResolverTestBase))]
		public void ResolveViewModelUnitTest(Type type, object data, BaseViewModel baseViewModel)
		{
			Page page = ViewModelResolver.ResolveViewModel(type, data, baseViewModel);
			Assert.NotNull(page);
		}

		[Theory]
		[ClassData(typeof(ViewModelResolverWithoutViewModelData))]
		public void ResolveViewModelWithoutViewModelUnitTest(Type type, object data)
		{
			Page page = ViewModelResolver.ResolveViewModel(type, data);
			Assert.NotNull(page);
		}

		[Theory]
		[ClassData(typeof(ViewModelResolverWithoutTypeData))]
		public void ResolveViewModelWithoutTypeUnitTest(object data, BaseViewModel baseViewModel)
		{
			Page page = ViewModelResolver.ResolveViewModel(data, baseViewModel);
			Assert.NotNull(page);
		}

		[Theory]
		[ClassData(typeof(ViewModelResolverGenericWithoutDataData))]
		public void ResolveViewModelGenericWithoutDataUnitTest(Type type)
		{
			Page page = (Page)typeof(ViewModelResolver).GetMethod(nameof(ViewModelResolver.ResolveViewModel), new Type[0]).MakeGenericMethod(type).Invoke(this, null);
			Assert.NotNull(page);
		}

		[Theory]
		[ClassData(typeof(ViewModelResolverGenericData))]
		public void ResolveViewModelGenericaUnitTest(Type type, object data)
		{
			object[] parameters = new object[] { data };
			Page page = (Page)typeof(ViewModelResolver).GetMethod(nameof(ViewModelResolver.ResolveViewModel), new Type[] { typeof(object) }).MakeGenericMethod(type).Invoke(this, parameters);
			Assert.NotNull(page);
		}
	}
}
