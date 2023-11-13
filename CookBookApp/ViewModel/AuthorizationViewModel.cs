using CookBookApp.Command;
using CookBookApp.Core;
using CookBookApp.Models.Entities;
using CookBookApp.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CookBookApp.ViewModel
{
    internal class AuthorizationViewModel : BaseViewModel
    {
        private string? _login;
        private string? _password;

        public string? Login { get => _login; set => SetProperty(ref _login, value); }
        public string? Password { get => _password; set => SetProperty(ref _password, value); }

        public ICommand SignInCommand { get; private set; }
        public ICommand OpenRegistrationCommand { get; private set; }

        public AuthorizationViewModel()
        {
            SignInCommand = new DelegateCommand(async obj =>
            {
                try
                {
                    using var context = new CookBookDbContext();

                    var user = await context.Users.FirstOrDefaultAsync(user => user.Login.Equals(Login) && user.Password.Equals(Password));
                    if (user != null)
                    {
                        LogginedUser.User = user;
                        OpenReceiptsWindow();
                    }
                    else throw new Exception("Неправильный логин или пароль");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
            OpenRegistrationCommand = new DelegateCommand(obj => OpenRegistrationWindow());
        }

        private static void OpenReceiptsWindow()
        {
            new ReceiptsWindow().Show();
            Application.Current?.Windows.Cast<Window>()?.FirstOrDefault(window => window is AuthorizationWindow)?.Close();
        }

        private static void OpenRegistrationWindow()
        {
            new RegistrationWindow().Show();
            Application.Current?.Windows.Cast<Window>()?.FirstOrDefault(window => window is AuthorizationWindow)?.Close();
        }
    }
}
