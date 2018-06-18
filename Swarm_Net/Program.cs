using MLP;
using Swarm_Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Swarm_Net
{
    class Program
    {
        private static void LoadData()
        {
            if (File.Exists("mlp.conf"))
            {
                try
                {
                    System.Xml.Serialization.XmlSerializer xmlreader = new System.Xml.Serialization.XmlSerializer(typeof(Configuration));
                    StreamReader file = new StreamReader(@"mlp.conf");
                    configuration = (Configuration)xmlreader.Deserialize(file);
                    file.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка в файле настроек!");
                    LoadDefaultConfig();
                }
            }
            else
            {
                LoadDefaultConfig();
            }
            FileStream stream = new FileStream(configuration.Filename + ".txt", FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            List<double[]> input = new List<double[]>();
            List<double[]> output = new List<double[]>();
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine().ToLower().Replace('.', ',');
                string[] data = s.Split('|');
                data[0] = data[0].Trim('\t');
                data[1] = data[1].Trim('\t');
                string[] _input = data[0].Split('\t');
                string[] _output = data[1].Split('\t');
                double[] tmp = new double[_input.Length];
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = double.Parse(_input[i]);
                }
                input.Add(tmp);
                tmp = new double[_output.Length];
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = double.Parse(_output[i]);
                }
                output.Add(tmp);
            }
            reader.Close();
            stream.Close();
            _input1 = new double[input.Count, input[0].Length];
            _output1 = new double[output.Count, output[0].Length];
            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    _input1[i, j] = input[i][j];
                }
                for (int j = 0; j < output[i].Length; j++)
                {
                    _output1[i, j] = output[i][j];
                }
            }
            stream = new FileStream(configuration.Filename + "_test.txt", FileMode.Open);
            reader = new StreamReader(stream);
            input = new List<double[]>();
            output = new List<double[]>();
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine().ToLower().Replace('.', ',');
                string[] data = s.Split('|');
                data[0] = data[0].Trim('\t');
                data[1] = data[1].Trim('\t');
                string[] _input = data[0].Split('\t');
                string[] _output = data[1].Split('\t');
                double[] tmp = new double[_input.Length];
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = double.Parse(_input[i]);
                }
                input.Add(tmp);
                tmp = new double[_output.Length];
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = double.Parse(_output[i]);
                }
                output.Add(tmp);
            }
            reader.Close();
            stream.Close();
            _input2 = new double[input.Count, input[0].Length];
            _output2 = new double[output.Count, output[0].Length];
            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    _input2[i, j] = input[i][j];
                }
                for (int j = 0; j < output[i].Length; j++)
                {
                    _output2[i, j] = output[i][j];
                }
            }
        }

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

        //private static void LoadData()
        //{
        //    _input1 = new double[196, 15];
        //    _input2 = new double[10, 15];
        //    _output1 = new double[196, 15];
        //    _output2 = new double[10, 15];
        //    FileStream stream = new FileStream("zernike.txt", FileMode.Open);
        //    StreamReader reader = new StreamReader(stream);
        //    int k = 0;
        //    while (!reader.EndOfStream)
        //    {
        //        string s = reader.ReadLine().ToLower().Replace('.', ',');
        //        string[] data = s.Split('\t', ' ');
        //        for (int j = 0; j < 15; j++)
        //        {
        //            _input1[k, j] = double.Parse(data[j]);
        //        }
        //        for (int j = 0; j < 15; j++)
        //        {
        //            _output1[k, j] = double.Parse(data[j + 15]);
        //        }
        //        k++;
        //    }
        //    reader.Close();
        //    stream.Close();
        //    stream = new FileStream("zernike_test.txt", FileMode.Open);
        //    reader = new StreamReader(stream);
        //    k = 0;
        //    while (!reader.EndOfStream)
        //    {
        //        string s = reader.ReadLine().ToLower().Replace('.', ',');
        //        string[] data = s.Split('\t', ' ');
        //        for (int j = 0; j < 15; j++)
        //        {
        //            _input2[k, j] = double.Parse(data[j]);
        //        }
        //        for (int j = 0; j < 15; j++)
        //        {
        //            _output2[k, j] = double.Parse(data[j + 15]);
        //        }
        //        k++;
        //    }
        //    reader.Close();
        //    stream.Close();
        //}

        private static void LoadDefaultConfig()
        {
            configuration = new Configuration();
            configuration.Filename = "zernike";
            configuration.Size = new int[4] { 15, 5, 5, 15 };
            configuration.Webs = 5;
            configuration.Count = 200;
            configuration.BackEnabled = true;
            configuration.BackInfluence = 0.05;
            configuration.GenBirth = 0.5;
            configuration.GenChange = 0.75;
            configuration.GenElite = 0.1;
            configuration.GenEnabled = true;
            configuration.GenM = 20;
            configuration.GenMutate = 0.01;
            configuration.GenMutateTime = 20;
            configuration.GenPoolSize = 50;
            configuration.SwarmA1 = 0.5;
            configuration.SwarmA2 = 0.5;
            configuration.SwarmEnabled = true;
            configuration.SwarmPoolSize = 50;
            configuration.SwarmVLimit = 0;
            configuration.SwarmXLimit = 0;
        }

        //╔═╗╟─╢║╚╝

        private static void ProgressBorder()
        {
            Console.Clear();
            //Console.SetBufferSize(102, 5);
            Console.SetCursorPosition(0, 0);
            Console.Write('╔');
            Console.Write(new string('═', 100));
            Console.WriteLine('╗');
            Console.Write('║');
            Console.Write(new string(' ', 100));
            Console.WriteLine('║');
            Console.Write('╟');
            Console.Write(new string('─', 100));
            Console.WriteLine('╢');
            Console.Write('║');
            Console.Write(new string(' ', 100));
            Console.WriteLine('║');
            Console.Write('╚');
            Console.Write(new string('═', 100));
            Console.WriteLine('╝');
        }

        private static double[,] _input1;
        private static double[,] _input2;
        private static double[,] _output1;
        private static double[,] _output2;
        private static string res_swarm = "swarm";
        private static string res_back = "back";
        private static string res_gen = "gen";
        private static string extension = ".txt";
        private static Configuration configuration;

        static void Main(string[] args)
        {
            try
            {
                LoadData();
                ProgressBorder();
                File.Delete("1.txt");
                File.Delete("2.txt");
                File.Delete("3.txt");
                string dirname = configuration.Filename + '_';
                for (int i = 0; i < configuration.Size.Length; i++)
                {
                    dirname += configuration.Size[i].ToString() + ';';
                }
                dirname = dirname.TrimEnd(';');
                Directory.CreateDirectory(dirname);
                ProgressBar allBar = new ProgressBar(configuration.Webs, 100);
                for (int z = 0; z < configuration.Webs; z++)
                {
                    allBar.SetValue(z);
                    Console.SetCursorPosition(1, 1);
                    Console.WriteLine(allBar);
                    int n = 0;
                    n += configuration.BackEnabled ? 1 : 0;
                    n += configuration.GenEnabled ? 1 : 0;
                    n += configuration.SwarmEnabled ? 1 : 0;
                    int k = 0;
                    ProgressBar bar = new ProgressBar(n * configuration.Count, 100);
                    if (configuration.BackEnabled)
                    {
                        FileStream stream = new FileStream(dirname + '\\' + res_back + '_' + configuration.BackInfluence.ToString() + extension, FileMode.Create);
                        StreamWriter writer = new StreamWriter(stream);
                        List<String> tmp = new List<string>();
                        if (File.Exists("1.txt"))
                        {
                            FileStream fileStream = new FileStream("1.txt", FileMode.Open);
                            StreamReader reader = new StreamReader(fileStream);
                            while (!reader.EndOfStream)
                                tmp.Add(reader.ReadLine());
                            fileStream.Close();
                        }
                        else tmp.AddRange(new String[configuration.Count]);
                        Web web = new Web(configuration.Size, configuration.BackInfluence);
                        Stopwatch stopwatch = new Stopwatch();
                        int l = 0;
                        bar.SetValue(0);
                        Console.WriteLine(bar);
                        for (int i = 0; i < configuration.Count; i++)
                        {
                            bar.SetValue(i);
                            if (bar.GetLength() != l)
                            {
                                l = bar.GetLength();
                                Console.SetCursorPosition(1, 3);
                                Console.WriteLine(bar);
                            }
                            stopwatch.Restart();
                            web.Teach(_input1, _output1);
                            stopwatch.Stop();
                            tmp[i] += (web.GetMistake(_input2, _output2) / (_output2.GetLength(0) * _output2.GetLength(1) - 1)).ToString() + "\t" + stopwatch.ElapsedMilliseconds.ToString() + "\t\t";
                        }
                        for (int i = 0; i < configuration.Count; i++)
                            writer.WriteLine(tmp[i]);
                        writer.Flush();
                        writer.Close();
                        stream.Close();
                        File.Delete("1.txt");
                        File.Move(dirname + '\\' + res_back + '_' + configuration.BackInfluence.ToString() + extension, "1.txt");
                        k++;
                    }
                    if (configuration.GenEnabled)
                    {
                        FileStream stream = new FileStream(dirname + '\\' + res_gen
                            + '_' + configuration.GenPoolSize.ToString() + '_' + configuration.GenMutate
                            + '_' + configuration.GenMutateTime.ToString() + '_' + configuration.GenChange.ToString()
                            + '_' + configuration.GenElite.ToString() + '_' + configuration.GenBirth.ToString()
                            + '_' + configuration.GenM.ToString() + extension, FileMode.Create);
                        StreamWriter writer = new StreamWriter(stream);
                        List<String> tmp = new List<string>();
                        if (File.Exists("2.txt"))
                        {
                            FileStream fileStream = new FileStream("2.txt", FileMode.Open);
                            StreamReader reader = new StreamReader(fileStream);
                            while (!reader.EndOfStream)
                                tmp.Add(reader.ReadLine());
                            fileStream.Close();
                        }
                        else tmp.AddRange(new String[configuration.Count]);
                        Genetic.size = configuration.Size;
                        Genetic genetic = new Genetic(configuration.GenPoolSize);
                        genetic.Mutatetime = configuration.GenMutateTime;
                        genetic.MutateRate = configuration.GenMutate;
                        genetic.Change = configuration.GenChange;
                        genetic.Elite = configuration.GenElite;
                        genetic.BirthRate = configuration.GenBirth;
                        genetic.M = configuration.GenM;
                        Stopwatch stopwatch = new Stopwatch();
                        int l = 0;
                        bar.SetValue(k * configuration.Count);
                        Console.SetCursorPosition(1, 3);
                        Console.WriteLine(bar);
                        for (int i = 0; i < configuration.Count; i++)
                        {
                            bar.SetValue(k * configuration.Count + i);
                            if (bar.GetLength() != l)
                            {
                                l = bar.GetLength();
                                Console.SetCursorPosition(1, 3);
                                Console.WriteLine(bar);
                            }
                            stopwatch.Restart();
                            genetic.Generate(_input1, _output1);
                            stopwatch.Stop();
                            tmp[i] += (genetic.GetMistake(_input2, _output2) / (_output2.GetLength(0) * _output2.GetLength(1) - 1)).ToString() + "\t" + stopwatch.ElapsedMilliseconds.ToString() + "\t\t";
                        }
                        for (int i = 0; i < configuration.Count; i++)
                            writer.WriteLine(tmp[i]);
                        writer.Flush();
                        writer.Close();
                        stream.Close();
                        File.Delete("2.txt");
                        File.Move(dirname + '\\' + res_gen
                            + '_' + configuration.GenPoolSize.ToString() + '_' + configuration.GenMutate
                            + '_' + configuration.GenMutateTime.ToString() + '_' + configuration.GenChange.ToString()
                            + '_' + configuration.GenElite.ToString() + '_' + configuration.GenBirth.ToString()
                            + '_' + configuration.GenM.ToString() + extension, "2.txt");
                        k++;
                    }
                    if (configuration.SwarmEnabled)
                    {
                        FileStream stream = new FileStream(dirname + '\\' + res_swarm + '_' + configuration.SwarmPoolSize.ToString() + '_' + configuration.SwarmA1.ToString() + '_' + configuration.SwarmA2.ToString() + '_' + configuration.SwarmXLimit.ToString() + '_' + configuration.SwarmVLimit.ToString() + extension, FileMode.Create);
                        StreamWriter writer = new StreamWriter(stream);
                        List<String> tmp = new List<string>();
                        if (File.Exists("3.txt"))
                        {
                            FileStream fileStream = new FileStream("3.txt", FileMode.Open);
                            StreamReader reader = new StreamReader(fileStream);
                            while (!reader.EndOfStream)
                                tmp.Add(reader.ReadLine());
                            fileStream.Close();
                        }
                        else tmp.AddRange(new String[configuration.Count]);
                        Swarm swarm = new Swarm(configuration.SwarmPoolSize, configuration.Size, configuration.SwarmXLimit, configuration.SwarmVLimit, configuration.SwarmA1, configuration.SwarmA2);
                        swarm.UpdateCoords(_input1, _output1);
                        Stopwatch stopwatch = new Stopwatch();
                        int l = 0;
                        bar.SetValue(k * configuration.Count);
                        Console.SetCursorPosition(1, 3);
                        Console.WriteLine(bar);
                        for (int i = 0; i < configuration.Count; i++)
                        {
                            bar.SetValue(k * configuration.Count + i);
                            if (bar.GetLength() != l)
                            {
                                l = bar.GetLength();
                                Console.SetCursorPosition(1, 3);
                                Console.WriteLine(bar);
                            }
                            stopwatch.Restart();
                            swarm.Move(_input1, _output1);
                            stopwatch.Stop();
                            tmp[i] += (swarm.GetMistake(_input2, _output2) / (_output2.GetLength(0) * _output2.GetLength(1) - 1)).ToString() + "\t" + stopwatch.ElapsedMilliseconds.ToString() + "\t\t";
                        }
                        for (int i = 0; i < configuration.Count; i++)
                            writer.WriteLine(tmp[i]);
                        writer.Flush();
                        writer.Close();
                        stream.Close();
                        File.Delete("3.txt");
                        File.Move(dirname + '\\' + res_swarm + '_' + configuration.SwarmPoolSize.ToString() + '_' + configuration.SwarmA1.ToString() + '_' + configuration.SwarmA2.ToString() + '_' + configuration.SwarmXLimit.ToString() + '_' + configuration.SwarmVLimit.ToString() + extension, "3.txt");
                    }
                }
                File.Move("1.txt", dirname + '\\' + res_back + '_' + configuration.BackInfluence.ToString() + extension);
                File.Move("2.txt", dirname + '\\' + res_gen
                            + '_' + configuration.GenPoolSize.ToString() + '_' + configuration.GenMutate
                            + '_' + configuration.GenMutateTime.ToString() + '_' + configuration.GenChange.ToString()
                            + '_' + configuration.GenElite.ToString() + '_' + configuration.GenBirth.ToString()
                            + '_' + configuration.GenM.ToString() + extension);
                File.Move("3.txt", dirname + '\\' + res_swarm + '_' + configuration.SwarmPoolSize.ToString() + '_' + configuration.SwarmA1.ToString() + '_' + configuration.SwarmA2.ToString() + '_' + configuration.SwarmXLimit.ToString() + '_' + configuration.SwarmVLimit.ToString() + extension);
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine("Для продолжения нажмите Enter...");
                Console.Read();
            }
        }
    }
}
