using System.Collections;
using System.Collections.Generic;
using EntityObject;
using Managers;
using Map;
using UnityEngine;

namespace AIBehavior
{
    [CreateAssetMenu(menuName = "AI/EnemyAI_Warrior")]
    public class EnemyAISOWarrior : EnemyAISO
    {
        public override Node GetMovementAction(GridManager gridManger, BattleManager battleManager, EntityEnemy enemy)
        {
            Node desNode = null;
            var players = battleManager.GetAllPlayers();
            var targetPlayer = players[0];

            int xDistance = (int)(targetPlayer.transform.position.x - enemy.transform.position.x);
            int yDistance = (int)(targetPlayer.transform.position.z - enemy.transform.position.z);
            int trueDistance = xDistance + yDistance;
            // int finalStep = Mathf.Min(trueDistance, (int)strength);

            //temp
            desNode = gridManger.GetNodeByIndex(enemy.currentNode.X + xDistance, enemy.currentNode.Y + yDistance + 1);
            return desNode;
        }
    }
}
