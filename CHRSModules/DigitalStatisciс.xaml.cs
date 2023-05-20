using System.Windows;
using System.Windows.Controls;
using ServiceLibrary;

namespace CHRSModules;

/// <summary>
///     Логика взаимодействия для DigitalStatiscic.xaml
/// </summary>
public partial class DigitalStatiscic : IReturnValue
{
    public DigitalStatiscic()
    {
        InitializeComponent();
        ElTitle.Content = "";
        ElValue.Text = "0";
        ElValue.IsEnabled = false;
        UpBtn.IsEnabled = false;
        DownBtn.IsEnabled = false;
    }

    public DigitalStatiscic(string _Title, string _Value)
    {
        InitializeComponent();
        ElTitle.Content = _Title;
        ElValue.Text = _Value;
    }


    public string Value
    {
        get => ElValue.Text;
        set => ElValue.Text = value;
    }

    public string Title
    {
        get => (string)ElTitle.Content;
        set => ElTitle.Content = value;
    }

    private void Value_TextChanged(object sender, TextChangedEventArgs e)
    {
        var text = ElValue.Text;
        if (ElValue.Text == "") text = "0";
        if (!char.IsDigit(text[text.Length - 1])) ElValue.Text = text.Substring(0, text.Length - 1);
        Value = ElValue.Text;
    }

    private void UpBtn_Click(object sender, RoutedEventArgs e)
    {
        var val = int.Parse(ElValue.Text);
        val++;
        ElValue.Text = val.ToString();
    }

    private void DownBtn_Click(object sender, RoutedEventArgs e)
    {
        var val = int.Parse(ElValue.Text);
        val--;
        ElValue.Text = val.ToString();
    }
}