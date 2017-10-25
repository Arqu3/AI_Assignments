using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        GridCreator m_Creator = null;

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
        public void Setup ()
        {
            m_Nodes.Add (new GridList ());
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
                if ( x >= 0 && x < m_Nodes[y].Count )
                {
                    GridNode node = m_Nodes[y].GetNode (x);
                    if (node.Walkable) return m_Nodes[y].GetNode (x);
                }
            }
            return null;
        }

        /// <summary>
        /// The node where the current agent should start
        /// </summary>
        public GridNode StartNode
        {
            get { return m_StartNode; }
            set
            {
                m_StartNode = value;
                m_StartNode.IsStart = true;
                m_StartNode.IsEnd = false;
                m_StartNode.Walkable = true;
            }
        }

        /// <summary>
        /// The node that the current agent should look for
        /// </summary>
        public GridNode EndNode
        {
            get { return m_EndNode; }
            set
            {
                m_EndNode = value;
                m_EndNode.IsStart = false;
                m_EndNode.IsEnd = true;
                m_EndNode.Walkable = true;
            }
        }

        public List<GridNode> Nodes
        {
            get { return m_CompleteNodesList; }
            set { m_CompleteNodesList = value; }
        }

        public GridCreator Creator
        {
            get { return m_Creator; }
            set { m_Creator = value; }
        }

        public void UpdateColors()
        {
            for (int i = 0 ; i < m_CompleteNodesList.Count ; ++i )
            {
                m_CompleteNodesList[i].UpdateColor ();
            }
        }

        public void UpdateEditorColors ()
        {
            for ( int i = 0 ; i < m_CompleteNodesList.Count ; ++i )
            {
                m_CompleteNodesList[i].UpdateEditorColor ();
            }
        }
    }

    [System.Serializable]
    public class GridCreator
    {
        List<GridNode> m_Nodes;
        GridController m_Controller;

        public GridCreator()
        {
            m_Nodes = new List<GridNode> ();
        }

        public void Generate ( GameObject grid, GameObject prefab, int x, int y, bool randomCost = false )
        {
            //Setup grid parent object and other local variables
            m_Nodes = new List<GridNode> ();
            GridController controller = grid.AddComponent<GridController> ();
            controller.Setup ();
            grid.transform.position = Vector3.zero;
            int currentX = 0;
            int currentY = 0;
            int total = x * y;
            float offset = 1.1f;

            for ( int i = 0 ; i < total ; ++i )
            {
                //Create node gameobject
                GameObject go = Object.Instantiate (prefab, new Vector3 (offset * currentX, 0.0f, offset * currentY), Quaternion.identity);
                go.transform.SetParent (grid.transform, true);

                //Get gridnode script and setup node properties if != null
                GridNode node = go.GetComponent<GridNode> ();
                if ( node )
                {
                    m_Nodes.Add (node);

                    node.ID = i;
                    node.SetCoordinate (currentX, currentY);

                    if ( randomCost ) node.Cost = Random.Range (1f, 3.0f);
                    if ( i == 0 ) node.IsStart = true;
                    else if ( i == total - 1 ) node.IsEnd = true;
                }

                //Add node to the current row in the gridcontroller
                if ( currentX < x ) controller.AddNode (currentY, node);

                //Increment row position, add new row if it has reached the end
                ++currentX;
                if ( currentX >= x )
                {
                    ++currentY;
                    if ( currentY < y )
                    {
                        currentX = 0;
                        controller.AddRow ();
                    }
                }
            }

            //Setup each node's adjacent nodes in the grid
            for ( int i = 0 ; i < m_Nodes.Count ; ++i )
            {
                int x1 = m_Nodes[i].X;
                int y1 = m_Nodes[i].Y;

                m_Nodes[i].AddToAdjacentNodes (controller.GetNode (y1 - 1, x1));
                m_Nodes[i].AddToAdjacentNodes (controller.GetNode (y1 + 1, x1));
                m_Nodes[i].AddToAdjacentNodes (controller.GetNode (y1, x1 - 1));
                m_Nodes[i].AddToAdjacentNodes (controller.GetNode (y1, x1 + 1));
            }

            controller.Nodes = m_Nodes;
            controller.StartNode = m_Nodes[0];
            controller.EndNode = m_Nodes[m_Nodes.Count - 1];
        }

        public void Reassign()
        {
            if ( !m_Controller )
            {
                m_Controller = Object.FindObjectOfType<GridController> ();
                m_Nodes = m_Controller.Nodes;
            }

            for ( int i = 0 ; i < m_Nodes.Count ; ++i )
            {
                m_Nodes[i].ClearAdjacentList ();
                if ( !m_Nodes[i].Walkable ) continue;
                int x = m_Nodes[i].X;
                int y = m_Nodes[i].Y;

                m_Nodes[i].AddToAdjacentNodes (m_Controller.GetNode (y - 1, x));
                m_Nodes[i].AddToAdjacentNodes (m_Controller.GetNode (y + 1, x));
                m_Nodes[i].AddToAdjacentNodes (m_Controller.GetNode (y, x - 1));
                m_Nodes[i].AddToAdjacentNodes (m_Controller.GetNode (y, x + 1));
            }
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
