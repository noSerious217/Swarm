using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swarm_Net
{
    class ProgressBar
    {
        private double _min = 0;
        private double _max;
        private int _length;
        private double _value;

        public ProgressBar(double Max, int Length)
        {
            _max = Max;
            _length = Length;
        }

        public ProgressBar(double Min, double Max, int Length)
        {
            _min = Min;
            _max = Max;
            _length = Length;
        }

        public void SetValue(double Value)
        {
            _value = Value;
        }

        public int GetLength()
        {
            return (int)(_length * ((_value - _min) / (_max - _min)));
        }

        public override String ToString()
        {
            return new string('█', GetLength()) + new string(' ', _length - GetLength());
        }
    }
}
