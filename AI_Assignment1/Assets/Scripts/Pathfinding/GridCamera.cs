using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_Assignments.Pathfinding.Camera
{
    public class GridCamera : MonoBehaviour
    {
        [SerializeField]
        float m_AdditionalOffset = 0.0f;

        void Start ()
        {
            GridController grid = FindObjectOfType<GridController> ();

            Vector3 pos = Vector3.zero;
            for (int i = 0 ; i < grid.Nodes.Count ; ++i )
            {
                pos += grid.Nodes[i].transform.position;
            }
            pos = pos / grid.Nodes.Count;

            Vector3 localpos = transform.position;
            localpos.x = pos.x;
            localpos.z = pos.z;
            localpos.y = pos.x + pos.z + m_AdditionalOffset;
            transform.position = localpos;
        }
    }
}