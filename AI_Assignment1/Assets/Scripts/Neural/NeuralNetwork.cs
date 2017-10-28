using System;

namespace AI_Assignments.Neural
{
    public class Layer
    {
        #region Private fields

        int m_NumberOfInputs;
        int m_NumberOfOutputs;

        float m_LearningRate;

        float[] m_Outputs;
        float[] m_Inputs;
        float[,] m_Weights;
        float[,] m_WeightsDelta;

        float[] m_Gamma;
        float[] m_Error;

        #endregion

        public Layer(int numberOfInputs, int numberOfOutputs, float learningRate = 0.033f)
        {
            m_NumberOfInputs = numberOfInputs;
            m_NumberOfOutputs = numberOfOutputs;

            m_Outputs = new float[m_NumberOfOutputs];
            m_Inputs = new float[m_NumberOfInputs];

            m_Weights = new float[m_NumberOfOutputs, m_NumberOfInputs];
            m_WeightsDelta = new float[m_NumberOfOutputs, m_NumberOfInputs];

            m_Gamma = new float[m_NumberOfOutputs];
            m_Error = new float[m_NumberOfOutputs];

            m_LearningRate = learningRate;

            InitializeWeights ();
        }

        void InitializeWeights()
        {
            for (int i = 0 ; i < m_NumberOfOutputs ; ++i )
            {
                for (int j = 0 ; j < m_NumberOfInputs ; ++j )
                {
                    m_Weights[i, j] = UnityEngine.Random.Range (-0.5f, 0.5f);
                }
            }
        }

        #region Accessors

        public float[] Outputs
        {
            get { return m_Outputs; }
        }

        public float[] Gamma
        {
            get { return m_Gamma; }
        }

        public float[,] Weights
        {
            get { return m_Weights; }
        }

        #endregion

        /// <summary>
        /// Feeds input into the layer
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public float[] FeedForward(float[] inputs)
        {
            m_Inputs = inputs;

            for (int i = 0 ; i < m_NumberOfOutputs ; ++i )
            {
                m_Outputs[i] = 0;

                for (int j = 0 ; j < m_NumberOfInputs ; ++j )
                {
                    m_Outputs[i] += inputs[j] * m_Weights[i, j];
                }

                m_Outputs[i] = (float)Math.Tanh (m_Outputs[i]);
            }

            return m_Outputs;
        }

        /// <summary>
        /// Maps the input value between -1 and 1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        float TanHDeri(float value)
        {
            return 1f - ( value * value );
        }

        /// <summary>
        /// Calculates and distributes the error across the layer
        /// </summary>
        /// <param name="expected"></param>
        public void BackPropOutput(float[] expected)
        {
            for ( int i = 0 ; i < m_NumberOfOutputs ; ++i )
            {
                m_Error[i] = m_Outputs[i] - expected[i];
            }

            for ( int i = 0 ; i < m_NumberOfOutputs ; ++i )
            {
                m_Gamma[i] = m_Error[i] * TanHDeri (m_Outputs[i]);
            }

            for (int i = 0 ; i < m_NumberOfOutputs ; ++i )
            {
                for (int j = 0 ; j < m_NumberOfInputs ; ++j )
                {
                    m_WeightsDelta[i, j] = m_Gamma[i] * m_Inputs[j];
                }
            }
        }

        public void BackPropHidden(float[] gammaForward, float[,] weightsForward)
        {
            for (int i = 0 ; i < m_NumberOfOutputs ; ++i )
            {
                m_Gamma[i] = 0;

                for (int j = 0 ; j < gammaForward.Length ; ++j )
                {
                    m_Gamma[i] += gammaForward[j] * weightsForward[j, i];
                }

                m_Gamma[i] *= TanHDeri (m_Outputs[i]);
            }

            for ( int i = 0 ; i < m_NumberOfOutputs ; ++i )
            {
                for ( int j = 0 ; j < m_NumberOfInputs ; ++j )
                {
                    m_WeightsDelta[i, j] = m_Gamma[i] * m_Inputs[j];
                }
            }
        }

        public void UpdateWeights()
        {
            for (int i = 0 ; i < m_NumberOfOutputs ; ++i )
            {
                for (int j = 0 ; j < m_NumberOfInputs ; ++j)
                {
                    m_Weights[i, j] -= m_WeightsDelta[i, j] * m_LearningRate;
                }
            }
        }
    }

    public class NeuralNetwork
    {
        #region Private fields

        int[] m_Layer;
        Layer[] m_Layers;

        #endregion

        public NeuralNetwork (int[] layer)
        {
            m_Layer = new int[layer.Length];
            for (int i = 0 ; i < layer.Length ; ++i )
            {
                m_Layer[i] = layer[i];
            }

            m_Layers = new Layer[layer.Length - 1];

            for (int i = 0 ; i < m_Layers.Length ; ++i )
            {
                m_Layers[i] = new Layer (m_Layer[i], m_Layer[i + 1]);
            }
        }

        /// <summary>
        /// Feeds input into the network
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public float[] FeedForward(float[] inputs)
        {
            m_Layers[0].FeedForward (inputs);
            for (int i = 1 ; i < m_Layers.Length ; ++i )
            {
                m_Layers[i].FeedForward (m_Layers[i - 1].Outputs);
            }

            return m_Layers[m_Layers.Length - 1].Outputs;
        }

        /// <summary>
        /// Distributes the error between expected and current values across the network
        /// </summary>
        /// <param name="expected"></param>
        public void BackProp(float[] expected)
        {
            for ( int i = m_Layers.Length - 1 ; i >= 0 ; --i )
            {
                if ( i == m_Layers.Length - 1 ) m_Layers[i].BackPropOutput (expected);
                else m_Layers[i].BackPropHidden (m_Layers[i + 1].Gamma, m_Layers[i + 1].Weights);
            }

            for ( int i = 0; i < m_Layers.Length ; ++i )
            {
                m_Layers[i].UpdateWeights();
            }
        }
    }
}
