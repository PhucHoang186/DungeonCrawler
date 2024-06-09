using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NaughtyAttributes.Test;
using UnityEngine;

namespace Map
{
    public class PathFinding : MonoBehaviour
    {
        private const int DiagonalValue = 14;
        private const int StraightValue = 10;
        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] List<Node> openList = new();
        [SerializeField] List<Node> closeList = new();
        [SerializeField] List<Node> resultPath = new();
        [SerializeField] Vector3Int startNode;
        [SerializeField] Vector3Int endNode;
        [SerializeField] GridManager gridManager;

        [Button]
        public void Test()
        {
            FindPath(gridManager.GetNodeByIndex(startNode.x, startNode.z), gridManager.GetNodeByIndex(endNode.x, endNode.z), gridManager);
        }

        public void VisualizePath(List<Node> resultPath)
        {
            if (resultPath == null || resultPath.Count == 0)
                return;

            lineRenderer.positionCount = resultPath.Count;
            for (int i = 0; i < resultPath.Count; i++)
            {
                lineRenderer.SetPosition(i, resultPath[i].transform.position + Vector3.up * 0.5f);
            }
        }

        public List<Node> FindPath(Node startNode, Node targetNode, GridManager gridManager, bool isDiagonal = true)
        {
            gridManager.ResetNodes();
            openList.Clear();
            closeList.Clear();
            startNode.GCost = 0f;
            Node currentNode = startNode;
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                currentNode = FindMinimumNodeFCost(openList);

                if (currentNode == targetNode)
                {
                    return GetResultPath(targetNode);
                }

                openList.Remove(currentNode);
                closeList.Add(currentNode);

                List<Node> neighborNodes = GetNeighborNodes(gridManager, currentNode, isDiagonal);
                foreach (var neighborNode in neighborNodes)
                {
                    if (closeList.Contains(neighborNode))
                        continue;

                    float gCost = currentNode.GCost + CalculateDistance(currentNode, neighborNode);
                    if (gCost < neighborNode.GCost)
                    {
                        neighborNode.parentNode = currentNode;
                        neighborNode.GCost = gCost;
                        neighborNode.HCost = CalculateDistance(neighborNode, targetNode);
                        if (!openList.Contains(neighborNode))
                            openList.Add(neighborNode);
                    }
                }
            }

            Debug.LogWarning("Can't find path");
            return null;
        }

        private List<Node> GetResultPath(Node endNode)
        {
            List<Node> resultNode = new List<Node> { endNode };
            Node currentNode = endNode;
            while (currentNode.parentNode != null)
            {
                currentNode = currentNode.parentNode;
                resultNode.Add(currentNode);
            }
            resultNode.Reverse();
            return resultNode;
        }

        private float CalculateDistance(Node startNode, Node endNode)
        {
            float xDistance = Mathf.Abs(endNode.X - startNode.X);
            float yDistance = Mathf.Abs(endNode.Y - startNode.Y);
            float remainingDistance = Mathf.Abs(xDistance - yDistance);

            return Mathf.Min(yDistance, xDistance) * DiagonalValue + remainingDistance * StraightValue;
        }

        private Node FindMinimumNodeFCost(List<Node> list)
        {
            Node resultNode = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (resultNode.FCost > list[i].FCost)
                {
                    resultNode = list[i];
                }
            }
            return resultNode;
        }

        private List<Node> GetNeighborNodes(GridManager gridManager, Node startNode, bool isDiagonal = true)
        {
            return gridManager.GetNeighborNodes(startNode, isDiagonal);
        }
    }
}
