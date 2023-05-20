using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ServiceLibrary;

namespace CHRSModules;

/// <summary>
///     Логика взаимодействия для BoolStatiscic.xaml
/// </summary>
public partial class BoolStatiscic : IReturnValue
{
    private bool _st;

    public BoolStatiscic()
    {
        InitializeComponent();
        ElTitle.Content = "";
        Status.IsEnabled = false;
    }

    public BoolStatiscic(string title = "", string value = "0")
    {
        InitializeComponent();
        ElTitle.Content = title;
        Value = value;
        Title = title;
    }

    public string Title
    {
        get => (string)ElTitle.Content;
        set => ElTitle.Content = value;
    }

    public string Value
    {
        get => _st.ToString();
        set => SetStatus(value);
    }

    public void SetStatus(string status)
    {
        if (status == "") status = "false";
        _st = bool.Parse(status);
        Status.Background = new SolidColorBrush(
            _st ? Colors.Green : Colors.Red
        );
        Status.Content = _st ? "True" : "False";
    }

    public void ChangeSt()
    {
        _st = !_st;
        Status.Background = new SolidColorBrush(
            _st ? Colors.Green : Colors.Red
        );
        Status.Content = _st ? "True" : "False";
    }

    private void Status_Click(object sender, RoutedEventArgs e)
    {
        ChangeSt();
    }
}