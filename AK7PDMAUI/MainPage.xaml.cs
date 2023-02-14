using AK7PDMAUI.ViewModels;

namespace AK7PDMAUI;

public partial class MainPage : ContentPage
{

	public MainPageViewModel Vm { get; set; }

	public MainPage(MainPageViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		Vm = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		try
		{
            Vm.RefreshUserBooksCommand.Execute(null);
        }
		catch(Exception)
		{

		}
    }
}

