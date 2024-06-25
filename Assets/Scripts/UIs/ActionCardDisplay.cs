using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameUI
{
    public class ActionCardDisplay : MonoBehaviour, IPointerDownHandler
    {
        public const string HIGHLIGHT = "Highlight";
        public const string IDLE = "Idle";
        [Header("Value")]
        [Space(10)]
        [SerializeField] Image actionIcon;
        [SerializeField] TMP_Text actionDescription;
        [Space(10)]
        [SerializeField] TMP_Text costText;
        [SerializeField] ActionValueDisplay costValue;
        [Space(10)]
        [SerializeField] TMP_Text modifyText;
        [SerializeField] ActionValueDisplay modifyValue;
        [Space(10)]
        [SerializeField] Image actionImage;
        [SerializeField] ActionValueDisplay actionValue;
        [SerializeField] Animator anim;

        private SpellData spellData;
        private Action<ActionCardDisplay> OnSelectCardCb;
        public SpellData SpellData => spellData;

        public void Init(SpellData spellData, ActionTypeColorConfig actionTypeColorConfig, Action<ActionCardDisplay> OnSelectCardCb)
        {
            this.OnSelectCardCb = OnSelectCardCb;
            actionIcon.sprite = spellData.SpellIcon;
            // actionDescription.text = spellData.SpellDescription;

            var modifyColorData = actionTypeColorConfig.GetMatchActionColorData(spellData.ModifierData.ModifyData.ModifyType);
            if (modifyColorData != null)
            {
                modifyValue.mainColor.color = modifyColorData.MainColor;
                modifyValue.outlineColor.effectColor = GetOutlineColor(modifyColorData.MainColor);
            }

            var costColorData = actionTypeColorConfig.GetMatchActionColorData(spellData.CostData.ModifyData.ModifyType);
            if (costColorData != null)
            {
                costValue.mainColor.color = costColorData.MainColor;
                costValue.outlineColor.effectColor = GetOutlineColor(costColorData.MainColor);
            }
            this.spellData = spellData;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSelectCardCb?.Invoke(this);
        }

        public void ToggleActive(bool isActive)
        {
            float duration = isActive ? 0f : 0.2f;
            Vector3 scale = isActive ? Vector3.one : new Vector3(0f, 1f, 1f);
            transform.DOScale(scale, duration).OnComplete(() =>
            {
                gameObject.SetActive(isActive);
            });
        }

        private Color GetOutlineColor(Color originColor, float decreaseAmount = 10f)
        {
            float r = originColor.r - decreaseAmount;
            float g = originColor.g - decreaseAmount;
            float b = originColor.b - decreaseAmount;
            r = Mathf.Max(r, 0f);
            g = Mathf.Max(g, 0f);
            b = Mathf.Max(b, 0f);

            return new Color(r, g, b, 1f);
        }

        public void ToggleSelected(bool isActive)
        {
            anim?.Play(isActive ? HIGHLIGHT : IDLE);
            if (isActive)
            {
                transform.DOShakeScale(0.3f, 0.2f);
            }
        }
    }

    [Serializable]
    public class ActionValueDisplay
    {
        public Image mainColor;
        public Outline outlineColor;
    }
}
