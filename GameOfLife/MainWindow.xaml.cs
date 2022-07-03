using System;
using System.Windows;
using System.Windows.Threading;

namespace GameOfLife
{
    public partial class MainWindow : Window
    {
        private readonly Grid mainGrid;
        private readonly DispatcherTimer timer;   //  Generation timer
        private int genCounter;
        private readonly AdWindow[] adWindows = new AdWindow[2];


        public MainWindow()
        {
            InitializeComponent();
            mainGrid = new Grid(MainCanvas);

            timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(200) };
            timer.Tick += OnTimer;
        }


        private void StartAd()
        {
            for (int i = 0; i < adWindows.Length; i++)
            {
                if (adWindows[i] == null)
                {
                    var adWindow = new AdWindow(this)
                    {
                        Top = this.Top + (330 * i) + 70,
                        Left = this.Left + 240
                    };
                    adWindow.Closed += AdWindowOnClosed;
                    adWindow.Show();

                    adWindows[i] = adWindow;
                }
            }
        }

        private void AdWindowOnClosed(object sender, EventArgs eventArgs)
        {
            var adWindow = sender as AdWindow;
            for (int i = 0; i < 2; i++)
            {
                if (ReferenceEquals(adWindow, adWindows[i]))
                {
                    adWindows[i].Closed -= AdWindowOnClosed;
                    adWindows[i] = null;
                }
            }
        }

        private void Button_OnClick(object sender, EventArgs e)
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
                ButtonStart.Content = "Stop";
                StartAd();
            }
            else
            {
                timer.Stop();
                ButtonStart.Content = "Start";
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            mainGrid.UpdateToNextGeneration();
            genCounter++;
            lblGenCount.Content = "Generations: " + genCounter;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.Clear();
        }
    }
}
