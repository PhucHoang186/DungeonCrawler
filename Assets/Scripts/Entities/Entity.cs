using System.Collections;
using System.Collections.Generic;
using Data;
using Map;
using UnityEngine;

namespace EntityObject
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] SpriteRenderer entityIcon;
        public EntityData entityData;
        public Node currentNode;

        public void Init(Node placeOnNode)
        {
            currentNode = placeOnNode;
            entityIcon.sprite = entityData.EntityIcon;
            placeOnNode.entityOnNode = this;
        }

        protected void UpdateNode(Node newNode)
        {
            currentNode?.SetEmptyState(true);
            currentNode = newNode;
            newNode.SetEmptyState(false);
        }

        protected virtual void Update()
        {
        }
    }
}
