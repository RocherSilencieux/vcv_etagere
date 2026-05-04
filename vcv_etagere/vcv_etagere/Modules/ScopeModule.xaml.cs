using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using vcv_etagere.Engines;

namespace vcv_etagere
{
    public partial class ScopeModule : UserControl, IAudioInput
    {
        public AudioPort PortIn;
        public ScopeEngine Engine;
        private DispatcherTimer _timer;

        public event Action<UserControl> RequestDelete;

        public ScopeModule()
        {
            InitializeComponent();
            Engine = new ScopeEngine();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(50); // Update every 50ms
            _timer.Tick += UpdateWaveform;
            _timer.Start();
        }

        public void InitializePort()
        {
            PortIn = new AudioPort
            {
                Node = Engine,
                IsInput = true,
                Visual = InputPort
            };
            PortIn.Visual.Tag = this;
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
            _timer.Stop();
            RequestDelete?.Invoke(this);
        }

        private void UpdateWaveform(object sender, EventArgs e)
        {
            float[] samples = Engine.GetSamples();
            WaveformCanvas.Children.Clear();

            if (samples.Length == 0) return;

            double width = WaveformCanvas.Width;
            double height = WaveformCanvas.Height;
            int numSamples = samples.Length;

            Polyline polyline = new Polyline();
            polyline.Stroke = Brushes.Green;
            polyline.StrokeThickness = 1;

            for (int i = 0; i < numSamples; i++)
            {
                double x = (i / (double)numSamples) * width;
                double y = height / 2 - samples[i] * height / 2; // Scale to canvas
                polyline.Points.Add(new Point(x, y));
            }

            WaveformCanvas.Children.Add(polyline);
        }
    }
}