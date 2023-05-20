using System;
using System.Windows;
using System.Windows.Controls;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для TestGamePage.xaml
/// </summary>
public partial class TestGamePage : Page
{
    private readonly DateTime _startTime;

    public TestGamePage()
    {
        InitializeComponent();

        _startTime = DateTime.Now;
        Back_btn.Content = _startTime.ToString();
    }

    public event EventHandler PageCloseClick;

    private void PageClose(EventArgs e)
    {
        if (PageCloseClick != null) PageCloseClick(this, e);
    }

    private void Back_btn_Click(object sender, RoutedEventArgs e)
    {
        PageClose(e);
    }
}