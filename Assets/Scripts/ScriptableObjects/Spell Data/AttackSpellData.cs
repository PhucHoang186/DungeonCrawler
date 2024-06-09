using System.Collections;
using System.Collections.Generic;
using EntityObject;
using GameUI;
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
            if (NeedTarget)
            {
                var enemies = battleManager.GetAllEnemies();
                battleManager.ShowModifyRange(player.currentNode, currentNodeSelected, ModifyRange);
                if (enemies.Contains(currentNodeSelected.entityOnNode as EntityEnemy))
                {
                    battleManager.ToggleModifiableState(true);
                    return;
                }
            }

            battleManager.ToggleModifiableState(false);
        }

        public override void ExcuteSpell(BattleManager battleManager)
        {
            Node currentNodeSelected = battleManager.CurrentNodeSelected;
            var enemy = currentNodeSelected.entityOnNode as EntityEnemy;
            if (enemy != null)
            {
                enemy.TakeDamage(ModifyData.ModifyValue);
                var screenPos = GameHelper.WorldToScreenPoint(enemy.transform.position);
                BattlePhaseUIManager.Instance.ShowModifyValue(ModifyData.ModifyValue, screenPos);
            }
        }

        public override void StartCastSpell(BattleManager battleManager)
        {
            var player = battleManager.currentPlayer;
            battleManager.ShowCastingRange(player.currentNode, CastRange);
        }
    }
}