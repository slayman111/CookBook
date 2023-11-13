using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CookBookApp.Command;
using CookBookApp.Models.Entities;
using CookBookApp.View;

namespace CookBookApp.ViewModel
{
    internal class RegistrationViewModel : BaseViewModel
    {
        private string? _login;
        private string? _password;
        private string? _passwordRepeat;
        private string? _firstName;
        private string? _lastName;

        public string? Login { get => _login; set => SetProperty(ref _login, value); }
        public string? Password { get => _password; set => SetProperty(ref _password, value); }
        public string? PasswordRepeat { get => _passwordRepeat; set => SetProperty(ref _passwordRepeat, value); }
        public string? FirstName { get => _firstName; set => SetProperty(ref _firstName, value); }
        public string? LastName { get => _lastName; set => SetProperty(ref _lastName, value); }

        public ICommand SignUpCommand { get; private set; }
        public ICommand OpenAuthorizationCommand { get; private set; }

        public RegistrationViewModel()
        {
            SignUpCommand = new DelegateCommand(async obj =>
            {
                try
                {
                    if (new string?[] { Login, Password, PasswordRepeat, FirstName, LastName }.Any(s => s is null))
                        throw new Exception("Заполните все поля!");

                    if (!Password!.Equals(_passwordRepeat))
                        throw new Exception("Пароли не совпадают!");

                    using var context = new CookBookDbContext();

                    context.Users.Add(new User(Login!, Password!, FirstName!, LastName!));
                    await context.SaveChangesAsync();

                    MessageBox.Show("Аккаунт успешно зарегистрирован", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    OpenAuthorizationWindow();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            OpenAuthorizationCommand = new DelegateCommand(obj => OpenAuthorizationWindow());
        }

        private static void OpenAuthorizationWindow()
        {
            new AuthorizationWindow().Show();
            Application.Current?.Windows.Cast<Window>()?.FirstOrDefault(window => window is RegistrationWindow)
                ?.Close();
        }
    }
}
