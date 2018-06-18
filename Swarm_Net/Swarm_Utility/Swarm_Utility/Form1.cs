using MLP;
using Swarm_Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Swarm_Utility
{
    public partial class Form1 : Form
    {
        private bool changed = false;
        public Form1()
        {
            InitializeComponent();
            if (File.Exists("mlp.conf"))
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Configuration));
                StreamReader file = new StreamReader(@"mlp.conf");
                Configuration configuration = (Configuration)reader.Deserialize(file);
                file.Close();
                textBox1.Text = configuration.Filename;
                textBox2.Text = "";
                for (int i = 0; i < configuration.Size.Length; i++)
                    textBox2.Text += configuration.Size[i].ToString() + ';';
                textBox2.Text = textBox2.Text.TrimEnd(';');
                if (configuration.BackEnabled)
                {
                    checkBox1.Checked = true;
                    textBox3.Text = configuration.BackInfluence.ToString();
                }
                else checkBox1.Checked = false;
                if (configuration.GenEnabled)
                {
                    checkBox2.Checked = true;
                    numericUpDown1.Value = configuration.GenPoolSize;
                    numericUpDown2.Value = configuration.GenMutateTime;
                    textBox11.Text = configuration.GenMutate.ToString();
                    textBox12.Text = configuration.GenChange.ToString();
                    textBox13.Text = configuration.GenElite.ToString();
                    textBox14.Text = configuration.GenBirth.ToString();
                    numericUpDown4.Value = configuration.GenM;
                }
                else checkBox2.Checked = false;
                if (configuration.SwarmEnabled)
                {
                    checkBox3.Checked = true;
                    numericUpDown3.Value = configuration.SwarmPoolSize;
                    textBox5.Text = configuration.SwarmA1.ToString();
                    textBox6.Text = configuration.SwarmA2.ToString();
                    textBox7.Text = configuration.SwarmXLimit.ToString();
                    textBox8.Text = configuration.SwarmVLimit.ToString();
                }
                else checkBox3.Checked = false;
                numericUpDown5.Value = configuration.Webs;
                numericUpDown6.Value = configuration.Count;
            }
            changed = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            panel4.Enabled = checkBox1.Checked;
            changed = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            panel3.Enabled = checkBox2.Checked;
            changed = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Enabled = checkBox3.Checked;
            changed = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void Save()
        {
            if (changed)
            {
                Configuration configuration = new Configuration();
                configuration.Filename = textBox1.Text;
                string[] s_size = textBox2.Text.Split(';');
                int[] size = new int[s_size.Length];
                for (int i = 0; i < s_size.Length; i++)
                {
                    if (!int.TryParse(s_size[i], out size[i]))
                    {
                        MessageBox.Show("Неверный формат данных в поле 'Размер сети'!");
                        return;
                    }
                }
                configuration.Size = size;
                configuration.Webs = (int)numericUpDown5.Value;
                configuration.Count = (int)numericUpDown6.Value;
                if (checkBox1.Checked)
                {
                    configuration.BackEnabled = true;
                    double d;
                    if (!double.TryParse(textBox3.Text.Replace('.', ','), out d))
                    {
                        MessageBox.Show("Неверный формат числа в поле 'Коэффициент обучения'!");
                        return;
                    }
                    else configuration.BackInfluence = d;
                }
                else configuration.BackEnabled = false;
                if (checkBox2.Checked)
                {
                    configuration.GenEnabled = true;
                    configuration.GenPoolSize = (int)numericUpDown1.Value;
                    configuration.GenMutateTime = (int)numericUpDown2.Value;
                    double d;
                    if (!double.TryParse(textBox11.Text.Replace('.', ','), out d))
                    {
                        MessageBox.Show("Неверный формат числа в поле 'Вероятность мутации'!");
                        return;
                    }
                    else configuration.GenMutate = d;
                    if (!double.TryParse(textBox12.Text.Replace('.', ','), out d))
                    {
                        MessageBox.Show("Неверный формат числа в поле 'Вероятность сдвига веса'!");
                        return;
                    }
                    else configuration.GenChange = d;
                    if (!double.TryParse(textBox13.Text.Replace('.', ','), out d))
                    {
                        MessageBox.Show("Неверный формат числа в поле 'Доля элиты'!");
                        return;
                    }
                    else configuration.GenElite = d;
                    if (!double.TryParse(textBox14.Text.Replace('.', ','), out d))
                    {
                        MessageBox.Show("Неверный формат числа в поле 'Доля рожденных'!");
                        return;
                    }
                    else configuration.GenBirth = d;
                    configuration.GenM = (int)numericUpDown4.Value;
                }
                else configuration.GenEnabled = false;
                if (checkBox3.Checked)
                {
                    configuration.SwarmEnabled = true;
                    configuration.SwarmPoolSize = (int)numericUpDown3.Value;
                    double d;
                    if (!double.TryParse(textBox5.Text.Replace('.', ','), out d))
                    {
                        MessageBox.Show("Неверный формат числа в поле 'Влияние локального ускорения'!");
                        return;
                    }
                    else configuration.SwarmA1 = d;
                    if (!double.TryParse(textBox6.Text.Replace('.', ','), out d))
                    {
                        MessageBox.Show("Неверный формат числа в поле 'Влияние глобального ускорения'!");
                        return;
                    }
                    else configuration.SwarmA2 = d;
                    if (!double.TryParse(textBox7.Text.Replace('.', ','), out d))
                    {
                        MessageBox.Show("Неверный формат числа в поле 'Разброс координат'!");
                        return;
                    }
                    else configuration.SwarmXLimit = d;
                    if (!double.TryParse(textBox8.Text.Replace('.', ','), out d))
                    {
                        MessageBox.Show("Неверный формат числа в поле 'Разброс скорости'!");
                        return;
                    }
                    else configuration.SwarmVLimit = d;
                }
                else configuration.SwarmEnabled = false;
                if (File.Exists("mlp.conf"))
                {
                    if (MessageBox.Show("Файл настроек уже существует. Перезаписать?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Delete("mlp.conf");
                    }
                    else return;
                }
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Configuration));
                FileStream stream = File.Create("mlp.conf");
                writer.Serialize(stream, configuration);
                stream.Close();
                changed = false;
            }
            MessageBox.Show("Сохранение успешно!");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changed)
            {
                if (MessageBox.Show("Хотите сохранить данные перед выходом?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Save();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Save();
            Main();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            changed = false;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            changed = true;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            changed = true;
        }

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
                    MessageBox.Show("Ошибка в файле настроек!");
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

        private static double[,] _input1;
        private static double[,] _input2;
        private static double[,] _output1;
        private static double[,] _output2;
        private static string res_swarm = "swarm";
        private static string res_back = "back";
        private static string res_gen = "gen";
        private static string extension = ".txt";
        private static Configuration configuration;

        void Main()
        {
            try
            {
                panel1.Enabled = false;
                panel2.Enabled = false;
                panel3.Enabled = false;
                panel4.Enabled = false;
                LoadData();
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
                progressBar1.Maximum = configuration.Webs;
                for (int z = 0; z < configuration.Webs; z++)
                {
                    progressBar1.Value = z;
                    int n = 0;
                    n += configuration.BackEnabled ? 1 : 0;
                    n += configuration.GenEnabled ? 1 : 0;
                    n += configuration.SwarmEnabled ? 1 : 0;
                    int k = 0;
                    progressBar2.Maximum = n * configuration.Count;
                    progressBar2.Value = 0;
                    this.Update();
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
                        for (int i = 0; i < configuration.Count; i++)
                        {
                            progressBar2.Value = i;
                            this.Update();
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
                        FileStream Stream = new FileStream(dirname + '\\' + "example" + '_' + res_back + '_' + configuration.BackInfluence.ToString() + extension, FileMode.Create);
                        StreamWriter streamWriter = new StreamWriter(Stream);
                        for (int i = 0; i < _output2.GetLength(0); i++)
                        {
                            double[] otmp = new double[_output2.GetLength(1)];
                            double[] itmp = new double[_input2.GetLength(1)];
                            for (int j = 0; j < _output2.GetLength(1); j++)
                            {
                                streamWriter.Write(_output2[i, j].ToString() + '\t');
                            }
                            for (int j = 0; j < _input2.GetLength(1); j++)
                                itmp[j] = _input2[i, j];
                            web.Input = itmp;
                            otmp = web.Output;
                            streamWriter.Write("|\t");
                            for (int j = 0; j < otmp.GetLength(0); j++)
                            {
                                streamWriter.Write(otmp[j].ToString() + '\t');
                            }
                            streamWriter.WriteLine();
                        }
                        streamWriter.Flush();
                        streamWriter.Close();
                        Stream.Close();
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
                        progressBar2.Value = k * configuration.Count;
                        for (int i = 0; i < configuration.Count; i++)
                        {
                            progressBar2.Value = k * configuration.Count + i;
                            this.Update();
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
                        FileStream Stream = new FileStream(dirname + '\\' + "example" + '_' + res_gen
                            + '_' + configuration.GenPoolSize.ToString() + '_' + configuration.GenMutate
                            + '_' + configuration.GenMutateTime.ToString() + '_' + configuration.GenChange.ToString()
                            + '_' + configuration.GenElite.ToString() + '_' + configuration.GenBirth.ToString()
                            + '_' + configuration.GenM.ToString() + extension, FileMode.Create);
                        StreamWriter streamWriter = new StreamWriter(Stream);
                        for (int i = 0; i < _output2.GetLength(0); i++)
                        {
                            double[] otmp = new double[_output2.GetLength(1)];
                            double[] itmp = new double[_input2.GetLength(1)];
                            for (int j = 0; j < _output2.GetLength(1); j++)
                            {
                                streamWriter.Write(_output2[i, j].ToString() + '\t');
                            }
                            for (int j = 0; j < _input2.GetLength(1); j++)
                                itmp[j] = _input2[i, j];
                            Web web = genetic.GetWeb();
                            web.Input = itmp;
                            otmp = web.Output;
                            streamWriter.Write("|\t");
                            for (int j = 0; j < otmp.GetLength(0); j++)
                            {
                                streamWriter.Write(otmp[j].ToString() + '\t');
                            }
                            streamWriter.WriteLine();
                        }
                        streamWriter.Flush();
                        streamWriter.Close();
                        Stream.Close();
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
                        for (int i = 0; i < configuration.Count; i++)
                        {
                            progressBar2.Value = k * configuration.Count + i;
                            this.Update();
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
                        File.Move(dirname + '\\' + res_swarm + '_' + configuration.SwarmPoolSize.ToString() + '_' + configuration.SwarmA1.ToString() 
                            + '_' + configuration.SwarmA2.ToString() + '_' + configuration.SwarmXLimit.ToString() + '_' + configuration.SwarmVLimit.ToString() + extension, "3.txt");
                        FileStream Stream = new FileStream(dirname + '\\' + "example" + '_' + res_swarm + '_' + configuration.SwarmPoolSize.ToString() + '_' + configuration.SwarmA1.ToString() 
                            + '_' + configuration.SwarmA2.ToString() + '_' + configuration.SwarmXLimit.ToString() + '_' + configuration.SwarmVLimit.ToString() + extension, FileMode.Create);
                        StreamWriter streamWriter = new StreamWriter(Stream);
                        for (int i = 0; i < _output2.GetLength(0); i++)
                        {
                            double[] otmp = new double[_output2.GetLength(1)];
                            double[] itmp = new double[_input2.GetLength(1)];
                            for (int j = 0; j < _output2.GetLength(1); j++)
                            {
                                streamWriter.Write(_output2[i, j].ToString() + '\t');
                            }
                            for (int j = 0; j < _input2.GetLength(1); j++)
                                itmp[j] = _input2[i, j];
                            Web web = swarm.GetWeb();
                            web.Input = itmp;
                            otmp = web.Output;
                            streamWriter.Write("|\t");
                            for (int j = 0; j < otmp.GetLength(0); j++)
                            {
                                streamWriter.Write(otmp[j].ToString() + '\t');
                            }
                            streamWriter.WriteLine();
                        }
                        streamWriter.Flush();
                        streamWriter.Close();
                        Stream.Close();
                    }
                }
                if (File.Exists("1.txt")) File.Move("1.txt", dirname + '\\' + res_back + '_' + configuration.BackInfluence.ToString() + extension);
                if (File.Exists("2.txt")) File.Move("2.txt", dirname + '\\' + res_gen
                            + '_' + configuration.GenPoolSize.ToString() + '_' + configuration.GenMutate
                            + '_' + configuration.GenMutateTime.ToString() + '_' + configuration.GenChange.ToString()
                            + '_' + configuration.GenElite.ToString() + '_' + configuration.GenBirth.ToString()
                            + '_' + configuration.GenM.ToString() + extension);
                if (File.Exists("3.txt")) File.Move("3.txt", dirname + '\\' + res_swarm + '_' + configuration.SwarmPoolSize.ToString() + '_' + configuration.SwarmA1.ToString() + '_' + configuration.SwarmA2.ToString() + '_' + configuration.SwarmXLimit.ToString() + '_' + configuration.SwarmVLimit.ToString() + extension);
                MessageBox.Show("Обучение завершено!");
            }
            /*catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }*/
            finally
            {
                panel1.Enabled = true;
                panel2.Enabled = true;
                panel3.Enabled = true;
                panel4.Enabled = true;
            }
        }
    }
}

