using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vcv_etagere
{
    public interface IAudioNode
    {

        void WriteAudio(float[] buffer, int offset, int count);

    }
}
