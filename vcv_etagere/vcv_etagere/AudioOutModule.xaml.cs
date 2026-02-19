using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace vcv_etagere
{
    public partial class AudioOutModule : UserControl
    {
        public AudioEngine Engine;
        public AudioPort PortIn;
        public IAudioNode InputNode;

        private DispatcherTimer _timer;

        public AudioOutModule()
        {
            InitializeComponent();


            Engine = new AudioEngine();
            _timer = new DispatcherTimer();
            _timer.Interval = System.TimeSpan.FromMilliseconds(50);
            _timer.Tick += UpdateVuMeter;
            _timer.Start();
        }

        public void InitializePort()
        {
            PortIn = new AudioPort(null, true, InputPort);
        }

        private void MasterSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Engine != null)
                Engine.SetMasterGain((float)e.NewValue);
        }

        private void MuteButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Engine != null)
                Engine.SetEnabled(!MuteButton.IsChecked.Value);
        }

        private void UpdateVuMeter(object sender, System.EventArgs e)
        {
            if (Engine != null)
                VuMeter.Value = Engine.CurrentLevel;
        }

        public void DisconnectInput()
        {
            if (Engine != null)
            {
                Engine.Input = null;  
                Engine.Stop();        
            }
        }

    }
}
