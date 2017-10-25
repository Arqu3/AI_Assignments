using System.Collections.Generic;
using System;

namespace AI_Assignments.Genetic
{
    public class GeneticAlgorithm<T>
    {
        #region Private fields

        List<DNA<T>> m_Population = new List<DNA<T>> ();
        int m_Generation = 0;
        float m_BestFitness = 0f;
        T[] m_BestGenes;
        int m_Elitism = 0;
        float m_MutationRate = 0.0f;

        List<DNA<T>> m_NewPopulation = new List<DNA<T>> ();
        float m_FitnessSum = 0.0f;

        #endregion

        public GeneticAlgorithm ( int populationSize, int dnaSize, Func<T> getRandomGene, Func<int, float> getFitness, int elitism, float mutationRate = 0.01f)
        {
            m_Generation = 1;
            m_Elitism = elitism;
            m_MutationRate = mutationRate;
            m_Population = new List<DNA<T>> (populationSize);
            m_NewPopulation = new List<DNA<T>> (populationSize);

            m_BestGenes = new T[dnaSize];

            for (int i = 0 ; i < populationSize ; ++i )
            {
                m_Population.Add (new DNA<T> (dnaSize, getRandomGene, getFitness, true));
            }
        }

        public void NewGeneration()
        {
            if ( m_Population.Count <= 0 ) return;

            CalculateFitness ();
            m_Population.Sort (CompareDNA);
            m_NewPopulation.Clear ();

            for (int i = 0 ; i < m_Population.Count ; ++i )
            {
                if ( i < m_Elitism ) m_NewPopulation.Add (m_Population[i]);
                else
                {
                    DNA<T> parent1 = ChooseParent ();
                    DNA<T> parent2 = ChooseParent ();

                    DNA<T> child = parent1.CrossOver (parent2);
                    child.Mutate (m_MutationRate);
                    m_NewPopulation.Add (child);
                }
            }

            List<DNA<T>> tmp = m_Population;
            m_Population = m_NewPopulation;
            m_NewPopulation = tmp;

            ++m_Generation;
        }

        public void CalculateFitness()
        {
            m_FitnessSum = 0.0f;
            DNA<T> best = m_Population[0];

            for (int i = 0 ; i < m_Population.Count ; ++i )
            {
                m_FitnessSum += m_Population[i].CalculateFitness(i);
                if ( m_Population[i].Fitness > best.Fitness ) best = Population[i];
            }

            m_BestFitness = best.Fitness;
            best.Genes.CopyTo (m_BestGenes, 0);
        }

        DNA<T> ChooseParent()
        {
            float random = UnityEngine.Random.Range (0f, 1f) * m_FitnessSum;

            for (int i = 0 ; i < m_Population.Count ; ++i )
            {
                if ( random < m_Population[i].Fitness ) return m_Population[i];

                random -= m_Population[i].Fitness;
            }
            return m_Population[0];
        }

        public int CompareDNA(DNA<T> a, DNA<T> b)
        {
            if ( a.Fitness > b.Fitness ) return -1;
            else if ( b.Fitness > a.Fitness ) return 1;
            else return 0;
        }

        public List<DNA<T>> Population
        {
            get { return m_Population; }
        }

        public int Generation
        {
            get { return m_Generation; }
        }

        public float BestFitness
        {
            get { return m_BestFitness; }
        }

        public T[] BestGenes
        {
            get { return m_BestGenes; }
        }

        public int Elitism
        {
            get { return m_Elitism; }
            set { m_Elitism = value; }
        }

        public float MutationRate
        {
            get { return m_MutationRate; }
            set { m_MutationRate = value; }
        }
    }
}