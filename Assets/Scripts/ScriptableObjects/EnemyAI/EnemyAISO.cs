using System.Collections;
using System.Collections.Generic;
using EntityObject;
using Managers;
using Map;
using UnityEngine;

namespace AIBehavior
{
    public abstract class EnemyAISO : ScriptableObject
    {
        public abstract Node GetMovementAction(GridManager gridManger, BattleManager battleManager, EntityEnemy enemy);
    }
}
