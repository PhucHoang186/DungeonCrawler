using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using EntityObject;
using GameUI;
using Map;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class BattleManager : MonoBehaviour
    {
        public EntityPlayer currentPlayer;
        public EntityEnemy currentEnemy;
        [SerializeField] int turnPerRound;
        [SerializeField] EnemyAIManager enemyAIManager;
        [SerializeField] SpellData spellData;
        private GridManager gridManager;
        private Entity currentEntity;
        private List<Entity> allEntities = new();
        private List<EntityPlayer> players = new();
        private List<EntityEnemy> enemies = new();
        private EntityManager entityManager;
        private MovementCombatManager movementCombatManager;
        private TurnBaseType currentTurnType;
        private ActionState currentActionState = ActionState.Done;
        private ActionType currentActionType;
        private HashSet<ActionType> actionPerTurnList = new();
        private SpellData currentSpellSelect;
        private Node currentNodeSelected;
        private int currentTurnIndex;
        public Node CurrentNodeSelected => currentNodeSelected;
        public bool inModifiableState;

        void Start()
        {
            GameEvents.ON_MOUSE_OVER_NODE += OnMouseOverNode;
            GameEvents.ON_SELECT_ACTION += OnSelectAction;
        }

        void OnDestroy()
        {
            GameEvents.ON_MOUSE_OVER_NODE -= OnMouseOverNode;
            GameEvents.ON_SELECT_ACTION -= OnSelectAction;
        }

        private void OnMouseOverNode(Node targetNode)
        {
            if (!IsActionState(ActionState.Done))
                return;
            if (currentTurnType != TurnBaseType.Player)
                return;
            if (currentPlayer == null)
                return;
            if (currentPlayer.currentNode == targetNode)
                return;
            currentNodeSelected = targetNode;
            switch (currentActionType)
            {
                case ActionType.Move:
                    UpdateMovePath(targetNode);
                    break;
                case ActionType.Action:
                    UpdatActionNode();
                    break;
            }
        }

        private void OnSelectAction(SpellData data)
        {
            StartCastSpell(data);
        }

        private void UpdateMovePath(Node targetNode)
        {
            if (!(targetNode is MoveableNode))
                return;
            if (!targetNode.isEmpty)
                return;

            movementCombatManager.ShowWalkablePath(currentPlayer.currentNode, targetNode, 2, true);
        }

        public void Init(EntityManager entityManager, MovementCombatManager movementCombatManager, GridManager gridManager)
        {
            (this.players, this.enemies) = entityManager.GetAllEntities();
            this.entityManager = entityManager;
            this.movementCombatManager = movementCombatManager;
            this.gridManager = gridManager;
            BattlePhaseUIManager.Instance?.GenerateTurnIcons(turnPerRound);
            BattlePhaseUIManager.Instance?.AssignChangeActionTypeCb(UpdateActionType);
            GetNextTurn();
        }

        public void GetNextTurn()
        {
            currentEnemy = null;
            currentPlayer = null;
            currentEntity = GetAvailableEntity();
            bool isPlayerTurn = IsPLayerTurn();

            BattlePhaseUIManager.Instance?.UpdateTurn(currentTurnIndex);
            currentTurnIndex++;

            if (isPlayerTurn)
                currentPlayer = currentEntity as EntityPlayer;
            else
                currentEnemy = currentEntity as EntityEnemy;

            OnChangeTurn(isPlayerTurn ? TurnBaseType.Player : TurnBaseType.Enemy);
            UpdateActionType(isPlayerTurn ? ActionType.Waiting : ActionType.Move); // temp
        }

        private void Update()
        {
            if (IsPLayerTurn())
            {
                PlayerTurn();
            }
            else
            {
                EnemyTurn();
            }
        }

        private void PlayerTurn()
        {
            switch (currentActionType)
            {
                case ActionType.Move:
                    PlayerMovement();
                    break;
                case ActionType.Action:
                    PlayerAction();
                    break;
            }
        }

        private void PlayerMovement()
        {
            if (!InputManager.InputData.isPointerOverUI && InputManager.InputData.isMouseDownLeft)
            {
                movementCombatManager.MoveEntityToNode(currentPlayer, finalCb: OnFinshAction, successCb: () => { UpdateActionState(ActionState.Progressing); });
            }
        }

        private void PlayerAction()
        {
            if (!inModifiableState)
                return;

            if (!InputManager.InputData.isPointerOverUI && InputManager.InputData.isMouseDownLeft)
            {
                currentSpellSelect.ExcuteSpell(this);
            }
        }

        private void UpdatActionNode()
        {
            if (currentSpellSelect == null)
                return;
            currentSpellSelect.CastingSpell(this);
        }

        private void OnFinshAction()
        {
            UpdateActionState(ActionState.Done);
            UpdateActionType(ActionType.Waiting);
        }

        private void EnemyTurn()
        {
            StartCoroutine(CorEnemyTurn());
        }

        private IEnumerator CorEnemyTurn()
        {
            //temp
            if (currentEnemy == null)
                yield break; ;
            if (IsActionState(ActionState.Progressing))
                yield break;
            UpdateActionState(ActionState.Progressing);
            yield return new WaitForSeconds(1f);
            Node desNode = enemyAIManager.GetMovementAction(currentEnemy, gridManager, this);
            if (desNode == null)
                yield break;
            movementCombatManager.ShowWalkablePath(currentEnemy.currentNode, desNode, 1, false);
            movementCombatManager.MoveEntityToNode(currentEnemy, finalCb: EnemyFinishAction, failCb: EnemyFinishAction);
        }

        // temp
        private void EnemyFinishAction()
        {
            UpdateActionState(ActionState.Done);
            UpdateActionType(ActionType.End_Turn);
        }

        public void OnChangeTurn(TurnBaseType newType)
        {
            if (this.currentTurnType == newType)
                return;

            this.currentTurnType = newType;
            switch (currentTurnType)
            {
                case TurnBaseType.Player:
                    break;
                case TurnBaseType.Enemy:
                    break;
            }
        }

        private void RandomTurnBase()
        {
            currentTurnIndex = 0;
            allEntities = new(players);
            allEntities.AddRange(enemies);
            allEntities.Shuffle();
            // temp
            int turn = turnPerRound - allEntities.Count;
            bool isPlayer = allEntities[^1] is EntityPlayer;
            while (turn > 0)
            {
                isPlayer = !isPlayer;
                List<Entity> pickedList = isPlayer ? new(players) : new(enemies);
                Entity chosenEntity = pickedList[UnityEngine.Random.Range(0, pickedList.Count)];
                allEntities.Add(chosenEntity);
                turn--;
            }
            BattlePhaseUIManager.Instance?.UpdateTurnIcons(allEntities);
        }

        public List<EntityPlayer> GetAllPlayers()
        {
            return players;
        }

        public List<EntityEnemy> GetAllEnemies()
        {
            return enemies;
        }

        private Entity GetAvailableEntity()
        {
            if (allEntities.Count == 0 || currentTurnIndex >= allEntities.Count)
            {
                RandomTurnBase();
            }
            Entity availableEntity = allEntities[currentTurnIndex];
            return availableEntity;
        }

        public bool IsPLayerTurn()
        {
            return currentEntity is EntityPlayer;
        }

        public void UpdateActionState(ActionState newState)
        {
            currentActionState = newState;
        }

        private bool IsActionState(ActionState checkState)
        {
            return currentActionState == checkState;
        }

        public void UpdateActionType(ActionType newType)
        {
            currentActionType = newType;
            movementCombatManager.OnResetNodes();
            UpdateExecuteedAction(newType);
            BattlePhaseUIManager.Instance.ToggleShowActionPanel(false);
            switch (currentActionType)
            {
                case ActionType.Waiting:
                    CamController.Instance.SetCurrentCam(CamType.Battle_View, currentEntity.transform);
                    BattlePhaseUIManager.Instance.ToggleShowActionPanel(true);
                    BattlePhaseUIManager.Instance.ToggleActionCardPanel(false);
                    movementCombatManager.ClearModifyPath();
                    break;
                case ActionType.Move:
                    CamController.Instance.SetCurrentCam(CamType.Full_View);
                    BattlePhaseUIManager.Instance.ToggleActionCardPanel(false);
                    movementCombatManager.ClearModifyPath();
                    break;
                case ActionType.Action:
                    BattlePhaseUIManager.Instance.GenerateActionCards(new List<SpellData> { spellData });
                    BattlePhaseUIManager.Instance.ToggleActionCardPanel(true);
                    break;
                case ActionType.End_Turn:
                    CamController.Instance.SetCurrentCam(CamType.Full_View);
                    BattlePhaseUIManager.Instance.ToggleActionCardPanel(false);
                    movementCombatManager.ClearModifyPath();
                    break;
            }
        }

        public bool IsActionType(ActionType checkType)
        {
            return currentActionType == checkType;
        }

        public void UpdateExecuteedAction(ActionType actionType)
        {
            if (!actionPerTurnList.Contains(actionType) && actionType != ActionType.Waiting)
            {
                actionPerTurnList.Add(actionType);
                BattlePhaseUIManager.Instance.UpdateExecutedActionUI(actionType);
            }

            bool isOutOfActionToExecute = CheckOutOfAction(actionType);
            if (isOutOfActionToExecute)
            {
                // reset
                actionPerTurnList.Clear();
                BattlePhaseUIManager.Instance.ResetExecutedActionUI();
                GetNextTurn();
            }
        }

        private bool CheckOutOfAction(ActionType actionType)
        {
            return actionType == ActionType.End_Turn;
        }

        private void StartCastSpell(SpellData spellData)
        {
            currentSpellSelect = spellData;
            spellData.StartCastSpell(this);
        }

        public void ShowCastingRange(Node startNode, int castRange)
        {
            movementCombatManager.ShowCastingRange(startNode, castRange);
        }

        public void ShowModifyRange(Node playerNode, Node startNode, int modifyRange)
        {
            movementCombatManager.ShowModifyRange(playerNode, startNode, modifyRange);
        }

        public void ToggleModifiableState(bool isModifiable)
        {
            inModifiableState = isModifiable;
        }

        public void ExecuteSpell(SpellData spellData)
        {
            StartCoroutine(CorExecuteSpell(spellData));
        }

        public IEnumerator CorExecuteSpell(SpellData spellData)
        {
            yield return new WaitForSeconds(spellData.DurationEffect);
            var enemy = currentNodeSelected.entityOnNode as EntityEnemy;
            if (enemy != null)
            {
                Debug.LogError("Attack");
                enemy.TakeDamage(spellData.ModifyData.ModifyValue);
                var screenPos = GameHelper.WorldToScreenPoint(enemy.transform.position);
                BattlePhaseUIManager.Instance.ShowModifyValue(spellData.ModifyData.ModifyValue, screenPos);
            }
        }
    }
}