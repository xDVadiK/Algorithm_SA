using System;
using System.Linq;
using System.Text;

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

        internal AdjacencyMatrix AdjacencyMatrix
        {
            get => default;
            set
            {
            }
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

        // Search for the shortest Hamiltonian path using an annealing simulation algorithm
        public void FindShortestHamiltonianСycle() 
        {
            // Setting the initial solution
            int[] currentCycle = Enumerable.Range(0, matrix.GetSize()).ToArray();
            // Determining the path length of the initial solution 
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

        // Modifying the solution randomly
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

        // Calculation of energy (path length) of the obtained state
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

        // Determining the probability of adopting a new state
        private bool GetProbabilityAcceptingNewState(long energyNewState) 
        {
            double k = 1.38e-23; // Boltzmann constant
            Random random = new Random();
            return Math.Exp(-energyNewState / (temperature * k)) > random.NextDouble();
        }
    }
}