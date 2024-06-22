using System.Collections;
using System.Collections.Generic;
using Data;
using Map;
using UnityEngine;

namespace EntityObject
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer entityIcon;
        [SerializeField] protected EntityData entityData;
        public Node currentNode;
        public EntityData EntityData => entityData;

        public void Init(Node placeOnNode, EntityData entityData)
        {
            currentNode = placeOnNode;
            entityIcon.sprite = entityData.EntityIcon;
            this.entityData = entityData;
            placeOnNode.entityOnNode = this;
            placeOnNode.SetEmptyState(false);
        }

        protected void UpdateNode(Node newNode)
        {
            currentNode?.SetEmptyState(true);
            currentNode = newNode;
            newNode.SetEmptyState(false);
        }
    }
}
