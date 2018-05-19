using MLP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swarm_Net
{
    class Program
    {
        private static void LoadData()
        {
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
        }

        private static double[,] _input1 = new double[120, 4];
        private static double[,] _input2 = new double[30, 4];
        private static double[,] _output1 = new double[120, 3];
        private static double[,] _output2 = new double[30, 3];


        static void Main(string[] args)
        {
            LoadData();
            Genetic.InputSize = 4;
            Genetic.HiddenSize = 4;
            Genetic.OutputSize = 3;
            File.Delete("1.txt");
            File.Delete("2.txt");
            File.Delete("3.txt");
            for (int z = 0; z < 20; z++)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(z.ToString());
                FileStream stream1 = new FileStream("res_swarm.txt", FileMode.Create);
                //FileStream stream1 = new FileStream("res_gen.txt", FileMode.Create);
                StreamWriter writer1 = new StreamWriter(stream1);
                FileStream stream2 = new FileStream("res_genTeach.txt", FileMode.Create);
                StreamWriter writer2 = new StreamWriter(stream2);
                FileStream stream3 = new FileStream("res_web.txt", FileMode.Create);
                StreamWriter writer3 = new StreamWriter(stream3);
                List<String> res_gen = new List<string>();
                List<String> res_genTeach = new List<string>();
                List<String> res_web = new List<string>();
                if (File.Exists("1.txt"))
                {
                    FileStream stream = new FileStream("1.txt", FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        res_gen.Add(reader.ReadLine());
                    }
                    stream.Close();
                }
                else res_gen.AddRange(new String[1000]);
                if (File.Exists("2.txt"))
                {
                    FileStream stream = new FileStream("2.txt", FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        res_genTeach.Add(reader.ReadLine());
                    }
                    stream.Close();
                }
                else res_genTeach.AddRange(new String[1000]);
                if (File.Exists("3.txt"))
                {
                    FileStream stream = new FileStream("3.txt", FileMode.Open);
                    StreamReader reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        res_web.Add(reader.ReadLine());
                    }
                    stream.Close();
                }
                else res_web.AddRange(new String[1000]);
                Swarm swarm = new Swarm(new int[] { 4, 4, 3 });
                swarm.UpdateCoords(_input1, _output1);
                Genetic genetic = new Genetic();
                Genetic geneticTeach = new Genetic();
                Web web = new Web(new int[]{4, 4, 3});
                genetic.Add(web);
                geneticTeach.Add(web);
                for (int i=0;i<1000;i++)
                {
                    web.Teach(_input1, _output1);
                    swarm.Move(_input1, _output1);
                    res_web[i]+="\t\t"+web.GetMistake(_input2, _output2).ToString();
                    res_gen[i] += "\t\t" + swarm.GetMistake(_input2, _output2).ToString(); ;
                }
                for (int i=0;i<res_gen.Count;i++)
                {
                    writer1.WriteLine(res_gen[i]);
                }
                for (int i = 0; i < res_genTeach.Count; i++)
                {
                    writer2.WriteLine(res_genTeach[i]);
                }
                for (int i = 0; i < res_web.Count; i++)
                {
                    writer3.WriteLine(res_web[i]);
                }
                writer1.Flush();
                writer2.Flush();
                writer3.Flush();
                writer1.Close();
                writer2.Close();
                writer3.Close();
                stream1.Close();
                stream2.Close();
                stream3.Close();
                File.Delete("1.txt");
                File.Delete("2.txt");
                File.Delete("3.txt");
                File.Move("res_swarm.txt", "1.txt");
                File.Move("res_genTeach.txt", "2.txt");
                File.Move("res_web.txt", "3.txt");
            }
            File.Move("1.txt", "res_swarm.txt");
        }
    }
}
