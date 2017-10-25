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

                GridCreator creator = new GridCreator ();
                creator.Generate (new GameObject (m_GridParentName), m_GridPrefab, m_XAmount, m_YAMount);
                FindObjectOfType<GridController> ().Creator = creator;
                FindObjectOfType<GridController> ().UpdateEditorColors ();

                m_WillGenerate = false;
            }
            else
            {
                GridController controller = FindObjectOfType<GridController> ();
                if (controller)
                {
                    if ( GUILayout.Button ("Re-assign adjacent nodes") )
                    {
                        controller.Creator.Reassign ();
                        controller.UpdateEditorColors ();
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
