using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using AIBehavior;
using Managers;
using Map;
using UnityEngine;

namespace EntityObject
{
    public class EntityEnemy : CharacterEntity
    {
        [SerializeField] EnemyAISO enemyAI;

        public virtual Node GetMovement(GridManager gridManger, BattleManager battleManager)
        {
            return enemyAI.GetMovement(gridManger, battleManager, this);
        }

        public virtual Node GetAction(GridManager gridManger, BattleManager battleManager)
        {
            return enemyAI.GetMovement(gridManger, battleManager, this);
        }
    }
}
