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
        private int _poolsize = 50;
        private int _mutatetime = 20;
        private double _mutaterate = 0.01;
        private double _change = 0.75;
        private double _elite = 0.1;
        private double _birthrate = 0.5;
        private int m = 20;
        private List<Web> _pool;

        public static int[] size = new int[] { 15, 15, 15 };

        public int Poolsize { get => _poolsize; set => _poolsize = value; }
        public int Mutatetime { get => _mutatetime; set => _mutatetime = value; }
        public double MutateRate { get => _mutaterate; set => _mutaterate = value; }
        public double Change { get => _change; set => _change = value; }
        public double Elite { get => _elite; set => _elite = value; }
        public double BirthRate { get => _birthrate; set => _birthrate = value; }
        public int M { get => m; set => m = value; }

        public Genetic()
        {
            _pool = new List<Web>();
            for (int i = 0; i < Poolsize; i++)
            {
                _pool.Add(new Web(size));
            }
        }

        public Genetic(int Poolsize)
        {
            this.Poolsize = Poolsize;
            _pool = new List<Web>();
            for (int i = 0; i < this.Poolsize; i++)
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
            Birth();
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
            int length = _pool.Count;
            for (int z = 0; z < length; z++)
            {
                if (Utility.NextDouble() < MutateRate)
                {
                    Web origin = _pool[z];
                    double[][,] tmp = origin.GetWeight();
                    int time = Utility.Next(Mutatetime + 1);
                    for (int i = 0; i < time; i++)
                    {
                        int l = Utility.Next(tmp.Length);
                        int a = Utility.Next(tmp[l].GetLength(0));
                        int b = Utility.Next(tmp[l].GetLength(1));
                        if (Utility.NextDouble() < Change)
                        {
                            double d = 0;
                            for (int q = 0; q < M; q++)
                            {
                                d += (Utility.NextDouble() < (1 / M)) ? Math.Pow(2, -i) : 0;
                            }
                            tmp[l][a, b] += d * Math.Sign(Utility.NextDouble() - 0.5);
                        }
                        else tmp[l][a, b] = (Utility.NextDouble() - 0.5);
                    }
                    Web res = new Web(size);
                    res.SetWeight(tmp);
                    _pool.Add(res);
                }
            }
        }

        private void Birth()
        {
            for (int i = 0; i < Poolsize * BirthRate + 1; i++)
            {
                Web f = _pool.ElementAt(Utility.Next(_pool.Count));
                Web m = _pool.ElementAt(Utility.Next(_pool.Count));
                double[][,] f_web = f.GetWeight();
                double[][,] m_web = m.GetWeight();
                double[][,] res1 = new double[f_web.Length][,];
                double[][,] res2 = new double[f_web.Length][,];
                for (int zi = 0; zi < res1.Length; zi++)
                {
                    int a = f_web[zi].GetLength(0) > m_web[zi].GetLength(0) ? m_web[zi].GetLength(0) : f_web[zi].GetLength(0);
                    int b = (f_web[zi].GetLength(1) > m_web[zi].GetLength(1)) ? m_web[zi].GetLength(1) : f_web[zi].GetLength(1);
                    res1[zi] = new double[a, b];
                    res2[zi] = new double[a, b];
                    for (int j = 0; j < a; j++)
                        for (int k = 0; k < b; k++)
                        {
                            double d = Utility.NextDouble();
                            res1[zi][j, k] = d > 0.5 ? f_web[zi][j, k] : m_web[zi][j, k];
                            res2[zi][j, k] = d > 0.5 ? m_web[zi][j, k] : f_web[zi][j, k];
                        }
                }
                Web web = new Web(size);
                web.SetWeight(res1);
                _pool.Add(web);
                Web web1 = new Web(size);
                web1.SetWeight(res2);
                _pool.Add(web1);
            }
        }

        private void Selection()
        {
            List<Web> NewPool = new List<Web>();
            _pool.Sort();
            for (int i = 0; i < Poolsize * Elite; i++)
                NewPool.Add(_pool[i]);
            while (NewPool.Count<Poolsize)
            {
                NewPool.Add(_pool.ElementAt(Utility.Next(Utility.Next(_pool.Count))));
            }
            _pool = NewPool;
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

        public Web GetWeb()
        {
            return _pool[0];
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
