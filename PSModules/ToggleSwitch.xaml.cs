﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ServiceLibrary;

namespace PSModules;

/// <summary>
///     Логика взаимодействия для ToggleSwitch.xaml
/// </summary>
public partial class ToggleSwitch : IReturnValue
{
    private bool _state;

    public ToggleSwitch()
    {
        InitializeComponent();
    }

    public ToggleSwitch(string title, string value = "false")
    {
        InitializeComponent();
        Title = title;
        if (value == "")
            value = "false";
        Value = value;
    }

    public string Title
    {
        get => (string)ElTitle.Content;
        set => ElTitle.Content = value;
    }

    public string Value
    {
        get => _state.ToString();
        set => Change_st(_state = bool.Parse(value));
    }

    private void Circle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _state = !_state;
        Change_st(_state);
    }

    public void Change_st(bool st)
    {
        if (st)
        {
            Circle.Margin = new Thickness(ToggleUserControl.ActualWidth - ToggleUserControl.ActualHeight, 0, 0, 0);
            Rect.Fill = new SolidColorBrush(Colors.Blue);
        }
        else
        {
            Circle.Margin = new Thickness(0, 0, 0, 0);
            Rect.Fill = new SolidColorBrush(Colors.Gray);
        }
    }
}