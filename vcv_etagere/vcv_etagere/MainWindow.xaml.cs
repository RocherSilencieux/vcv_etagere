using System.Collections.Generic;
using System.Linq;
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

        private AudioOutModule currentMaster = null;

        private List<AudioPort> allPorts = new List<AudioPort>();
        private List<Cable> allCables = new List<Cable>();


        public MainWindow()
        {
            InitializeComponent();
        }

        // ==============================
        // DELETE MODULE
        // ==============================
        private void DeleteModule(UserControl module)
        {
            if (module.Parent is Panel panel)
                panel.Children.Remove(module);

            // Supprimer tous les câbles liés à ce module
            var cablesToRemove = allCables
                .Where(c =>
                    c.OutPort.Visual.IsDescendantOf(module) ||
                    c.InPort.Visual.IsDescendantOf(module))
                .ToList();

            foreach (var cable in cablesToRemove)
            {
                RemoveCable(cable);
            }

            // Supprimer ports liés
            allPorts.RemoveAll(p => p.Visual.IsDescendantOf(module));

            if (module == currentMaster)
                currentMaster = null;
        }


        // ==============================
        // PORT CLICK
        // ==============================
        private void PortClicked(object sender, MouseButtonEventArgs e)
        {
        
            AudioPort clickedPort = allPorts.FirstOrDefault(p => p.Visual == sender);

            if (clickedPort == null)
                return;

            // --------------------------
            // PREMIER CLIC
            // --------------------------
            if (selectedPort == null)
            {
                selectedPort = clickedPort;

                tempCable = new Path
                {
                    Stroke = Brushes.Yellow,
                    StrokeThickness = 4,
                    IsHitTestVisible = false
                };

                Panel.SetZIndex(tempCable, 1000);
                CableLayer.Children.Add(tempCable);

                MouseMove += DragCable;
                return;
            }

            // --------------------------
            // DEUXIÈME CLIC
            // --------------------------
            MouseMove -= DragCable;

            if (tempCable != null)
                CableLayer.Children.Remove(tempCable);

            // Vérifie OUT → IN uniquement
            AudioPort outPort = null;
            AudioPort inPort = null;

            if (!selectedPort.IsInput && clickedPort.IsInput)
            {
                outPort = selectedPort;
                inPort = clickedPort;
            }
            else if (selectedPort.IsInput && !clickedPort.IsInput)
            {
                outPort = clickedPort;
                inPort = selectedPort;
            }

            if (outPort != null && inPort != null)
            {
                if (inPort.Visual.Tag is IAudioInput inputModule)
                {
              
                    inputModule.Connect(outPort.Node);
                }

                DrawCable(outPort, inPort);
            }



            tempCable = null;
            selectedPort = null;
        }

        // ==============================
        // DRAG CABLE
        // ==============================
        private void DragCable(object sender, MouseEventArgs e)
        {
            if (tempCable == null || selectedPort == null)
                return;

            var start = selectedPort.Visual.TranslatePoint(new Point(8, 8), CableLayer);
            var end = e.GetPosition(CableLayer);

            tempCable.Data = CreateBezier(start, end);
        }

        // ==============================
        // BEZIER CURVE
        // ==============================
        private PathGeometry CreateBezier(Point start, Point end)
        {
            double offset = Math.Abs(end.X - start.X) * 0.5;

            var figure = new PathFigure { StartPoint = start };

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

        // ==============================
        // DRAW FINAL CABLE
        // ==============================
        private void DrawCable(AudioPort outPort, AudioPort inPort)
        {
            var path = new Path
            {
                Stroke = Brushes.Orange,
                StrokeThickness = 6,
                Fill = Brushes.Transparent,
                Cursor = Cursors.Hand
            };

            var start = outPort.Visual.TranslatePoint(new Point(8, 8), CableLayer);
            var end = inPort.Visual.TranslatePoint(new Point(8, 8), CableLayer);

            path.Data = CreateBezier(start, end);

            Panel.SetZIndex(path, 900);

            var cable = new Cable
            {
                OutPort = outPort,
                InPort = inPort,
                Visual = path
            };

            // Suppression clic droit
            path.MouseRightButtonDown += (s, e) =>
            {
                RemoveCable(cable);
            };

            allCables.Add(cable);
            CableLayer.Children.Add(path);
        }


        // ==============================
        // CONTEXT MENU
        // ==============================
        private void MainGrid_RightClick(object sender, MouseButtonEventArgs e)
        {
            ContextMenu menu = new ContextMenu();

            MenuItem vcoItem = new MenuItem { Header = "Add VCO" };
            vcoItem.Click += (s, args) => AddVco(e.GetPosition(ModuleLayer));

            MenuItem masterItem = new MenuItem { Header = "Add Master" };
            masterItem.Click += (s, args) => AddMaster(e.GetPosition(ModuleLayer));

            menu.Items.Add(vcoItem);
            menu.Items.Add(masterItem);

            menu.IsOpen = true;
        }

        // ==============================
        // ADD VCO
        // ==============================
        private void AddVco(Point position)
        {

            var vco = new VcoModule();
            vco.InitializePort();
            vco.RequestDelete += DeleteModule;

            ModuleLayer.Children.Add(vco);

            Canvas.SetLeft(vco, position.X);
            Canvas.SetTop(vco, position.Y);

            allPorts.Add(vco.PortOut);
            vco.PortOut.Visual.MouseLeftButtonDown += PortClicked;
 

        }

        // ==============================
        // ADD MASTER
        // ==============================
        private void AddMaster(Point position)
        {
            if (currentMaster != null)
                return;

            var master = new AudioOutModule();
            master.InitializePort();
            master.RequestDelete += DeleteModule;

            ModuleLayer.Children.Add(master);

            Canvas.SetLeft(master, position.X);
            Canvas.SetTop(master, position.Y);

            currentMaster = master;

            allPorts.Add(master.PortIn);
            master.PortIn.Visual.MouseLeftButtonDown += PortClicked;
          

        }

        private void RemoveCable(Cable cable)
        {
            // Déconnecter audio
            if (cable.InPort.Visual.Tag is IAudioInput inputModule)
            {
                inputModule.Disconnect();
            }

            CableLayer.Children.Remove(cable.Visual);
            allCables.Remove(cable);
        }

    }
}
