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
        [SerializeField] List<CommandButtonData> commandButtonDatas;

        public void ToggleCommandPanel(bool isActive)
        {
            display.DOScaleY(isActive ? 1f : 0f, 0.2f).SetEase(Ease.InOutBounce);
        }

        public void ShowCancelButton(bool isActive)
        {
            cancelButton.gameObject.SetActive(isActive);
        }

        public void UpdateExecutedCommandUI(CommandType actionType)
        {
            for (int i = 0; i < commandButtonDatas.Count; i++)
            {
                if (commandButtonDatas[i].actionType == actionType)
                {
                    commandButtonDatas[i].actionButton.interactable = false;
                }
            }
        }

        public void ResetExecutedActionUI()
        {
            for (int i = 0; i < commandButtonDatas.Count; i++)
            {
                commandButtonDatas[i].actionButton.interactable = true;
            }
        }
    }
}

[Serializable]
public class CommandButtonData
{
    public CommandType actionType;
    public Button actionButton;
}
