using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Swarm_Utility
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static long FactFactor(int n)
        {
            if (n < 0)
                return 0;
            if (n == 0)
                return 1;
            if (n == 1 || n == 2)
                return n;
            bool[] u = new bool[n + 1]; // маркеры для решета Эратосфена
            List<Tuple<int, int>> p = new List<Tuple<int, int>>(); // множители и их показатели степеней
            for (int i = 2; i <= n; ++i)
                if (!u[i]) // если i - очередное простое число
                {
                    // считаем показатель степени в разложении
                    int k = n / i;
                    int c = 0;
                    while (k > 0)
                    {
                        c += k;
                        k /= i;
                    }
                    // запоминаем множитель и его показатель степени
                    p.Add(new Tuple<int, int>(i, c));
                    // просеиваем составные числа через решето               
                    int j = 2;
                    while (i * j <= n)
                    {
                        u[i * j] = true;
                        ++j;
                    }
                }
            // вычисляем факториал
            long r = 1;
            for (int i = p.Count() - 1; i >= 0; --i)
                r *= (long)Math.Pow(p[i].Item1, p[i].Item2);
            return r;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            try
            {
                progressBar1.Maximum = (int)numericUpDown1.Value;
                FileStream stream = new FileStream(textBox1.Text + ".txt", FileMode.Create);
                StreamWriter writer = new StreamWriter(stream);
                for (int i = 0; i < numericUpDown1.Value; i++)
                {
                    progressBar1.Value = i;
                    double x1 = random.NextDouble() * 49 + 1;
                    double x2 = random.NextDouble() * 49 + 1;
                    double x3 = random.NextDouble() * 49 + 1;
                    writer.Write(x1);
                    writer.Write('\t');
                    writer.Write(x2);
                    writer.Write('\t');
                    writer.Write(x3);
                    writer.Write('|');
                    double y1, y2, y3 = 0;
                    y1 = Math.Pow(x1, 3) * Math.Cos(x2) / ((Math.Log(x3) + Math.Sin(x1)) * Math.Pow(x2, 2));
                    writer.Write(y1);
                    writer.Write('\t');
                    y2 = (x1 - x2) / (x3 - x2) + Math.Cos(x1) * Math.Sin(x2);
                    writer.Write(y2);
                    writer.Write('\t');
                    y3 = (x1 * x2 + x2 * x3 + x3 * x1) * Math.Sin(x1) * Math.Cos(x2) / (Math.Sin(x1) + Math.Cos(x2) + Math.Log(x3));
                    writer.WriteLine(y3);
                }
                progressBar1.Value = 0;
                writer.Flush();
                writer.Close();
                stream.Close();
                MessageBox.Show("Успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
