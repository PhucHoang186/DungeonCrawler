using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using EntityObject;
using Managers;
using UnityEngine;

namespace GameUI
{
    public class BattlePhaseUIManager : MonoBehaviour
    {
        public static BattlePhaseUIManager Instance;
        [SerializeField] TurnIconDisplay turnIconDisplayPrefab;
        [SerializeField] Transform turnHolder;
        [SerializeField] ActionPanelUI commandPanel;
        [SerializeField] ActionCardPanel actionCardPanel;
        [SerializeField] BattleUIEffect battleEffect;
        [SerializeField] float shakeAmplitude;
        [SerializeField] float shakeFrequency;

        private List<TurnIconDisplay> turnIconDisplays = new();
        private Action<CommandType> onChangeActionTypeCb;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void AssignChangeActionTypeCb(Action<CommandType> onChangeActionTypeCb)
        {
            this.onChangeActionTypeCb = onChangeActionTypeCb;
        }

        public void GenerateTurnIcons(int iconNumber)
        {
            for (int i = 0; i < iconNumber; i++)
            {
                var turnIcon = Instantiate(turnIconDisplayPrefab, turnHolder);
                turnIconDisplays.Add(turnIcon);
            }
        }

        public void UpdateTurnIcons(List<Entity> entities) // temp
        {
            for (int i = 0; i < entities.Count; i++)
            {
                turnIconDisplays[i].SetIcon(entities[i].EntityData.EntityIcon);
            }
        }

        public void UpdateTurn(int turnIndex)
        {
            if (turnIndex >= turnIconDisplays.Count)
                return;

            for (int i = 0; i < turnIconDisplays.Count; i++)
                turnIconDisplays[i].ToggleTurn(i == turnIndex);
        }

        public void InitActionCards(List<SpellData> spellDatas, Action<ActionCardDisplay> OnSelectCardCb)
        {
            actionCardPanel.InitActionCards(spellDatas, OnSelectCardCb);
        }

        public void DestroyActionCards()
        {
            actionCardPanel.DestroyActionCards();
        }

        public void ToggleActionCardPanel(bool isActive)
        {
            actionCardPanel.ToggleActionCardPanel(isActive);
        }

        public void ToggleShowCommandPanel(bool isActive)
        {
            commandPanel.ToggleCommandPanel(isActive);
            commandPanel.ShowCancelButton(!isActive);
        }

        public void UpdateExecutedCommandUI(CommandType actionType)
        {
            commandPanel.UpdateExecutedCommandUI(actionType);
        }

        public void ResetExecutedActionUI()
        {
            commandPanel.ResetExecutedActionUI();
        }

        public void ShowModifyValue(float value, Vector2 spawnPosition)
        {
            battleEffect.ShowModifyValue(value, spawnPosition);
            CamController.Instance.ShakeCam(0.15f, shakeAmplitude, shakeFrequency);
        }

        // button UI
        public void OnMoveCallback()
        {
            onChangeActionTypeCb?.Invoke(CommandType.Move);
        }

        // button UI
        public void OnActionCallback()
        {
            onChangeActionTypeCb?.Invoke(CommandType.Action);
        }

        // button UI
        public void OnEndTurnCallback()
        {
            onChangeActionTypeCb?.Invoke(CommandType.End_Turn);
        }

        // button UI
        public void OnCancelAction()
        {
            onChangeActionTypeCb?.Invoke(CommandType.Waiting);
        }

        public void OnSelectCard(ActionCardDisplay actionCard)
        {
            actionCardPanel.OnSelectCard(actionCard);
        }

        public void OnUseCard(ActionCardDisplay actionCard)
        {
            actionCardPanel.OnUseCard(actionCard);
        }
    }
}
