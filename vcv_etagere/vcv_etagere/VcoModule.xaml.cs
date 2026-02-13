using System.Windows;
using System.Windows.Controls;


namespace vcv_etagere
{
    public partial class VcoModule : UserControl
    {
        private VcoEngine engine;

        public VcoModule()
        {
            InitializeComponent();

            //engine = new VcoEngine();
            WaveCombo.SelectedIndex = 0;
        }

        private void PitchSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (engine != null)
                //engine.SetFrequency(e.NewValue);
        }


        private void WaveCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (WaveCombo.SelectedIndex)
            {
                //case 0:
                //    engine.SetWaveform(OscType.Sin);
                //    break;
                //case 1:
                //    engine.SetWaveform(OscType.Square);
                //    break;
                //case 2:
                //    engine.SetWaveform(OscType.Saw);
                //    break;
            }
        }

        private void GainSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (engine != null)
            //    engine.SetGain((float)e.NewValue);
        }

    }
}
