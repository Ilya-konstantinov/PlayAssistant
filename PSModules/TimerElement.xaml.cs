using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ServiceLibrary;

namespace PSModules;

/// <summary>
///     Логика взаимодействия для TimerElement.xaml
/// </summary>
public partial class TimerElement : IReturnValue
{
    private readonly DispatcherTimer _dispatcherTimer = new();
    private readonly Stopwatch _timer = new();

    private double _btnFontSize = 6; // процент от высоты окна
    private double _labelFontSize = 12;
    private DateTime _startTime;
    private bool _status;
    private int _tempTime;
    private int _time = 5;

    public TimerElement()
    {
        InitializeComponent();

        _tempTime = _time;

        _dispatcherTimer.Tick += Timer_tick;
        _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
    }

    public TimerElement(string title, string value)
    {
        InitializeComponent();
        Title = title;
        if (value == "")
            value = "5";
        Value = value;

        _dispatcherTimer.Tick += Timer_tick;
        _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
    }

    public string Title
    {
        get => (string)ElTitle.Content;
        set => ElTitle.Content = value;
    }

    public string Value
    {
        get => _tempTime.ToString();
        set => _tempTime = int.Parse(value);
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

    private void Timer_tick(object sender, EventArgs e)
    {
        _tempTime -= 1;
        if (_tempTime <= 0)
        {
            _tempTime = _time;
            _timer.Stop();
            _dispatcherTimer.Stop();
            _status = false;
            StartBtn.Content = "Start";
        }

        Update_text(_tempTime);
    }

    private void Update_text(int setTime)
    {
        if (setTime < 0) setTime = 0;
        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        {
            SecondsTextbox.Text = (setTime % 60).ToString();
            MinutesTextbox.Text = Convert.ToInt32(setTime % 3600 / 60).ToString();
            HoursTextbox.Text = Convert.ToInt32(setTime / 3600).ToString();
        }));
    }

    private void Input_time(object sender, TextChangedEventArgs e)
    {
        if (SecondsTextbox != null && !_status)
        {
            _time = 0;
            try
            {
                _time += Convert.ToInt32(SecondsTextbox.Text);
                _time += Convert.ToInt32(MinutesTextbox.Text) * 60;
                _time += Convert.ToInt32(HoursTextbox.Text) * 3600;
            }
            catch
            {
                ErrorTextblock.Text = "Invalid input!!!";
            }

            Update_text(_time);
        }
    }

    private void HPlus_btn_Click(object sender, RoutedEventArgs e)
    {
        if (!_status)
        {
            _time += 3600;
            Update_text(_time);
            _tempTime = _time;
        }
    }

    private void MPlus_btn_Click(object sender, RoutedEventArgs e)
    {
        if (!_status)
        {
            _time += 60;
            Update_text(_time);
            _tempTime = _time;
        }
    }

    private void SPlus_btn_Click(object sender, RoutedEventArgs e)
    {
        if (!_status)
        {
            _time += 1;
            Update_text(_time);
            _tempTime = _time;
        }
    }

    private void HMinus_btn_Click(object sender, RoutedEventArgs e)
    {
        if (!_status)
        {
            _time -= 3600;
            if (_time < 0) _time = 0;
            Update_text(_time);
            _tempTime = _time;
        }
    }

    private void MMinus_btn_Click(object sender, RoutedEventArgs e)
    {
        if (!_status)
        {
            _time -= 60;
            if (_time < 0) _time = 0;
            Update_text(_time);
            _tempTime = _time;
        }
    }

    private void SMinus_btn_Click(object sender, RoutedEventArgs e)
    {
        if (!_status)
        {
            _time -= 1;
            if (_time < 0) _time = 0;
            Update_text(_time);
            _tempTime = _time;
        }
    }

    private void Start_btn_Click(object sender, RoutedEventArgs e)
    {
        if (!_status)
        {
            _status = true;
            StartBtn.Content = "Stop";
            _startTime = DateTime.Now;
            _timer.Start();
            _dispatcherTimer.Start();
        }
        else
        {
            _status = false;
            StartBtn.Content = "Start";
            _tempTime = _time - Convert.ToInt32(Math.Round((DateTime.Now - _startTime).TotalSeconds));
            _timer.Stop();
            _dispatcherTimer.Stop();
        }
    }

    private void Reset_btn_Click(object sender, RoutedEventArgs e)
    {
        _status = false;
        StartBtn.Content = "Start";
        _tempTime = _time;
        Update_text(_tempTime);
        _timer.Stop();
        _dispatcherTimer.Stop();
    }
}