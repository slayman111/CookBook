using CookBookApp.Command;
using CookBookApp.Core;
using CookBookApp.Models;
using CookBookApp.Models.Entities;
using CookBookApp.View;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CookBookApp.ViewModel
{
    internal class ReceiptsViewModel : BaseViewModel
    {
        private IEnumerable<ReceiptModel> _receipts;
        private bool _onlyUserReceipts;
        private string _searchString;
        private ReceiptModel _selectedReceipt;

        public IEnumerable<ReceiptModel> Receipts { get => _receipts; set => SetProperty(ref _receipts, value); }
        public bool OnlyUserReceipts
        {
            get => _onlyUserReceipts;
            set
            {
                SetProperty(ref _onlyUserReceipts, value);

                if (value) DisplayOnlyUserReceipts();
                else DisplayReceipts();
            }
        }
        public string SearchString { get => _searchString; set => SetProperty(ref _searchString, value); }
        public string Username { get => LogginedUser.User!.GetFullname(); }
        public ReceiptModel SelectedReceipt
        {
            get => _selectedReceipt;
            set
            {
                SetProperty(ref _selectedReceipt, value);

                using var context = new CookBookDbContext();
                ReceiptToDisplay.Receipt = context.Receipts
                    .Include(r => r.Instructions)
                    .Include(r => r.Ingridients)
                    .Include(r => r.Categories)
                    .Include(r => r.Users)
                    .ToList()
                    .FirstOrDefault(r => r.Id.Equals(value.Id));
                OpenInfoWindow();
            }
        }

        public ICommand SignOutCommand { get; private set; }
        public ICommand OpenAddNewReceiptCommand { get; private set; }
        public ICommand FilterCommand { get; private set; }
        public ICommand OpenInfoWindowCommand { get; private set; }

        public ReceiptsViewModel()
        {
            using var context = new CookBookDbContext();

            OnlyUserReceipts = false;

            SignOutCommand = new DelegateCommand(obj =>
            {
                LogginedUser.User = null;
                OpenAuthorizationWindow();
            });

            OpenAddNewReceiptCommand = new DelegateCommand(obj => OpenAddNewReceipt());

            FilterCommand = new DelegateCommand(obj => DoFilter());
        }

        private static void OpenAuthorizationWindow()
        {
            new AuthorizationWindow().Show();
            Close();
        }

        private static void OpenAddNewReceipt()
        {
            new AddNewReceiptWindow().Show();
            Close();
        }

        private static void OpenInfoWindow()
        {
            new ReceiptInfoWindow().Show();
            Close();
        }

        private static void Close()
        {
            Application.Current?.Windows.Cast<Window>()?.FirstOrDefault(window => window is ReceiptsWindow)?.Close();
        }

        private void DisplayOnlyUserReceipts()
        {
            using var context = new CookBookDbContext();

            Receipts = new ObservableCollection<ReceiptModel>(context.Receipts
                .Include(r => r.Instructions)
                .Include(r => r.Ingridients)
                .Include(r => r.Categories)
                .Include(r => r.Users)
                .Where(r => r.Users.FirstOrDefault()!.Id.Equals(LogginedUser.User!.Id))
                .Select(r => (ReceiptModel)r));
        }

        private void DisplayReceipts()
        {
            using var context = new CookBookDbContext();

            Receipts = new ObservableCollection<ReceiptModel>(context.Receipts
                .Include(r => r.Instructions)
                .Include(r => r.Ingridients)
                .Include(r => r.Categories)
                .Include(r => r.Users)
                .Select(r => (ReceiptModel)r));
        }

        private void DoFilter()
        {
            using var context = new CookBookDbContext();

            if (string.IsNullOrEmpty(SearchString))
            {
                DisplayReceipts();
                return;
            }

            var receipts = context.Receipts
                .Include(r => r.Instructions)
                .Include(r => r.Ingridients)
                .Include(r => r.Categories)
                .Include(r => r.Users)
                .AsEnumerable();

            if (OnlyUserReceipts)
                Receipts = new ObservableCollection<ReceiptModel>(receipts
                    .Where(r => (r.Name.Contains(SearchString) || string.Join(" ", r.Ingridients.Select(i => i.Name)).ToLower().Contains(SearchString.ToLower()))
                        && r.Users.FirstOrDefault()!.Id.Equals(LogginedUser.User!.Id))
                    .Select(r => (ReceiptModel)r));
            else
                Receipts = new ObservableCollection<ReceiptModel>(receipts
                    .Where(r => r.Name.Contains(SearchString) || string.Join(" ", r.Ingridients.Select(i => i.Name)).ToLower().Contains(SearchString.ToLower()))
                    .Select(r => (ReceiptModel)r));
        }
    }
}