using MLP;

namespace Swarm_Net
{
    class Swarm
    {
        internal class _particle : System.IComparable<_particle>
        {
            private Web web;
            private double[][,] _x;
            private double[][,] _v;
            private const double _a1 = 0.5; // Локальное влияние ускорения
            private const double _a2 = 0.5; // Глобальное влияние ускорения
            internal double[][,] _pbest;
            internal double _pbestvalue = double.MaxValue;

            internal _particle(int[] size)
            {
                web = new Web(size);
                int s = 0;
                for (int i = 1; i < size.Length; i++)
                {
                    s += size[i] * size[i - 1];
                }
                _x = new double[size.Length][,];
                _v = new double[size.Length][,];
                _x[0] = new double[0, 0];
                _v[0] = new double[0, 0];
                for (int i = 1; i < _x.Length; i++)
                {
                    _x[i] = new double[size[i], size[i - 1]];
                    _v[i] = new double[size[i], size[i - 1]];
                    for (int j = 0; j < size[i]; j++)
                    {
                        for (int k = 0; k < size[i - 1]; k++)
                        {
                            _x[i][j, k] = (Utility.NextDouble() - 0.5) * Utility.Next(50);
                            _v[i][j, k] = (Utility.NextDouble() - 0.5) * Utility.Next(50);
                        }
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
                FoundBestCoords(input, output, ref _gbest, ref _gbestvalue);
            }

            internal void FoundBestCoords(double[,] input, double[,] output, ref double[][,] _gbest, ref double _gbestvalue)
            {
                double d = web.GetMistake(input, output);
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

            private double[][,] Clone(double[][,] input)
            {
                double[][,] res = new double[input.Length][,];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = new double[input[i].GetLength(0), input[i].GetLength(1)];
                    for (int j = 0; j < res[i].GetLength(0); j++)
                        for (int k = 0; k < res[i].GetLength(1); k++)
                            res[i][j, k] = input[i][j, k];
                }
                return res;
            }

            public int CompareTo(_particle other)
            {
                return System.Math.Sign(_pbestvalue - other._pbestvalue);
            }
        }

        // сделать класс-обертку для сеток, в котором будут координаты и скорости
        private const int _poolsize = 50;
        private _particle[] _pool = new _particle[_poolsize];
        internal double[][,] _gbest;
        internal double _gbestvalue = double.MaxValue;

        public Swarm(int[] size)
        {
            for (int i = 0; i < _poolsize; i++)
            {
                _pool[i] = new _particle(size);
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
            System.Array.Sort(_pool);
        }

        public Web GetWeb()
        {
            return new Web(_gbest);
        }

        public double GetMistake(double[,] input, double[,] output)
        {
            return GetWeb().GetMistake(input, output);
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