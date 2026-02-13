using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vcv_etagere
{
    public class AudioEngine : ISampleProvider, IDisposable
    {
        public WaveFormat WaveFormat { get; }

        // ecrit le son dans la carte son de notre pc
        public WaveOutEvent _waveOut;


        public IAudioNode input;

        public AudioEngine()
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);
            

            _waveOut = new WaveOutEvent { DesiredLatency = 100 };
            _waveOut.Init(this);
            _waveOut.Play();
        }


        public int Read(float[] buffer, int offset, int count)
        {

            input.WriteAudio(buffer, offset, count);


            return count;
        }

        public void Dispose()
        {
            ((IDisposable)_waveOut).Dispose();
        }
    }
}
