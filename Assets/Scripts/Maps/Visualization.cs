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

        public void ResetRange(List<Node> neighborNodes, highlightType highlightType, bool recordThisState = true, bool revertLastState = false)
        {
            for (int i = 0; i < neighborNodes.Count; i++)
            {
                MoveableNode moveableNode = neighborNodes[i] as MoveableNode;
                if (moveableNode)
                    moveableNode.ToggleHighlight(false, highlightType, recordThisState, revertLastState);
            }
        }

        public void VisualizeModifyPath(Node startNode, Node endNode)
        {
            modifyIndicator.gameObject.SetActive(true);
            modifyIndicator.position = endNode.transform.position;

            float stepAmount = 1f / step;
            modifyVisualize.positionCount = step;
            Vector3 crossPorduct = Vector3.Cross(endNode.transform.position - startNode.transform.position, Vector3.up).normalized * 3f;
            for (int i = 0; i < step; i++)
            {
                Vector3 pos = GameHelper.ShowBezierCurve(startNode.transform.position, startNode.transform.position + crossPorduct,
                endNode.transform.position + crossPorduct, endNode.transform.position, i * stepAmount);
                modifyVisualize.SetPosition(i, pos);
            }
        }

        public void ClearModifyPath()
        {
            modifyVisualize.positionCount = 0;
            modifyIndicator.gameObject.SetActive(false);
        }

        public void ClearPath()
        {
            pathVisualize.positionCount = 0;
            arrowIndicator.gameObject.SetActive(false);
        }
    }
}
