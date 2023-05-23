using CookBookApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CookBookApp.View
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as RegistrationViewModel).Password = ((PasswordBox)sender).Password;
        }

        private void PasswordRepeatChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as RegistrationViewModel).PasswordRepeat = ((PasswordBox)sender).Password;
        }
    }
}