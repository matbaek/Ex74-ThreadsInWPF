using System;
using System.Collections.Generic;
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

namespace ThreadsInWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private volatile bool m_StopThread1;
        private volatile bool m_StopThread2;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnPutIn1_Click(object sender, RoutedEventArgs e)
        {
            if (lbFruits.SelectedItem != null)
            {
                var fruit = (lbFruits.SelectedItem as ListBoxItem).Content;
                lbBlender1.Items.Add(new ListBoxItem { Content = fruit });
            }
        }

        private void BtnPutIn2_Click(object sender, RoutedEventArgs e)
        {
            if (lbFruits.SelectedItem != null)
            {
                var fruit = (lbFruits.SelectedItem as ListBoxItem).Content;
                lbBlender2.Items.Add(new ListBoxItem { Content = fruit });
            }
        }

        private void BtnBlend1_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(Blend1);
            thread.Start();
        }

        private void BtnBlend2_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(Blend2);
            thread.Start();
        }

        private void BtnClean1_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(Clean1);
            thread.Start();
        }

        private void BtnClean2_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(Clean2);
            thread.Start();
        }

        private void BtnStop1_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(Stop1);
            thread.Start();
        }

        private void BtnStop2_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(Stop2);
            thread.Start();
        }

        private void Blend1()
        {
            DisableButtons1();
            lblStatus1.Dispatcher.Invoke(new Action(() => lblStatus1.Content = $"Working"));
            pbStatus1.Dispatcher.Invoke(new Action(() => pbStatus1.Foreground = Brushes.Green));
            int blendTime = 10;
            for (int i = 0; i < blendTime; i++)
            {
                if (!m_StopThread1)
                {
                    Thread.Sleep(1000);
                    pbStatus1.Dispatcher.Invoke(new Action(() => pbStatus1.Value += 10));
                }
                else
                {
                    pbStatus1.Dispatcher.Invoke(new Action(() => pbStatus1.Foreground = Brushes.Red));
                    pbStatus1.Dispatcher.Invoke(new Action(() => pbStatus1.Value = 100));
                }
            }
            if (!m_StopThread1)
            {
                lblStatus1.Dispatcher.Invoke(new Action(() => lblStatus1.Content = $"Juice Ready"));
            }
            EnableButtons1();
        }

        private void Blend2()
        {
            DisableButtons2();
            lblStatus2.Dispatcher.Invoke(new Action(() => lblStatus2.Content = $"Working"));
            pbStatus2.Dispatcher.Invoke(new Action(() => pbStatus2.Foreground = Brushes.Green));
            int blendTime = 10;
            for (int i = 0; i < blendTime; i++)
            {
                if (!m_StopThread2)
                {
                    Thread.Sleep(1000);
                    pbStatus2.Dispatcher.Invoke(new Action(() => pbStatus2.Value += 10));
                }
                else
                {
                    pbStatus2.Dispatcher.Invoke(new Action(() => pbStatus2.Foreground = Brushes.Red));
                    pbStatus2.Dispatcher.Invoke(new Action(() => pbStatus2.Value = 100));
                }
            }
            if (!m_StopThread2)
            {
                lblStatus2.Dispatcher.Invoke(new Action(() => lblStatus2.Content = $"Juice Ready"));
            }
            EnableButtons2();
        }

        private void Clean1()
        {
            DisableButtons1();
            lblStatus1.Dispatcher.Invoke(new Action(() => lblStatus1.Content = $"Working"));
            pbStatus1.Dispatcher.Invoke(new Action(() => pbStatus1.Foreground = Brushes.Green));
            for (int i = 0; i < 2; i++)
            {
                Thread.Sleep(1000);
                pbStatus1.Dispatcher.Invoke(new Action(() => pbStatus1.Value += 50));
            }

            lblStatus1.Dispatcher.Invoke(new Action(() => lblStatus1.Content = $"Cleaned"));
            lbBlender1.Dispatcher.Invoke(new Action(() => lbBlender1.Items.Clear()));
            EnableButtons1();
        }

        private void Clean2()
        {
            DisableButtons2();
            lblStatus2.Dispatcher.Invoke(new Action(() => lblStatus2.Content = $"Working"));
            pbStatus2.Dispatcher.Invoke(new Action(() => pbStatus2.Foreground = Brushes.Green));
            for (int i = 0; i < 2; i++)
            {
                Thread.Sleep(1000);
                pbStatus2.Dispatcher.Invoke(new Action(() => pbStatus2.Value += 50));
            }

            lblStatus2.Dispatcher.Invoke(new Action(() => lblStatus2.Content = $"Cleaned"));
            lbBlender2.Dispatcher.Invoke(new Action(() => lbBlender2.Items.Clear()));
            EnableButtons2();
        }

        private void Stop1()
        {
            m_StopThread1 = true;
            EnableButtons1();
            lblStatus1.Dispatcher.Invoke(new Action(() => lblStatus1.Content = $"Stopped"));
        }

        private void Stop2()
        {
            m_StopThread2 = true;
            EnableButtons2();
            lblStatus2.Dispatcher.Invoke(new Action(() => lblStatus2.Content = $"Stopped"));
        }

        private void EnableButtons1()
        {
            btnBlend1.Dispatcher.Invoke(new Action(() => btnBlend1.IsEnabled = true));
            btnClean1.Dispatcher.Invoke(new Action(() => btnClean1.IsEnabled = true));
            btnStop1.Dispatcher.Invoke(new Action(() => btnStop1.IsEnabled = false));
        }

        private void EnableButtons2()
        {
            btnBlend2.Dispatcher.Invoke(new Action(() => btnBlend2.IsEnabled = true));
            btnClean2.Dispatcher.Invoke(new Action(() => btnClean2.IsEnabled = true));
            btnStop2.Dispatcher.Invoke(new Action(() => btnStop2.IsEnabled = false));
        }

        private void DisableButtons1()
        {
            m_StopThread1 = false;
            pbStatus1.Dispatcher.Invoke(new Action(() => pbStatus1.Value = 0));
            btnBlend1.Dispatcher.Invoke(new Action(() => btnBlend1.IsEnabled = false));
            btnClean1.Dispatcher.Invoke(new Action(() => btnClean1.IsEnabled = false));
            btnStop1.Dispatcher.Invoke(new Action(() => btnStop1.IsEnabled = true));
        }

        private void DisableButtons2()
        {
            m_StopThread2 = false;
            pbStatus2.Dispatcher.Invoke(new Action(() => pbStatus2.Value = 0));
            btnBlend2.Dispatcher.Invoke(new Action(() => btnBlend2.IsEnabled = false));
            btnClean2.Dispatcher.Invoke(new Action(() => btnClean2.IsEnabled = false));
            btnStop2.Dispatcher.Invoke(new Action(() => btnStop2.IsEnabled = true));
        }
    }
}
