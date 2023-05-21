using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для GameChooseMenu.xaml
/// </summary>
public partial class GameChooseMenu
{
    public GameChooseMenu(List<string> titles)
    {
        InitializeComponent();

        var gameList = new GameList(titles);
        GameListFrame.Content = gameList;
    }

    public void SetPage(TestGamePage page)
    {
        GameListFrame.Content = page;
    }

    private void Stels()
    {
        Hide.IsEnabled = true;
        Hide.Visibility = Visibility.Visible;
    }

    public void UnStels()
    {
        Hide.IsEnabled = false;
        Hide.Visibility = Visibility.Hidden;
    }

    public void OpenGameCreate(object sender, RoutedEventArgs e)
    {
        Stels();
        GameCreateGrid.Visibility = Visibility.Visible;
        GameCreateGrid.IsEnabled = true;
        var gcm = new GameCreationMenu();

        Grid.SetRow(gcm, 1);
        Grid.SetColumn(gcm, 1);

        GameCreateGrid.Children.Add(gcm);
    }

    public void CloseGameCreate()
    {
        GameCreateGrid.Visibility = Visibility.Hidden;
        GameCreateGrid.IsEnabled = false;

        GameCreateGrid.Children.Clear();

        UnStels();
    }

    private void Search_btn_Click(object sender, RoutedEventArgs e)
    {
        if (SearchTextbox.Text.Trim() == "")
            ((GameList)GameListFrame.Content).ResetSearch();
        else
            ((GameList)GameListFrame.Content).Search(SearchTextbox.Text.Trim());
    }
}