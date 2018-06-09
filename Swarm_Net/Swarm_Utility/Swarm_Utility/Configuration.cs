using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swarm_Utility
{
    [Serializable]
    public class Configuration
    {
        private string filename;
        private int[] size;
        private int webs;
        private int count;
        private bool backEnabled;
        private double backInfluence;
        private bool genEnabled;
        private int genPoolSize;
        private int genMutateTime;
        private double genMutate;
        private double genChange;
        private double genElite;
        private double genBirth;
        private int genM;
        private bool swarmEnabled;
        private int swarmPoolSize;
        private double swarmXLimit;
        private double swarmVLimit;
        private double swarmA1;
        private double swarmA2;

        public string Filename { get => filename; set => filename = value; }
        public int[] Size { get => size; set => size = value; }
        public double BackInfluence { get => backInfluence; set => backInfluence = value; }
        public int GenPoolSize { get => genPoolSize; set => genPoolSize = value; }
        public int GenMutateTime { get => genMutateTime; set => genMutateTime = value; }
        public double GenMutate { get => genMutate; set => genMutate = value; }
        public double GenChange { get => genChange; set => genChange = value; }
        public double GenElite { get => genElite; set => genElite = value; }
        public double GenBirth { get => genBirth; set => genBirth = value; }
        public int GenM { get => genM; set => genM = value; }
        public int SwarmPoolSize { get => swarmPoolSize; set => swarmPoolSize = value; }
        public double SwarmXLimit { get => swarmXLimit; set => swarmXLimit = value; }
        public double SwarmVLimit { get => swarmVLimit; set => swarmVLimit = value; }
        public double SwarmA1 { get => swarmA1; set => swarmA1 = value; }
        public double SwarmA2 { get => swarmA2; set => swarmA2 = value; }
        public bool SwarmEnabled { get => swarmEnabled; set => swarmEnabled = value; }
        public bool BackEnabled { get => backEnabled; set => backEnabled = value; }
        public bool GenEnabled { get => genEnabled; set => genEnabled = value; }
        public int Count { get => count; set => count = value; }
        public int Webs { get => webs; set => webs = value; }

        public Configuration()
        {

        }
    }
}
