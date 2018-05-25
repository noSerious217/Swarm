using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLP
{
    [Serializable]
    class Web : IComparable<Web>, ICloneable
    {
        private double _mistake;
        NeuronLayer[] _web;
        public double[] Input
        {
            set
            {
                _web[0].SetInput(value);
            }
        }
        public double[] Output
        {
            get
            {
                return _web[_web.Length-1].Output;
            }
        }

        public Web(int[] size)
        {
            _web = new NeuronLayer[size.Length];
            _web[0] = new NeuronLayer(null, size[0]);
            for (int i = 1; i < size.Length; i++)
            {
                _web[i] = new NeuronLayer(_web[i - 1], size[i]);
            }
        }
        
        public Web(double[][,] weights)
        {
            _web = new NeuronLayer[weights.Length];
            _web[0] = new NeuronLayer(null, weights[0]);
            for (int i=1;i<weights.Length;i++)
            {
                _web[i] = new NeuronLayer(_web[i - 1], weights[i]);
            }
        }

        public void CalculateMistake(double[] perf_output)
        {
            double[] output = Output;
            for (int i = 0; i < perf_output.Length; i++)
                _mistake += Math.Pow(perf_output[i] - output[i], 2);
        }

        public void Clear()
        {
            _mistake = 0;
        }

        public void Teach(double[] target)
        {
            double[] mistake = new double[target.Length];
            for (int i = 0; i < mistake.Length; i++)
            {
                mistake[i] = target[i] - Output[i];
            }
            _web[_web.Length - 1].Teach(mistake);
        }

        public double[][,] GetWeight()
        {
            if (_web==null || _web.Length==0) return null;
            double[][,] res = new double[_web.Length][,];
            for (int i=0;i<_web.Length;i++)
            {
                res[i] = _web[i].GetWeights();
            }
            return res;
        }

        public void SetWeight(double[][,] value)
        {
            _web = new NeuronLayer[value.Length];
            _web[0]=new NeuronLayer(null,1);
            for (int i=1;i<value.Length;i++)
            {
                _web[i] = new NeuronLayer(_web[i-1],value[i].GetLength(0));
                _web[i].SetWeights(value[i]);
            }
        }

        public bool Equalse(Web other)
        {
            bool b = true;
            for (int i=0;i<_web.Length;i++)
            {
                b&=_web[i].Equals(other._web[i]);
            }
            return b;
        }

        public int CompareTo(Web other)
        {
            return Math.Sign(_mistake - other._mistake);
        }

        public double GetMistake()
        {
            return Math.Sqrt(_mistake);
        }

        public double Teach(double[,] input, double[,] output)
        {
            Clear();
            for (int i = 0; i < input.GetLength(0); i++)
            {
                double[] tmp_in = new double[input.GetLength(1)];
                for (int j = 0; j < tmp_in.Length; j++)
                {
                    tmp_in[j] = input[i, j];
                }
                double[] tmp_out = new double[output.GetLength(1)];
                for (int j = 0; j < tmp_out.Length; j++)
                {
                    tmp_out[j] = output[i, j];
                }
                Input = tmp_in;
                CalculateMistake(tmp_out);
                Teach(tmp_out);
            }
            return GetMistake();
        }

        public double[,] GetResult(double[,] input)
        {
            double[,] output = new double[input.GetLength(0), _web[_web.Length-1].Length];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                double[] tmp_in = new double[input.GetLength(1)];
                for (int j = 0; j < tmp_in.Length; j++)
                    tmp_in[j] = input[i, j];
                Input = tmp_in;
                double[] tmp_out = Output;
                for (int j = 0; j < tmp_out.Length; j++)
                {
                    output[i, j] = tmp_out[j];
                }
            }
            return output;
        }

        public double GetMistake(double[,] input, double[,] output)
        {
            double[,] res = GetResult(input);
            double mistake = 0;
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    mistake += Math.Pow(output[i, j] - res[i, j], 2);
                }
            }
            return Math.Sqrt(mistake);
        }

        public double GetEntropy(double[,] input, double[,] output)
        {
            double[,] res = GetResult(input);
            double mistake = 0;
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    double y = (output[i, j] + 1) / 2;
                    double a = (res[i, j] + 1) / 2;
                    mistake += Math.Abs(y * Math.Log(a) + (1 - y) * Math.Log(1 - a));
                }
            }
            return -mistake / (output.GetLength(0) * output.GetLength(1));
        }

        object ICloneable.Clone()
        {
            int[] size = new int[_web.Length];
            for (int i = 0; i < size.Length; i++)
                size[i] = _web[i].Length;
            Web web = new Web(size);
            web.SetWeight(GetWeight());
            return web;
        }
    }
}
