using UnityEngine;
using UnityEngine.UI;

namespace AI_Assignments.Neural
{
    public class NeuralTester : MonoBehaviour
    {
        #region Exposed fields

        [SerializeField]
        Text m_Text;

        #endregion

        void Start ()
        {
            SimulateNetwork ();
        }

        public void SimulateNetwork()
        {
            int inputNeurons = 3;
            int hiddenNeurons1 = 10;
            int hiddenNeurons2 = 5;
            int outputNeurons = 1;

            NeuralNetwork net = new NeuralNetwork (new int[] { inputNeurons, hiddenNeurons1, hiddenNeurons2, outputNeurons });

            int iterations = 5000;

            for ( int i = 0 ; i < iterations ; ++i )
            {
                net.FeedForward (new float[] { 0, 0, 0 });
                net.BackProp (new float[] { 0 });

                net.FeedForward (new float[] { 0, 0, 1 });
                net.BackProp (new float[] { 1 });

                net.FeedForward (new float[] { 0, 1, 0 });
                net.BackProp (new float[] { 1 });

                net.FeedForward (new float[] { 1, 0, 0 });
                net.BackProp (new float[] { 1 });

                net.FeedForward (new float[] { 0, 1, 1 });
                net.BackProp (new float[] { 0 });

                net.FeedForward (new float[] { 1, 0, 1 });
                net.BackProp (new float[] { 0 });

                net.FeedForward (new float[] { 1, 1, 0 });
                net.BackProp (new float[] { 0 });

                net.FeedForward (new float[] { 1, 1, 1 });
                net.BackProp (new float[] { 1 });
            }

            m_Text.text = "Iterating " + iterations + " times on a neural network with:\n" +
                "Input layer with: " + inputNeurons + " neurons,\n" +
                "2 hidden layers with: " + hiddenNeurons1 + " and " + hiddenNeurons2 + " neurons each,\n" +
                "Output layer with: " + outputNeurons + " neurons\n\n" +
                "Result:\n" +
                net.FeedForward (new float[] { 0, 0, 0 })[0] + ", expected was 0\n" +
                net.FeedForward (new float[] { 0, 0, 1 })[0] + ", expected was 1\n" +
                net.FeedForward (new float[] { 0, 1, 0 })[0] + ", expected was 1\n" +
                net.FeedForward (new float[] { 1, 0, 0 })[0] + ", expected was 1\n" +
                net.FeedForward (new float[] { 0, 1, 1 })[0] + ", expected was 0\n" +
                net.FeedForward (new float[] { 1, 0, 1 })[0] + ", expected was 0\n" +
                net.FeedForward (new float[] { 1, 1, 0 })[0] + ", expected was 0\n" +
                net.FeedForward (new float[] { 1, 1, 1 })[0] + ", expected was 1\n";
        }
    }
}