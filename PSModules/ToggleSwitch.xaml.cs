using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ServiceLibrary;

namespace PSModules;

/// <summary>
///     Логика взаимодействия для ToggleSwitch.xaml
/// </summary>
public partial class ToggleSwitch : IReturnValue
{
    private bool state;

    public ToggleSwitch()
    {
        InitializeComponent();
    }

    public ToggleSwitch(string _Title, string _Value = "0")
    {
        InitializeComponent();
        Title = _Title;
        if (_Value == "")
            _Value = "0";
        Value = _Value;
    }

    public string Title
    {
        get => (string)ElTitle.Content;
        set => ElTitle.Content = value;
    }

    public string Value
    {
        get => state.ToString();
        set => bool.Parse(value);
    }

    private void Circle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        state = !state;

        if (state)
        {
            Circle.Margin = new Thickness(ToggleUserControl.ActualWidth - ToggleUserControl.ActualHeight, 0, 0, 0);
            Rect.Fill = new SolidColorBrush(Colors.Blue);
        }
        else
        {
            Circle.Margin = new Thickness(0, 0, 0, 0);
            Rect.Fill = new SolidColorBrush(Colors.Gray);
        }
    }
}