using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace algorithmSA
{
    internal class SimulatedAnnealing
    {
        private double temperature;
        private double coolingRate;
        private long shortestCycleDistanse;
        private int[] shortestCycle;
        AdjacencyMatrix matrix;

        public SimulatedAnnealing(double temperature, double coolingRate, int[,] matrix)
        {
            this.temperature = temperature;
            this.coolingRate = coolingRate;
            this.matrix = new AdjacencyMatrix(matrix);
        }

        public long GetShortestCycleDistance()
        {
            return shortestCycleDistanse;
        }

        public String GetShortestHamiltonianCycle()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < shortestCycle.Length; i++)
            {
                sb.Append(shortestCycle[i]).Append(" -> ");
            }
            sb.Append(shortestCycle[0]);
            return sb.ToString();
        }

        // Поиск кратчайшего гамельтонова пути с помощью алгоритма имитации отжига
        public void FindShortestHamiltonianСycle() 
        {
            // Задание начального решения 
            int[] currentCycle = Enumerable.Range(0, matrix.GetSize()).ToArray();
            // Определение длины пути начального решения 
            long distance = GetNewCycleDistanse(currentCycle);
            while (temperature > 0)
            {
                int[] newCycle = GetNewCycle(currentCycle);
                
                long newDistance = GetNewCycleDistanse(newCycle);

                long energyNewState = newDistance - distance;

                if(energyNewState < 0 || GetProbabilityAcceptingNewState(energyNewState))
                {
                    currentCycle = newCycle;
                    distance = newDistance;
                }  

                temperature -= coolingRate;
            }
            shortestCycleDistanse = distance;
            shortestCycle = currentCycle;
        }

        // Модифицирование решения случайным образом
        private int[] GetNewCycle(int[] currentRoute) 
        {
            int[] route = new int[currentRoute.Length];
            Array.Copy(currentRoute, route, currentRoute.Length);
            Random random = new Random();
            int indexFirst = random.Next(1, matrix.GetSize());
            int indexSecond;
            do
            {
                indexSecond = random.Next(1, matrix.GetSize());
            } while (indexFirst == indexSecond);
            int temp = route[indexFirst];
            route[indexFirst] = route[indexSecond];
            route[indexSecond] = temp;
            return route;
        }

        // Вычисление энергии (длины пути) полученного состояния
        private long GetNewCycleDistanse(int[] route) 
        {
            long distance = 0;
            for (int i = 0; i < route.Length - 1; i++)
            {
                distance += matrix.Get(route[i], route[i + 1]);
            }
            distance += matrix.Get(route[route.Length - 1], route[0]);
            return distance;
        }

        // Определение вероятности принятия нового состояния
        private bool GetProbabilityAcceptingNewState(long energyNewState) 
        {
            double k = 1.38e-23; // Постоянная Больцмана
            Random random = new Random();
            return Math.Exp(-energyNewState / (temperature * k)) > random.NextDouble();
        }
    }
}