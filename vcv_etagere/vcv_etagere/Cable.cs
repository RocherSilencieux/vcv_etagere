using System.Windows.Shapes;

namespace vcv_etagere
{
    public class Cable
    {
        public AudioPort OutPort { get; set; }
        public AudioPort InPort { get; set; }
        public Path Visual { get; set; }
    }

}
