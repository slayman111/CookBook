using CookBookApp.Command;
using CookBookApp.Core;
using CookBookApp.Models.Entities;
using CookBookApp.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CookBookApp.ViewModel
{
    internal class AddNewReceiptViewModel : BaseViewModel
    {
        private string _name;
        private string _description;
        private string _caloric;
        private string _squirrels;
        private string _fatness;
        private string _carbohydrates;

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public string Caloric { get => _caloric; set => SetProperty(ref _caloric, value); }
        public string Squirrels { get => _squirrels; set => SetProperty(ref _squirrels, value); }
        public string Fatness { get => _fatness; set => SetProperty(ref _fatness, value); }
        public string Carbohydrates { get => _carbohydrates; set => SetProperty(ref _carbohydrates, value); }

        public ICommand AddReceiptCommand { get; private set; }
        public ICommand OpenReceiptsCommand { get; private set; }

        public AddNewReceiptViewModel()
        {
            AddReceiptCommand = new DelegateCommand(async obj =>
            {
                try
                {
                    using var context = new CookBookDbContext();

                    if (new string?[] { Name, Description, Caloric, Squirrels, Fatness, Carbohydrates }.Any(s => s is null))
                        throw new Exception("Заполните все поля!");

                    context.Receipts.Add(new Receipt(Name, Description, Caloric, Squirrels, Fatness, Carbohydrates)
                    {
                        Users = new HashSet<User>() { context.Users.Find(LogginedUser.User.Id) }
                    });
                    await context.SaveChangesAsync();

                    OpenReceiptsWindow();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            OpenReceiptsCommand = new DelegateCommand(obj => OpenReceiptsWindow());
        }

        private static void OpenReceiptsWindow()
        {
            new ReceiptsWindow().Show();
            Application.Current?.Windows.Cast<Window>()?.FirstOrDefault(window => window is AddNewReceiptWindow)?.Close();
        }
    }
}