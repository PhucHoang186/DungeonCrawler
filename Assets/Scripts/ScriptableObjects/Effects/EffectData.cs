using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Data
{
    public abstract class EffectData : ScriptableObject
    {
        public GameObject CastingEffect;
        public GameObject ExecuteEffect;
        public float DurationEffect; // the time the effect last

        public abstract void ShowCastingEffect(BattleManager battleManager);
        public abstract void ShowExecuteEffect(BattleManager battleManager);
        public abstract void EndEffect(BattleManager battleManager);
    }
}
