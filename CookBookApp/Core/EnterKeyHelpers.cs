using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;

namespace CookBookApp.Core
{
    public static class EnterKeyHelpers
    {
        public static ICommand GetEnterKeyCommand(DependencyObject target) => (ICommand)target.GetValue(EnterKeyCommandProperty);

        public static void SetEnterKeyCommand(DependencyObject target, ICommand value) => target.SetValue(EnterKeyCommandProperty, value);

        public static readonly DependencyProperty EnterKeyCommandProperty =
            DependencyProperty.RegisterAttached(
                "EnterKeyCommand",
                typeof(ICommand),
                typeof(EnterKeyHelpers),
                new PropertyMetadata(null, OnEnterKeyCommandChanged));

        private static void OnEnterKeyCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ICommand command = (ICommand)e.NewValue;
            FrameworkElement fe = (FrameworkElement)target;
            Control control = (Control)target;
            control.KeyDown += (s, args) => UpdateSource(control, command);
            control.KeyUp += (s, args) => UpdateSource(control, command);
        }

        private static void UpdateSource(Control control, ICommand command)
        {
            BindingExpression b = control.GetBindingExpression(TextBox.TextProperty);
            b.UpdateSource();
            command.Execute(null);
        }
    }
}