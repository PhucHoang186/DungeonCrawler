using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameUI
{
    public class ActionCardDisplay : MonoBehaviour, IPointerDownHandler
    {
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

        private SpellData spellData;

        public void Init(SpellData spellData, ActionTypeColorConfig actionTypeColorConfig)
        {
            actionIcon.sprite = spellData.SpellIcon;
            actionDescription.text = spellData.SpellDescription;

            var modifyColorData = actionTypeColorConfig.GetMatchActionColorData(spellData.ModifyData.ModifyCategory.ModifyType);
            if (modifyColorData != null)
            {
                modifyValue.mainColor.color = modifyColorData.MainColor;
                modifyValue.outlineColor.effectColor = GetOutlineColor(modifyColorData.MainColor);
            }

            var costColorData = actionTypeColorConfig.GetMatchActionColorData(spellData.CostData.ModifyCategory.ModifyType);
            if (costColorData != null)
            {
                costValue.mainColor.color = costColorData.MainColor;
                costValue.outlineColor.effectColor = GetOutlineColor(costColorData.MainColor);
            }
            this.spellData = spellData;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GameEvents.ON_SELECT_ACTION?.Invoke(spellData);
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
    }

    [Serializable]
    public class ActionValueDisplay
    {
        public Image mainColor;
        public Outline outlineColor;
    }
}
