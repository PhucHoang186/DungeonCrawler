using System.Collections;
using System.Collections.Generic;
using EntityObject;
using Managers;
using Map;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Spell/AttackSpell")]
    public class AttackSpellData : SpellData
    {
        public override void CastingSpell(BattleManager battleManager)
        {
            var player = battleManager.currentPlayer;
            Node currentNodeSelected = battleManager.CurrentNodeSelected;
            var enemies = battleManager.GetAllEnemies();
            battleManager.ShowModifyRange(player.currentNode, currentNodeSelected, ModifyRange);
            if (enemies.Contains(currentNodeSelected.entityOnNode as EntityEnemy))
            {
                Debug.LogError("Empty");
            }
        }

        public override void ExcuteSpell(BattleManager battleManager)
        {
        }

        public override void StartCastSpell(BattleManager battleManager)
        {
            var player = battleManager.currentPlayer;
            battleManager.ShowCastingRange(player.currentNode, CastRange);
        }
    }
}