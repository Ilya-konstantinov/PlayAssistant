using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ServiceLibrary;

namespace PSModules;

/// <summary>
///     Логика взаимодействия для dice_d6.xaml
/// </summary>
public partial class dice_d6 : IReturnValue
{
    private const int anim_time = 500 * 10000;
    private const int anim_step = 35 * 10000;
    private bool animation_is_active;
    private long animation_timestamp;

    private double btnFontSize = 6; // процент от высоты окна
    private int current_step;
    private double labelFontSize = 6;
    private long timer_start_time;
    private int timer_time;
    private int value;

    public dice_d6(string _Title, string _Value)
    {
        InitializeComponent();
        throw_btn.IsEnabled = false;
        Title = _Title;
        if (_Value == "")
            Value = "0";
        Value = _Value;
    }

    public dice_d6()
    {
        InitializeComponent();
        throw_btn.IsEnabled = false;
    }

    public string Title { get; set; }

    public string Value
    {
        get => value.ToString();
        set => SetValue(int.Parse(value));
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

    private void throw_btn_Click(object sender, RoutedEventArgs e)
    {
        if (Convert.ToBoolean(animation_checkbox.IsChecked) && !animation_is_active)
        {
            animation_is_active = true;
            timer_time = 0;
            timer_start_time = Stopwatch.GetTimestamp();
            current_step = anim_step;

            animation_timestamp = Stopwatch.GetTimestamp();

            var anim_thread = new Thread(throw_animation);
            anim_thread.IsBackground = true;
            anim_thread.Start();
        }
        else if (!Convert.ToBoolean(animation_checkbox.IsChecked))
        {
            var rand = new Random().Next(1, 7);
            value = rand;
            dice_img.Source = new BitmapImage(new Uri($"/Images/{rand}.png", UriKind.Relative));
        }
    }

    private void throw_animation()
    {
        if (Stopwatch.GetTimestamp() - animation_timestamp >= current_step)
        {
            timer_time += current_step;
            current_step += anim_step;

            var rand = new Random().Next(1, 7);

            while (rand == value) rand = new Random().Next(1, 7);

            value = rand;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                dice_img.Source = new BitmapImage(new Uri($"/Images/{rand}.png", UriKind.Relative));
            }));

            animation_timestamp = Stopwatch.GetTimestamp();
        }

        if (Stopwatch.GetTimestamp() - timer_start_time < anim_time)
        {
            var tmp = new Thread(throw_animation);
            tmp.IsBackground = true;
            tmp.Start();
        }
        else
        {
            var tmp = new Thread(throw_end_animation);
            tmp.IsBackground = true;
            tmp.Start();
        }
    }

    private void throw_end_animation()
    {
        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
        {
            dice_border.BorderBrush = new SolidColorBrush(Colors.Green);
        }));
        Thread.Sleep(TimeSpan.FromMilliseconds(300));
        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
        {
            dice_border.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }));
        animation_is_active = false;
    }

    public void SetValue(int _value)
    {
        value = Clamp(_value, 1, 6);
        dice_img.Source = new BitmapImage(new Uri($"/Images/{value}.png", UriKind.Relative));
    }


    private static int Clamp(int val, int min, int max)
    {
        if (val.CompareTo(min) < 0) return min;
        if (val.CompareTo(max) > 0) return max;
        return val;
    }
}