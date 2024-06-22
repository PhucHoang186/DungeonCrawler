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
        private GridManager gridManager;
        private Entity currentEntity;
        private List<Entity> allEntities = new();
        private List<EntityPlayer> players = new();
        private List<EntityEnemy> enemies = new();
        private EntityManager entityManager;
        private MovementCombatManager movementCombatManager;
        private TurnBaseType currentTurnType;
        private CommandStatus currentCommandStatus = CommandStatus.Done;
        private CommandType currentActionType;
        private HashSet<CommandType> actionPerTurnList = new();
        private SpellData currentSpellSelect;
        private Node currentNodeSelected;
        private int currentTurnIndex;
        private ActionCardDisplay curentUsedCard;
        public bool inModifiableState;
        private bool isPlayerState;

        public Node CurrentNodeSelected => currentNodeSelected;

        void Start()
        {
            GameEvents.ON_MOUSE_OVER_NODE += OnMouseOverNode;
        }

        void OnDestroy()
        {
            GameEvents.ON_MOUSE_OVER_NODE -= OnMouseOverNode;
        }

        private void OnMouseOverNode(Node targetNode)
        {
            if (!CheckCommandStatus(CommandStatus.Done))
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
                case CommandType.Move:
                    UpdateMovePath(targetNode);
                    break;
                case CommandType.Waiting_Action:
                case CommandType.Action:
                    UpdatActionNode(targetNode);
                    break;
            }
        }

        private void OnSelectAction(ActionCardDisplay actionCard)
        {
            curentUsedCard = actionCard;
            StartCastSpell(actionCard.SpellData);
            BattlePhaseUIManager.Instance.OnSelectCard(actionCard);
        }

        private void UpdateMovePath(Node targetNode)
        {
            if (!(targetNode is MoveableNode))
                return;
            if (!targetNode.isEmpty)
                return;

            movementCombatManager.ShowWalkablePath(currentPlayer.currentNode, targetNode, 5, true);
        }

        public void Init(EntityManager entityManager, MovementCombatManager movementCombatManager, GridManager gridManager)
        {
            (this.players, this.enemies) = entityManager.GetAllEntities();
            this.entityManager = entityManager;
            this.movementCombatManager = movementCombatManager;
            this.gridManager = gridManager;
            BattlePhaseUIManager.Instance?.GenerateTurnIcons(turnPerRound);
            BattlePhaseUIManager.Instance?.AssignChangeActionTypeCb(UpdateCommandTypeImmediately);
            GetNextTurn();
            CamController.Instance.SetCurrentCam(CamType.Full_View);
        }

        private void UpdateCommandTypeImmediately(CommandType newType)
        {
            UpdateCommandType(newType, 0f);
        }

        public void GetNextTurn()
        {
            currentEnemy = null;
            currentPlayer = null;
            currentEntity = GetAvailableEntity();
            isPlayerState = IsPLayerTurn();

            BattlePhaseUIManager.Instance?.UpdateTurn(currentTurnIndex);
            currentTurnIndex++;

            if (isPlayerState)
                currentPlayer = currentEntity as EntityPlayer;
            else
                currentEnemy = currentEntity as EntityEnemy;

            OnChangeTurnType(isPlayerState ? TurnBaseType.Player : TurnBaseType.Enemy);
            UpdateCommandType(CommandType.Waiting, 1f);
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
                case CommandType.Move:
                    PlayerMovement();
                    break;
                case CommandType.Waiting_Action:
                case CommandType.Action:
                    PlayerAction();
                    break;
            }
        }

        private void PlayerMovement()
        {
            if (!InputManager.InputData.isPointerOverUI && InputManager.InputData.isMouseDownLeft)
            {
                movementCombatManager.MoveEntityToNode(currentPlayer, finalCb: OnFinshAction, successCb: () => { UpdateCommandStatus(CommandStatus.Progressing); });
            }
        }

        private void PlayerAction()
        {
            if (!inModifiableState)
                return;

            if (!InputManager.InputData.isPointerOverUI && InputManager.InputData.isMouseDownLeft)
            {
                OnUseSpell();
            }
        }

        private void UpdatActionNode(Node node)
        {
            SpellManager.Instance.GetNodeMouseOn(node);
            SpellManager.Instance.CastingSpell(movementCombatManager);
        }

        private void OnFinshAction()
        {
            UpdateCommandStatus(CommandStatus.Done);
            UpdateCommandType(CommandType.Waiting);
        }

        private void EnemyTurn()
        {
            if (currentEnemy == null)
                return;
            if (CheckCommandStatus(CommandStatus.Progressing))
                return;

            // UpdateActionStatus(ActionStatus.Progressing);
        }

        public void StartEnemyMovement(Node desNode)
        {
            UpdateCommandStatus(CommandStatus.Progressing);
            movementCombatManager.ShowWalkablePath(currentEnemy.currentNode, desNode, 1, false);
            movementCombatManager.MoveEntityToNode(currentEnemy, finalCb: EnemyFinishCommand, failCb: EnemyFinishCommand);
        }

        // temp
        private void EnemyFinishCommand()
        {
            UpdateCommandStatus(CommandStatus.Done);
            UpdateCommandType(CommandType.Waiting);
        }

        public void OnChangeTurnType(TurnBaseType newType)
        {
            if (this.currentTurnType == newType)
                return;
            isPlayerState = newType == TurnBaseType.Player;
            this.currentTurnType = newType;
            switch (currentTurnType)
            {
                case TurnBaseType.Player:
                SpellManager.Instance.GetCurrentPlayerSelected(currentPlayer);
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

        public void UpdateCommandStatus(CommandStatus newStatus)
        {
            currentCommandStatus = newStatus;
        }

        private bool CheckCommandStatus(CommandStatus checkStatus)
        {
            return currentCommandStatus == checkStatus;
        }

        public void UpdateCommandType(CommandType newType, float delay = 0f)
        {
            StartCoroutine(CorUpdateCommandType(newType, delay));
        }

        private IEnumerator CorUpdateCommandType(CommandType newType, float delay = 0f)
        {
            currentActionType = newType;
            movementCombatManager.OnResetNodes();
            ClearModifyPath();
            BattlePhaseUIManager.Instance.ToggleShowCommandPanel(false);

            bool isPlayerState = this.isPlayerState;
            UpdateExecutedCommandUI(newType, isPlayerState);
            yield return new WaitForSeconds(delay);

            switch (currentActionType)
            {
                case CommandType.Waiting:
                    if (isPlayerState)
                        PlayerWaitingPhase();
                    else
                        EnemyWaitingPhase();
                    break;
                case CommandType.Move:
                    if (isPlayerState)
                        PlayerMovePhase();
                    else
                        EnemyMovePhase();
                    break;
                case CommandType.Action:
                    if (isPlayerState)
                        PlayerActionPhase();
                    else
                        EnemyActionPhase();
                    break;
                case CommandType.End_Turn:
                    if (isPlayerState)
                        PlayerEndPhase();
                    else
                        EnemyEndTurnPhase();
                    break;
                case CommandType.Waiting_Action:
                    break;
            }
        }

        private void PlayerEndPhase()
        {
            CamController.Instance.SetCurrentCam(CamType.Full_View);
            BattlePhaseUIManager.Instance.ToggleActionCardPanel(false);
            BattlePhaseUIManager.Instance.ResetExecutedActionUI();
            actionPerTurnList.Clear();
            GetNextTurn();
        }

        private void PlayerActionPhase()
        {
            CharacterData characterData = currentPlayer.EntityData as CharacterData;
            List<SpellData> spells = SpellManager.Instance.GetSpellsForTurn(5, characterData);

            BattlePhaseUIManager.Instance.InitActionCards(spells, OnSelectAction);
            BattlePhaseUIManager.Instance.ToggleActionCardPanel(true);
        }

        private void PlayerMovePhase()
        {
            CamController.Instance.SetCurrentCam(CamType.Full_View);
            BattlePhaseUIManager.Instance.ToggleActionCardPanel(false);
        }

        private void PlayerWaitingPhase()
        {
            CamController.Instance.SetCurrentCam(CamType.Battle_View, currentEntity.transform);
            BattlePhaseUIManager.Instance.ToggleShowCommandPanel(true);
            BattlePhaseUIManager.Instance.ToggleActionCardPanel(false);
        }

        private void EnemyEndTurnPhase()
        {
            CamController.Instance.SetCurrentCam(CamType.Full_View);
            GetNextTurn();
        }

        private void EnemyActionPhase()
        {

        }

        private void EnemyMovePhase()
        {

        }

        private void EnemyWaitingPhase()
        {
            enemyAIManager.GetEnemyAction(currentEnemy, gridManager, this);
        }

        private void ClearModifyPath()
        {
            movementCombatManager.ClearModifyPath();
        }

        public bool IsActionType(CommandType checkType)
        {
            return currentActionType == checkType;
        }

        public void UpdateExecutedCommandUI(CommandType commandType, bool isPlayer)
        {
            if (!isPlayer)
                return;

            if (!actionPerTurnList.Contains(commandType) && commandType != CommandType.Waiting)
            {
                actionPerTurnList.Add(commandType);
                BattlePhaseUIManager.Instance.UpdateExecutedCommandUI(commandType);
            }
        }

        private bool CheckEndTurn(CommandType actionType)
        {
            return actionType == CommandType.End_Turn;
        }

        private void StartCastSpell(SpellData spellData)
        {
            currentSpellSelect = spellData;
            SpellManager.Instance.GetCurrentUsedSpell(spellData);
            spellData.StartCastSpell(this);
        }

        private void OnUseSpell()
        {
            currentSpellSelect.ExcuteSpell(this);
            BattlePhaseUIManager.Instance.OnUseCard(curentUsedCard);
            currentSpellSelect = null;
            UpdateCommandType(CommandType.Waiting_Action);
            ToggleModifiableState(false);
        }

        // public void ShowCastingRange(Node startNode, int castRange)
        // {
        //     movementCombatManager.ShowCastingRange(startNode, castRange);
        // }

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
                enemy.TakeDamage(spellData.ModifierData.ModifyValue);
                var screenPos = GameHelper.WorldToScreenPoint(enemy.transform.position);
                BattlePhaseUIManager.Instance.ShowModifyValue(spellData.ModifierData.ModifyValue, screenPos);
            }
        }

        public List<Node> GetCastRange()
        {
            return movementCombatManager.CastNodes;
        }
    }
}