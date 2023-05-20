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
    private bool st;

    public BoolStatiscic()
    {
        InitializeComponent();
        ElTitle.Content = "";
        Status.IsEnabled = false;
    }

    public BoolStatiscic(string _Title = "", string _Value = "0")
    {
        InitializeComponent();
        ElTitle.Content = _Title;
        Value = _Value;
        Title = _Title;
    }

    public string Title
    {
        get => (string)ElTitle.Content;
        set => ElTitle.Content = value;
    }

    public string Value
    {
        get => st.ToString();
        set => SetStatus(value);
    }

    public void SetStatus(string status)
    {
        if (status == "") status = "false";
        st = bool.Parse(status);
        Status.Background = new SolidColorBrush(
            st ? Colors.Green : Colors.Red
        );
        Status.Content = st ? "True" : "False";
    }

    public void ChangeSt()
    {
        st = !st;
        Status.Background = new SolidColorBrush(
            st ? Colors.Green : Colors.Red
        );
        Status.Content = st ? "True" : "False";
    }

    private void Status_Click(object sender, RoutedEventArgs e)
    {
        ChangeSt();
    }
}