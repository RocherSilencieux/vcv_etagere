using System;

namespace vcv_etagere.Engines
{
    public enum AdsrState
    {
        Idle,
        Attack,
        Decay,
        Sustain,
        Release
    }

    public class AdsrEngine : IAudioNode
    {
        public float Attack  = 0.1f;
        public float Decay   = 0.2f;
        public float Sustain = 0.7f;
        public float Release = 0.3f;
        //convertir les durées en secondes (Attack, Decay, Release) en nombre d’échantillons.
        public float SampleRate { get; set; } = 44100f;

        public AdsrState State { get; private set; } = AdsrState.Idle;
        public float CurrentLevel { get; private set; }
        public bool IsGated { get; private set; } = false;

        private IAudioNode? _input;
        private float _phaseSampleIndex;
        private float _releaseStartLevel;

        public void SetInput(IAudioNode node) => _input = node;
        public void ClearInput() => _input = null;

        public void GateOn()
        {
            State = AdsrState.Attack;
            _phaseSampleIndex = 0f;
            CurrentLevel = 0f;
            IsGated = true;
        }

        public void GateOff()
        {
            if (State != AdsrState.Idle)
            {
                State = AdsrState.Release;
                _phaseSampleIndex = 0f;
                _releaseStartLevel = CurrentLevel;
            }
        }

        public void Reset()
        {
            State = AdsrState.Idle;
            CurrentLevel = 0f;
            _phaseSampleIndex = 0f;
            _releaseStartLevel = 0f;
            IsGated = false;
        }

        public void WriteAudio(float[] buffer, int offset, int count)
        {
            if (_input != null)
                _input.WriteAudio(buffer, offset, count);
            else
                Array.Clear(buffer, offset, count);

            for (int i = 0; i < count; i++)
            {
                float envelope = NextEnvelopeSample();
                buffer[offset + i] *= envelope;
            }
        }

        private float NextEnvelopeSample()
        {
            float level = CurrentLevel;
            float attackSamples = Math.Max(1f, Attack * SampleRate);
            float decaySamples = Math.Max(1f, Decay * SampleRate);
            float releaseSamples = Math.Max(1f, Release * SampleRate);

            switch (State)
            {
                case AdsrState.Idle:
                    level = 0f;
                    break;

                case AdsrState.Attack:
                    _phaseSampleIndex += 1f;
                    level = _phaseSampleIndex / attackSamples;
                    if (level >= 1f)
                    {
                        level = 1f;
                        State = AdsrState.Decay;
                        _phaseSampleIndex = 0f;
                    }
                    break;

                case AdsrState.Decay:
                    _phaseSampleIndex += 1f;
                    float decayProgress = _phaseSampleIndex / decaySamples;
                    level = 1f + (Sustain - 1f) * decayProgress;
                    if (decayProgress >= 1f)
                    {
                        level = Sustain;
                        State = AdsrState.Sustain;
                        _phaseSampleIndex = 0f;
                    }
                    break;

                case AdsrState.Sustain:
                    level = Sustain;
                    break;

                case AdsrState.Release:
                    _phaseSampleIndex += 1f;
                    float releaseProgress = _phaseSampleIndex / releaseSamples;
                    level = _releaseStartLevel * (1f - releaseProgress);
                    if (releaseProgress >= 1f)
                    {
                        level = 0f;
                        State = AdsrState.Idle;
                        _phaseSampleIndex = 0f;
                        _releaseStartLevel = 0f;
                        IsGated = false;
                    }
                    break;
            }

            CurrentLevel = Math.Clamp(level, 0f, 1f);
            return CurrentLevel;
        }
    }
}
