using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderScript: MonoBehaviour
{

    PathRequestManager requestManager;

    private GridScript grid;

    private void Awake()
    {
        grid = GetComponent<GridScript>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPosition, Vector3 goalPosition)
    {
        StartCoroutine(FindPath(startPosition, goalPosition));
    }


    IEnumerator FindPath(Vector3 startPosition, Vector3 goalPosition)
    {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        
        Node startNode = grid.FromWorldToGrid(startPosition);
        Node goalNode = grid.FromWorldToGrid(goalPosition);



        // we need open set -> nodes that need to be evaluated
        // closed set -> nodes that were explored
        if (!startNode.obstacle && !goalNode.obstacle)
        {
            Heap<Node> openSet = new Heap<Node>(grid.getGrid().Length);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count != 0)
            {

                Node current = openSet.RemoveFirst();
                closedSet.Add(current);

                // if goal, you can compare nodes, cuz they will actually be the same node from the grid array
                if (current == goalNode)
                {
                    pathSuccess = true;
                    break;
                }
                // if not goal
                // go through children
                List<Node> children = grid.GetChildren(current);
                foreach (Node kid in children)
                {
                    if (!closedSet.Contains(kid))
                    {
                        float newG = current.gCost + 1;
                        if (!openSet.Contains(kid) || newG < kid.gCost)
                        {
                            kid.gCost = newG;
                            kid.parent = current;
                            kid.hCost = GetDistance(kid, goalNode);

                            if (!openSet.Contains(kid))
                            {
                                openSet.Add(kid);
                            }
                            else
                            {
                                openSet.UpdateItem(kid);
                            }
                        }
                    }

                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, goalNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
      
    }
    
    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node current = endNode;
        while(current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }
        Vector3[] waypoints = path.Count == 1 ? new Vector3[] { path[0].position } : SimplePath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplePath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDir = Vector2.zero;

        for(int i = 0; i < path.Count-1; i++)
        {

            Vector2 newDir = new Vector2(path[i].position.x - path[i + 1].position.x, path[i].position.y - path[i + 1].position.y);
            if(newDir != oldDir)
            {
                waypoints.Add(path[i].position);
            }
            oldDir = newDir;
        }
        // no need to add last array, it is your position afaik
        return waypoints.ToArray();
    }

    float GetDistance(Node a, Node b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.x-b.x, 2) + Mathf.Pow(a.y-b.y, 2));
    }




}

