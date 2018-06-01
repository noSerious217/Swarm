using MLP;

namespace Swarm_Net
{
    class Swarm
    {
        internal class _particle : System.IComparable<_particle>
        {
            private static System.Collections.Queue queue = new System.Collections.Queue();
            private Web web;
            private double[][,] _x;
            private double[][,] _v;
            private const double _a1 = 2.05; // Локальное влияние ускорения
            private const double _a2 = 2.05; // Глобальное влияние ускорения
            internal double[][,] _pbest;
            internal double _pbestvalue = double.MaxValue;    
            
            private double _avr
            {
                get
                {
                    double res = 0;
                    foreach(double d in queue)
                    {
                        res += d;
                    }
                    return res / queue.Count;
                }
            }

            internal _particle(int[] size, double x, double v)
            {
                web = new Web(size);
                _x = new double[size.Length][,];
                _v = new double[size.Length][,];
                _x[0] = new double[0, 0];
                _v[0] = new double[0, 0];
                for (int i = 1; i < size.Length; i++)
                {
                    _x[i] = new double[size[i], size[i - 1]];
                    _v[i] = new double[size[i], size[i - 1]];
                    for (int j=0;j<size[i];j++)
                        for (int k=0;k<size[i-1];k++)
                        {
                            _x[i][j,k] = (Utility.NextDouble() - 0.5)*x;
                            _v[i][j,k] = (Utility.NextDouble() - 0.5)*v;
                        }
                }
                web.SetWeight(_x);
            }

            internal _particle(Web web)
            {
                this.web = web;
                _x = web.GetWeight();
                _v = web.GetWeight();
            }

            internal void Move(double[,] input, double[,] output, ref double[][,] _gbest, ref double _gbestvalue)
            {
                for (int i = 1; i < _x.Length; i++)
                {
                    for (int j = 0; j < _x[i].GetLength(0); j++)
                        for (int k = 0; k < _x[i].GetLength(1); k++)
                        {
                            _v[i][j, k] += _a1 * Utility.NextDouble() * (_pbest[i][j, k] - _x[i][j, k]) + _a2 * Utility.NextDouble() * (_gbest[i][j, k] - _x[i][j, k]);
                            _x[i][j, k] += _v[i][j, k];
                        }
                }
                web.SetWeight(_x);
                FoundBestCoords(input, output, ref _gbest, ref _gbestvalue);
            }

            internal void FoundBestCoords(double[,] input, double[,] output, ref double[][,] _gbest, ref double _gbestvalue)
            {
                double d = web.GetMistake(input, output);
                if (queue.Count == _poolsize) queue.Dequeue();
                queue.Enqueue(d);
                if (d < _pbestvalue)
                {
                    _pbest = Clone(_x);
                    _pbestvalue = d;
                    if (d < _gbestvalue)
                    {
                        _gbest = Clone(_x);
                        _gbestvalue = d;
                    }
                }
            }

            public int CompareTo(_particle other)
            {
                return System.Math.Sign(_pbestvalue - other._pbestvalue);
            }

            private double[][,] Clone(double[][,] input)
            {
                double[][,] res = new double[input.Length][,];
                for (int i=0;i<res.Length;i++)
                {
                    res[i] = new double[input[i].GetLength(0), input[i].GetLength(1)];
                    for (int j = 0; j < res[i].GetLength(0); j++)
                        for (int k = 0; k < res[i].GetLength(1); k++)
                            res[i][j, k] = input[i][j, k];
                }
                return res;
            }
        }

        // сделать класс-обертку для сеток, в котором будут координаты и скорости
        private const int _poolsize = 50;
        private _particle[] _pool = new _particle[_poolsize];
        internal double[][,] _gbest;
        internal double _gbestvalue = double.MaxValue;
        public double XLimit = 2;
        public double VLimit = 1;

        public Swarm(int[] size,double x,double v)
        {
            XLimit = x;
            VLimit = v;
            for (int i = 0; i < _poolsize; i++)
            {
                _pool[i] = new _particle(size, XLimit, VLimit);
            }
        }

        public void UpdateCoords(double[,] input, double[,] output)
        {
            foreach (_particle p in _pool)
                p.FoundBestCoords(input, output, ref _gbest, ref _gbestvalue);
        }

        public void Move(double[,] input, double[,] output)
        {
            foreach (_particle p in _pool)
                p.Move(input, output, ref _gbest, ref _gbestvalue);
        }

        public Web GetWeb()
        {
            return new Web(_gbest);
        }

        public double GetMistake(double[,] input, double[,] output)
        {
            return GetWeb().GetMistake(input, output);
        }

        public double GetEntropy(double[,] input, double[,] output)
        {
            return GetWeb().GetEntropy(input, output);
        }

        public void AddWeb(Web web)
        {
            _particle[] tmp = new _particle[_pool.Length + 1];
            _pool.CopyTo(tmp, 0);
            tmp[tmp.Length - 1] = new _particle(web);
            _pool = tmp;
        }
    }
}