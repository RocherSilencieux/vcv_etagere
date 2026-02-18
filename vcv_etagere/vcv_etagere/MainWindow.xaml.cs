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

            //pour lier les trucs la
            MyVcoModule.Engine = vco; 
            MasterModule.Engine = audio;
        }
    }

}
