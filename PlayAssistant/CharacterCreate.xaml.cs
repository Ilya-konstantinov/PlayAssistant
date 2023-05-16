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
    /// Логика взаимодействия для CharacterCreate.xaml
    /// </summary>
    public partial class CharacterCreate : UserControl
    {
        Character character = new Character("");
        public CharacterCreate()
        {
            InitializeComponent();
            var lst = character.GetAttributes();
            foreach (var attr in lst)
            {
                Characteristic.Items.Add(attr);
            }
            
        }

        public bool NameCorrect()
        {
            if (Name.Text.Length > 0) { 
                return true;
            }
            else{
                return false;   
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (NameCorrect())
            {
                MainWindow parentWindow = Window.GetWindow(this) as MainWindow;

                character.Name= Name.Text;

                parentWindow.AddCharacter(character);
                parentWindow.RemoveCreateCharacter();
            }
        }

        private void AddCharacteriscit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.CreateList(false, false, character);
        }
        public void AddCharacter(IReturnValue chr)
        {
            Characteristic.Items.Add(chr);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.RemoveCreateCharacter();
        }
        public void Refrash()
        {
            Characteristic.Items.Clear();
            var lst = character.GetAttributes();
            foreach (var attr in lst)
            {
                Characteristic.Items.Add(attr);
            }
        }
    }
}
