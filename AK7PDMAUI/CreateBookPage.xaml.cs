using AK7PDMAUI.ViewModels;

namespace AK7PDMAUI;

public partial class CreateBookPage : ContentPage
{
	public CreateBookPage(CreateBookPageViewModel vm)
	{
        BindingContext = vm;
		InitializeComponent();
		
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Button b = (Button)sender;
        Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(50), () =>
        {
            VisualStateManager.GoToState(b, "Normal");
        });
    }
}