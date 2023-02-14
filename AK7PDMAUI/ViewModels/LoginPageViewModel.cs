using AK7PDMAUI.Resources.Resx;
using AK7PDMAUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AK7PDMAUI.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {

        #region properties
        public RepositoryService Repository { get; set; }

        public string Username { get; set; } = string.Empty;
        public string Password1 { get; set; } = string.Empty;
        public string Password2 { get; set; } = string.Empty;

        private string _hint = "Nemáte účet?";
        public string Hint
        {
            get => _hint;
            set => SetProperty(ref _hint, value);
        }

        private bool _registrationControlsVisible;
        public bool RegistrationControlsVisible
        {
            get => _registrationControlsVisible;
            set => SetProperty(ref _registrationControlsVisible, value);
        }

        private bool _loginControlsVisible = true;
        public bool LoginControlsVisible
        {
            get => _loginControlsVisible;
            set => SetProperty(ref _loginControlsVisible, value);
        }

        private int _windowHeight;
        public int WindowHeight
        {
            get => _windowHeight;
            set => SetProperty(ref _windowHeight, value);
        }

        private int _windowWidth;
        public int WindowWidth
        {
            get => _windowWidth;
            set => SetProperty(ref _windowWidth, value);
        }

        #endregion



        public LoginPageViewModel(RepositoryService repository)
        {
            Repository = repository;
            LoginCommand = new Command(async () => await Login());
            ChangeHintLabelCommand = new Command(() => ChangeHintLabel());
        }

        private void ChangeHintLabel()
        {
            if (LoginControlsVisible)
            {
                LoginControlsVisible = false;
                RegistrationControlsVisible = true;
                Hint = "Máte účet?";
            }
            else
            {
                LoginControlsVisible = true;
                RegistrationControlsVisible = false;
                Hint = "Nemáte účet?";
            }

        }

        private async Task Login()
        {
            try
            {
                IsBusy = true;
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password1) ||
                    Username.Length == 0 || Password1.Length <= 4)
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.WrongUsernamePassword, AppResources.OK);
                    return;
                }
                else
                {
                    await Repository.LoginAsync(Username, Password1);
                    Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
                    await Shell.Current.GoToAsync("///MainPage");
                    MauiProgram.ChangeWindowSize(1200, 800);
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.WrongUsernamePassword, AppResources.OK);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand ChangeHintLabelCommand { get; set; }
    }

}

