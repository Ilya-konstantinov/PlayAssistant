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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ServiceLibrary;

namespace PSModules
{
    /// <summary>
    /// Логика взаимодействия для ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : UserControl, IReturnValue
    {
        private bool state = false;

        public ToggleSwitch()
        {
            InitializeComponent();

        }

        private void Element_Resized(object sender, SizeChangedEventArgs e)
        {
            double circlesize = ToggleUserControl.ActualHeight - ElTitle.ActualHeight;
            if (Convert.ToBoolean(DynamicResourcesHelper.Update("CircleSize", circlesize)))
                DynamicResourcesHelper.Create("CircleSize", circlesize);
        }

        public ToggleSwitch(string _Title, string _Value = "0")
        {
            InitializeComponent();
            Title = _Title;
            if (_Value == "")
                _Value = "0";
            Value = _Value;
        }

        private void Circle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            state = !state;

            if (state)
            {
                Circle.HorizontalAlignment = HorizontalAlignment.Right;
                Rect.Fill = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                Circle.HorizontalAlignment = HorizontalAlignment.Left;
                Rect.Fill = new SolidColorBrush(Colors.Gray);
            }
        }

        public string Title { get => (string)ElTitle.Content; set => ElTitle.Content = value; }
        public string Value { get => state.ToString(); set => bool.Parse(value); }
    }
}
