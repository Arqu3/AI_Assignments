using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AI_Assignments.Pathfinding;

namespace AI_Assignments.Editor
{
    [ExecuteInEditMode]
    public class GridGenerator : EditorWindow
    {
        #region Private fields

        //Internal variables
        string m_GridParentName = "Grid";
        bool m_WillGenerate = false;
        GameObject m_GridPrefab = null;
        int m_XAmount = 10;
        int m_YAMount = 10;
        int m_CurrentY = 0;
        int m_CurrentX = 0;
        bool m_RandomCost = false;
        List<GridNode> m_Nodes = new List<GridNode> ();

        #endregion

        [MenuItem("Tools/Gridgenerator _F6")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow editorWindow = GetWindow (typeof (GridGenerator));
            editorWindow.autoRepaintOnSceneChange = true;
            editorWindow.Show ();
            GUIContent title = new GUIContent ("Gridgen");
            editorWindow.titleContent = title;
            editorWindow.minSize = new Vector2 (400, 200);
            editorWindow.maxSize = new Vector2 (440, 220);
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField ("Select grid prefab", EditorStyles.boldLabel);
            m_GridPrefab = EditorGUILayout.ObjectField (m_GridPrefab, typeof (GameObject), true, null) as GameObject;
            if ( !m_GridPrefab ) m_GridPrefab = Resources.Load ("GridNode") as GameObject;

            EditorGUILayout.Space ();

            EditorGUILayout.LabelField ("Grid size options", EditorStyles.boldLabel);
            m_XAmount = Mathf.Clamp (EditorGUILayout.IntField ("Number of X nodes", m_XAmount), 1, 100);
            m_YAMount = Mathf.Clamp (EditorGUILayout.IntField ("Number of Y nodes", m_YAMount), 1, 100);
            m_RandomCost = EditorGUILayout.Toggle("Randomize cost", m_RandomCost);

            EditorGUILayout.Space ();

            if ( GUILayout.Button ("Genrate") ) GenerateGrid ();

            EditorGUILayout.Space ();

            GameObject grid = GameObject.Find (m_GridParentName);
            if ( grid )
            {
                if ( GUILayout.Button ("Remove grid") ) DestroyImmediate (grid);
            }

            if (m_WillGenerate)
            {
                if ( grid )
                {
                    Debug.Log ("Another grid was found, removing it");
                    DestroyImmediate (grid);
                }

                //Setup grid parent object and other local variables
                m_Nodes = new List<GridNode> ();
                grid = new GameObject (m_GridParentName);
                GridController controller = grid.AddComponent<GridController> ();
                controller.Setup ();
                grid.transform.position = Vector3.zero;
                m_CurrentY = 0;
                m_CurrentX = 0;
                int total = m_XAmount * m_YAMount;
                float offset = 1.1f;

                for (int i = 0 ; i < total ; ++i )
                {
                    //Create node gameobject
                    GameObject go = Instantiate (m_GridPrefab, new Vector3 (offset * m_CurrentX, 0.0f, offset * m_CurrentY), Quaternion.identity);
                    go.transform.SetParent (grid.transform, true);

                    //Get gridnode script and setup node properties if != null
                    GridNode node = go.GetComponent<GridNode> ();
                    if ( node )
                    {
                        m_Nodes.Add (node);

                        node.ID = i;
                        node.SetCoordinate (m_CurrentX, m_CurrentY);

                        if (m_RandomCost) node.Cost = Random.Range(1f, 3.0f);
                        if (i == 0) node.IsStart = true;
                        else if (i == total - 1) node.IsEnd = true;
                    }

                    //Add node to the current row in the gridcontroller
                    if (m_CurrentX < m_XAmount) controller.AddNode(m_CurrentY, node);

                    //Increment row position, add new row if it has reached the end
                    ++m_CurrentX;
                    if ( m_CurrentX >= m_XAmount )
                    {
                        ++m_CurrentY;
                        if (m_CurrentY < m_YAMount)
                        {
                            m_CurrentX = 0;
                            controller.AddRow();
                        }
                    }
                }

                //Setup each node's adjacent nodes in the grid
                for (int i = 0; i < m_Nodes.Count; ++i)
                {
                    int x = m_Nodes[i].X;
                    int y = m_Nodes[i].Y;

                    m_Nodes[i].AddToAdjacentNodes(controller.GetNode(y - 1, x));
                    m_Nodes[i].AddToAdjacentNodes(controller.GetNode(y + 1, x));
                    m_Nodes[i].AddToAdjacentNodes(controller.GetNode(y, x - 1));
                    m_Nodes[i].AddToAdjacentNodes(controller.GetNode(y, x + 1));
                }

                controller.Nodes = m_Nodes;

                m_WillGenerate = false;
            }
            else if (grid)
            {
                GridController controller = grid.GetComponent<GridController> ();
                if (controller)
                {
                    if ( GUILayout.Button ("Re-assign adjacent nodes") )
                    {
                        m_Nodes = new List<GridNode> ();
                        var nodes = FindObjectsOfType<GridNode> ();
                        foreach(GridNode node in nodes)
                        {
                            m_Nodes.Add (node);
                        }

                        for ( int i = 0 ; i < m_Nodes.Count ; ++i )
                        {
                            m_Nodes[i].ClearAdjacentList ();
                            if ( !m_Nodes[i].Walkable ) continue;
                            int x = m_Nodes[i].X;
                            int y = m_Nodes[i].Y;

                            m_Nodes[i].AddToAdjacentNodes (controller.GetNode (y - 1, x));
                            m_Nodes[i].AddToAdjacentNodes (controller.GetNode (y + 1, x));
                            m_Nodes[i].AddToAdjacentNodes (controller.GetNode (y, x - 1));
                            m_Nodes[i].AddToAdjacentNodes (controller.GetNode (y, x + 1));
                        }
                    }
                }
            }
        }

        void GenerateGrid()
        {
            m_WillGenerate = true;
        }
    }
}
