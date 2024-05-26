using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    [SerializeField] private Transform top;
    [SerializeField] private int columns = 50;
    [SerializeField] private int rows = 30;
    [SerializeField] private float cellWidth;
    [SerializeField] public float cellHeight;
    [SerializeField] private LayerMask obstacleLayer;

    Node[,] nodes;

    public class Node
    {
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX;
        public int gridY;
        public int cost;
        public Node parent;

        public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
        {
            walkable = _walkable;
            worldPosition = _worldPosition;
            gridX = _gridX;
            gridY = _gridY;
            cost = int.MaxValue;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cellWidth = (float) top.position.x / columns;
        cellHeight = (float) top.position.y / rows;
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateGrid()
    {
        nodes = new Node[columns, rows];
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                bool walkable = true;
                Vector3 worldPoint = new Vector3(x * cellWidth, y * cellHeight, 0);
                RaycastHit2D rc = Physics2D.CircleCast(worldPoint, cellHeight, Vector2.zero, obstacleLayer);
                if (rc.collider != null)
                {
                    if (rc.collider.CompareTag("Building"))
                    {
                        walkable = false;
                    }
                }
                //if (!walkable) { Debug.Log("Building"); }
                nodes[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node getNearestNode(Vector3 worldPosition)
    {
        int Xpos = Mathf.RoundToInt(worldPosition.x / cellWidth);
        int Ypos = Mathf.RoundToInt(worldPosition.y / cellHeight);
        //Debug.Log(worldPosition.x + " " + worldPosition.y);
        //Debug.Log(cellWidth + " " + cellHeight);
        //Debug.Log(Xpos + " " + Ypos);
        return nodes[Xpos, Ypos];
    }

    public List<Node> GetPath(Vector3 startPos, Vector3 endPos)
    {
        if (cellWidth == 0 && cellHeight  == 0)
        {
            return null;
        }

        Node startNode = getNearestNode(startPos);
        Node endNode = getNearestNode(endPos);

        if (!endNode.walkable)
        {
            return null;
        }

        List<Node> openList = new List<Node>();
        bool[,] closedList = new bool[columns, rows];
        int[,] distances = new int[columns, rows];

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                distances[i, j] = int.MaxValue;
            }
        } 

        startNode.cost = 0;
        startNode.parent = startNode;

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            openList = openList.OrderBy(n => n.cost).ToList();
            Node currentNode = openList[0];
            openList.RemoveAt(0);

            closedList[currentNode.gridX, currentNode.gridY] = true;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        //Debug.Log(currentNode.gridX + " " + currentNode.gridY);
                        Node neighbor = nodes[currentNode.gridX + i, currentNode.gridY + j];
                        if (neighbor.walkable)
                        {
                            if (neighbor == endNode)
                            {
                                return TracePath(endNode);
                            }

                            if (!closedList[neighbor.gridX, neighbor.gridY] && neighbor.walkable)
                            {
                                neighbor.cost =
                                GetDistance(currentNode, nodes[currentNode.gridX + i, currentNode.gridY + j]) +
                                GetDistance(endNode, nodes[currentNode.gridX + i, currentNode.gridY + j]);

                                if (distances[neighbor.gridX, neighbor.gridY] == int.MaxValue ||
                                    neighbor.cost < distances[neighbor.gridX, neighbor.gridY])
                                {

                                    distances[neighbor.gridX, neighbor.gridY] = neighbor.cost;
                                    neighbor.parent = currentNode;
                                    nodes[currentNode.gridX + i, currentNode.gridY + j] = neighbor;
                                    openList.Add(neighbor);
                                }
                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    public List<Node> TracePath(Node endNode)
    {
        List<Node> nodeList = new List<Node>();
        nodeList.Add(endNode);

        Node current = endNode;

        while (current.parent != null && current.parent != current)
        {
            Debug.DrawLine(current.worldPosition, current.parent.worldPosition, Color.blue, 0.5f);
            current = current.parent;
            nodeList.Add(current);
        }

        nodeList.Reverse();

        return nodeList;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return dstX + dstY;
    }

    private void OnDrawGizmos()
    {
        if (nodes != null)
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Gizmos.color = nodes[x, y].walkable ? Color.blue : Color.red;
                    Gizmos.DrawSphere(nodes[x, y].worldPosition, 0.1f);
                }
            }
        }
    }
}
