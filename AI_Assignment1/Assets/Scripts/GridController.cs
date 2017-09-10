using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_Assignments.Pathfinding
{
    public class GridController : MonoBehaviour
    {
        #region Private fields

        [SerializeField]
        List<GridList> m_Nodes = new List<GridList>();
        [SerializeField]
        List<GridNode> m_CompleteNodesList = new List<GridNode>();

        [SerializeField]
        GridNode m_StartNode = null;
        [SerializeField]
        GridNode m_EndNode = null;

        #endregion

        void Awake()
        {
            SetupInternal();
        }

        /// <summary>
        /// Sets up start and end node
        /// </summary>
        void SetupInternal()
        {
            for (int i = 0; i < m_Nodes.Count; ++i)
            {
                if (m_StartNode && m_EndNode) break;

                for (int j = 0; j < m_Nodes[i].Count; ++j)
                {
                    if (m_StartNode && m_EndNode) break;

                    GridNode node = m_Nodes[i].GetNode(j);
                    if (node.IsStart) m_StartNode = node;
                    else if (node.IsEnd) m_EndNode = node;
                }
            }
        }

        /// <summary>
        /// Adds an initial row to the controller
        /// </summary>
        public void Setup()
        {
            m_Nodes.Add (new GridList());
        }

        /// <summary>
        /// Adds a new node to the specified row in the controller
        /// </summary>
        /// <param name="y"></param>
        /// <param name="nodeToAdd"></param>
        public void AddNode(int y, GridNode nodeToAdd)
        {
            m_Nodes[y].Add (nodeToAdd);
        }

        /// <summary>
        /// Adds a new row in the controller
        /// </summary>
        public void AddRow()
        {
            m_Nodes.Add(new GridList());
        }

        /// <summary>
        /// Returns a node from a specified X and Y if it exists, otherwise returns null
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public GridNode GetNode(int y, int x)
        {
            if (y >= 0 && y < m_Nodes.Count)
            {
                if (x >= 0 && x < m_Nodes[y].Count) return m_Nodes[y].GetNode(x);
            }
            return null;
        }

        /// <summary>
        /// The node where the current agent should start
        /// </summary>
        public GridNode StartNode
        {
            get { return m_StartNode; }
        }

        /// <summary>
        /// The node that the current agent should look for
        /// </summary>
        public GridNode EndNode
        {
            get { return m_EndNode; }
        }

        public List<GridNode> Nodes
        {
            get { return m_CompleteNodesList; }
            set { m_CompleteNodesList = value; }
        }
    }

    /// <summary>
    /// Serializable Gridnode container-class
    /// </summary>
    [System.Serializable]
    public class GridList
    {
        [SerializeField]
        List<GridNode> m_List = new List<GridNode>();

        public void Add(GridNode node)
        {
            m_List.Add(node);
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public GridNode GetNode(int index)
        {
            return m_List[index];
        }
    }
}
