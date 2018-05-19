using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP
{
    [Serializable]
    public class NeuronLayer
    {
        private NeuronLayer _previousLayer;
        private Neuron[] _layer;
        private double[] _input;
        public int Length
        {
            get
            {
                return _layer.Length;
            }
        }
        public double[] Output
        {
            get
            {
                if (_previousLayer != null)
                {
                    double[] res = new double[_layer.Length];
                    for (int i = 0; i < _layer.Length; i++)
                    {
                        _layer[i].Input = _previousLayer.Output;
                        res[i] = _layer[i].Output;
                    }
                    return res;
                }
                return _input;
            }
        }

        public NeuronLayer(NeuronLayer PLayer, int size)
        {
            _previousLayer = PLayer;
            _layer = new Neuron[size];
            for (int i = 0; i < size; i++)
            {
                if (PLayer != null) _layer[i] = new Neuron(PLayer.Length);
                else _layer[i] = new Neuron(1);
            }
        }

        public NeuronLayer(NeuronLayer PLayer, double[,] weights)
        {
            _previousLayer = PLayer;
            if (weights != null)
            {
                _layer = new Neuron[weights.GetLength(0)];
                for (int i = 0; i < _layer.Length; i++)
                {
                    _layer[i] = new Neuron(weights.GetLength(1));
                    double[] tmp = new double[weights.GetLength(1)];
                    for (int j = 0; j < tmp.Length; j++)
                        tmp[j] = weights[i, j];
                    _layer[i].SetWeight(tmp);
                }
            } 
        }

        public void SetInput(double[] input)
        {
            _input = input;
        }

        public void Teach(double[] mistakes)
        {
            if (_previousLayer != null)
            {
                for (int i = 0; i < _layer.Length; i++)
                {
                    _layer[i].CalculateMistake(mistakes[i]);
                }
                double[] pMistakes = new double[0];
                pMistakes = new double[_previousLayer.Length];
                for (int i = 0; i < _previousLayer.Length; i++)
                {
                    for (int j = 0; j < _layer.Length; j++)
                    {
                        pMistakes[i] += _layer[j].GetMistake(i);
                    }
                }
                for (int i = 0; i < _layer.Length; i++)
                {
                    _layer[i].Teach(mistakes[i]);
                }
                _previousLayer.Teach(pMistakes);
            }
        }

        public double[,] GetWeights()
        {
            if (_layer.Length > 0)
            {
                double[,] res = new double[_layer.Length, _layer[0].GetWeight().Length];
                for (int i = 0; i < _layer.Length; i++)
                {
                    double[] tmp = _layer[i].GetWeight();
                    for (int j = 0; j < tmp.Length; j++)
                    {
                        res[i, j] = tmp[j];
                    }
                }
                return res;
            }
            return null;
        }

        public void SetWeights(double[,] value)
        {
            for (int i = 0; i < value.GetLength(0); i++)
            {
                double[] tmp = new double[value.GetLength(1)];
                for (int j = 0; j < tmp.Length; j++)
                {
                    tmp[j] = value[i, j];
                }
                _layer[i].SetWeight(tmp);
            }
        }

        public bool Equalse(NeuronLayer other)
        {
            bool b = true;
            if (_layer.Length != other.Length) return false;
            for (int i = 0; i < _layer.Length; i++)
            {
                b &= _layer[i].Equals(other._layer[i]);
            }
            return b;
        }
    }
}
