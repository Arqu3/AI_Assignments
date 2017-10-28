using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AI_Assignments.Genetic
{
    public class GeneticHandler : MonoBehaviour
    {
        #region Exposed fields

        [Header ("Algorithm variables")]
        [SerializeField]
        string m_TargetString = "Look mom, no hands!";
        [SerializeField]
        string m_ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$%&/()=? ";
        [SerializeField]
        int m_PopulationSize = 200;
        [SerializeField]
        float m_MutationRate = 0.01f;
        [SerializeField]
        int m_Elitism = 5;

        [Header ("Component variables")]
        [SerializeField]
        int m_NumCharText = 15000;
        [SerializeField]
        Text m_TargetText;
        [SerializeField]
        Text m_CurrentText;
        [SerializeField]
        Text m_GenerationText;
        [SerializeField]
        Text m_FillText;

        #endregion

        #region Private fields

        GeneticAlgorithm<char> m_GA;
        int m_CurrentNum = 0;

        #endregion

        void Start ()
        {
            m_TargetText.text = "Target:\n" + m_TargetString;
            m_GA = new GeneticAlgorithm<char> (m_PopulationSize, m_TargetString.Length, GetRandomCharacter, GetFitness, m_Elitism, m_MutationRate);
        }

        void Update ()
        {
            m_GA.NewGeneration ();
            UpdateTexts ();

            if ( m_GA.BestFitness >= 1f ) enabled = false;
        }

        char GetRandomCharacter()
        {
            return m_ValidCharacters[Random.Range (0, m_ValidCharacters.Length)];
        }

        float GetFitness(int index)
        {
            float score = 0.0f;
            DNA<char> dna = m_GA.Population[index];

            for (int i = 0 ; i < dna.Genes.Length ; ++i )
            {
                if ( dna.Genes[i] == m_TargetString[i] ) ++score;
            }

            score /= m_TargetString.Length;

            float pow = 5f;
            return score = ( Mathf.Pow (pow, score) - 1 ) / ( pow - 1 );
        }

        void UpdateTexts()
        {
            m_CurrentText.text = "Current:\n" + CharArrayToString (m_GA.BestGenes);
            m_GenerationText.text = "Generation: " + m_GA.Generation.ToString ();
            for (int i = 0 ; i < m_GA.Population.Count ; ++i )
            {
                m_CurrentNum += m_GA.Population[i].Genes.Length;
                if (m_CurrentNum >= m_NumCharText)
                {
                    m_FillText.text = "";
                    m_CurrentNum = 0;
                }

                string s = CharArrayToString (m_GA.Population[i].Genes);
                m_FillText.text += s + "\n";
            }
        }

        string CharArrayToString(char[] array)
        {
            var sb = new StringBuilder ();

            foreach(var c in array)
            {
                sb.Append (c);
            }

            return sb.ToString ();
        }
    }
}