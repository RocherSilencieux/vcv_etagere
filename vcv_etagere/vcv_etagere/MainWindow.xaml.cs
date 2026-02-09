using NAudio.Wave.SampleProviders;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace vcv_etagere
{
    public partial class MainWindow : Window
    {
        private Vco vcoTest;

        public MainWindow()
        {
            InitializeComponent();
            vcoTest = new Vco();
        }

        private void Button_Down(object sender, MouseButtonEventArgs e)
        {
            vcoTest.NoteOn(440);
        }

        private void Button_Down_Mute(object sender, MouseButtonEventArgs e)
        {
            vcoTest.NoteOff();
        }

        private void Menu_Change_Onde(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem)sender;
            OscType type = (OscType)item.Tag;

            switch (type)
            {
                case OscType.Sin:
                    vcoTest.ChangeTypeSignal(SignalGeneratorType.Sin);
                    break;
                case OscType.Square:
                    vcoTest.ChangeTypeSignal(SignalGeneratorType.Square);
                    break;
                case OscType.Saw:
                    vcoTest.ChangeTypeSignal(SignalGeneratorType.SawTooth);
                    break;
            }
        }



    }
}
