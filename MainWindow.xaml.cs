using System;
using System.Windows;
using System.Windows.Threading;
using Interface_MediaPlayer.ViewModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Interface_MediaPlayer
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel.PlayListViewModel();
            WeakReferenceMessenger.Default.Register<PlayListViewModel>(this, (r, m) =>
            {
                if (m.MidiaAtual.MediaState == "Stopped")
                    me.Stop();

                else if (m.MidiaAtual.MediaState == "Paused")
                    me.Pause();
                    
                else
                    me.Play();
            });
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            slider_seek.Value = me.Position.TotalSeconds;
        }
        private void SeekSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            me.Position = TimeSpan.FromSeconds(this.slider_seek.Value);
            position.Text = me.Position.ToString(@"mm\:ss");
        }
        private void VolSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            me.Volume = this.slider_vol.Value;
            volume.Text = slider_vol.Value.ToString("#%");
        }
        private void Media_MediaOpened(object sender, RoutedEventArgs e)
        {
            slider_seek.Value = 0;
            me.Position = TimeSpan.FromSeconds(this.slider_seek.Value);
            duration.Text = me.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            slider_seek.Maximum = me.NaturalDuration.TimeSpan.TotalSeconds;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            me.Close();
            WeakReferenceMessenger.Default.Send(this);
        }
    }
}