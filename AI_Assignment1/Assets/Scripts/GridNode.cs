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
        #region Exposed fields

        [Header("Node properties")]
        [SerializeField]
        [Range(0.0f, 500.0f)]
        float m_Weight = 1.0f;
        [SerializeField]
        bool m_Walkable = true;

        [Header ("Is this node the start or end node?")]
        [SerializeField]
        bool m_IsStart = false;
        [SerializeField]
        bool m_IsEnd = false;

        [Header("Exposed fields that are stored from editor to play-mode")]
        [SerializeField]
        IntPair m_Coordinate;
        [SerializeField]
        int m_ID = 0;
        [SerializeField]
        List<GridNode> m_AdjacentNodes = new List<GridNode>();

        #endregion

        bool m_Searched = false;
        bool m_Taken = false;

        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public int X
        {
            get { return m_Coordinate.X; }
        }

        public int Y
        {
            get { return m_Coordinate.Y; }
        }

        public bool IsStart
        {
            get { return m_IsStart; }
            set { m_IsStart = value; }
        }

        public bool IsEnd
        {
            get { return m_IsEnd; }
            set { m_IsEnd = value; }
        }

        public bool Searched
        {
            get { return m_Searched; }
            set { m_Searched = value; }
        }

        public bool Taken
        {
            get { return m_Taken; }
            set { m_Taken = value; }
        }

        public float Cost
        {
            get { return m_Weight; }
            set { m_Weight = value; }
        }

        public bool Walkable
        {
            get { return m_Walkable; }
            set { m_Walkable = value; }
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
            if (nodeToAdd)
            {
                if (!m_AdjacentNodes.Contains(nodeToAdd)) m_AdjacentNodes.Add(nodeToAdd);
            }
        }

        void OnDrawGizmos()
        {
            if (m_IsStart)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(transform.position + Vector3.up, 0.3f);
            }
            else if (m_IsEnd)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(transform.position + Vector3.up, 0.3f);
            }

            if (m_Taken)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(transform.position, Vector3.one * 1.05f);
                return;
            }
            else if (m_Searched)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(transform.position, Vector3.one * 1.05f);
            }
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

        public List<GridNode> AdjacentNodes
        {
            get { return m_AdjacentNodes; }
        }

        public void ClearAdjacentList()
        {
            m_AdjacentNodes.Clear ();
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