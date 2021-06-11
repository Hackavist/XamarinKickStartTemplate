using AutoMapper;
using TemplateFoundation.ViewModelFoundation;

namespace BaseTemplate.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public MainViewModel(IMapper mapper) : base(mapper)
		{
			Title = "MainPage";
		}
	}
}