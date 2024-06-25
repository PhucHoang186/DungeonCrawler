using System.Collections;
using System.Collections.Generic;
using EntityObject;
using GameUI;
using Managers;
using Map;
using UnityEngine;

namespace Data
{
    public abstract class ActiveSpell : SpellData
    {
        public CastPattern CastPattern;
        public bool NeedTarget = true;

        public abstract bool CheckCastable(Node node);
    }
}