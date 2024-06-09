using EntityObject;
using UnityEngine;

namespace Map
{
    public class MoveableNode : Node
    {
        [SerializeField] GameObject movehighlight;
        [SerializeField] GameObject actionHighlight;
        [SerializeField] GameObject actionRangeHighlight;
        protected NodeHighlightState nodeHighlightState = new();

        void Start()
        {
            GameEvents.ON_RESET_NODE += OnResetNode;
        }

        void OnDestroy()
        {
            GameEvents.ON_RESET_NODE -= OnResetNode;
        }

        private void OnResetNode()
        {
            ToggleHighlight(false, highlightType.All, true, false, true);
        }

        public void ToggleHighlight(bool isActive, highlightType highlightType, bool recordThisState = true, bool revertLastState = false, bool ignoreCheck = false)
        {
            if (nodeHighlightState.HighlightType == highlightType && nodeHighlightState.ActiveState == isActive && !ignoreCheck)
                return;

            movehighlight.SetActive(false);
            actionHighlight.SetActive(false);
            actionRangeHighlight.SetActive(false);
            switch (highlightType)
            {
                case highlightType.Move:
                    movehighlight.SetActive(isActive);
                    break;
                case highlightType.Action:
                    actionHighlight.SetActive(isActive);
                    break;
                case highlightType.ActionRange:
                    actionRangeHighlight.SetActive(isActive);
                    break;
                    // default:
                    //     movehighlight.SetActive(isActive);
                    //     actionHighlight.SetActive(isActive);
                    //     actionRangeHighlight.SetActive(isActive);
                    //     break;
            }

            if (!isActive && revertLastState)
            {
                ToggleHighlight(nodeHighlightState.ActiveState, nodeHighlightState.HighlightType, ignoreCheck: true);
                return;
            }

            if (recordThisState)
            {
                RecordHighlightState(highlightType, isActive);
            }

        }

        protected void RecordHighlightState(highlightType highlightType, bool activeState)
        {
            nodeHighlightState.HighlightType = highlightType;
            nodeHighlightState.ActiveState = activeState;
        }
    }

    public class NodeHighlightState
    {
        public highlightType HighlightType;
        public bool ActiveState;
    }
}
