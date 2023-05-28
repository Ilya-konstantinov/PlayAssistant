using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ServiceLibrary;

namespace PSModules;

/// <summary>
///     Логика взаимодействия для dice_d6.xaml
/// </summary>
public partial class DiceD6 : IReturnValue
{
    private const int AnimTime = 500 * 10000;
    private const int AnimStep = 35 * 10000;
    private bool _animationIsActive;
    private long _animationTimestamp;

    private double _btnFontSize = 6; // процент от высоты окна
    private int _currentStep;
    private double _labelFontSize = 6;
    private long _timerStartTime;
    private int _timerTime;
    private int _value;

    public DiceD6(string title, string value)
    {
        InitializeComponent();
        ThrowBtn.IsEnabled = true;
        Title = title;
        if (value == "")
            Value = "0";
        Value = value;
    }

    public DiceD6()
    {
        InitializeComponent();
        ThrowBtn.IsEnabled = true;
    }

    public string Title { get; set; }

    public string Value
    {
        get => _value.ToString();
        set => SetValue(value);
    }

    private void Element_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Element_Resized(object sender, SizeChangedEventArgs e)
    {

    }

    private void throw_btn_Click(object sender, RoutedEventArgs e)
    {
        if (Convert.ToBoolean(AnimationCheckbox.IsChecked) && !_animationIsActive)
        {
            _animationIsActive = true;
            _timerTime = 0;
            _timerStartTime = Stopwatch.GetTimestamp();
            _currentStep = AnimStep;

            _animationTimestamp = Stopwatch.GetTimestamp();

            var animThread = new Thread(throw_animation);
            animThread.IsBackground = true;
            animThread.Start();
        }
        else if (!Convert.ToBoolean(AnimationCheckbox.IsChecked))
        {
            var rand = new Random().Next(1, 7);
            _value = rand;
            DiceImg.Text = rand.ToString();
        }
    }

    private void throw_animation()
    {
        if (Stopwatch.GetTimestamp() - _animationTimestamp >= _currentStep)
        {
            _timerTime += _currentStep;
            _currentStep += AnimStep;

            var rand = new Random().Next(1, 7);

            while (rand == _value) rand = new Random().Next(1, 7);

            _value = rand;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                DiceImg.Text = rand.ToString();
            }));

            _animationTimestamp = Stopwatch.GetTimestamp();
        }

        if (Stopwatch.GetTimestamp() - _timerStartTime < AnimTime)
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
            DiceBorder.BorderBrush = new SolidColorBrush(Colors.Green);
        }));
        Thread.Sleep(TimeSpan.FromMilliseconds(300));
        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
        {
            DiceBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }));
        _animationIsActive = false;
    }

    public void SetValue(string value)
    {
        if (value == "") value = "0";
        var val = int.Parse(value);
        _value = Clamp(val, 1, 6);
        DiceImg.Text = _value.ToString();
    }


    private static int Clamp(int val, int min, int max)
    {
        if (val.CompareTo(min) < 0) return min;
        if (val.CompareTo(max) > 0) return max;
        return val;
    }
}