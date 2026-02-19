using NAudio.Wave;
using System;

namespace vcv_etagere
{
    public class AudioEngine : ISampleProvider, IDisposable
    {
        public WaveFormat WaveFormat { get; }
        private WaveOutEvent _waveOut;

        public IAudioNode Input { get; private set; }
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

        public void SetInput(IAudioNode node)
        {
            Input = node;
            _enabled = true;
        }

        public void DisconnectInput()
        {
            Input = null;       
            _enabled = false;    
            ClearWaveOutBuffer();
        }

        public void SetMasterGain(float gain)
        {
            _masterGain = gain;
        }

        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
        }

        //On ecrit dans le buffer
        public int Read(float[] buffer, int offset, int count)
        {
            if (Input == null)
            {
                Array.Clear(buffer, offset, count);
                CurrentLevel = 0;
                return count;
            }

            Input.WriteAudio(buffer, offset, count);

            float sum = 0;
            for (int i = 0; i < count; i++)
            {
                float sample = buffer[offset + i];
                buffer[offset + i] = _enabled ? sample * _masterGain : 0;
                sum += sample * sample;
            }

            CurrentLevel = (float)Math.Sqrt(sum / count);
            return count;
        }

        public void Dispose()
        {
            _waveOut.Dispose();
        }

        //on clear le buffer pour pas avoir de bug chiant sur la carte son la sinon ça fait bipbipbipbip
        private void ClearWaveOutBuffer()
        {
            if (_waveOut == null) return;

            _waveOut.Stop();     
                                
            float[] silence = new float[WaveFormat.SampleRate]; 
            Read(silence, 0, silence.Length);                 
            _waveOut.Init(this);                               
            _waveOut.Play();                                 
            _enabled = true;                              
        }

    }
}
