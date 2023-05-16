using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlayAssistant
{
    /// <summary>
    /// Логика взаимодействия для GameChooseMenu.xaml
    /// </summary>
    public partial class GameChooseMenu : Page
    {
        public GameChooseMenu(List<string> titles)
        {
            InitializeComponent();

            GameList gameList = new GameList(titles);
            GameList_frame.Content = gameList;
        }

        public void SetPage(TestGamePage _page)
        {
            GameList_frame.Content = _page;
        }

        public void Stels()
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
            GameCreate_grid.Visibility = Visibility.Visible;
            GameCreate_grid.IsEnabled = true;
            GameCreateMenu gcm = new GameCreateMenu();

            Grid.SetRow(gcm, 1);
            Grid.SetColumn(gcm, 1);

            GameCreate_grid.Children.Add(gcm);
        }

        public void CloseGameCreate()
        {
            GameCreate_grid.Visibility = Visibility.Hidden;
            GameCreate_grid.IsEnabled = false;

            GameCreate_grid.Children.Clear();

            UnStels();
        }

        private void Search_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Search_textbox.Text.Trim() == "")
            {
                ((GameList)GameList_frame.Content).ResetSearch();
            }
            else
            {
                ((GameList)GameList_frame.Content).Search(Search_textbox.Text.Trim());
            }
        }

        private void Search_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;

            if (Search_textbox.Text.Trim() == "")
            {
                ((GameList)GameList_frame.Content).ResetSearch();
            }
            else
            {
                ((GameList)GameList_frame.Content).Search(Search_textbox.Text.Trim());
            }
        }
    }
}
