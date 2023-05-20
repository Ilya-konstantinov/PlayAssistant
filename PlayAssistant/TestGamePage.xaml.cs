using System;
using System.Windows;
using System.Windows.Controls;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для TestGamePage.xaml
/// </summary>
public partial class TestGamePage : Page
{
    private readonly DateTime start_time;

    public TestGamePage()
    {
        InitializeComponent();

        start_time = DateTime.Now;
        Back_btn.Content = start_time.ToString();
    }

    public event EventHandler PageClose_Click;

    private void PageClose(EventArgs e)
    {
        if (PageClose_Click != null) PageClose_Click(this, e);
    }

    private void Back_btn_Click(object sender, RoutedEventArgs e)
    {
        PageClose(e);
    }
}