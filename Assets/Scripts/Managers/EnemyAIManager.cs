using System.Collections;
using System.Collections.Generic;
using Data;
using EntityObject;
using Map;
using UnityEngine;

namespace Managers
{
    public class EnemyAIManager : MonoBehaviour
    {
        public int actionOrder;
        private bool endOfActionOrder;

        public void GetEnemyAction(EntityEnemy enemy, GridManager gridManager, BattleManager battleManager)
        {
            switch (actionOrder)
            {
                case 0:
                    CheckEnemyConditions(enemy, battleManager);
                    break;
                case 1:
                    EnemyMovement(enemy, gridManager, battleManager);
                    break;
                case 2:
                    EnemyAction(enemy, gridManager, battleManager);
                    break;
                case 3:
                    EnemyEndAction(battleManager);
                    break;

            }
        }

        private void CheckEnemyConditions(EntityEnemy enemy, BattleManager battleManager)
        {
            if (enemy.EntityState == CharacterEntityState.Down)
            {
                // temp
                EnemyEndAction(battleManager);
            }
            else if (enemy.EntityState == CharacterEntityState.Destroy)
            {
                EnemyEndAction(battleManager);
            }
            else
            {
                UpdateActionOrder();
                battleManager.UpdateCommandType(CommandType.Waiting);
            }
        }

        private void EnemyMovement(EntityEnemy enemy, GridManager gridManager, BattleManager battleManager)
        {
            Node desNode = GetMovement(enemy, gridManager, battleManager);
            if (desNode != null)
                battleManager.StartEnemyMovement(desNode);
            UpdateActionOrder();
        }

        private Node GetMovement(EntityEnemy enemy, GridManager gridManager, BattleManager battleManager)
        {
            return enemy.GetMovement(gridManager, battleManager);
        }

        private void EnemyAction(EntityEnemy enemy, GridManager gridManager, BattleManager battleManager)
        {
            SpellData spellData = GetAction(enemy, gridManager, battleManager);
            if (spellData)
            {
                battleManager.ExecuteSpell(spellData);
                UpdateActionOrder();
            }
            else
            {
                EnemyEndAction(battleManager);
            }
        }

        private SpellData GetAction(EntityEnemy enemy, GridManager gridManager, BattleManager battleManager)
        {
            return enemy.GetAction(gridManager, battleManager);
        }

        private void EnemyEndAction(BattleManager battleManager)
        {
            actionOrder = 0;
            battleManager.UpdateCommandType(CommandType.End_Turn);
            endOfActionOrder = true;
        }

        private void UpdateActionOrder()
        {
            if (endOfActionOrder)
            {
                endOfActionOrder = false;
                return;
            }
            actionOrder++;
        }

    }
}