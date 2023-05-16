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
    /// Логика взаимодействия для ListOfUserControls.xaml
    /// </summary>
    public partial class ListOfUserControls : UserControl
    {
        bool IsPSList;
        public bool InMainWindow;
        public Character curCh;
        public ListOfUserControls(List<IReturnValue> userControls, bool _IsPSList, bool _InMainWindow, Character _curCh = null)
        {
            InitializeComponent();
            foreach(var item in userControls) { MainList.Items.Add(item); }
            
            IsPSList = _IsPSList;
            InMainWindow = _InMainWindow;
            curCh= _curCh;
        }

        private void MainList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            var selected = (IReturnValue)MainList.SelectedItem;
            if (selected == null)
                return;
            selected.Title = ElementTitle.Text;
            if (IsPSList)
            {
                parentWindow.AddPS((IReturnValue)
                    Activator.CreateInstance(
                        selected.GetType(),
                        selected.Title,
                        "")
                    );
            }
            else
            {
                if (curCh == null)
                {
                    Character.AddGeneralAttributes(selected);
                }
                else
                {
                    curCh.AddAttribute(selected);
                }
                parentWindow.Refrash();
            }
            parentWindow.RemoveList(InMainWindow);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
            parentWindow.RemoveList(InMainWindow);
        }
    }
}
