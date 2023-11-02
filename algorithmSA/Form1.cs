using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace algorithmSA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TemperatureBox.Text = "10000";
            CoolingRateBox.Text = "0,1";
            GraphSelection.SelectedIndex = 0;
            label1.Text = "Результат";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double temperature = double.Parse(TemperatureBox.Text);
                if (temperature < 0)
                {
                    throw new Exception("Отрицательное значение температуры");
                }
                double coolingRate = double.Parse(CoolingRateBox.Text);
                if (coolingRate < 0)
                {
                    throw new Exception("Отрицательное значение коэффициента охлаждения");
                }
                int[,] graph = ParseGraph(textBox3.Text);
                SimulatedAnnealing annealing = new SimulatedAnnealing(temperature, coolingRate, graph);
                annealing.FindShortestHamiltonianСycle();
                String path = "Путь: " + annealing.GetShortestHamiltonianCycle();
                String distance = "Длина: " + annealing.GetShortestCycleDistance();
                label1.Text = distance + "\r\n" + path;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Исключительная ситуация");
            }
        }

        private int[,] ParseGraph(string graph)
        {
            char[] chars = { '\r', '\n' };
            string[] rows = graph.Trim().Split(chars);

            List<string> list = new List<string>(rows);
            list.RemoveAll(string.IsNullOrEmpty);
            rows = list.ToArray();

            int rowCount = rows.Length;

            int[,] matrix = new int[rowCount, rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                string[] values = rows[i].Split(' ');

                if (values.Length != rowCount)
                {
                    throw new Exception("Несимметричный двумерный массив");
                }

                for (int j = 0; j < rowCount; j++)
                {
                    if (!int.TryParse(values[j], out int element))
                    {
                        throw new Exception("Некорректное значение элемента графа");
                    }
                    matrix[i, j] = element;
                }
            }

            return matrix;
        }

        private void GraphSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(GraphSelection.SelectedIndex)
            {                                       
                case 0: textBox3.Text = "0 5 2 3\r\n5 0 1 1\r\n2 1 0 1\r\n3 1 1 0\r\n"; break;
                case 1: textBox3.Text = "0 1 4 5 6 8 5\r\n1 0 5 9 7 3 4\r\n4 5 0 1 4 2 4\r\n5 9 1 0 4 2 2\r\n6 7 4 4 0 1 3\r\n8 3 2 2 1 0 9\r\n5 4 4 2 3 9 0\r\n"; break;
                case 2: textBox3.Text = ""; break;
            }
        }
    }
}
