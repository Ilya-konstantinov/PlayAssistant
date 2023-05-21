using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для GameList.xaml
/// </summary>
public partial class GameList
{
    private readonly List<GameBtn> _games = new();

    public GameList(List<string> titles)
    {
        InitializeComponent();

        foreach (var title in titles)
            _games.Add(new GameBtn(title));

        for (var i = 0; i < titles.Count; i++)
        {
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            _games[i].SetValue(Grid.ColumnProperty, i);
            MainGrid.Children.Add(_games[i]);
        }
    }

    public void Search(string name)
    {
        MainGrid.Children.Clear();
        MainGrid.ColumnDefinitions.Clear();

        for (var i = 0; i < _games.Count; i++)
            if (_games[i].Game.Contains(name.Trim()))
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                _games[i].SetValue(Grid.ColumnProperty, i);
                MainGrid.Children.Add(_games[i]);
            }
    }

    public void ResetSearch()
    {
        MainGrid.Children.Clear();
        MainGrid.ColumnDefinitions.Clear();

        for (var i = 0; i < _games.Count; i++)
        {
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            _games[i].SetValue(Grid.ColumnProperty, i);
            MainGrid.Children.Add(_games[i]);
        }
    }
}