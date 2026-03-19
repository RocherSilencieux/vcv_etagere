using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vcv_etagere.Engines
{
    internal class MixerEngine
    {
        public VcoEngine? _engine1 { private get; set; }
        public VcoEngine? _engine2 { private get; set; }
        public MixerEngine() { }

        public float[] Mix()
        {
            if(_engine1 != null && _engine2 != null)
            {

            }
            return new float[0];
        }
    }
}
