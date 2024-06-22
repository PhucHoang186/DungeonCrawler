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
        public CastPattern CastPattern;

        public override void CastingSpell(BattleManager battleManager)
        {
            var player = battleManager.currentPlayer;
            Node currentNodeSelected = battleManager.CurrentNodeSelected;
            if (NeedTarget)
            {
                // var castableNodes = battleManager.ShowModifyRange(player.currentNode, currentNodeSelected, ModifyRange);
                // if (castableNodes == null)
                    // return;

                var enemies = battleManager.GetAllEnemies();
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
            //effect
            if (EffectModifier.Turn > 1)
            {
                //long cast spell
                EffectModifier.effectData.ShowCastingEffect(battleManager);
            }
            else
            {
                EffectModifier.effectData.ShowExecuteEffect(battleManager);
            }

            battleManager.ExecuteSpell(this);

        }

        public override void StartCastSpell(BattleManager battleManager)
        {
            var player = battleManager.currentPlayer;
            // battleManager.ShowCastingRange(player.currentNode, CastRange);
        }
    }
}