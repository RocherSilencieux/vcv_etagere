using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace vcv_etagere
{
    public class LinearRamp
    {
        private float _current; // la valeur actuel de ton vco for example
        private float _increment; // de combien de valeurs de float tu va augmenter a chaque sample pour arrivé a ta target
        private float _sampleRemaining; // combien de sample il te reste pour arrivé a la target
        private float _time; // en combien de temps tu dois faire l'interpolation entre l'actuel et la target.
        private float _sampleRate; // le sample rate de ton app (44100)


        public float Target // la valeurs target(celle de ton slider UI)
        {
            get => _current;
            set
            {
                //on calcule l'increment entre chaque valeurs en partant de _current jusqu'a target
                _sampleRemaining = (_sampleRate * _time);
                if (_sampleRemaining != 0)
                {
                    _increment = (value - _current) / _sampleRemaining;

                }
            }
        }

        public LinearRamp(int sampleRate, float time)
        {
            this._time = time;
            this._sampleRate = sampleRate;
        }

        public float Next()
        {
            if(_sampleRemaining > 0)
            {
                _sampleRemaining--;
                _current += _increment;
            }else
            {
                _current = Target;
               
            }
            return _current;
        }
    }
}
