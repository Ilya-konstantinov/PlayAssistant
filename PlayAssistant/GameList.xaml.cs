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
    /// Логика взаимодействия для GameList.xaml
    /// </summary>
    public partial class GameList : Page
    {
        private List<GameBtn> btn_list = new List<GameBtn>();

        public GameList(List<string> titles)
        {
            InitializeComponent();

            foreach(var title in titles)
                btn_list.Add(new GameBtn(title));

            for (int i = 0;i < titles.Count;i++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { 
                    Width = new GridLength(1, GridUnitType.Star)
                });
                btn_list[i].SetValue(Grid.ColumnProperty, i);
                MainGrid.Children.Add(btn_list[i]);
            }
        }

        public void Search(string name)
        {
            MainGrid.Children.Clear();
            MainGrid.ColumnDefinitions.Clear();

            for (int i = 0;i < btn_list.Count;i++)
            {
                if (btn_list[i].game.ToLower().Contains(name.Trim().ToLower()))
                {
                    MainGrid.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    });
                    btn_list[i].SetValue(Grid.ColumnProperty, i);
                    MainGrid.Children.Add(btn_list[i]);
                }
            }
        }

        public void ResetSearch()
        {
            MainGrid.Children.Clear();
            MainGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < btn_list.Count; i++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                btn_list[i].SetValue(Grid.ColumnProperty, i);
                MainGrid.Children.Add(btn_list[i]);
            }
        }
    }
}
