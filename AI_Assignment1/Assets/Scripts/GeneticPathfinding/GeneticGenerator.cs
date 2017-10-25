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
        [SerializeField]
        GridCreator.AdjacentMode m_AdjacentMode = GridCreator.AdjacentMode.XFIRST;

        #endregion

        #region Private fields

        GeneticAlgorithm<int> m_Generator;
        int m_Total = 0;
        GridController m_Controller;
        PathfindingAgent m_Agent;
        bool m_ShouldUpdate = true;

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

        public int Y
        {
            get { return m_Rows; }
            set { m_Rows = value; }
        }

        public int X
        {
            get { return m_Columns; }
            set { m_Columns = value; }
        }

        int GetRandomInt()
        {
            return Random.Range (1, 11);
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
                m_Controller.Nodes[i].Walkable = dna.Genes[i] <= 6;
                if ( !m_Controller.Nodes[i].Walkable ) fitness += 1f;
            }

            m_Controller.Creator.Reassign (m_AdjacentMode);
            GridNode start = m_Controller.StartNode;
            start.Searched = true;
            start.Taken = true;
            GridNode end = m_Controller.EndNode;
            end.Searched = false;
            end.Taken = false;
            if ( m_Agent.Search (start, end) ) fitness += m_Agent.PathLength * 4f + m_Agent.Steps;
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