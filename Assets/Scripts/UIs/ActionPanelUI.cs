using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class ActionPanelUI : MonoBehaviour
    {
        [SerializeField] Transform display;
        [SerializeField] Button cancelButton;
        [SerializeField] List<ActionButtonData> actionButtonDatas;

        public void ToggleShowPanel(bool isActive)
        {
            display.DOScaleY(isActive ? 1f : 0f, 0.2f).SetEase(Ease.InOutBounce);
        }

        public void ShowCancelButton(bool isActive)
        {
            cancelButton.gameObject.SetActive(isActive);
        }

        public void UpdateExecutedActionUI(ActionType actionType)
        {
            for (int i = 0; i < actionButtonDatas.Count; i++)
            {
                if (actionButtonDatas[i].actionType == actionType)
                {
                    actionButtonDatas[i].actionButton.interactable = false;
                }
            }
        }

        public void ResetExecutedActionUI()
        {
            for (int i = 0; i < actionButtonDatas.Count; i++)
            {
                actionButtonDatas[i].actionButton.interactable = true;
            }
        }
    }
}

[Serializable]
public class ActionButtonData
{
    public ActionType actionType;
    public Button actionButton;
}
