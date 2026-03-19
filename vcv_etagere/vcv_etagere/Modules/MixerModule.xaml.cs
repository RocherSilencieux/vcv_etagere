using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using vcv_etagere.Engines;

namespace vcv_etagere
{
    /// <summary>
    /// Interaction logic for MixerModule.xaml
    /// </summary>
    public partial class MixerModule : Page
    {
        private MixerEngine _engine; 
        public MixerModule()
        {
            InitializeComponent();
            _engine = new MixerEngine();
            
        }
    }
}
