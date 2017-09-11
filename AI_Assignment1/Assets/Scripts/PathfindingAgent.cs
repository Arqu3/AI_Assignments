﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_Assignments.Pathfinding
{
    public class PathfindingAgent : MonoBehaviour
    {
        List<GridNode> m_ClosedList = new List<GridNode>();
        List<GridNode> m_OpenList = new List<GridNode>();

        GridController m_Controller;
        bool m_FoundTarget = false;

        void Awake()
        {
            m_Controller = FindObjectOfType<GridController>();
        }

        void Start()
        {
            GridNode start = m_Controller.StartNode;
            GridNode end = m_Controller.EndNode;
            start.Searched = true;
            start.Taken = true;

            Search(start, end);
        }

        /// <summary>
        /// Performs A* search on the available grid given the start-and-end-nodes
        /// </summary>
        /// <param name="startnode"></param>
        /// <param name="endnode"></param>
        void Search(GridNode startnode, GridNode endnode)
        {
            if (!m_ClosedList.Contains(startnode))
            {
                GridNode currentNode = startnode;
                //Current cost
                float g = currentNode.Cost;
                float h = ManhattanDistance(currentNode, endnode);
                //Current "value" of moving from one node to another
                float f = g + h;

                //Never do a search through more than the available nodes
                for (int i = 0; i < m_Controller.Nodes.Count; ++i)
                {
                    //Look for adjacent nodes, add them to the open list if they're not in the closed one
                    for (int j = 0; j < currentNode.AdjacentNodes.Count; ++j)
                    {
                        //print(j + " fist iteration");
                        if (!m_ClosedList.Contains(currentNode.AdjacentNodes[j]))
                        {
                            currentNode.AdjacentNodes[j].Searched = true;
                            m_OpenList.Add(currentNode.AdjacentNodes[j]);
                        }
                    }
                    m_ClosedList.Add(currentNode);

                    //Go through the current open list and chose the move valuable node to go to
                    float gg = g;
                    f = g + m_OpenList[0].Cost + ManhattanDistance(m_OpenList[0], endnode);
                    bool chosen = false;
                    for (int j = 0; j < m_OpenList.Count; ++j)
                    {
                        if (m_OpenList[j].IsEnd)
                        {
                            currentNode = m_OpenList[j];
                            break;
                        }

                        float localg = g + m_OpenList[j].Cost;
                        float localh = ManhattanDistance(m_OpenList[j], endnode);
                        float localf = localg + localh;

                        if (localf <= f || m_OpenList.Count == 1 || (!chosen && j == m_OpenList.Count - 1))
                        {
                            gg = localg;
                            currentNode = m_OpenList[j];
                            chosen = true;
                        }

                        f = localf;
                        if (!m_ClosedList.Contains(m_OpenList[j])) m_ClosedList.Add(m_OpenList[j]);
                    }

                    g = gg;

                    m_OpenList.Clear();
                    currentNode.Taken = true;
                    if (currentNode.IsEnd) break;
                }
            }
        }

        /// <summary>
        /// Returns the cost of the distance between from and to, measured by only moving either X or Y once per iteration (no diagonal movement)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        float ManhattanDistance(GridNode from, GridNode to)
        {
            if (from != to)
            {
                float cost = 0.0f;
                int x = from.X;
                int y = from.Y;
                for (int i = 0; i < m_Controller.Nodes.Count; ++i)
                {
                    if (x == to.X && y == to.Y) break;

                    if (x < to.X) ++x;
                    else if (x > to.X) --x;
                    else
                    {
                        if (y < to.Y) ++y;
                        else if (y > to.Y) --y;
                    }

                    cost += 1.0f;
                }
                return cost;
            }
            return -1.0f;
        }
    }
}
