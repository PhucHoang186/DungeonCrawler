using System.Collections;
using System.Collections.Generic;
using Data;
using EntityObject;
using Managers;
using Map;
using UnityEngine;

namespace AIBehavior
{
    [CreateAssetMenu(menuName = "AI/EnemyAI_Warrior")]
    public class EnemyAISOWarrior : EnemyAISO
    {
        #region Abstract Class

        public override SpellData GetAction(GridManager gridManger, BattleManager battleManager, EntityEnemy enemy)
        {
            var castRange = battleManager.GetCastRange();
            SpellData spellUsed = null;
            for (int i = 0; i < castRange.Count; i++)
            {
                Entity entity = castRange[i].entityOnNode;
                if (entity != null && entity is EntityPlayer)
                {
                    // attack
                    // temp
                    var listSpells = enemy.EntityData.EquippedSpellList;
                    listSpells.Shuffle();
                    spellUsed = listSpells[0];
                    break;
                }
            }

            return spellUsed;
        }

        public override Node GetMovement(GridManager gridManger, BattleManager battleManager, EntityEnemy enemy)
        {
            var players = battleManager.GetAllPlayers();
            var targetPlayer = GetNearestPlayer(players, enemy);
            var neighBorNode = gridManger.GetNeighborNodes(targetPlayer.currentNode);
            // temp
            Node desNode = GetShortestPath(neighBorNode, enemy);
            return desNode;
        }

        #endregion

        private EntityPlayer GetNearestPlayer(List<EntityPlayer> players, EntityEnemy enemy)
        {
            EntityPlayer player = null;
            float minDistance = Mathf.Infinity;

            for (int i = 0; i < players.Count; i++)
            {
                float distance = Vector3.Distance(enemy.transform.position, players[i].transform.position);
                if (distance < minDistance)
                {
                    player = players[i];
                    minDistance = distance;
                }
            }
            return player;
        }


        private Node GetShortestPath(List<Node> nodes, EntityEnemy enemy)
        {
            Node node = null;
            float minDistance = Mathf.Infinity;
            for (int i = 0; i < nodes.Count; i++)
            {
                float distance = Vector3.Distance(nodes[i].transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    node = nodes[i];
                    minDistance = distance;
                }
            }
            return node;
        }
    }
}
