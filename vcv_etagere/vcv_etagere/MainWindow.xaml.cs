using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace vcv_etagere
{
    public partial class MainWindow : Window
    {
        private AudioPort selectedPort = null;
        private Line tempCable = null;

        public MainWindow()
        {
            InitializeComponent();

            // Initialisation des ports
            MyVcoModule.InitializePort();
            MasterModule.InitializePort();

            // Brancher les events souris sur les ports
            MyVcoModule.PortOut.Visual.MouseLeftButtonDown += PortClicked;
            MasterModule.PortIn.Visual.MouseLeftButtonDown += PortClicked;
        }

        private void PortClicked(object sender, MouseButtonEventArgs e)
        {
            AudioPort clickedPort = null;

            if (sender == MyVcoModule.PortOut.Visual)
                clickedPort = MyVcoModule.PortOut;

            if (sender == MasterModule.PortIn.Visual)
                clickedPort = MasterModule.PortIn;

            if (clickedPort == null)
                return;

            // 🟡 PREMIER CLIC
            if (selectedPort == null)
            {
                selectedPort = clickedPort;

                tempCable = new Line
                {
                    Stroke = Brushes.Yellow,
                    StrokeThickness = 3
                };

                CableLayer.Children.Add(tempCable);

                var startPos = selectedPort.Visual.TranslatePoint(
                    new Point(8, 8), CableLayer);

                tempCable.X1 = startPos.X;
                tempCable.Y1 = startPos.Y;

                MouseMove += DragCable;
                return;
            }

            // 🔵 DEUXIÈME CLIC

            MouseMove -= DragCable;

            if (tempCable != null)
                CableLayer.Children.Remove(tempCable);

            // Vérification ULTRA IMPORTANTE
            if (!selectedPort.IsInput && clickedPort.IsInput)
            {
                if (selectedPort.Node != null && MasterModule.Engine != null)
                {
                    MasterModule.Engine.Input = selectedPort.Node;
                    DrawCable(selectedPort, clickedPort);
                }
            }

            tempCable = null;
            selectedPort = null;
        }

        private void DragCable(object sender, MouseEventArgs e)
        {
            if (tempCable == null)
                return;

            var pos = e.GetPosition(CableLayer);

            tempCable.X2 = pos.X;
            tempCable.Y2 = pos.Y;
        }

        private void DrawCable(AudioPort outPort, AudioPort inPort)
        {
            var line = new Line
            {
                Stroke = Brushes.Orange,
                StrokeThickness = 3
            };

            var p1 = outPort.Visual.TranslatePoint(new Point(8, 8), CableLayer);
            var p2 = inPort.Visual.TranslatePoint(new Point(8, 8), CableLayer);

            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;

            CableLayer.Children.Add(line);
        }


        }
    }
