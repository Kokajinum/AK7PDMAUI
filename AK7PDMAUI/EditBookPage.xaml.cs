using AK7PDMAUI.ViewModels;

namespace AK7PDMAUI;

public partial class EditBookPage : ContentPage
{
	public EditBookPageViewModel Vm { get; set; }
	
	public EditBookPage(EditBookPageViewModel vm)
	{
		BindingContext = vm;
		Vm = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		Vm.RefreshEditedBookCommand.Execute(null);
    }
}