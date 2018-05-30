using MLP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swarm_Net
{
    class Program
    {
        /*private static void LoadData()
        {
            _input1 = new double[120, 4];
            _input2 = new double[30, 4];
            _output1 = new double[120, 3];
            _output2 = new double[30, 3];
            FileStream stream = new FileStream("iris_1.txt", FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            int k = 0;
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine();
                string[] data = s.Split('\t');
                for (int j = 0; j < 4; j++)
                {
                    _input1[k, j] = double.Parse(data[j]);
                }
                for (int j = 0; j < 3; j++)
                {
                    _output1[k, j] = double.Parse(data[j + 4]);
                }
                k++;
            }
            reader.Close();
            stream.Close();
            stream = new FileStream("iris_2.txt", FileMode.Open);
            reader = new StreamReader(stream);
            k = 0;
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine();
                string[] data = s.Split('\t');
                for (int j = 0; j < 4; j++)
                {
                    _input2[k, j] = double.Parse(data[j]);
                }
                for (int j = 0; j < 3; j++)
                {
                    _output2[k, j] = double.Parse(data[j + 4]);
                }
                k++;
            }
            reader.Close();
            stream.Close();
        }*/

        private static void LoadData()
        {
            _input1 = new double[196, 15];
            _input2 = new double[10, 15];
            _output1 = new double[196, 15];
            _output2 = new double[10, 15];
            FileStream stream = new FileStream("zernike.txt", FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            int k = 0;
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine().ToLower().Replace('.',',');
                string[] data = s.Split('\t',' ');
                for (int j = 0; j < 15; j++)
                {
                    _input1[k, j] = double.Parse(data[j]);
                }
                s = reader.ReadLine().ToLower().Replace('.', ',');
                data = s.Split('\t', ' ');
                for (int j = 0; j < 15; j++)
                {
                    _output1[k, j] = double.Parse(data[j]);
                }
                k++;
                reader.ReadLine();
            }
            reader.Close();
            stream.Close();
            stream = new FileStream("zernike_test.txt", FileMode.Open);
            reader = new StreamReader(stream);
            k = 0;
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine().ToLower().Replace('.', ',');
                string[] data = s.Split('\t', ' ');
                for (int j = 0; j < 15; j++)
                {
                    _input2[k, j] = double.Parse(data[j]);
                }
                s = reader.ReadLine().ToLower().Replace('.', ',');
                data = s.Split('\t', ' ');
                for (int j = 0; j < 15; j++)
                {
                    _output2[k, j] = double.Parse(data[j]);
                }
                k++;
                reader.ReadLine();
            }
            reader.Close();
            stream.Close();
        }

        private static int _count = 100;
        private static double[,] _input1;
        private static double[,] _input2;
        private static double[,] _output1;
        private static double[,] _output2;
        private static string res_swarm = "res_swarm.txt";
        private static string res_fiswarm = "res_fiswarm.txt";
        private static string res_back = "res_back.txt";
        private static string res_gen = "res_gen.txt";
        private static int[] size = new int[4] { 15, 5, 5, 15 };

        static void Main(string[] args)
        {

            LoadData();
            Genetic.size = size;
            File.Delete("1.txt");
            File.Delete("2.txt");
            File.Delete("3.txt");
            File.Delete("4.txt");
            for (int z = 0; z < 5; z++)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(z.ToString());
                FileStream SwarmStream = new FileStream(res_swarm, FileMode.Create);
                FileStream FISwarmStream = new FileStream(res_fiswarm, FileMode.Create);
                FileStream BackStream = new FileStream(res_back, FileMode.Create);
                FileStream GenStream = new FileStream(res_gen, FileMode.Create);
                StreamWriter SwarmWriter = new StreamWriter(SwarmStream);
                StreamWriter FISwarmWriter = new StreamWriter(FISwarmStream);
                StreamWriter BackWriter = new StreamWriter(BackStream);
                StreamWriter GenWriter = new StreamWriter(GenStream);
                List<String> tmp_swarm = new List<string>();
                List<String> tmp_fiswarm = new List<string>();
                List<String> tmp_back = new List<string>();
                List<String> tmp_gen = new List<string>();
                if (File.Exists("1.txt"))
                {
                    FileStream stream = new FileStream("1.txt", FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        tmp_swarm.Add(reader.ReadLine());
                    }
                    stream.Close();
                }
                else tmp_swarm.AddRange(new String[1000]);
                if (File.Exists("2.txt"))
                {
                    FileStream stream = new FileStream("2.txt", FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        tmp_fiswarm.Add(reader.ReadLine());
                    }
                    stream.Close();
                }
                else tmp_fiswarm.AddRange(new String[1000]);
                if (File.Exists("3.txt"))
                {
                    FileStream stream = new FileStream("3.txt", FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        tmp_back.Add(reader.ReadLine());
                    }
                    stream.Close();
                }
                else tmp_back.AddRange(new String[1000]);
                if (File.Exists("4.txt"))
                {
                    FileStream stream = new FileStream("4.txt", FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        tmp_gen.Add(reader.ReadLine());
                    }
                    stream.Close();
                }
                else tmp_gen.AddRange(new String[1000]);
                Swarm swarm = new Swarm(size);
                swarm.UpdateCoords(_input1, _output1);
                FISwarm fiswarm = new FISwarm(size);
                fiswarm.UpdateCoords(_input1, _output1);
                Web web = new Web(size);
                Genetic genetic = new Genetic();
                Stopwatch swarm_stopwatch = new Stopwatch();
                Stopwatch fiswarm_stopwatch = new Stopwatch();
                Stopwatch back_stopwatch = new Stopwatch();
                Stopwatch gen_stopwatch = new Stopwatch();
                ProgressBar bar = new ProgressBar(_count, 40);
                int l = 0;
                bar.SetValue(0);
                Console.WriteLine(bar);
                for (int i = 0; i < _count; i++)
                {
                    bar.SetValue(i);
                    if (bar.GetLength()!=l)
                    {
                        l = bar.GetLength();
                        Console.SetCursorPosition(0, 1);
                        Console.WriteLine(bar);
                    }
                    swarm_stopwatch.Restart();
                    swarm.Move(_input1, _output1);
                    swarm_stopwatch.Stop();
                    //fiswarm_stopwatch.Restart();
                    //fiswarm.Move(_input1, _output1);
                    //fiswarm_stopwatch.Stop();
                    back_stopwatch.Restart();
                    web.Teach(_input1, _output1);
                    back_stopwatch.Stop();
                    gen_stopwatch.Restart();
                    genetic.Generate(_input1, _output1);
                    gen_stopwatch.Stop();
                    tmp_swarm[i] += (swarm.GetMistake(_input2, _output2) / (_output2.GetLength(0) * _output2.GetLength(1))).ToString() + "\t" + swarm_stopwatch.ElapsedMilliseconds.ToString() + "\t\t";
                    //tmp_fiswarm[i] += fiswarm.GetMistake(_input2, _output2).ToString() + "\t" + fiswarm_stopwatch.ElapsedMilliseconds.ToString() + "\t\t";
                    tmp_back[i] += (web.GetMistake(_input2, _output2) / (_output2.GetLength(0) * _output2.GetLength(1))).ToString() + "\t" + back_stopwatch.ElapsedMilliseconds.ToString() + "\t\t";
                    tmp_gen[i] += (genetic.GetMistake(_input2, _output2) / (_output2.GetLength(0) * _output2.GetLength(1))).ToString() + "\t" + gen_stopwatch.ElapsedMilliseconds.ToString() + "\t\t";
                }
                for (int i = 0; i < tmp_gen.Count; i++)
                {
                    GenWriter.WriteLine(tmp_gen[i]);
                }
                for (int i = 0; i < tmp_swarm.Count; i++)
                {
                    SwarmWriter.WriteLine(tmp_swarm[i]);
                }
                /*for (int i = 0; i < tmp_fiswarm.Count; i++)
                {
                    FISwarmWriter.WriteLine(tmp_fiswarm[i]);
                }*/
                for (int i = 0; i < tmp_back.Count; i++)
                {
                    BackWriter.WriteLine(tmp_back[i]);
                }
                GenWriter.Flush();
                GenWriter.Close();
                BackWriter.Flush();
                BackWriter.Close();
                SwarmWriter.Flush();
                SwarmWriter.Close();
                FISwarmWriter.Flush();
                FISwarmWriter.Close();
                File.Delete("1.txt");
                File.Delete("2.txt");
                File.Delete("3.txt");
                File.Delete("4.txt");
                File.Move(res_swarm, "1.txt");
                File.Move(res_fiswarm, "2.txt");
                File.Move(res_back, "3.txt");
                File.Move(res_gen, "4.txt");
            }
            File.Move("1.txt", res_swarm);
            File.Move("2.txt", res_fiswarm);
            File.Move("3.txt", res_back);
            File.Move("4.txt", res_gen);

        }
    }
}
