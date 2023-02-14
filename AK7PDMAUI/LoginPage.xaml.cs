using AK7PDMAUI.Resources.Resx;
using AK7PDMAUI.ViewModels;

namespace AK7PDMAUI;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class LoginPage : ContentPage
{

    public LoginPageViewModel Vm { get; set; }

    public LoginPage(LoginPageViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
        Vm = vm;
    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        ActivityIndicator.IsRunning = true;
        Button b = (Button)sender;
        Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(50), () =>
        {
            VisualStateManager.GoToState(b, "Normal");
        });

        //await Shell.Current.GoToAsync("///MainPage");

//        try
//        {
//            if (string.IsNullOrWhiteSpace(EmailLabel.Text) || string.IsNullOrWhiteSpace(PasswordLabel1.Text) ||
//                EmailLabel.Text.Length == 0 || PasswordLabel1.Text.Length <= 4)
//            {
//                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.WrongUsernamePassword, AppResources.OK);
//            }
//            else
//            {
//                await Vm.LoginAsync(EmailLabel.Text, PasswordLabel1.Text);
//#if WINDOWS
//                Window.Height = 800;
//                Window.Width = 1200;
//#endif
//                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
//                await Shell.Current.GoToAsync("///MainPage");

//            }

//        }
//        catch (Exception ex)
//        {
//            await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.WrongUsernamePassword, AppResources.OK);
//        }
//        finally
//        {
//            ActivityIndicator.IsRunning = false;
//        }
    }

    private void RegisterButton_Clicked(object sender, EventArgs e)
    {
        Button b = (Button)sender;
        Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(50), () =>
        {
            VisualStateManager.GoToState(b, "Normal");
        });
    }

    private void NewAccountLabel_Tapped(object sender, TappedEventArgs e)
    {
        if (PasswordLabel2.IsVisible)
        {
            PasswordLabel2.IsVisible = false;
            RegisterButton.IsVisible = false;
            LoginButton.IsVisible = true;
            HintLabel.Text = "Nemáte úèet?";
        }
        else
        {
            PasswordLabel2.IsVisible = true;
            RegisterButton.IsVisible = true;
            LoginButton.IsVisible = false;
            HintLabel.Text = "Máte úèet?";
        }
    }
}