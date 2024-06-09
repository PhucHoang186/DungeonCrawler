using EntityObject;
using UnityEngine;

namespace Map
{
    public class Node : MonoBehaviour
    {
        public bool isEmpty;
        [HideInInspector] public float GCost;
        [HideInInspector] public float HCost;
        [HideInInspector] public float FCost => GCost + HCost;
        [HideInInspector] public int X;
        [HideInInspector] public int Y;
        [HideInInspector] public Node parentNode;
        [HideInInspector] public Entity entityOnNode;

        public virtual void ResetNode()
        {
            GCost = Mathf.Infinity;
            HCost = 0f;
            parentNode = null;
        }

        public void ResetEntityOnNode()
        {
            entityOnNode = null;
        }

        public void SetEmptyState(bool isEmpty)
        {
            this.isEmpty = isEmpty;
        }
    }
}
