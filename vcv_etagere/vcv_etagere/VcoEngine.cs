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
        private float _gain = 0.2f;

        private LinearRamp _rampFrequency = new LinearRamp(44100, 0.05f);
        // this is local frequency
        public float _frequency = 440;
        public float Frequency
        {
            get => _frequency;
            set {

                _rampFrequency.Target = value;
                _frequency = value;

            } 
        }

        public VcoEngine(float frequency)
        {
            _frequency = frequency;
            _rampFrequency.Target = _frequency;
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
                        // Normaliser la phase entre 0 et 1
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

                _phase += (float)(2 * Math.PI * _rampFrequency.Next() / 44100);

                if (_phase > 2 * Math.PI)
                    _phase -= (float)(2 * Math.PI);
            }
        }


        public void SetFrequency(float freq)
        {
            _frequency = freq;
        }

        public void SetGain(float gain)
        {
            _gain = gain;
        }

        public void SetWaveform(Wave wave)
        {
            waveShape = wave;
        }

    }
}
