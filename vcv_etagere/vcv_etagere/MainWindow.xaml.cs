using System.Windows;
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
            MessageBox.Show("Note On");
            vcoTest.NoteOn(440);
        }

        private void Button_Up(object sender, MouseButtonEventArgs e)
        {
            vcoTest.NoteOff();
        }
    }
}
