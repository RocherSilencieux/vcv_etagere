using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Security.Cryptography.X509Certificates;

namespace vcv_etagere
{

    public enum Wave {
        sin,
        square,
        saw,
        triangle
    }

    public class VcoEngine : IAudioNode
    {
        public Wave waveShape = Wave.sin;
        public float _phase;
        public float _frequency = 440;
        private float _gain = 0.2f;

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

                    case Wave.square:
                        sampleValue = Math.Sin(_phase) >= 0 ? 1f : -1f;
                        break;

                    case Wave.saw:
                        float normalizedPhase = _phase / (2 * (float)Math.PI);
                        sampleValue = 2f * normalizedPhase - 1f;
                        break;

                    case Wave.triangle:
                        float p = _phase / (2 * (float)Math.PI);
                        sampleValue = 2f * Math.Abs(2f * p - 1f) - 1f;
                        break;
                }

                sampleValue *= _gain;

                buffer[offset + i] = sampleValue;

                _phase += (float)(2 * Math.PI * _frequency / 44100);

                if (_phase > 2 * Math.PI)
                    _phase -= (float)(2 * Math.PI);
            }
        }


        public void SetFrequency(float freq)
        {
            _frequency = freq;
        }


        public void SetWaveform(Wave wave)
        {
            waveShape = wave;
        }

    }
}
