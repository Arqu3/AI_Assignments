using System;

namespace AI_Assignments.Genetic
{
    public class DNA<T>
    {
        #region Private fields

        T[] m_Genes;
        float m_Fitness = 0;

        Func<T> m_GetRandomGene;
        Func<int, float> m_GetFitness;

        #endregion

        public DNA ( int size, Func<T> getRandomGene, Func<int, float> getFitness, bool initGenes = true )
        {
            m_Genes = new T[size];
            m_GetRandomGene = getRandomGene;
            m_GetFitness = getFitness;

            if ( initGenes )
            {
                for ( int i = 0 ; i < m_Genes.Length ; ++i )
                {
                    m_Genes[i] = m_GetRandomGene ();
                }
            }
        }

        public float Fitness
        {
            get { return m_Fitness; }
        }

        public T[] Genes
        {
            get { return m_Genes; }
        }

        public float CalculateFitness ( int index )
        {
            return m_Fitness = m_GetFitness (index);
        }

        public DNA<T> CrossOver ( DNA<T> other, float chance = 0.5f )
        {
            DNA<T> child = new DNA<T> (m_Genes.Length, m_GetRandomGene, m_GetFitness, false);
            for ( int i = 0 ; i < m_Genes.Length ; ++i )
            {
                child.Genes[i] = UnityEngine.Random.Range (0f, 1f) < chance ? m_Genes[i] : other.Genes[i];
            }
            return child;
        }

        public void Mutate ( float chance )
        {
            for ( int i = 0 ; i < m_Genes.Length ; ++i )
            {
                if ( UnityEngine.Random.Range (0f, 1f) < chance ) m_Genes[i] = m_GetRandomGene ();
            }
        }
    }
}