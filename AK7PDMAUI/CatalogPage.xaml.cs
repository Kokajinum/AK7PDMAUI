using AK7PDMAUI.ViewModels;
using Microsoft.Maui.Controls;

namespace AK7PDMAUI;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CatalogPage : ContentPage
{

    public CatalogPageViewModel Vm { get; set; }

    public CatalogPage(CatalogPageViewModel vm)
    {
        BindingContext = vm;
        Vm = vm;
        InitializeComponent();


    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            Vm.RefreshBooksCommand.Execute(null);
        }
        catch (Exception ex)
        {

        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Button b = (Button)sender;
        Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(50), () =>
        {
            VisualStateManager.GoToState(b, "Normal");
        });
    }

    private void _SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            Vm.RefreshBooksCommand.Execute(null);
        }
        else if (e.NewTextValue.Length >= 4) 
        {
            Vm.PerformSearchCommand.Execute(e.NewTextValue);
        }
    }
}