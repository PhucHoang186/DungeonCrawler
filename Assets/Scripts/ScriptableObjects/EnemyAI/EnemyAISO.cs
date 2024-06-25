using System.Collections;
using System.Collections.Generic;
using Data;
using EntityObject;
using Managers;
using Map;
using UnityEngine;

namespace AIBehavior
{
    public abstract class EnemyAISO : ScriptableObject
    {
        public abstract Node GetMovement(GridManager gridManger, BattleManager battleManager, EntityEnemy enemy);
        public abstract SpellData GetAction(GridManager gridManger, BattleManager battleManager, EntityEnemy enemy);
    }
}
