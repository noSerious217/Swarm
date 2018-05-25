using MLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swarm_Net
{
    class FISwarm
    {
        internal class _particle : System.IComparable<_particle>
        {
            private Web web;
            private double[][,] _x;
            private double[][,] _v;
            private double _a = 2.05; // коэффициент ускорения
            internal double _pbestvalue = double.MaxValue;
            internal double[][,] _pbest;
            internal List<_particle> _neighborhood = new List<_particle>();

            internal _particle(int[] size)
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
                    for (int j = 0; j < size[i]; j++)
                        for (int k = 0; k < size[i - 1]; k++)
                        {
                            _x[i][j, k] = (Utility.NextDouble() - 0.5) * 10;
                            _v[i][j, k] = (Utility.NextDouble() - 0.5) * 10;
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

            internal void UpdateCoords(double[,] input, double[,] output)
            {
                double d = web.GetMistake(input, output);
                if (d < _pbestvalue)
                {
                    _pbestvalue = d;
                    _pbest = Clone(_x);
                }
            }

            internal void Move(double[,] input, double[,] output)
            {
                foreach (_particle p in _neighborhood)
                {
                    double d = _a / p.web.GetMistake(input, output);
                    for (int i = 0; i < _x.Length; i++)
                    {
                        for (int j = 0; j < _x[i].GetLength(0); j++)
                            for (int k = 0; k < _x[i].GetLength(1); k++)
                            {
                                _v[i][j, k] += d * Utility.NextDouble() * (p._pbest[i][j, k] - _x[i][j, k]);
                                _x[i][j, k] += _v[i][j, k];
                            }
                    }
                }
                web.SetWeight(_x);
                UpdateCoords(input, output);
            }

            public int CompareTo(_particle other)
            {
                return Math.Sign(_pbestvalue - other._pbestvalue);
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

            internal void AddNeighboor(_particle p)
            {
                _neighborhood.Add(p);
            }

            internal double GetMistake(double[,] input, double[,] output)
            {
                return web.GetMistake(input, output);
            }

            internal double[][,] GetWeights()
            {
                return Clone(_x);
            }
        }

        // сделать класс-обертку для сеток, в котором будут координаты и скорости
        private int[] _clustersize = new int[] { 7, 6, 7, 6 };
        private int _poolsize
        {
            get
            {
                int n = 0;
                for (int i = 0; i < _clustersize.Length; i++)
                    n += _clustersize[i];
                return n;
            }
        }
        private _particle[] _pool;
        private double[][,] _gbest;
        private double _gbestvalue = double.MaxValue;

        public FISwarm(int[] size)
        {
            _pool = new _particle[_poolsize];
            for (int i = 0; i < _poolsize; i++)
            {
                _pool[i] = new _particle(size);
            }
            _particle[] special = new _particle[12]
            {
                // шлюзы первого кластера
                _pool[0],
                _pool[_clustersize[0]/2],
                _pool[_clustersize[0]-1],
                // шлюзы второго кластера
                _pool[_clustersize[0]],
                _pool[_clustersize[1]/2 + _clustersize[0]],
                _pool[_clustersize[1]-1+_clustersize[0]],
                // шлюзы третьего кластера
                _pool[_clustersize[0]+_clustersize[1]],
                _pool[_clustersize[2]/2 + _clustersize[0]+_clustersize[1]],
                _pool[_clustersize[2]-1+_clustersize[0]+_clustersize[1]],
                // шлюзы четвертого кластера
                _pool[_clustersize[0]+_clustersize[1]+_clustersize[2]],
                _pool[_clustersize[3]/2 + _clustersize[0]+_clustersize[1]+_clustersize[2]],
                _pool[_clustersize[3]-1+_clustersize[0]+_clustersize[1]+_clustersize[3]]
            };
            int d = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < _clustersize[i]; j++)
                    for (int k = 0; k < _clustersize[i]; k++)
                    {
                        if (i != j) _pool[j + d].AddNeighboor(_pool[i + d]);
                    }
                d += _clustersize[i];
            }
            special[0].AddNeighboor(special[5]);
            special[1].AddNeighboor(special[7]);
            special[2].AddNeighboor(special[9]);
            special[3].AddNeighboor(special[8]);
            special[4].AddNeighboor(special[10]);
            special[5].AddNeighboor(special[0]);
            special[6].AddNeighboor(special[11]);
            special[7].AddNeighboor(special[1]);
            special[8].AddNeighboor(special[3]);
            special[9].AddNeighboor(special[2]);
            special[10].AddNeighboor(special[4]);
            special[11].AddNeighboor(special[6]);
        }

        public void UpdateCoords(double[,] input, double[,] output)
        {
            foreach (_particle p in _pool)
            {
                p.UpdateCoords(input, output);
                double d = p.GetMistake(input, output);
                if (d < _gbestvalue)
                {
                    _gbestvalue = d;
                    _gbest = p.GetWeights();
                }
            }
        }

        public void Move(double[,] input, double[,] output)
        {
            foreach (_particle p in _pool)
            {
                p.Move(input, output);
                double d = p.GetMistake(input, output);
                if (d < _gbestvalue)
                {
                    _gbestvalue = d;
                    _gbest = p.GetWeights();
                }
            }
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

