using System.Collections;
using System.Collections.Generic;
using EntityObject;
using Managers;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Effect/CloseRange")]
    public class CloseRangeEffectData : EffectData
    {
        private GameObject effect;
        public override void ShowCastingEffect(BattleManager battleManager)
        {
            Entity player = battleManager.currentPlayer;
            effect = Instantiate(CastingEffect, player.transform);
        }

        public override void ShowExecuteEffect(BattleManager battleManager)
        {
            Entity player = battleManager.currentPlayer;
            effect = Instantiate(ExecuteEffect, player.transform);
            effect.transform.rotation = Quaternion.LookRotation(battleManager.CurrentNodeSelected.transform.position - player.transform.position);
            effect.transform.position = battleManager.CurrentNodeSelected.transform.position;
        }

        public override void EndEffect(BattleManager battleManager)
        {
            // temp
            Destroy(effect);
        }
    }
}
