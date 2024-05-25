using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    [SerializeField] private Transform topLeft;
    [SerializeField] private Transform bottomRight;
    [SerializeField] private int columns = 50;
    [SerializeField] private int rows = 30;
    [SerializeField] private float cellWidth;
    [SerializeField] private float cellHeight;
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
        cellWidth = (float)(bottomRight.position.x - topLeft.position.x) / columns;
        cellHeight = (float)(topLeft.position.y - bottomRight.position.y) / rows;
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
                Vector3 worldPoint = new Vector3(x * cellWidth + topLeft.position.x , y * cellHeight + bottomRight.position.y, 0);
                bool walkable = !Physics.CheckSphere(worldPoint, cellWidth / 2, obstacleLayer);
                nodes[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x - topLeft.position.x) / cellWidth);
        int y = Mathf.RoundToInt(worldPosition.y - bottomRight.position.y / cellHeight);
        Debug.Log(x.ToString() + " " + y.ToString());
        return nodes[x, y];
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = GetNodeFromWorldPoint(startPos);
        Node targetNode = GetNodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        startNode.cost = 0;
        openSet.Add(startNode);

        //Debug.Log(openSet.Count);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.OrderBy(n => n.cost).First();
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int newCost = currentNode.cost + GetDistance(currentNode, neighbor);
                if (newCost < neighbor.cost || !openSet.Contains(neighbor))
                {
                    neighbor.cost = newCost;
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < cellWidth && checkY >= 0 && checkY < cellHeight)
                {
                    neighbors.Add(nodes[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
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
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(nodes[x, y].worldPosition, 0.1f);
                }
            }
        }
    }
}
