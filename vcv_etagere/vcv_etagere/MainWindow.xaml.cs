using System.Windows;

namespace vcv_etagere
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AudioEngine audio = new AudioEngine();
            VcoEngine vco = new VcoEngine(440);

            audio.input = vco;

            MyVcoModule.Engine = vco; //  LIAISON
        }
    }

}
