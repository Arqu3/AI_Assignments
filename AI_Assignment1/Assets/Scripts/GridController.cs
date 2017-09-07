using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_Assignments.Pathfinding
{
    public class GridController : MonoBehaviour
    {
        [SerializeField]
        List<List<GridNode>> m_Nodes = new List<List<GridNode>> ();

        public void Setup()
        {
            m_Nodes.Add (new List<GridNode> ());
        }

        public void AddNode(int y, GridNode nodeToAdd, bool newRow)
        {
            print (y);
            if ( newRow ) m_Nodes.Add (new List<GridNode> ());
            m_Nodes[y].Add (nodeToAdd);
        }

        public GridNode GetNode(int y, int x)
        {
            return m_Nodes[y][x];
        }
    }
}
