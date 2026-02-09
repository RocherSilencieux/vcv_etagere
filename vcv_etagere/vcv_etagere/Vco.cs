using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace vcv_etagere
{
    public class Vco
    {
        private WaveOutEvent output;
        private SignalGenerator osc;

        public Vco()
        {

            osc = new SignalGenerator()
            {
                Gain = 0.2f,
                Frequency = 440,
                Type = SignalGeneratorType.Sin
            };

            output = new WaveOutEvent();
            output.Init(osc);
        }

        public void NoteOn(double frequency)
        {
            osc.Frequency = frequency;

            if (output.PlaybackState != PlaybackState.Playing)
                output.Play();
        }

        public void NoteOff()
        {
            output.Stop();
        }

        public void ChangeTypeSignal(SignalGeneratorType type)
        {
            osc.Type = type;
        }


    }
}
