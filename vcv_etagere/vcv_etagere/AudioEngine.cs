using NAudio.Wave;
using System;

namespace vcv_etagere
{
    public class AudioEngine : ISampleProvider, IDisposable
    {
        public WaveFormat WaveFormat { get; }

        private WaveOutEvent _waveOut;

        public IAudioNode Input;

        private float _masterGain = 0.8f;
        private bool _enabled = true;

        public float CurrentLevel { get; private set; }

        public AudioEngine()
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);

            _waveOut = new WaveOutEvent { DesiredLatency = 100 };
            _waveOut.Init(this);
            _waveOut.Play();
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (Input == null)
            {
                Array.Clear(buffer, offset, count);
                return count;
            }

            Input.WriteAudio(buffer, offset, count);

            float sum = 0;

            for (int i = 0; i < count; i++)
            {
                float sample = buffer[offset + i];

                if (_enabled)
                    sample *= _masterGain;
                else
                    sample = 0;

                buffer[offset + i] = sample;

                sum += sample * sample;
            }

            CurrentLevel = (float)Math.Sqrt(sum / count);

            return count;
        }

        public void SetMasterGain(float gain)
        {
            _masterGain = gain;
        }

        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
        }

        public void Dispose()
        {
            _waveOut.Dispose();
        }
    }
}
