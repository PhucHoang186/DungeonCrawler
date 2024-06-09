using EntityObject;
using UnityEngine;

namespace Map
{
    public class ImmoveableNode : Node
    {
        public override void ResetNode()
        {
            GCost = Mathf.Infinity;
            HCost = 0f;
            SetEmptyState(false);
            parentNode = null;
        }
    }
}
