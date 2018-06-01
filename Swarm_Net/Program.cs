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
                string s = reader.ReadLine().ToLower().Replace('.', ',');
                string[] data = s.Split('\t', ' ');
                for (int j = 0; j < 15; j++)
                {
                    _input1[k, j] = double.Parse(data[j]);
                }
                for (int j = 0; j < 15; j++)
                {
                    _output1[k, j] = double.Parse(data[j + 15]);
                }
                k++;
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
                for (int j = 0; j < 15; j++)
                {
                    _output2[k, j] = double.Parse(data[j + 15]);
                }
                k++;
            }
            reader.Close();
            stream.Close();
        }

        private static int _webs = 5;
        private static int _count = 200;
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
            //Genetic.size = size;
            //File.Delete("1.txt");
            //File.Delete("2.txt");
            //File.Delete("3.txt");
            //File.Delete("4.txt");
            //ProgressBar allBar = new ProgressBar(_webs, 100);
            //for (int z = 0; z < _webs; z++)
            //{
            //    allBar.SetValue(z);
            //    Console.SetCursorPosition(0, 0);
            //    Console.WriteLine(allBar);
            //    FileStream SwarmStream = new FileStream(res_swarm, FileMode.Create);
            //    FileStream FISwarmStream = new FileStream(res_fiswarm, FileMode.Create);
            //    FileStream BackStream = new FileStream(res_back, FileMode.Create);
            //    FileStream GenStream = new FileStream(res_gen, FileMode.Create);
            //    StreamWriter SwarmWriter = new StreamWriter(SwarmStream);
            //    StreamWriter FISwarmWriter = new StreamWriter(FISwarmStream);
            //    StreamWriter BackWriter = new StreamWriter(BackStream);
            //    StreamWriter GenWriter = new StreamWriter(GenStream);
            //    List<String> tmp_swarm = new List<string>();
            //    List<String> tmp_fiswarm = new List<string>();
            //    List<String> tmp_back = new List<string>();
            //    List<String> tmp_gen = new List<string>();
            //    if (File.Exists("1.txt"))
            //    {
            //        FileStream stream = new FileStream("1.txt", FileMode.Open);
            //        StreamReader reader = new StreamReader(stream);
            //        while (!reader.EndOfStream)
            //        {
            //            tmp_swarm.Add(reader.ReadLine());
            //        }
            //        stream.Close();
            //    }
            //    else tmp_swarm.AddRange(new String[_count]);
            //    if (File.Exists("2.txt"))
            //    {
            //        FileStream stream = new FileStream("2.txt", FileMode.Open);
            //        StreamReader reader = new StreamReader(stream);
            //        while (!reader.EndOfStream)
            //        {
            //            tmp_fiswarm.Add(reader.ReadLine());
            //        }
            //        stream.Close();
            //    }
            //    else tmp_fiswarm.AddRange(new String[_count]);
            //    if (File.Exists("3.txt"))
            //    {
            //        FileStream stream = new FileStream("3.txt", FileMode.Open);
            //        StreamReader reader = new StreamReader(stream);
            //        while (!reader.EndOfStream)
            //        {
            //            tmp_back.Add(reader.ReadLine());
            //        }
            //        stream.Close();
            //    }
            //    else tmp_back.AddRange(new String[_count]);
            //    if (File.Exists("4.txt"))
            //    {
            //        FileStream stream = new FileStream("4.txt", FileMode.Open);
            //        StreamReader reader = new StreamReader(stream);
            //        while (!reader.EndOfStream)
            //        {
            //            tmp_gen.Add(reader.ReadLine());
            //        }
            //        stream.Close();
            //    }
            //    else tmp_gen.AddRange(new String[_count]);
            //    Swarm swarm = new Swarm(size,0.5,0.1);
            //    swarm.UpdateCoords(_input1, _output1);
            //    //FISwarm fiswarm = new FISwarm(size);
            //    //fiswarm.UpdateCoords(_input1, _output1);
            //    Web web = new Web(size);
            //    Genetic genetic = new Genetic();
            //    Stopwatch swarm_stopwatch = new Stopwatch();
            //    Stopwatch fiswarm_stopwatch = new Stopwatch();
            //    Stopwatch back_stopwatch = new Stopwatch();
            //    Stopwatch gen_stopwatch = new Stopwatch();
            //    ProgressBar bar = new ProgressBar(_count, 100);
            //    int l = 0;
            //    bar.SetValue(0);
            //    Console.WriteLine(bar);
            //    for (int i = 0; i < _count; i++)
            //    {
            //        bar.SetValue(i);
            //        if (bar.GetLength() != l)
            //        {
            //            l = bar.GetLength();
            //            Console.SetCursorPosition(0, 1);
            //            Console.WriteLine(bar);
            //        }
            //        swarm_stopwatch.Restart();
            //        swarm.Move(_input1, _output1);
            //        swarm_stopwatch.Stop();
            //        //fiswarm_stopwatch.Restart();
            //        //fiswarm.Move(_input1, _output1);
            //        //fiswarm_stopwatch.Stop();
            //        back_stopwatch.Restart();
            //        web.Teach(_input1, _output1);
            //        back_stopwatch.Stop();
            //        gen_stopwatch.Restart();
            //        genetic.Generate(_input1, _output1);
            //        gen_stopwatch.Stop();
            //        tmp_swarm[i] += (swarm.GetMistake(_input2, _output2) / (_output2.GetLength(0) * _output2.GetLength(1))).ToString() + "\t" + swarm_stopwatch.ElapsedMilliseconds.ToString() + "\t\t";
            //        //tmp_fiswarm[i] += fiswarm.GetMistake(_input2, _output2).ToString() + "\t" + fiswarm_stopwatch.ElapsedMilliseconds.ToString() + "\t\t";
            //        tmp_back[i] += (web.GetMistake(_input2, _output2) / (_output2.GetLength(0) * _output2.GetLength(1))).ToString() + "\t" + back_stopwatch.ElapsedMilliseconds.ToString() + "\t\t";
            //        tmp_gen[i] += (genetic.GetMistake(_input2, _output2) / (_output2.GetLength(0) * _output2.GetLength(1))).ToString() + "\t" + gen_stopwatch.ElapsedMilliseconds.ToString() + "\t\t";
            //    }
            //    for (int i = 0; i < tmp_gen.Count; i++)
            //    {
            //        GenWriter.WriteLine(tmp_gen[i]);
            //    }
            //    for (int i = 0; i < tmp_swarm.Count; i++)
            //    {
            //        SwarmWriter.WriteLine(tmp_swarm[i]);
            //    }
            //    /*for (int i = 0; i < tmp_fiswarm.Count; i++)
            //    {
            //        FISwarmWriter.WriteLine(tmp_fiswarm[i]);
            //    }*/
            //    for (int i = 0; i < tmp_back.Count; i++)
            //    {
            //        BackWriter.WriteLine(tmp_back[i]);
            //    }
            //    GenWriter.Flush();
            //    GenWriter.Close();
            //    BackWriter.Flush();
            //    BackWriter.Close();
            //    SwarmWriter.Flush();
            //    SwarmWriter.Close();
            //    FISwarmWriter.Flush();
            //    FISwarmWriter.Close();
            //    File.Delete("1.txt");
            //    File.Delete("2.txt");
            //    File.Delete("3.txt");
            //    File.Delete("4.txt");
            //    File.Move(res_swarm, "1.txt");
            //    File.Move(res_fiswarm, "2.txt");
            //    File.Move(res_back, "3.txt");
            //    File.Move(res_gen, "4.txt");
            //}
            //File.Move("1.txt", res_swarm);
            //File.Move("2.txt", res_fiswarm);
            //File.Move("3.txt", res_back);
            //File.Move("4.txt", res_gen);


            ProgressBar MainBar = new ProgressBar(_webs, 100);
            File.Delete("1.txt");
            File.Delete("2.txt");
            File.Delete("3.txt");
            for (int z = 0; z < _webs; z++)
            {
                Console.SetCursorPosition(0, 0);
                MainBar.SetValue(z);
                Console.WriteLine(MainBar);
                FileStream stream_1 = new FileStream("res_swarm_0.05_0.01.txt", FileMode.Create);
                StreamWriter writer_1 = new StreamWriter(stream_1);
                FileStream stream_2 = new FileStream("res_swarm_0.1_0.05.txt", FileMode.Create);
                StreamWriter writer_2 = new StreamWriter(stream_2);
                FileStream stream_3 = new FileStream("res_swarm_0.5_0.1.txt", FileMode.Create);
                StreamWriter writer_3 = new StreamWriter(stream_3);
                List<String> tmp_1 = new List<string>();
                List<String> tmp_2 = new List<string>();
                List<String> tmp_3 = new List<string>();
                if (File.Exists("1.txt"))
                {
                    FileStream stream = new FileStream("1.txt", FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        tmp_1.Add(reader.ReadLine());
                    }
                    stream.Close();
                }
                else tmp_1.AddRange(new String[_count]);
                if (File.Exists("2.txt"))
                {
                    FileStream stream = new FileStream("2.txt", FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        tmp_2.Add(reader.ReadLine());
                    }
                    stream.Close();
                }
                else tmp_2.AddRange(new String[_count]);
                if (File.Exists("3.txt"))
                {
                    FileStream stream = new FileStream("3.txt", FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        tmp_3.Add(reader.ReadLine());
                    }
                    stream.Close();
                }
                else tmp_3.AddRange(new String[_count]);
                Swarm swarm_1 = new Swarm(size, 0.05, 0.01);
                swarm_1.UpdateCoords(_input1, _output1);
                Swarm swarm_2 = new Swarm(size, 0.1, 0.05);
                swarm_2.UpdateCoords(_input1, _output1);
                Swarm swarm_3 = new Swarm(size, 0.5, 0.1);
                swarm_3.UpdateCoords(_input1, _output1);
                ProgressBar bar = new ProgressBar(_count, 100);
                bar.SetValue(0);
                Console.WriteLine(bar);
                int l = 0;
                for (int i = 0; i < _count; i++)
                {
                    bar.SetValue(i);
                    if (bar.GetLength() != l)
                    {
                        l = bar.GetLength();
                        Console.SetCursorPosition(0, 1);
                        Console.WriteLine(bar);
                    }
                    swarm_1.Move(_input1, _output1);
                    swarm_2.Move(_input1, _output1);
                    swarm_3.Move(_input1, _output1);
                    tmp_1[i] += swarm_1.GetMistake(_input2, _output2).ToString() + '\t';
                    tmp_2[i] += swarm_2.GetMistake(_input2, _output2).ToString() + '\t';
                    tmp_3[i] += swarm_3.GetMistake(_input2, _output2).ToString() + '\t';
                }
                for (int i = 0; i < tmp_1.Count; i++)
                {
                    writer_1.WriteLine(tmp_1[i]);
                    writer_2.WriteLine(tmp_2[i]);
                    writer_3.WriteLine(tmp_3[i]);
                }
                writer_1.Flush();
                writer_1.Close();
                stream_1.Close();
                writer_2.Flush();
                writer_2.Close();
                stream_2.Close();
                writer_3.Flush();
                writer_3.Close();
                stream_3.Close();
                File.Delete("1.txt");
                File.Delete("2.txt");
                File.Delete("3.txt");
                File.Move("res_swarm_0.05_0.01.txt", "1.txt");
                File.Move("res_swarm_0.1_0.05.txt", "2.txt");
                File.Move("res_swarm_0.5_0.1.txt", "3.txt");
            }
            File.Move("1.txt", "res_swarm_0.05_0.01.txt");
            File.Move("2.txt", "res_swarm_0.1_0.05.txt");
            File.Move("3.txt", "res_swarm_0.5_0.1.txt");
        }
    }
}
