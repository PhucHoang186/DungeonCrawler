using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class TurnIconDisplay : MonoBehaviour
    {
        [SerializeField] float toggleScaleSize;
        [SerializeField] Image characterIcon;
        [SerializeField] Image backgroundDisplay;
        [SerializeField] Color normalColor;
        [SerializeField] Color highlightColor;

        public void SetIcon(Sprite characterIcon)
        {
            this.characterIcon.sprite = characterIcon;
        }

        public void ToggleTurn(bool isActive)
        {
            transform.localScale = isActive ? Vector3.one * toggleScaleSize : Vector3.one;
            backgroundDisplay.color = isActive ? highlightColor : normalColor;
        }

    }
}