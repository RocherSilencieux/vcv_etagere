using System.Windows;
using System.Windows.Controls;

namespace vcv_etagere
{
    public class AudioPort
    {
        public IAudioNode Node;
        public bool IsInput;
        public FrameworkElement Visual;

        public AudioPort(IAudioNode node, bool isInput, FrameworkElement visual)
        {
            Node = node;
            IsInput = isInput;
            Visual = visual;
        }
    }
}
