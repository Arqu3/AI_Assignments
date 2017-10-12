﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_Assignments.Neural
{
    public class NeuralTester : MonoBehaviour
    {
        #region Private fields



        #endregion

        void Start ()
        {
            NeuralNetwork net = new NeuralNetwork (new int[] { 3, 25, 25, 1 });

            for (int i = 0 ; i < 5000 ; ++i )
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

            Debug.Log (net.FeedForward (new float[] { 0, 0, 0 })[0]);
            Debug.Log (net.FeedForward (new float[] { 0, 0, 1 })[0]);
            Debug.Log (net.FeedForward (new float[] { 0, 1, 0 })[0]);
            Debug.Log (net.FeedForward (new float[] { 1, 0, 0 })[0]);
            Debug.Log (net.FeedForward (new float[] { 0, 1, 1 })[0]);
            Debug.Log (net.FeedForward (new float[] { 1, 0, 1 })[0]);
            Debug.Log (net.FeedForward (new float[] { 1, 1, 0 })[0]);
            Debug.Log (net.FeedForward (new float[] { 1, 1, 1 })[0]);
        }
    }
}