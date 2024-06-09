using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public abstract class EntityData : ScriptableObject
    {
        public Sprite EntityIcon;
        public EntityType EntityType;
        public CharacterClass CharaterClass;
        public List<SpellData> EquippedSpellList = new(5); // max 5 spells can be equipped
        public List<SpellData> LearnedSpellList;
    }
}
