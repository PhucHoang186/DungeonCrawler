using System;
using System.Collections;
using System.Collections.Generic;
using EntityObject;
using Managers;
using UnityEngine;

namespace Data
{
    public abstract class SpellData : ScriptableObject
    {
        [Header("Info")]
        public Sprite SpellIcon;
        public string SpellDescription = "Some useful info";
        [Header("Value")]
        public int CastRange;
        public int ModifyRange;
        public ModifierData ModifierData;
        public ModifierData CostData;
        public EffectModifier EffectModifier;
        public float DurationEffect => EffectModifier.effectData.DurationEffect;

        // public abstract void StartCastSpell(BattleManager battleManager);
        // public abstract void CastingSpell(BattleManager battleManager);
        public abstract void ExcuteSpell(BattleManager battleManager);
    }

    [Serializable]
    public class ModifierData
    {
        public ModifyData ModifyData;
        public float ModifyValue;
    }

    [Serializable]
    public class EffectModifier
    {
        public EffectData effectData;
        public int Turn;
    }
}