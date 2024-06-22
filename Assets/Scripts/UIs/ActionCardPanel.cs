using System;
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

        public void InitActionCards(List<SpellData> spellDatas, Action<ActionCardDisplay> OnSelectCardCb)
        {
            DestroyActionCards();
            for (int i = 0; i < spellDatas.Count; i++)
            {
                var actionCard = Instantiate(actionCardDisplayPrefab, actionCardHolder);
                actionCard.Init(spellDatas[i], actionTypeColorConfig, OnSelectCardCb);
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

        public void OnSelectCard(ActionCardDisplay actionCard)
        {
            for (int i = 0; i < actionCardDisplays.Count; i++)
            {
                actionCardDisplays[i].ToggleSelected(actionCardDisplays[i] == actionCard);
            }
        }

        public void OnUseCard(ActionCardDisplay actionCard)
        {
            int index = actionCardDisplays.IndexOf(actionCard);
            if (index >= actionCardDisplays.Count)
            {
                Debug.Log("Out OF Index");
                return;
            }
            actionCardDisplays[index].ToggleActive(false);
        }

        public void ToggleActionCardPanel(bool isActive)
        {
            display.DOScaleY(isActive ? 1f : 0f, 0.2f).SetEase(Ease.InOutBounce);

            if (isActive)
            {
                for (int i = 0; i < actionCardDisplays.Count; i++)
                    actionCardDisplays[i].ToggleActive(true);
            }
        }
    }
}
