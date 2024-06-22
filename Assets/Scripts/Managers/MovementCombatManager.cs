using System;
using System.Collections;
using System.Collections.Generic;
using EntityObject;
using Managers;
using Map;
using UnityEngine;

public enum MovementType
{
    Free_Move,
    TurnBase_Move_Player,
    TurnBase_Move_Enemy,
}

public class MovementCombatManager : MonoBehaviour
{
    [SerializeField] MovementType currentMoveType;
    [SerializeField] Visualization visualization;
    [SerializeField] BattleManager battleManager;
    [SerializeField] EnemyAIManager enemyAIManager;
    [SerializeField] bool isDiagonal;
    private GridManager gridManager;
    private EntityPlayer player;
    private Vector3Int moveVec;
    private List<Node> pathToNodeList = new();
    private List<Node> moveNodes = new();
    private List<Node> castNodes = new();
    private List<Node> modifyNodes = new();
    private bool startMoveToNode;
    private float currentMoveCountDown;

    public List<Node> MoveNodes => moveNodes;
    public List<Node> CastNodes => castNodes;
    public List<Node> ModifyNodes => modifyNodes;


    public void Init(GridManager gridManager, EntityManager entityManager)
    {
        this.gridManager = gridManager;
        battleManager.Init(entityManager, this, gridManager);
    }

    void Update()
    {
        if (currentMoveType == MovementType.Free_Move)
        {
            // move by button
            PlayerFreeMovement();
        }
    }

    private void PlayerFreeMovement()
    {
        if (currentMoveCountDown <= 0f)
        {
            GetFreeMovementInput();
            MoveToNodeByButton();
        }
        else
            currentMoveCountDown -= Time.deltaTime;
    }

    private void MoveToNodeByButton()
    {
        if (moveVec == Vector3.zero)
            return;

        Node currentNodeOn = player.currentNode;
        int xIndex = currentNodeOn.X + moveVec.x;
        int yIndex = currentNodeOn.Y + moveVec.y;
        if (!gridManager.IsNodeValid(xIndex, yIndex))
            return;

        Node newNode = gridManager.GetNodeByIndex(xIndex, yIndex);
        currentMoveCountDown = player.MoveToNode(newNode) + 0.2f;
    }

    private void GetFreeMovementInput()
    {
        moveVec = Vector3Int.zero;
        moveVec.x = (int)Input.GetAxisRaw("Horizontal");
        if (moveVec.x == 0)
            moveVec.y = (int)Input.GetAxisRaw("Vertical");
    }

    public void OnResetNodes()
    {
        GameEvents.ON_RESET_NODE?.Invoke();
    }

    public void ShowWalkablePath(Node startNode, Node targetNode, int stepCount, bool isVisualize)
    {
        GetPathToNodeList(startNode, targetNode);
        TrimPathInMoveRange(startNode, stepCount);
        if (isVisualize)
        {
            VisualizePath(pathToNodeList);
            VisualizeRange(moveNodes, highlightType.Move, false);
        }
    }

    public void ShowCastingRange(List<Node> castNodes)
    {
        this.castNodes = castNodes;
        VisualizeRange(castNodes, highlightType.ActionRange);
    }

    public List<Node> ShowModifyRange(Node currentPlayerNode, Node targetNode, List<Node> modifyNodes)
    {
        if (!castNodes.Contains(targetNode))
            return null;
        ResetNode(modifyNode: true, revertLastState: true);
        this.modifyNodes = modifyNodes;
        VisualizeRange(modifyNodes, highlightType.Action, false);
        VisualizeModifyPath(currentPlayerNode, targetNode);

        return modifyNodes;
    }

    public void GetPathToNodeList(Node startNode, Node targetNode)
    {
        pathToNodeList = gridManager.GetPathToNode(startNode, targetNode, isDiagonal);
    }

    private void TrimPathInMoveRange(Node startNode, int stepCount)
    {
        moveNodes.Clear();
        moveNodes = gridManager.GetNeighborNodesWithStep(startNode, stepCount, false);

        if (pathToNodeList == null)
            return;
        var tempPathToNodeList = new List<Node>(pathToNodeList);
        foreach (var node in tempPathToNodeList)
        {
            if (!moveNodes.Contains(node))
            {
                pathToNodeList.Remove(node);
            }
        }
    }

    public void MoveEntityToNode(Entity player, Action finalCb = null, Action successCb = null, Action failCb = null)
    {
        if (pathToNodeList == null || pathToNodeList.Count == 0)
        {
            failCb?.Invoke();
        }
        else
        {
            successCb?.Invoke();
            MoveToNodeByPath(pathToNodeList, player, finalCb);
        }
    }

    private void VisualizePath(List<Node> path)
    {
        visualization.VisualizePath(path);
    }

    public void VisualizeRange(List<Node> neighborNodes, highlightType highlightType, bool recordThisState = true, bool revertLastState = false)
    {
        visualization.VisualizeRange(neighborNodes, highlightType, recordThisState, revertLastState);
    }

    public void VisualizeModifyPath(Node startNode, Node endNode)
    {
        visualization.VisualizeModifyPath(startNode, endNode);
    }

    private void ClearPath()
    {
        pathToNodeList.Clear();
        visualization.ClearPath();
    }


    public void ClearModifyPath()
    {
        visualization.ClearModifyPath();
    }

    private void MoveToNodeByPath(List<Node> path, Entity entity, Action cb = null)
    {
        StartCoroutine(CorMoveToNodeByPath(path, entity, cb));
    }

    private IEnumerator CorMoveToNodeByPath(List<Node> path, Entity entity, Action cb = null)
    {
        if (path.Count == 0)
            yield break;
        entity.currentNode.SetEmptyState(true);
        entity.currentNode.ResetEntityOnNode();
        startMoveToNode = true;
        while (path.Count > 0)
        {
            Node newNode = path[0];
            path.RemoveAt(0);
            float moveTime = (entity as CharacterEntity).MoveToNode(newNode) + 0.2f;
            yield return new WaitForSeconds(moveTime);
            if (path.Count == 0)
            {
                newNode.SetEmptyState(false);
                newNode.entityOnNode = entity;
            }
        }
        startMoveToNode = false;
        ClearPath();
        cb?.Invoke();
    }

    public void ResetNode(bool moveNode = false, bool castNode = false, bool modifyNode = false, bool recordThisState = true, bool revertLastState = false)
    {
        if (moveNode)
            visualization.ClearRange(moveNodes, highlightType.Move, recordThisState, revertLastState);

        if (castNode)
            visualization.ClearRange(castNodes, highlightType.ActionRange, recordThisState, revertLastState);

        if (modifyNode)
            visualization.ClearRange(modifyNodes, highlightType.Action, recordThisState, revertLastState);
    }
}
