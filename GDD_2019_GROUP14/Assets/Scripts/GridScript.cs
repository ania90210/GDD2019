using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// generates grid overlay over the map ingame

public class GridScript : MonoBehaviour
{
    // x axis
    // y axis
    public Transform player;
    public int gridXsize = 100, gridYsize = 50;
    // its a square grid
    public float cellSize = 1f;
    Node[,] gridCells;

    private Vector3 lowerLeft;
    // Start is called before the first frame update
    void Awake()
    {
        gridCells = new Node[Mathf.RoundToInt(gridXsize / cellSize), Mathf.RoundToInt(gridYsize / cellSize)];
        this.lowerLeft = new Vector3(this.transform.position.x - this.gridXsize / 2.0f, this.transform.position.y - this.gridYsize / 2.0f, 1);
        createGrid();
    }

    void createGrid()
    {

        for (int x = 0; x < Mathf.RoundToInt(gridXsize / cellSize); x++)
        {
            for (int y = 0; y < Mathf.RoundToInt(gridYsize / cellSize); y++)
            {
                Vector3 worldPoint = FromGridToWorld(x, y);

                //bool obstacle = Physics.CheckSphere(worldPoint, this.cellSize/2);
                Collider2D hit = Physics2D.OverlapPoint(worldPoint);
                bool obstacle = hit != null;
                if (obstacle)
                {
                    obstacle = hit.gameObject.tag == "Obstacle";
                }
                this.gridCells[x, y] = new Node(obstacle, worldPoint, x, y);

                FromWorldToGrid(worldPoint);
            }
        }
    }


    public Vector3 FromGridToWorld(int x, int y)
    {
        // lower left corner is -gridXsize/2 and -gridYsize/2
        Vector3 nodeValue = new Vector3(lowerLeft.x + x * this.cellSize + this.cellSize / 2.0f, lowerLeft.y + y * this.cellSize + this.cellSize / 2.0f, 1);
        return nodeValue;
    }

    public Node FromWorldToGrid(Vector3 worldPos)
    {
        int x, y;
        x = Mathf.RoundToInt((worldPos.x - lowerLeft.x - (this.cellSize / 2.0f)) / this.cellSize);
        y = Mathf.RoundToInt((worldPos.y - lowerLeft.y - (this.cellSize / 2.0f)) / this.cellSize);
        return gridCells[x, y];
    }

    public List<Node> GetChildren(Node n)
    {
        List<Node> children = new List<Node>();
        // col
        for (int j = -1; j <= 1; j++)
        {
            if (OkChild(n.x, n.y + j) && j != 0)
            {
                children.Add(gridCells[n.x, n.y + j]);
            }
        }

        // row
        for (int i = -1; i <= 1; i++)
        {
            if (OkChild(n.x + i, n.y) && i != 0)
            {
                children.Add(gridCells[n.x + i, n.y]);
            }
        }

        return children;
    }

    private bool OkChild(int i, int j)
    {
        return i >= 0 && j >= 0 && i < gridCells.GetLength(0) && j < gridCells.GetLength(1) && !gridCells[i, j].obstacle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(this.gridXsize, this.gridYsize, 0));
        if (this.gridCells != null)
        {
            int i = 0;
            Node playerNode = FromWorldToGrid(player.position);
            foreach (Node n in this.gridCells)
            {

                Gizmos.color = n.obstacle ? Color.red : Color.white;
                if (playerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.position, Vector3.one * (this.cellSize - 0.01f));
                i++;
            }
        }
    }

    public Node[,] getGrid()
    {
        return gridCells;
    }

}
