using CookBookApp.Command;
using CookBookApp.Core;
using CookBookApp.Models.Entities;
using CookBookApp.View;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CookBookApp.ViewModel
{
    internal class ReceiptInfoViewModel : BaseViewModel
    {
        public Receipt Receipt { get; private set; }
        public string Author { get; private set; }
        public string Instruction { get; private set; }
        public string Categories { get; private set; }

        public ICommand OpenReceiptsCommand { get; private set; }

        public ReceiptInfoViewModel()
        {
            Receipt = ReceiptToDisplay.Receipt!;

            Author = Receipt.Users.FirstOrDefault()!.GetFullname();

            OpenReceiptsCommand = new DelegateCommand(obj =>
            {
                OpenReceiptsWindow();
                ReceiptToDisplay.Receipt = null;
            });

            int counter = 0;
            foreach (var instruction in Receipt.Instructions)
                Instruction += $"{++counter}. {instruction.Name}\n\n";

            Categories = string.Join(" • ", Receipt.Categories.Select(c => c.Name));
        }

        private static void OpenReceiptsWindow()
        {
            new ReceiptsWindow().Show();
            Application.Current?.Windows.Cast<Window>()?.FirstOrDefault(window => window is ReceiptInfoWindow)?.Close();
        }
    }
}