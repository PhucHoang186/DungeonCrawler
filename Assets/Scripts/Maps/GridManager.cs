using System;
using System.Collections.Generic;
using System.Linq;
using Map;
using NaughtyAttributes;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] PathFinding pathFinding;
    [SerializeField] MoveableNode moveableNodePrefab;
    [SerializeField] ImmoveableNode immoveableNodePrefab;
    [SerializeField] float offsetStep;
    public int width;
    public int height;
    [Header("Test")]
    [Range(0f, 1f)]
    public float obstaclePercent;
    private Dictionary<Vector2, Node> nodesList = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        GenerateGrid();
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    [Button]
    public void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool isSpawnObstacle = CheckSpawnObstacle(x, y);
                Node NodeToSpawnPrefab = isSpawnObstacle ? immoveableNodePrefab : moveableNodePrefab;
                Node newNode = Instantiate(NodeToSpawnPrefab, new Vector3(x * offsetStep, 0, y * offsetStep), Quaternion.identity, transform);
                newNode.X = x;
                newNode.Y = y;
                newNode.GCost = Mathf.Infinity;
                nodesList.Add(new Vector2(x, y), newNode);
                newNode.gameObject.name = "Node" + x + "_" + y;

            }
        }
    }

    private bool CheckSpawnObstacle(int x, int y)
    {
        if (y > 0 && y < height - 1 && x > 0 && x < width - 1)
        {
            return UnityEngine.Random.value < obstaclePercent;
        }
        return false;
    }

    public bool IsNodeValid(int x, int y)
    {
        bool isValid = x >= 0 && x < width &&
                        y >= 0 && y < height;
        return isValid;
    }

    public Node GetNodeByIndex(int x, int y)
    {
        nodesList.TryGetValue(new Vector2(x, y), out Node node);
        return node;
    }

    public void ResetNodes()
    {
        foreach (var key in nodesList.Keys)
        {
            nodesList[key].ResetNode();
        }
    }

    public List<Node> GetPathToNode(Node startNode, Node endNode, bool isDiagonal = true)
    {
        var path = pathFinding.FindPath(startNode, endNode, this, isDiagonal);
        return path;
    }

    public List<Node> GetNeighborNodesWithStep(Node startNode, int stepCount, bool isDiagonal = false, bool getEmptyOnly = true)
    {
        HashSet<Node> neighborNodesAll = new();
        List<Node> previousStepNodes = new();
        neighborNodesAll.Add(startNode);
        previousStepNodes.Add(startNode);
        int step = 0;
        while (step < stepCount)
        {
            var neighborNodes = new List<Node>();
            foreach (Node node in previousStepNodes)
            {
                neighborNodes.AddRange(GetNeighborNodes(node, isDiagonal, getEmptyOnly));
            }
            neighborNodesAll.UnionWith(neighborNodes);
            previousStepNodes = neighborNodes;
            step++;
        }
        return neighborNodesAll.ToList();
    }

    public List<Node> GetNeighborNodes(Node startNode, bool isDiagonal = false, bool getEmptyOnly = true)
    {
        List<Node> neighborNodes = new();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int xIndex = startNode.X + i;
                int yIndex = startNode.Y + j;

                if (xIndex < width && xIndex >= 0 && yIndex < height && yIndex >= 0 && (i != 0 || j != 0))
                {
                    if (!isDiagonal && Mathf.Abs(i - j) != 1)
                        continue;
                    // add node
                    Node neighborNode = GetNodeByIndex(xIndex, yIndex);
                    if ((!neighborNode.isEmpty && getEmptyOnly) || neighborNode is ImmoveableNode)
                        continue;
                    if (neighborNode != null)
                    {
                        neighborNodes.Add(neighborNode);
                    }
                }
            }
        }
        return neighborNodes;
    }

    public List<Node> GetStraightNodesWithStep(Node startNode, int stepCount, bool getEmptyOnly = true)
    {
        HashSet<Node> straightNodesAll = new();
        return straightNodesAll.ToList();
    }
}
