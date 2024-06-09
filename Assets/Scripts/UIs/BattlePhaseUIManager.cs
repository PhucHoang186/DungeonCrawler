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
        [SerializeField] ActionPanelUI actionPanel;
        [SerializeField] ActionCardPanel actionCardPanel;
        [SerializeField] BattleUIEffect battleEffect;
        [SerializeField] float shakeAmplitude;
        [SerializeField] float shakeFrequency;

        private List<TurnIconDisplay> turnIconDisplays = new();
        private Action<ActionType> onChangeActionTypeCb;

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

        public void AssignChangeActionTypeCb(Action<ActionType> onChangeActionTypeCb)
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
                turnIconDisplays[i].SetIcon(entities[i].entityData.EntityIcon);
            }
        }

        public void UpdateTurn(int turnIndex)
        {
            if (turnIndex >= turnIconDisplays.Count)
                return;

            for (int i = 0; i < turnIconDisplays.Count; i++)
                turnIconDisplays[i].ToggleTurn(i == turnIndex);
        }

        public void GenerateActionCards(List<SpellData> spellDatas)
        {
            actionCardPanel.GenerateActionCards(spellDatas);
        }

        public void DestroyActionCards()
        {
            actionCardPanel.DestroyActionCards();
        }

        public void ToggleActionCardPanel(bool isActive)
        {
            actionCardPanel.ToggleActionCardPanel(isActive);
        }

        public void ToggleShowActionPanel(bool isActive)
        {
            actionPanel.ToggleShowPanel(isActive);
            actionPanel.ShowCancelButton(!isActive);
        }

        public void UpdateExecutedActionUI(ActionType actionType)
        {
            actionPanel.UpdateExecutedActionUI(actionType);
        }

        public void ResetExecutedActionUI()
        {
            actionPanel.ResetExecutedActionUI();
        }

        public void ShowModifyValue(float value, Vector2 spawnPosition)
        {
            battleEffect.ShowModifyValue(value, spawnPosition);
            CamController.Instance.ShakeCam(0.15f, shakeAmplitude, shakeFrequency);
        }

        // button UI
        public void OnMoveCallback()
        {
            onChangeActionTypeCb?.Invoke(ActionType.Move);
        }

        // button UI
        public void OnActionCallback()
        {
            onChangeActionTypeCb?.Invoke(ActionType.Action);
        }

        // button UI
        public void OnEndTurnCallback()
        {
            onChangeActionTypeCb?.Invoke(ActionType.End_Turn);
        }

        // button UI
        public void OnCancelAction()
        {
            onChangeActionTypeCb?.Invoke(ActionType.Waiting);
        }
    }
}
