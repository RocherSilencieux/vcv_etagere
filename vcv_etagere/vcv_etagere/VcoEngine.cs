using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Security.Cryptography.X509Certificates;

namespace vcv_etagere
{

    public enum Wave {
        sin,
        square,
    }

    public class VcoEngine : IAudioNode
    {
        public Wave waveShape = Wave.sin;
        public float _phase;
        public float _frequency = 440;

        public VcoEngine(float frequency)
        {
            _frequency = frequency;
        }

        public void WriteAudio(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                float sampleValue = 0;

                switch (waveShape)
                {
                    case Wave.sin:

                        sampleValue = (float)Math.Sin(_phase);
                        break;
                }

                buffer[i] = sampleValue;

                // Augmenter la phase en fonction de la frequence
                _phase += (float)(2 * Math.PI * _frequency / 44100);


                // Reset la phase quand elle deviens trop grande
                if(_phase > 2 * Math.PI)
                {
                    _phase -= (float)(2 * Math.PI);
                }

            }
        }

        //public void SetFrequency(double frequency)
        //{
        //    osc.Frequency = frequency;
        //}

        //public void SetWaveform(OscType type)
        //{
        //    switch (type)
        //    {
        //        case OscType.Sin:
        //            osc.Type = SignalGeneratorType.Sin;
        //            break;
        //        case OscType.Square:
        //            osc.Type = SignalGeneratorType.Square;
        //            break;
        //        case OscType.Saw:
        //            osc.Type = SignalGeneratorType.SawTooth;
        //            break;
        //    }
        //}

        //public void SetGain(float gain)
        //{
        //    osc.Gain = gain;
        //}

    }
}
