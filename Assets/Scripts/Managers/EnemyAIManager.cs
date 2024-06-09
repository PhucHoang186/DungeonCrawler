using System.Collections;
using System.Collections.Generic;
using EntityObject;
using Map;
using UnityEngine;

namespace Managers
{
    public class EnemyAIManager : MonoBehaviour
    {
        public Node GetMovementAction(EntityEnemy enemy, GridManager gridManger, BattleManager battleManager)
        {
            return enemy.GetMovementAction(gridManger, battleManager);
        }
    }
}