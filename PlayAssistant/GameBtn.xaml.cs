using System.Windows;
using System.Windows.Controls;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для GameBtn.xaml
/// </summary>
public partial class GameBtn
{
    public readonly string Game;

    public GameBtn(string gameName = "Game")
    {
        InitializeComponent();

        Game = gameName;

        SelectBtn.Content = gameName;
    }

    private void Select_btn_Click(object sender, RoutedEventArgs e)
    {
        SessionService.SessionName = Game;
        var parentWindow = Window.GetWindow(this) as MainWindow;
        ((MainWindow)Application.Current.MainWindow).OpenGameCreationWindow();
        parentWindow.StartSession();
    }
}