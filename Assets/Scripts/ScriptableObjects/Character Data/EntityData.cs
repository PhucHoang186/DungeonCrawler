using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public abstract class EntityData : ScriptableObject
    {
        public Sprite EntityIcon;
        public EntityType EntityType;
        public float MaxHealth;
        public float MaxStamina;
        public float MaxMana;
        public CharacterClass CharaterClass;
        public List<SpellData> EquippedSpellList = new();
        public List<SpellData> LearnedSpellList;

    }
}
