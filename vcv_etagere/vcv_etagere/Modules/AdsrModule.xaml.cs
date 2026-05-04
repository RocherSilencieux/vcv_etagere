using System.Windows;
using System.Windows.Controls;
using vcv_etagere.Engines;

namespace vcv_etagere
{
    public partial class AdsrModule : UserControl, IAudioInput
    {
        public AudioPort PortIn;
        public AudioPort PortOut;
        public AdsrEngine Engine;

        public event Action<UserControl> RequestDelete;

        public AdsrModule()
        {
            InitializeComponent();
            Engine = new AdsrEngine();
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

            PortOut = new AudioPort
            {
                Node = Engine,
                IsInput = false,
                Visual = OutputPort
            };
        }

        public void Connect(IAudioNode node)
        {
            Engine.SetInput(node);
        }

        public void Disconnect()
        {
            Engine.ClearInput();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            RequestDelete?.Invoke(this);
        }

        private void AttackSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Engine != null) Engine.Attack = (float)e.NewValue;
        }

        private void DecaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Engine != null) Engine.Decay = (float)e.NewValue;
        }

        private void SustainSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Engine != null) Engine.Sustain = (float)e.NewValue;
        }

        private void ReleaseSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Engine != null) Engine.Release = (float)e.NewValue;
        }

        private void GateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Engine == null)
                return;

            if (Engine.IsGated)
            {
                Engine.GateOff();
                GateButton.Content = "Start";
            }
            else
            {
                Engine.GateOn();
                GateButton.Content = "Stop";
            }
        }
    }
}
