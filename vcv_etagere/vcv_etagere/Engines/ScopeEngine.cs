using System;

namespace vcv_etagere.Engines
{
    public class ScopeEngine : IAudioNode
    {
        private const int BufferSize = 1024;
        private float[] _samples = new float[BufferSize];
        private int _writeIndex = 0;
        private IAudioNode? _input;

        public float[] GetSamples()
        {
            lock (_samples)
            {
                return (float[])_samples.Clone();
            }
        }

        public void SetInput(IAudioNode node) => _input = node;
        public void ClearInput() => _input = null;

        public void WriteAudio(float[] buffer, int offset, int count)
        {
            if (_input != null)
                _input.WriteAudio(buffer, offset, count);
            else
                Array.Clear(buffer, offset, count);

            // Copy samples for display (take left channel)
            lock (_samples)
            {
                for (int i = 0; i < count && i < BufferSize; i++)
                {
                    _samples[_writeIndex] = buffer[offset + i];
                    _writeIndex = (_writeIndex + 1) % BufferSize;
                }
            }
        }
    }
}