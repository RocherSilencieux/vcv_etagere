using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace vcv_etagere
{
    public partial class AudioOutModule : UserControl, IAudioInput
    {
        public AudioEngine Engine;
        public AudioPort PortIn;
        public IAudioNode InputNode;

        private DispatcherTimer _timer;
        public event Action<UserControl> RequestDelete;

        public AudioOutModule()
        {
            InitializeComponent();

            Engine = new AudioEngine();
            
            _timer = new DispatcherTimer
            {
                Interval = System.TimeSpan.FromMilliseconds(50)
            };
            _timer.Tick += UpdateVuMeter;
            _timer.Start();
        }

        public void InitializePort()
        {


            PortIn = new AudioPort
            {
                Node = null,          
                IsInput = true,
                Visual = InputPort
            };

            PortIn.Visual.Tag = this;


        }


        public void Connect(IAudioNode node)
        {
            Engine.SetInput(node);
            MessageBox.Show("Connected !");
        }

        public void Disconnect()
        {
            Engine.DisconnectInput();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            RequestDelete?.Invoke(this);
        }

        private void MasterSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Engine?.SetMasterGain((float)e.NewValue);
        }

        private void MuteButton_Checked(object sender, RoutedEventArgs e)
        {
            Engine?.SetEnabled(!MuteButton.IsChecked.Value);
        }

        private void UpdateVuMeter(object sender, System.EventArgs e)
        {
            if (Engine != null)
                VuMeter.Value = Engine.CurrentLevel;
        }

        public void DisconnectInput()
        {
            Engine?.DisconnectInput(); 
        }

        public void ReconnectInput(IAudioNode node)
        {
            Engine?.SetInput(node); 
        }
    }
}
