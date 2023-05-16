using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using System.Windows.Threading;
using ServiceLibrary;

namespace PSModules
{
    /// <summary>
    /// Логика взаимодействия для TimerElement.xaml
    /// </summary>
    public partial class TimerElement : UserControl, IReturnValue
    {
        int time = 5;
        int temp_time;
        bool status = false;
        DateTime start_time;
        private readonly Stopwatch timer = new Stopwatch();
        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();

        double btnFontSize = 6;         // процент от высоты окна
        double labelFontSize = 12;

        private void Element_Resized(object sender, SizeChangedEventArgs e)
        {
            double labelfontsize = Application.Current.MainWindow.Height * (labelFontSize / 100);
            double btnfontsize = Application.Current.MainWindow.Height * (btnFontSize / 100);
            if (Convert.ToBoolean(DynamicResourcesHelper.Update("LabelFontSize", labelfontsize)))
                DynamicResourcesHelper.Create("LabelFontSize", labelfontsize);
            if (Convert.ToBoolean(DynamicResourcesHelper.Update("BtnFontSize", btnfontsize)))
                DynamicResourcesHelper.Create("BtnFontSize", btnfontsize);
        }

        public TimerElement()
        {
            InitializeComponent();

            temp_time = time;

            dispatcherTimer.Tick += new EventHandler(Timer_tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        public TimerElement(string _Title, string _Value)
        {
            InitializeComponent();
            Title = _Title;
            if (_Value == "")
                _Value = "5";
            Value = _Value;

            dispatcherTimer.Tick += new EventHandler(Timer_tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void Timer_tick(object sender, EventArgs e)
        {
            temp_time -= 1;
            if (temp_time <= 0)
            {
                temp_time = time;
                timer.Stop();
                dispatcherTimer.Stop();
                status = false;
                Start_btn.Content = "Start";
            }
            Update_text(temp_time);
        }

        private void Update_text(int set_time)
        {
            if (set_time < 0)
            {
                set_time = 0;
            }
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Seconds_textbox.Text = (set_time % 60).ToString();
                Minutes_textbox.Text = (Convert.ToInt32(set_time % 3600 / 60)).ToString();
                Hours_textbox.Text = (Convert.ToInt32(set_time / 3600)).ToString();
            }));
        }

        private void Input_time(object sender, TextChangedEventArgs e)
        {
            if (Seconds_textbox != null && !status)
            {
                time = 0;
                try
                {
                    time += Convert.ToInt32(Seconds_textbox.Text);
                    time += Convert.ToInt32(Minutes_textbox.Text) * 60;
                    time += Convert.ToInt32(Hours_textbox.Text) * 3600;
                }
                catch
                {
                    Error_textblock.Text = "Invalid input!!!";
                }
                Update_text(time);
            }
        }

        private void HPlus_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!status)
            {
                time += 3600;
                Update_text(time);
                temp_time = time;
            }
        }

        private void MPlus_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!status)
            {
                time += 60;
                Update_text(time);
                temp_time = time;
            }
        }

        private void SPlus_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!status)
            {
                time += 1;
                Update_text(time);
                temp_time = time;
            }
        }

        private void HMinus_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!status)
            {
                time -= 3600;
                if (time < 0)
                {
                    time = 0;
                }
                Update_text(time);
                temp_time = time;
            }
        }

        private void MMinus_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!status)
            {
                time -= 60;
                if (time < 0)
                {
                    time = 0;
                }
                Update_text(time);
                temp_time = time;
            }
        }

        private void SMinus_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!status)
            {
                time -= 1;
                if (time < 0)
                {
                    time = 0;
                }
                Update_text(time);
                temp_time = time;
            }
        }

        private void Start_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!status)
            {
                status = true;
                Start_btn.Content = "Stop";
                start_time = DateTime.Now;
                timer.Start();
                dispatcherTimer.Start();
            }
            else
            {
                status = false;
                Start_btn.Content = "Start";
                temp_time = time - Convert.ToInt32(Math.Round((DateTime.Now - start_time).TotalSeconds));
                timer.Stop();
                dispatcherTimer.Stop();
            }
        }

        private void Reset_btn_Click(object sender, RoutedEventArgs e)
        {
            status = false;
            Start_btn.Content = "Start";
            temp_time = time;
            Update_text(temp_time);
            timer.Stop();
            dispatcherTimer.Stop();
        }

        public string Title { get => (string)ElTitle.Content; set => ElTitle.Content = value; }
        public string Value { get => temp_time.ToString(); set => temp_time = Int32.Parse(value); }
    }
}
