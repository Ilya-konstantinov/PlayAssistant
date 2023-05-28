using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace PlayAssistant;

/// <summary>
///     Логика взаимодействия для GameCreateMenu.xaml
/// </summary>
public partial class GameCreationMenu
{
    public GameCreationMenu()
    {
        InitializeComponent();

        var templates = Directory.EnumerateDirectories("GameTemplates");
        
        foreach (var template in templates)
        {
            var radioButton = new RadioButton
            {
                Content = template.Split('\\').Last(),
                Margin = new Thickness(10),
                GroupName = "Templates",
            };
            // radioButton.Checked += (sender, args) =>
            // {
            //     var settings = JsonConvert.DeserializeObject<string[]>(@$"{template}\Parameters.json");
            //
            //     foreach (var setting in settings)
            //     {
            //         Parameters.Items.Add(
            //             new Label
            //             {
            //                 FontSize = 18,
            //                 Content = "Количество игроков"
            //             }
            //         );
            //         
            //         Parameters.Items.Add(
            //             new TextBox())
            //     }
            // };
            Templates.Items.Add(radioButton);
        }

        (Templates.Items[0] as RadioButton)!.IsChecked = true;
    }
    
    

    private void Back_btn_Click(object sender, RoutedEventArgs e)
    {
        ((GameChooseMenu)Application.Current.MainWindow.Content).CloseGameCreate();
    }

    private void Create_btn_Click(object sender, RoutedEventArgs e)
    {
        var b = !int.TryParse(NumberOfPlayers.Text, out GameCreationSettings.NumberOfPlayers);
        if (b)
        {
            GameCreationSettings.NumberOfPlayers = 0;
        }

        GameCreationSettings.ShouldRun = true;
        
        if (SessionService.SessionsList().Contains(ElTitle.Text))
            return;
        ((GameChooseMenu)Application.Current.MainWindow.Content).CloseGameCreate();
        SessionService.CreateSession(ElTitle.Text);
        ((MainWindow)Application.Current.MainWindow).OpenGameCreationWindow();
    }
}