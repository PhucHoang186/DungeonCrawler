using System.Collections;
using System.Collections.Generic;
using Map;
using DG.Tweening;
using UnityEngine;
using TMPro;

namespace EntityObject
{

    public class CharacterEntity : Entity, IDamageable
    {
        [SerializeField] protected CharacterEntityState characterEntityState;
        [SerializeField] protected float moveTime;
        [SerializeField] protected TMP_Text healthText;
        [SerializeField] protected TMP_Text staminaText;
        [SerializeField] protected TMP_Text manaText;
        protected float currentMoveTime;
        protected Vector3 currentPos;
        protected float maxHealth;
        protected float maxMana;
        protected float maxStamina;
        protected float currentHealth;
        protected float currentMana;
        protected float currentStamina;
        protected Animator anim;
        public CharacterEntityState EntityState => characterEntityState;

        protected void Start()
        {
            Init();
        }

        private void Init()
        {
            anim = GetComponent<Animator>();
            maxHealth = entityData.MaxHealth;
            UpdateHealth(maxHealth);

            maxStamina = entityData.MaxStamina;
            UpdateStamina(maxStamina);

            maxMana = entityData.MaxMana;
            UpdateMana(maxMana);
        }

        public float MoveToNode(Node newNode)
        {
            UpdateNode(newNode);
            UpdateMovement();
            PlayMoveAnimation();
            return moveTime;
        }

        private void UpdateMovement()
        {
            currentMoveTime = 0f;
            currentPos = transform.position;
        }

        private void PlayMoveAnimation()
        {
            float halfMoveTime = (moveTime + 0.2f) * 0.5f;
            transform.DOScale(Vector3.one * 0.9f, halfMoveTime).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, halfMoveTime).SetEase(Ease.OutBounce);
            });
        }

        protected void Move()
        {
            if (currentNode == null)
                return;
            if (currentMoveTime >= moveTime)
                return;

            currentMoveTime += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPos, currentNode.transform.position, currentMoveTime / moveTime);
        }

        protected void Update()
        {
            Move();
        }

        public void TakeDamage(float damageAmount)
        {
            transform.DOShakePosition(0.1f, new Vector3(0.2f, 0f, 0.2f));
            UpdateHealth(-damageAmount);
        }

        public virtual void UpdateHealth(float healthAmount)
        {
            currentHealth += healthAmount;
            healthText.text = currentHealth.ToString();
            currentHealth = Mathf.Max(currentHealth, 0);
            if (currentHealth <= 0)
            {
                OnChangeState(CharacterEntityState.Down);
            }
        }

        public virtual void UpdateMana(float manaAmount)
        {
            currentMana += manaAmount;
            manaText.text = currentMana.ToString();
            currentMana = Mathf.Max(currentMana, 0);
            if (currentMana <= 0)
            {

            }
        }

        public virtual void UpdateStamina(float staminaAmount)
        {
            currentStamina += staminaAmount;
            staminaText.text = currentStamina.ToString();
            currentStamina = Mathf.Max(currentStamina, 0);
            if (currentStamina <= 0)
            {
            }
        }

        protected void OnChangeState(CharacterEntityState entityState)
        {
            characterEntityState = entityState;
            switch (entityState)
            {
                case CharacterEntityState.Live:
                    break;
                case CharacterEntityState.Down:
                    OnDownState();
                    break;
                case CharacterEntityState.Destroy:
                    OnDetroyState();
                    break;
            }
        }


        protected void OnDownState()
        {
            PlayAnim(CharacterEntityState.Down);
        }

        protected void OnDetroyState()
        {
            PlayAnim(CharacterEntityState.Destroy);
        }

        protected void PlayAnim(CharacterEntityState stateName)
        {
            anim?.Play(stateName.ToString());
        }
    }
}