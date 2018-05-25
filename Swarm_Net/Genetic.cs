using MLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swarm_Net
{
    class Genetic
    {
        private const int _poolsize = 50;
        private const int _mutatetime = 20;
        private const double _mutate = 0.01;
        private const double _birth = 0.5;
        private static Random random = new Random();
        private List<Web> _pool;

        public static int[] size = new int[] { 15, 15, 15 };

        public Genetic()
        {
            _pool = new List<Web>();
            for (int i = 0; i < _poolsize; i++)
            {
                _pool.Add(new Web(size));
            }
        }

        public double[] GenerateTeach(int g_time, int t_time, double[,] input, double[,] output)
        {
            double[] res = new double[g_time + t_time];
            for (int i = 0; i < g_time; i++)
            {
                res[i] = Generate(input, output);
            }
            for (int i = 0; i < t_time; i++)
            {
                _pool[0].Clear();
                Web web = new Web(size);
                web.SetWeight(_pool[0].GetWeight());
                res[i + g_time] = web.Teach(input, output);
                _pool.Add(web);
            }
            return res;
        }

        public double Generate(double[,] input, double[,] output)
        {
            foreach (Web w in _pool) w.Clear();
            for (int i = 0; i < _poolsize * _birth + 1; i++)
                Birth();
            for (int i = 0; i < _poolsize * _mutate + 1; i++)
                Mutate();
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
                foreach (Web w in _pool)
                {
                    w.Input = tmp_in;
                    w.CalculateMistake(tmp_out);
                }
            }
            Selection();
            return _pool[0].GetMistake();
        }

        public double[] GetResult(double[] input)
        {
            _pool[0].Input = input;
            return _pool[0].Output;
        }

        private void Mutate()
        {
            Web origin = _pool.ElementAt(random.Next(_pool.Count));
            double[][,] tmp = origin.GetWeight();
            int time = random.Next(_mutatetime + 1);
            for (int i = 0; i < time; i++)
            {
                int l = random.Next(tmp.Length);
                int a = random.Next(tmp[l].GetLength(0));
                int b = random.Next(tmp[l].GetLength(1));
                if (random.NextDouble() > 0.75) tmp[l][a, b] += 0.5 - random.NextDouble();
                else tmp[l][a, b] = (0.5 - random.NextDouble()) * random.Next(50);
            }
            Web res = new Web(size);
            res.SetWeight(tmp);
            _pool.Add(res);
        }

        private void Birth()
        {
            Web f = _pool.ElementAt(random.Next(_pool.Count));
            Web m = _pool.ElementAt(random.Next(_pool.Count));
            double[][,] f_web = f.GetWeight();
            double[][,] m_web = m.GetWeight();
            double[][,] res = new double[f_web.Length][,];
            for (int i = 0; i < res.Length; i++)
            {
                int a = f_web[i].GetLength(0) > m_web[i].GetLength(0) ? m_web[i].GetLength(0) : f_web[i].GetLength(0);
                int b = (f_web[i].GetLength(1) > m_web[i].GetLength(1)) ? m_web[i].GetLength(1) : f_web[i].GetLength(1);
                res[i] = new double[a, b];
                for (int j = 0; j < a; j++)
                    for (int k = 0; k < b; k++)
                        res[i][j, k] = random.NextDouble() > 0.5 ? f_web[i][j, k] : m_web[i][j, k];
            }
            Web web = new Web(size);
            web.SetWeight(res);
            _pool.Add(web);
        }

        private void Selection()
        {
            _pool.Sort();
            while (_pool.Count > _poolsize) _pool.RemoveAt(_pool.Count - 1);
        }

        public double[,] GetResult(double[,] input)
        {
            return _pool[0].GetResult(input);
        }

        public double GetMistake(double[,] input, double[,] output)
        {
            double res = _pool[0].GetMistake(input, output);
            foreach (Web w in _pool)
            {
                double t = w.GetMistake(input, output);
                if (t < res) res = t;
            }
            return res;
        }

        public double GetEntropy(double[,] input, double[,] output)
        {
            return _pool[0].GetEntropy(input, output);
        }

        public void Add(Web web)
        {
            Web tmp = new Web(size);
            tmp.SetWeight(web.GetWeight());
            _pool.Add(tmp);
        }
    }
}
