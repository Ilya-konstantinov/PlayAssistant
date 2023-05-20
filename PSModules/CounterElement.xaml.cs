using System.Windows;
using ServiceLibrary;

namespace PSModules;

/// <summary>
///     Логика взаимодействия для CounterElement.xaml
/// </summary>
public partial class CounterElement : IReturnValue
{
    private double _btnFontSize = 6; // процент от высоты окна
    private double _labelFontSize = 12;
    private int _value;

    public CounterElement()
    {
        InitializeComponent();
    }

    public CounterElement(string title, string value = "0")
    {
        InitializeComponent();
        Title = title;
        if (value == "" || value == null)
            value = "0";
        Value = value;
    }

    public string Title
    {
        get => (string)ElTitle.Content;
        set => ElTitle.Content = value;
    }

    public string Value
    {
        get => _value.ToString();
        set
        {
            _value = int.Parse(value);
            Update_text();
        }
    }

    private void Element_Loaded(object sender, RoutedEventArgs e)
    {
        /*
                    double labelfontsize = Application.Current.MainWindow.Height * (labelFontSize / 100);
                    double btnfontsize = App.Current.MainWindow.Height * (btnFontSize / 100);
                    System.Windows.Application.Current.Resources.Remove("LabelFontSize");
                    System.Windows.Application.Current.Resources.Add("LabelFontSize", labelfontsize);
                    System.Windows.Application.Current.Resources.Remove("BtnFontSize");
                    System.Windows.Application.Current.Resources.Add("BtnFontSize", btnfontsize);*/
    }

    private void Element_Resized(object sender, SizeChangedEventArgs e)
    {
        /*
                    double labelfontsize = Application.Current.MainWindow.Height * (labelFontSize / 100);
                    double btnfontsize = App.Current.MainWindow.Height * (btnFontSize / 100);
                    System.Windows.Application.Current.Resources.Remove("LabelFontSize");
                    System.Windows.Application.Current.Resources.Add("LabelFontSize", labelfontsize);
                    System.Windows.Application.Current.Resources.Remove("BtnFontSize");
                    System.Windows.Application.Current.Resources.Add("BtnFontSize", btnfontsize);*/
    }

    private void Minus_btn_Click(object sender, RoutedEventArgs e)
    {
        _value--;
        Update_text();
    }

    private void Reset_btn_Click(object sender, RoutedEventArgs e)
    {
        _value = 0;
        Update_text();
    }

    private void Plus_btn_Click(object sender, RoutedEventArgs e)
    {
        _value++;
        Update_text();
    }

    private void Update_text()
    {
        ValueLabel.Content = _value.ToString();
    }
}