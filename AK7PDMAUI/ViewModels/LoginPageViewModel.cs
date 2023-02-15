using AK7PDMAUI.Models;
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

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

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

        private bool _registrationInfoVisible;
        public bool RegistrationInfoVisible
        {
            get => _registrationInfoVisible;
            set
            {
                SetProperty(ref _registrationInfoVisible, value);
                LoginInfoVisible = !value;
            }
        }

        private bool _loginInfoVisible = true;
        public bool LoginInfoVisible
        {
            get => _loginInfoVisible;
            set => SetProperty(ref _loginInfoVisible, value);
        }

        #endregion



        public LoginPageViewModel(RepositoryService repository)
        {
            Repository = repository;
            LoginCommand = new Command(async () => await Login());
            RegisterCommand = new Command(async () => await Register());
            ChangeHintLabelCommand = new Command(() => ChangeHintLabel());
            RequestRegistrationCommand = new Command(async () => await RequestRegistration());
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
                    await App.Current.MainPage.DisplayAlert(AppResources.Error, "Špatně vyplněné uživ. jméno nebo heslo", AppResources.OK);
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

        private async Task Register()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Username) ||
                    string.IsNullOrWhiteSpace(Password1) ||
                    string.IsNullOrWhiteSpace(Password2))
                {
                    await App.Current.MainPage.DisplayAlert
                        (AppResources.Error, "Je potřeba vyplnit požadované údaje!", AppResources.OK);
                }
                else if (Password1.CompareTo(Password2) != 0)
                {
                    await App.Current.MainPage.DisplayAlert
                        (AppResources.Error, "Hesla se neshodují!", AppResources.OK);
                }
                else if (Password1.Length <= 4)
                {
                    await App.Current.MainPage.DisplayAlert
                        (AppResources.Error, "Heslo musí mít více než 4 znaky", AppResources.OK);
                }
                else if (await Repository.CheckIfUserExistsAsync(Username))
                {
                    await App.Current.MainPage.DisplayAlert
                       (AppResources.Error, "Uživatel již existuje!", AppResources.OK);
                }
                else
                {
                    RegistrationInfoVisible = true;
                }
            }
            catch (Exception)
            {

            }
        }

        private async Task RequestRegistration()
        {
            try
            {
                User newUser = new()
                {
                    Login = Username,
                    Password = Password1,
                    FirstName = FirstName,
                    LastName = LastName,
                    Address = new()
                    {
                        Street = Street,
                        City = City,
                        ZipCode = ZipCode,
                    },
                    AccountState = "approved"
                };
                await Repository.RegisterAsync(newUser);
                await App.Current.MainPage.DisplayAlert
                        ("Oznámení", "Registrace proběhla v pořádku.", AppResources.OK);
                await Login();
            }
            catch (NullReferenceException ex)
            {
                await App.Current.MainPage.DisplayAlert
                        (AppResources.Error, "Je potřeba vyplnit požadované údaje!", AppResources.OK);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert
                        (AppResources.Error, ex.Message, AppResources.OK);
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand RequestRegistrationCommand { get; set; }
        public ICommand ChangeHintLabelCommand { get; set; }
    }

}

