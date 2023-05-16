using ServiceLibrary;
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

namespace CHRSModules
{
    /// <summary>
    /// Логика взаимодействия для BoolStatiscic.xaml
    /// </summary>
    public partial class BoolStatiscic : UserControl, IReturnValue
    {
        bool st;
        public string Title { get => (string)ElTitle.Content; set => ElTitle.Content = value; }
        public string Value { get => st.ToString(); set => SetStatus(value); }
        public BoolStatiscic() { 
            InitializeComponent();
            ElTitle.Content = "";
            Status.IsEnabled = false;
        }
        public BoolStatiscic(string _Title = "", string _Value = "0")
        {
            InitializeComponent();
            ElTitle.Content = _Title;
            Value = _Value;
            Title = _Title;

        }
        public void SetStatus(string status)
        {
            if (status == "")
            {
                status = "false";
            }
            st = bool.Parse(status);
            Status.Background = new SolidColorBrush(
                st ? Colors.Green : Colors.Red
                );
            Status.Content = st ? "True" : "False";
        }
        public void ChangeSt()
        {
            st = ! st;
            Status.Background = new SolidColorBrush(
                st ? Colors.Green : Colors.Red
                );
            Status.Content = st ? "True" : "False";
        }

        private void Status_Click(object sender, RoutedEventArgs e)
        {
            ChangeSt();
        }
    }
}
