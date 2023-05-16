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

namespace CHRSModules
{
    /// <summary>
    /// Логика взаимодействия для StringStatiscic.xaml
    /// </summary>
    public partial class StringStatiscic : UserControl, IReturnValue
    {
        public string Title { get => (string)ElTitle.Content; set => ElTitle.Content = value; }
        public string Value { get => Field.Text; set => Field.Text = value; }

        public StringStatiscic()
        {
            InitializeComponent();
            ElTitle.Content = "";
            Field.IsEnabled = false;
        }

        public StringStatiscic(String _Title = "", String _Value = "")
        {
            InitializeComponent();
            this.ElTitle.Content = _Title;
            Value = _Value;
            if (_Title == _Value)
            {
                Field.IsEnabled = false;
            }
        }

        private void Field_TextChanged(object sender, TextChangedEventArgs e)
        {
            Value= Field.Text;
        }
    }
}
