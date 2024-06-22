using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class Visualization : MonoBehaviour
    {
        [SerializeField] LineRenderer pathVisualize;
        [SerializeField] Transform arrowIndicator;
        [SerializeField] LineRenderer modifyVisualize;
        [SerializeField] Transform modifyIndicator;
        [SerializeField] int step;
        private float stepAmount;

        void Start()
        {
            stepAmount = 1f / step;
        }

        public void VisualizePath(List<Node> path)
        {
            pathVisualize.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                pathVisualize.SetPosition(i, path[i].transform.position);

                if (i == path.Count - 1)
                {
                    Vector3 dir = path[i].transform.position - path[i - 1].transform.position;
                    arrowIndicator.gameObject.SetActive(true);
                    arrowIndicator.rotation = Quaternion.LookRotation(dir, Vector3.up);
                    arrowIndicator.position = path[i].transform.position;
                }
            }
        }

        public void VisualizeRange(List<Node> neighborNodes, highlightType highlightType, bool recordThisState = true, bool revertLastState = false)
        {
            for (int i = 0; i < neighborNodes.Count; i++)
            {
                MoveableNode moveableNode = neighborNodes[i] as MoveableNode;
                if (moveableNode)
                    moveableNode.ToggleHighlight(true, highlightType, recordThisState, revertLastState);
            }
        }

        public void VisualizeModifyPath(Node startNode, Node endNode)
        {
            modifyVisualize.positionCount = step;
            Vector3 endPosition = endNode.transform.position;
            Vector3 startPosition = startNode.transform.position;
            modifyIndicator.position = endPosition;
            modifyIndicator.gameObject.SetActive(true);

            float distance = Vector3.Distance(startPosition, endPosition);
            Vector3 crossPorduct = Vector3.Cross(endPosition - startPosition, Vector3.up * (endPosition.x - startPosition.x)).normalized * distance;

            for (int i = 0; i < step; i++)
            {
                Vector3 pos = GameHelper.ShowBezierCurve(startPosition, startPosition + crossPorduct,
                endPosition + crossPorduct, endPosition, i * stepAmount);
                modifyVisualize.SetPosition(i, pos);
            }
        }

        public void ClearModifyPath()
        {
            modifyVisualize.positionCount = 0;
            modifyIndicator.gameObject.SetActive(false);
        }

        public void ClearRange(List<Node> neighborNodes, highlightType highlightType, bool recordThisState = true, bool revertLastState = false)
        {
            for (int i = 0; i < neighborNodes.Count; i++)
            {
                MoveableNode moveableNode = neighborNodes[i] as MoveableNode;
                if (moveableNode)
                    moveableNode.ToggleHighlight(false, highlightType, recordThisState, revertLastState);
            }
        }

        public void ClearPath()
        {
            pathVisualize.positionCount = 0;
            arrowIndicator.gameObject.SetActive(false);
        }
    }
}
