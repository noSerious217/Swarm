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
        private bool changed = true;
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
                    if (!double.TryParse(textBox3.Text.Replace('.',','), out d))
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
            MessageBox.Show("Успешно!");
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
            Process.Start("Swarm_Net.exe");
            this.Close();
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
    }
}
