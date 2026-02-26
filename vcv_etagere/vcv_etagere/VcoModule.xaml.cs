using System.Windows;
using System.Windows.Controls;


namespace vcv_etagere
{
    public partial class VcoModule : UserControl
    {
       
        public AudioPort PortOut;
        public VcoEngine Engine;

        public event Action<UserControl> RequestDelete;

     

        public VcoModule()
        {
            InitializeComponent();
            Engine = new VcoEngine(440);
            WaveCombo.SelectedIndex = 0;
        }


        public void InitializePort()
        { 
            PortOut = new AudioPort
            {
                Node = Engine,
                IsInput = false,
                Visual = OutputPort,
            };
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            RequestDelete?.Invoke(this);
        }

        private void PitchSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Engine != null)
                Engine.Frequency = (float)e.NewValue;
        }



        private void WaveCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Engine == null) return;

            switch (WaveCombo.SelectedIndex)
            {
                case 0:
                    Engine.SetWaveform(Wave.sin);
                    break;

                case 1:
                    Engine.SetWaveform(Wave.square);
                    break;

                case 2:
                    Engine.SetWaveform(Wave.saw);
                    break;

                case 3:
                    Engine.SetWaveform(Wave.triangle);
                    break;
            }
        }
    }
}
