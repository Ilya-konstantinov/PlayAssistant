using System;
using System.Windows;
using ServiceLibrary;

namespace PSModules;

/// <summary>
///     Логика взаимодействия для Random.xaml
/// </summary>
public partial class RandomGenerator : IReturnValue
{
    private readonly Random _rand = new();

    private double _btnFontSize = 6; // процент от высоты окна
    private double _labelFontSize = 12;
    private int _lastValue;

    public RandomGenerator()
    {
        InitializeComponent();
    }

    public RandomGenerator(string title, string value)
    {
        InitializeComponent();
        Title = title;
        if (value == "")
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
        get => _lastValue.ToString();
        set => Set_Value(value);
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

    private void Generate_btn_Click(object sender, RoutedEventArgs e)
    {
        int from, to;

        try
        {
            from = Convert.ToInt32(FromTextbox.Text);
            to = Convert.ToInt32(ToTextbox.Text);
            _lastValue = _rand.Next(from, to);
            ResultTextblock.Text = _lastValue.ToString();
        }
        catch
        {
            ResultTextblock.Text = "Not a number";
        }
    }

    public void Set_Value(string value)
    {
        ResultTextblock.Text = value;
        _lastValue = int.Parse(value);
    }
}