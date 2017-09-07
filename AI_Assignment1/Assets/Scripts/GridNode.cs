using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_Assignments.Pathfinding
{
    /// <summary>
    /// Represents a single node in a grid
    /// </summary>
    [ExecuteInEditMode]
    public class GridNode : MonoBehaviour
    {
        [SerializeField]
        IntPair m_Coordinate;

        [SerializeField]
        int m_ID = 0;

        [SerializeField]
        List<GridNode> m_AdjacentNodes = new List<GridNode> ();

        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public void SetCoordinate(int x, int y)
        {
            m_Coordinate = new IntPair (x, y);
        }

        /// <summary>
        /// Adds adjacent nodes to the list in this node
        /// </summary>
        public void SetAdjacentNodes(List<GridNode> nodesToAdd)
        {
            for (int i = 0 ; i < nodesToAdd.Count ; ++i )
            {
                if ( !m_AdjacentNodes.Contains (nodesToAdd[i]) ) m_AdjacentNodes.Add (nodesToAdd[i]);
            }
        }

        /// <summary>
        /// Adds a specified node the adjacent list if it isn't already in the list
        /// </summary>
        /// <param name="nodeToAdd"></param>
        public void AddToAdjacentNodes(GridNode nodeToAdd)
        {
            if ( !m_AdjacentNodes.Contains (nodeToAdd) ) m_AdjacentNodes.Add (nodeToAdd);
        }

        void OnDrawGizmosSelected()
        {
            if (m_AdjacentNodes.Count > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube (transform.position, Vector3.one * 1.05f);

                Gizmos.color = Color.green;
                for (int i = 0 ; i < m_AdjacentNodes.Count ; ++i )
                {
                    Gizmos.DrawCube (m_AdjacentNodes[i].transform.position, Vector3.one * 1.1f);
                }
            }
        }

        public List<GridNode> GetAdjacentNodes()
        {
            return m_AdjacentNodes;
        }
    }

    [System.Serializable]
    class IntPair
    {
        [SerializeField]
        int m_X = 0;
        [SerializeField]
        int m_Y = 0;

        public IntPair(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        public int X
        {
            get { return m_X; }
        }

        public int Y
        {
            get { return m_Y; }
        }
    }
}