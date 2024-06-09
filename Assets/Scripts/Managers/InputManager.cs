using System.Collections;
using System.Collections.Generic;
using Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] Camera mainCam;
        [SerializeField] LayerMask nodeLayerMask;
        private Ray ray;
        private Node currentNode;
        public static InputData InputData = new();

        private void Update()
        {
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            GetInput();
            DetectNode();
        }

        private void GetInput()
        {
            if (!Application.isFocused)
                return;
            // left
            InputData.isMouseDownLeft = Input.GetMouseButtonDown(0);
            InputData.isMouseUpLeft = Input.GetMouseButtonUp(0);
            InputData.isMouseHoldLeft = Input.GetMouseButton(0);
            // right
            InputData.isMouseDownRight = Input.GetMouseButtonDown(1);
            InputData.isMouseUpRight = Input.GetMouseButtonUp(1);
            InputData.isMouseHoldRight = Input.GetMouseButton(1);
            // UI
            InputData.isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
        }

        private void DetectNode()
        {
            if (InputData.isPointerOverUI)
                return;
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, nodeLayerMask))
            {
                if (hit.collider.TryGetComponent<Node>(out var node))
                {
                    if (currentNode != node)
                    {
                        currentNode = node;
                        GameEvents.ON_MOUSE_OVER_NODE?.Invoke(node);
                    }
                }
            }
        }
    }
}

public class InputData
{
    public bool isMouseDownLeft;
    public bool isMouseUpLeft;
    public bool isMouseHoldLeft;
    public bool isMouseDownRight;
    public bool isMouseUpRight;
    public bool isMouseHoldRight;
    public bool isPointerOverUI;
}