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
using Newtonsoft.Json.Linq;
using ServiceLibrary;

namespace CHRSModules
{
    /// <summary>
    /// Логика взаимодействия для DigitalStatiscic.xaml
    /// </summary>
    public partial class DigitalStatiscic : UserControl, IReturnValue
    {
        

        public string Value { get => ElValue.Text; set => ElValue.Text = value; }
        public string Title { get => (string)ElTitle.Content; set => ElTitle.Content= value; }

        public DigitalStatiscic()
        {
            InitializeComponent();
            this.ElTitle.Content = "";
            ElValue.Text = "0";
            ElValue.IsEnabled = false;
            UpBtn.IsEnabled = false;
            DownBtn.IsEnabled = false;
        }

        public DigitalStatiscic(String _Title, String _Value)
        {
            InitializeComponent();
            this.ElTitle.Content = _Title;
            this.ElValue.Text = _Value;
        }

        private void Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = ElValue.Text;
            if (ElValue.Text == "") text = "0";
            if (!Char.IsDigit(text[text.Length-1]))
            {
                ElValue.Text = text.Substring(0,text.Length - 1);
            }            
            Value= ElValue.Text;
        }

        private void UpBtn_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(ElValue.Text);
            val++;
            ElValue.Text = val.ToString();
        }

        private void DownBtn_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(ElValue.Text);
            val--;
            ElValue.Text = val.ToString();
        }
    }
}
