using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node: IHeapItem<Node>
{
    public bool obstacle;
    public Vector2 position;
    public int x;
    public int y;
    public float hCost, gCost;
    public Node parent;
    int heapIndex;
    public Node(bool obstacle, Vector2 position, int x, int y)
    {
        this.obstacle = obstacle;
        this.position = position;
        this.x = x;
        this.y = y;
    }

    public float fCost
    {
        get
        {
            return hCost + gCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }
        // if int is higher, compareto return 1, so we want reverse
        return -compare;
    }
}
