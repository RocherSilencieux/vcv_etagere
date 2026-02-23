using System.Windows;
using System.Windows.Controls;

namespace vcv_etagere
{
    public class AudioPort
    {
        public IAudioNode Node { get; set; }
        public bool IsInput { get; set; }
        public FrameworkElement Visual { get; set; }


    }



}
