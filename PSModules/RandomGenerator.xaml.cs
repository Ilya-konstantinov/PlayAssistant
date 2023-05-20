using System;
using System.Windows;
using System.Windows.Controls;
using ServiceLibrary;

namespace PSModules;

/// <summary>
///     Логика взаимодействия для Random.xaml
/// </summary>
public partial class RandomGenerator : IReturnValue
{
    private readonly Random rand = new();

    private double btnFontSize = 6; // процент от высоты окна
    private double labelFontSize = 12;
    private int last_value;

    public RandomGenerator()
    {
        InitializeComponent();
    }

    public RandomGenerator(string _Title, string _Value)
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
        get => last_value.ToString();
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
            from = Convert.ToInt32(From_textbox.Text);
            to = Convert.ToInt32(To_textbox.Text);
            last_value = rand.Next(from, to);
            Result_textblock.Text = last_value.ToString();
        }
        catch
        {
            Result_textblock.Text = "Not a number";
        }
    }

    public void Set_Value(string _value)
    {
        Result_textblock.Text = _value;
        last_value = int.Parse(_value);
    }
}