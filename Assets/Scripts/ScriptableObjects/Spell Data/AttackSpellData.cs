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
    public class AttackSpellData : ActiveSpell
    {
        // public override void CastingSpell(BattleManager battleManager)
        // {
        // }

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

        // public override void StartCastSpell(BattleManager battleManager)
        // {
        //     var player = battleManager.currentPlayer;
        //     // battleManager.ShowCastingRange(player.currentNode, CastRange);
        // }

        public override bool CheckCastable(Node node)
        {
            if (NeedTarget)
            {
                bool isEnemyNode = node.entityOnNode is EntityEnemy;
                return isEnemyNode;
            }
            else
            {

                return false;
            }
        }
    }
}