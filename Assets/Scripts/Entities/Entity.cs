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
        [SerializeField] protected float moveTime;
        public EntityData entityData;
        public Node currentNode;
        protected Vector3 currentPos;
        protected float currentMoveTime;

        public void Init(Node placeOnNode)
        {
            currentNode = placeOnNode;
            entityIcon.sprite = entityData.EntityIcon;
        }

        public float MoveToNode(Node newNode)
        {
            UpdateNode(newNode);
            UpdateMovement();
            return moveTime;
        }

        private void UpdateMovement()
        {
            currentMoveTime = 0f;
            currentPos = transform.position;
        }

        protected void UpdateNode(Node newNode)
        {
            currentNode?.SetEmptyState(true);
            currentNode = newNode;
            newNode.SetEmptyState(false);
        }

        protected void Update()
        {
            Move();
        }

        protected void Move()
        {
            if (currentNode == null)
                return;
            if (currentMoveTime < moveTime)
                currentMoveTime += Time.deltaTime;
            else
                return;
            transform.position = Vector3.Lerp(currentPos, currentNode.transform.position, currentMoveTime / moveTime);
        }
    }
}
