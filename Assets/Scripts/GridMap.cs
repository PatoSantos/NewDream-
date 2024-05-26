using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    [SerializeField] private Transform top;
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
                Vector3 worldPoint = new Vector3(x * cellWidth, y * cellHeight, 0);
                bool walkable = !Physics.CheckSphere(worldPoint, cellWidth / 2, obstacleLayer);
                nodes[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node getNearestNode(Vector3 worldPosition)
    {
        int Xpos = Mathf.RoundToInt(worldPosition.x / cellWidth);
        int Ypos = Mathf.RoundToInt(worldPosition.y / cellHeight);
        return nodes[Xpos, Ypos];
    }

    public List<Node> getPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = getNearestNode(startPos);
        Node endNode = getNearestNode(endPos);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        startNode.cost = 0;
        startNode.parent = startNode;

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            openList = openList.OrderBy(n => n.cost).ToList();
            Node currentNode = openList[0];
            openList.RemoveAt(0);

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        if (currentNode == endNode)
                        {

                        }
                    }
                }
            }
        }


        return new List<Node>();
    }

    float calculateCost(Node n)
    {
        return 0;
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
