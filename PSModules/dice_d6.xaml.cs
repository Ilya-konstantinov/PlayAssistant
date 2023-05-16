using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

namespace PSModules
{
    /// <summary>
    /// Логика взаимодействия для dice_d6.xaml
    /// </summary>
    public partial class dice_d6 : UserControl, IReturnValue
    {
        bool animation_is_active = false;
        int timer_time = 0;
        long timer_start_time;
        int current_step = 0;
        long animation_timestamp;
        int value = 0;

        private string exe_path;

        double btnFontSize = 6;         // процент от высоты окна
        double labelFontSize = 6;

        const int anim_time = 500 * 10000;
        const int anim_step = 35 * 10000;

        private void Element_Resized(object sender, SizeChangedEventArgs e)
        {
            double labelfontsize = Application.Current.MainWindow.Height * (labelFontSize / 100);
            double btnfontsize = Application.Current.MainWindow.Height * (btnFontSize / 100);
            if (Convert.ToBoolean(DynamicResourcesHelper.Update("LabelFontSize", labelfontsize)))
                DynamicResourcesHelper.Create("LabelFontSize", labelfontsize);
            if (Convert.ToBoolean(DynamicResourcesHelper.Update("BtnFontSize", btnfontsize)))
                DynamicResourcesHelper.Create("BtnFontSize", btnfontsize);
        }
        public dice_d6(string _Title, string _Value)
        {
            InitializeComponent();
            //throw_btn.IsEnabled = false;
            Title = _Title;
            if (_Value == "")
                   Value = "0";
            Value = _Value;
        }
        public dice_d6()
        {
            InitializeComponent();
            //throw_btn.IsEnabled = false;

            exe_path = System.Reflection.Assembly.GetEntryAssembly().Location.ToString();
            exe_path = exe_path.Substring(0, exe_path.Length - 17);

            dice_img.Source = new BitmapImage(new Uri(exe_path + "/Images/" + "1" + ".png", UriKind.Absolute));
        }

        private void throw_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(animation_checkbox.IsChecked) && !animation_is_active)
            {
                animation_is_active = true;
                timer_time = 0;
                timer_start_time = Stopwatch.GetTimestamp();
                current_step = anim_step;

                animation_timestamp = Stopwatch.GetTimestamp();

                Thread anim_thread = new Thread(throw_animation);
                anim_thread.IsBackground = true;
                anim_thread.Start();
            }
            else if (!Convert.ToBoolean(animation_checkbox.IsChecked))
            {
                int rand = new Random().Next(1, 7);
                value = rand;
                dice_img.Source = new BitmapImage(new Uri(exe_path + "/Images/" + rand.ToString() + ".png", UriKind.Absolute));
            }
        }

        private void throw_animation()
        {
            if (Stopwatch.GetTimestamp() - animation_timestamp >= current_step)
            {
                timer_time += current_step;
                current_step += anim_step;

                int rand = new Random().Next(1, 7);
                
                while (rand == value)
                {
                    rand = new Random().Next(1, 7);
                }

                value = rand;
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { dice_img.Source = new BitmapImage(new Uri(exe_path + "/Images/" + rand.ToString() + ".png", UriKind.Absolute)); }));

                animation_timestamp = Stopwatch.GetTimestamp();
            }

            if (Stopwatch.GetTimestamp() - timer_start_time < anim_time)
            {
                Thread tmp = new Thread(throw_animation);
                tmp.IsBackground = true;
                tmp.Start();
            }
            else
            {
                Thread tmp = new Thread(throw_end_animation);
                tmp.IsBackground = true;
                tmp.Start();
            }
        }

        private void throw_end_animation()
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() => { dice_border.BorderBrush = new SolidColorBrush(Colors.Green);}));
            Thread.Sleep(TimeSpan.FromMilliseconds(300));
            Application.Current.Dispatcher.BeginInvoke((Action)(() => { dice_border.BorderBrush = new SolidColorBrush(Colors.Transparent); }));
            animation_is_active = false;
        }
        public string Title { get; set; }
        public string Value { get => value.ToString(); set => SetValue( Int32.Parse(value)); }

        public void SetValue(int _value) 
        { 
            value = Clamp(_value, 1, 6);
            dice_img.Source = new BitmapImage(new Uri("/Images/" + value.ToString() + ".png", UriKind.Relative));
        }




        private static int Clamp(int val, int min, int max)
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }
}
