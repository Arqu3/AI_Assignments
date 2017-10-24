using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI_Assignments.Genetic;
using AI_Assignments.Pathfinding;

namespace AI_Assignments.GeneticPathfinding
{
    public class GeneticGenerator : MonoBehaviour
    {
        #region Exposed fields

        [Header ("Labyrinth generation variables")]
        [SerializeField]
        int m_Rows = 10;
        [SerializeField]
        int m_Columns = 10;
        [SerializeField]
        int m_PopulationSize = 200;
        [SerializeField]
        int m_Elitism = 5;
        [SerializeField]
        float m_MutationRate = 0.01f;

        [Header ("Spawnable prefabs")]
        [SerializeField]
        GameObject[] m_NodePrefabs;

        #endregion

        #region Private fields

        GeneticAlgorithm<int> m_Generator;
        int m_Total = 0;

        #endregion

        private void Start ()
        {
            m_Total = m_Rows * m_Columns;

            m_Generator = new GeneticAlgorithm<int> (m_PopulationSize, m_Total, GetRandomInt, GetFitness, m_Elitism, m_MutationRate);
        }

        private void Update ()
        {
            //if ( Input.GetKeyDown (KeyCode.Space) )
                Generate ();
        }

        public void Generate ()
        {
            m_Generator.NewGeneration ();
            //print (m_Generator.BestFitness);
        }

        GameObject GetRandomObject()
        {
            return Instantiate (m_NodePrefabs[Random.Range (0, m_NodePrefabs.Length)]);
        }

        int GetRandomInt()
        {
            return Random.Range (1, 4);
        }

        float GetFitness(int index)
        {
            float fitness = 0.0f;
            DNA<int> dna = m_Generator.Population[index];

            for (int i = 0 ; i < dna.Genes.Length - 1 ; ++i )
            {
                if ( dna.Genes[i] == dna.Genes[i + 1] ) ++fitness;
            }

            fitness /= m_Total;

            float pow = 5f;
            return fitness = ( Mathf.Pow (pow, fitness) - 1 ) / ( pow - 1 );
        }
    }

}