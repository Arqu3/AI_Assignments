using UnityEngine;
using AI_Assignments.Genetic;
using AI_Assignments.Pathfinding;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        [Header ("Fitness function variables")]
        [SerializeField]
        float m_WallIncrease = 1.0f;
        [SerializeField]
        float m_PathMultiplier = 4.0f;
        [SerializeField]
        float m_StepMultiplier = 1.0f;

        [Header ("Assignable variables")]
        [SerializeField]
        Text m_GenerationText;

        #endregion

        #region Private fields

        GeneticAlgorithm<int> m_Generator;
        int m_Total = 0;
        GridController m_Controller;
        PathfindingAgent m_Agent;
        bool m_ShouldUpdate = false;

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
            if ( Input.GetKeyDown (KeyCode.Escape) ) Exit ();

            m_GenerationText.text = "Generation: " + m_Generator.Generation;

            if ( !m_ShouldUpdate ) return;

            Generate ();
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
        }

        public void Exit()
        {
            Application.Quit ();
        }

        public void ToggleUpdate()
        {
            m_ShouldUpdate = !m_ShouldUpdate;
        }

        public void SetPopulation(float value)
        {
            m_PopulationSize = (int)value;
            m_Generator = new GeneticAlgorithm<int> (m_PopulationSize, m_Total, GetRandomInt, GetFitness, m_Elitism, m_MutationRate);
        }

        public void SetElitism(float value)
        {
            m_Generator.Elitism = m_Elitism = (int)value;
        }

        public void SetMutationRate(float value)
        {
            m_Generator.MutationRate = m_MutationRate = value;
        }

        public void Generate ()
        {
            m_Generator.NewGeneration ();
        }

        /// <summary>
        /// Shows the current best generation
        /// </summary>
        public void SetBestGeneration()
        {
            int[] best = m_Generator.BestGenes;

            for (int i = 0 ; i < best.Length ; ++i )
            {
                if ( i <= 0 || i >= best.Length - 1 ) continue;
                m_Controller.Nodes[i].ResetInformation ();
                m_Controller.Nodes[i].Walkable = best[i] <= 6;
            }

            m_Agent.ClearInformation ();
            m_Controller.Creator.Reassign (m_Controller.AdjacentMode);
            GridNode start = m_Controller.StartNode;
            start.Searched = true;
            start.Taken = true;
            GridNode end = m_Controller.EndNode;
            end.Searched = false;
            end.Taken = false;
            m_Agent.Search (start, end);
            m_Controller.UpdateColors ();
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

        /// <summary>
        /// Calculates fitness for the genetic generator
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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
                if ( !m_Controller.Nodes[i].Walkable ) fitness += m_WallIncrease;
            }

            m_Controller.Creator.Reassign (m_Controller.AdjacentMode);
            GridNode start = m_Controller.StartNode;
            start.Searched = true;
            start.Taken = true;
            GridNode end = m_Controller.EndNode;
            end.Searched = false;
            end.Taken = false;
            if ( m_Agent.Search (start, end) ) fitness += m_Agent.PathLength * m_PathMultiplier + m_Agent.Steps * m_StepMultiplier;
            else return 0.0f;

            if ( index >= m_Generator.Population.Count - 1 ) m_Controller.UpdateColors ();

            fitness /= m_Total;

            float pow = 5f;
            return fitness = ( Mathf.Pow (pow, fitness) - 1 ) / ( pow - 1 );
        }
    }
}