using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace vcv_etagere
{
    public partial class MainWindow : Window
    {
        private AudioPort selectedPort = null;
        private Path tempCable = null;


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

            //  PREMIER CLIC
            if (selectedPort == null)
            {
                selectedPort = clickedPort;

                tempCable = new Path
                {
                    Stroke = Brushes.Yellow,
                    StrokeThickness = 4
                };

                Panel.SetZIndex(tempCable, 1000);
                CableLayer.Children.Add(tempCable);

                MouseMove += DragCable;
                return;
            }

            // DEUXIÈME CLIC

            MouseMove -= DragCable;

            if (tempCable != null)
                CableLayer.Children.Remove(tempCable);

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
            if (tempCable == null || selectedPort == null)
                return;

            var start = selectedPort.Visual.TranslatePoint(new Point(8, 8), CableLayer);
            var end = e.GetPosition(CableLayer);

            tempCable.Data = CreateBezier(start, end);
        }

        //Fonction pour faire les belles courbes la
        private PathGeometry CreateBezier(Point start, Point end)
        {
            double offset = Math.Abs(end.X - start.X) * 0.5;

            var figure = new PathFigure
            {
                StartPoint = start
            };

            var bezier = new BezierSegment
            {
                Point1 = new Point(start.X + offset, start.Y),
                Point2 = new Point(end.X - offset, end.Y),
                Point3 = end,
                IsStroked = true
            };

            figure.Segments.Add(bezier);

            return new PathGeometry(new[] { figure });
        }


        private void DrawCable(AudioPort outPort, AudioPort inPort)
        {
            var path = new Path
            {
                Stroke = Brushes.Orange,
                StrokeThickness = 4,
                Cursor = Cursors.Hand
            };

            var start = outPort.Visual.TranslatePoint(new Point(8, 8), CableLayer);
            var end = inPort.Visual.TranslatePoint(new Point(8, 8), CableLayer);

            path.Data = CreateBezier(start, end);

            Panel.SetZIndex(path, 900);

            // Suppression clic droit
            path.MouseRightButtonDown += (s, e) =>
            {
                CableLayer.Children.Remove(path);
                MasterModule.Engine.Input = null;
            };

            CableLayer.Children.Add(path);
        }



    }
}
