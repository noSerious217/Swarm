using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP
{
    [Serializable]
    public class Neuron
    {
        internal double a;
        private double[] _weights;
        private double _o;
        public double[] Input;
        double _sum
        {
            get
            {
                double k = 0;
                for (int i = 0; i < Input.Length; i++)
                    k += _weights[i] * Input[i];
                k += _weights[_weights.Length - 1];
                return k;
            }
        }
        public double Output
        {
            get
            {
                return Math.Tanh(_sum);
            }
        }

        public Neuron(int size)
        {
            Random random = new Random();
            _weights = new double[size + 1];
            Input = new double[size];
            for (int i = 0; i < size + 1; i++)
            {
                _weights[i] = random.NextDouble();
            }
        }

        public void CalculateMistake(double mistake)
        {
            double der = Output;
            der = a * (1 - Math.Pow(der, 2));
            _o = mistake * der;
        }

        public void Teach(double mistake)
        {
            for (int i = 0; i < Input.Length; i++)
            {
                double dw = a * _o * Input[i];
                _weights[i] = _weights[i] + dw;
            }
            _weights[_weights.Length - 1] = _weights[_weights.Length - 1] + a * _o;
        }

        public double GetMistake(int i)
        {
            if (i < Input.Length) return _weights[i] * _o;
            return _o;
        }

        public double GetWeight(int i)
        {
            if (i < _weights.Length) return _weights[i];
            return 0;
        }

        public double[] GetWeight()
        {
            return _weights;
        }

        public void SetWeight(double value, int i)
        {
            _weights[i] = value;
        }

        public void SetWeight(double[] value)
        {
            _weights = value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Neuron);
        }

        public bool Equals(Neuron other)
        {
            if (other == null) return false;
            if (_weights.Length != other._weights.Length) return false;
            for (int i = 0; i < _weights.Length; i++)
            {
                if (_weights[i] != other._weights[i]) return false;
            }
            return true;
        }
    }
}
