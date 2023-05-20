using System.Windows;
using System.Windows.Controls;
using ServiceLibrary;

namespace PSModules;

/// <summary>
///     Логика взаимодействия для CounterElement.xaml
/// </summary>
public partial class CounterElement : IReturnValue
{
    private double btnFontSize = 6; // процент от высоты окна
    private double labelFontSize = 12;
    public int value;

    public CounterElement()
    {
        InitializeComponent();
    }

    public CounterElement(string _Title, string _Value = "0")
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
        get => value.ToString();
        set => this.value = int.Parse(value);
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
        value--;
        Update_text();
    }

    private void Reset_btn_Click(object sender, RoutedEventArgs e)
    {
        value = 0;
        Update_text();
    }

    private void Plus_btn_Click(object sender, RoutedEventArgs e)
    {
        value++;
        Update_text();
    }

    private void Update_text()
    {
        Value_label.Content = value.ToString();
    }

    public int GetValue()
    {
        return value;
    }

    public void SetValue(int _value)
    {
        value = _value;
    }
}