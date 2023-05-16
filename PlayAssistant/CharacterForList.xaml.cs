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
using ServiceLibrary;

namespace PlayAssistant
{
    /// <summary>
    /// Логика взаимодействия для CharacterForList.xaml
    /// </summary>
    public enum Status { Open, Close }
    public partial class CharacterForList : UserControl
    {
        public Character character;
        Status st = Status.Close;
        internal CharacterForList(Character character)
        {
            InitializeComponent();
            this.character = character;
            Name.Content= character.Name;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (st== Status.Open)
            {
                st= Status.Close;
                var lst = grid.Children.OfType<ListBox>().ToList();
                Height /= 2;
                character.Refrash(lst[0].Items.OfType<IReturnValue>().ToList());
                foreach (var item in lst)
                {
                    grid.Children.Remove(item);
                }
                
            }
            else
            {
                st= Status.Open;
                var lst = new ListBox();
                foreach(var item in character.GetAttributes())
                {
                    lst.Items.Add(item);
                }
                Height *= 2;
                lst.Margin= new Thickness(
                    Width * 0.05, Avatar.Height*1.2, Width * 0.05, Height*0.05
                    );
                grid.Children.Add(lst);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.CreateList(false, true, character);
        }

        public void Refresh()
        {
            if (st == Status.Open)
            {
                var lst = grid.Children.OfType<ListBox>().ToList();
                foreach (var item in lst)
                {
                    grid.Children.Remove(item);
                }
                var lstt = new ListBox();
                foreach (var item in character.GetAttributes())
                {
                    lstt.Items.Add(item);
                }
                lstt.Margin = new Thickness(
                    Width * 0.05, Avatar.Height * 1.2, Width * 0.05, Height * 0.05
                    );
            }
        }
    }
}
