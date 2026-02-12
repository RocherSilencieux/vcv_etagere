using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace vcv_etagere
{
    public class VcoEngine
    {
        private WaveOutEvent output;
        private SignalGenerator osc;

        public VcoEngine()
        {
            osc = new SignalGenerator()
            {
                Gain = 0.2f,
                Frequency = 440,
                Type = SignalGeneratorType.Sin
            };

            output = new WaveOutEvent();
            output.Init(osc);
            output.Play();
        }

        public void SetFrequency(double frequency)
        {
            osc.Frequency = frequency;
        }

        public void SetWaveform(OscType type)
        {
            switch (type)
            {
                case OscType.Sin:
                    osc.Type = SignalGeneratorType.Sin;
                    break;
                case OscType.Square:
                    osc.Type = SignalGeneratorType.Square;
                    break;
                case OscType.Saw:
                    osc.Type = SignalGeneratorType.SawTooth;
                    break;
            }
        }

        public void SetGain(float gain)
        {
            osc.Gain = gain;
        }
    }
}
