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
            battleManager.ShowCastingRange(player.currentNode, CastRange);



        }
    }
}