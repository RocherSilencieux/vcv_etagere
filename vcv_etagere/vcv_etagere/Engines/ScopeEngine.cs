using System;

namespace vcv_etagere.Engines
{
    public class ScopeEngine : IAudioNode
    {
        private const int BufferSize = 1024;
        private float[] _samples = new float[BufferSize];
        private int _writeIndex = 0;
        private IAudioNode? _input;

        

        public void SetInput(IAudioNode node) => _input = node;
        public void ClearInput() => _input = null;

        
    }
}