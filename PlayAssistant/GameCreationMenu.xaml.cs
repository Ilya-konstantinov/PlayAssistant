using System.Windows;
using System.Windows.Controls;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для GameCreateMenu.xaml
/// </summary>
public partial class GameCreationMenu
{
    public GameCreationMenu()
    {
        InitializeComponent();
    }

    private void Back_btn_Click(object sender, RoutedEventArgs e)
    {
        ((GameChooseMenu)Application.Current.MainWindow.Content).CloseGameCreate();
    }

    private void Create_btn_Click(object sender, RoutedEventArgs e)
    {
        if (SessionService.SessionsList().Contains(ElTitle.Text))
            return;
        ((GameChooseMenu)Application.Current.MainWindow.Content).CloseGameCreate();
        SessionService.CreateSession(ElTitle.Text);
        ((MainWindow)Application.Current.MainWindow).OpenGameCreationWindow();
    }
}