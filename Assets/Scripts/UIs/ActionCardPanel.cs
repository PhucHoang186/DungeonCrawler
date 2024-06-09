using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using UnityEngine;

namespace GameUI
{
    public class ActionCardPanel : MonoBehaviour
    {
        [SerializeField] Transform display;
        [SerializeField] Transform actionCardHolder;
        [SerializeField] ActionCardDisplay actionCardDisplayPrefab;
        [SerializeField] ActionTypeColorConfig actionTypeColorConfig;
        private List<ActionCardDisplay> actionCardDisplays = new();

        public void GenerateActionCards(List<SpellData> spellDatas)
        {
            DestroyActionCards();
            for (int i = 0; i < spellDatas.Count; i++)
            {
                var actionCard = Instantiate(actionCardDisplayPrefab, actionCardHolder);
                actionCard.Init(spellDatas[i], actionTypeColorConfig);
                actionCardDisplays.Add(actionCard);
            }
        }

        public void DestroyActionCards()
        {
            for (int i = 0; i < actionCardDisplays.Count; i++)
            {
                Destroy(actionCardDisplays[i].gameObject);
            }
            actionCardDisplays.Clear();
        }

        public void ToggleActionCardPanel(bool isActive)
        {
            display.DOScaleY(isActive ? 1f : 0f, 0.2f).SetEase(Ease.InOutBounce);
        }
    }
}
