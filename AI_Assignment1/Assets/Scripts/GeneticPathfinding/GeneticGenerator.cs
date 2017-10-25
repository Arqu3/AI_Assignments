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

        #endregion

        #region Private fields

        GeneticAlgorithm<int> m_Generator;
        int m_Total = 0;
        GridController m_Controller;
        PathfindingAgent m_Agent;

        #endregion

        private void Start ()
        {
            m_Total = m_Rows * m_Columns;

            m_Generator = new GeneticAlgorithm<int> (m_PopulationSize, m_Total, GetRandomInt, GetFitness, m_Elitism, m_MutationRate);
            m_Controller = FindObjectOfType<GridController> ();
            m_Agent = FindObjectOfType<PathfindingAgent> ();
        }

        private void Update ()
        {
            //if ( Input.GetKeyDown (KeyCode.Space) )
                Generate ();
        }

        public void Generate ()
        {
            m_Generator.NewGeneration ();
        }

        int GetRandomInt()
        {
            return Random.Range (0, 10);
        }

        float GetFitness(int index)
        {
            float fitness = 0.0f;
            DNA<int> dna = m_Generator.Population[index];
            m_Agent.ClearInformation ();


            for (int i = 0 ; i < dna.Genes.Length ; ++i )
            {
                if ( i <= 0 || i >= dna.Genes.Length - 1 ) continue;

                m_Controller.Nodes[i].ResetInformation ();
                m_Controller.Nodes[i].Walkable = dna.Genes[i] <= 5;
                if ( !m_Controller.Nodes[i].Walkable ) fitness += 1f;
            }

            m_Controller.Creator.Reassign ();
            GridNode start = m_Controller.StartNode;
            start.Searched = true;
            start.Taken = true;
            if ( m_Agent.Search (start, m_Controller.EndNode) ) fitness += m_Agent.PathLength * 2f + m_Agent.Steps;
            else
            {
                m_Agent.ClearInformation ();
                return 0.0f;
            }

            if ( index >= m_Generator.Population.Count - 1 ) m_Controller.UpdateColors ();

            fitness /= m_Total;

            float pow = 5f;
            return fitness = ( Mathf.Pow (pow, fitness) - 1 ) / ( pow - 1 );
        }
    }

}