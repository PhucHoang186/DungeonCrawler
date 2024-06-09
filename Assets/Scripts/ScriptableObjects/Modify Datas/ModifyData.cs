using System.Collections;
using System.Collections.Generic;
using EntityObject;
using Managers;
using UnityEngine;

namespace Data
{
    public abstract class ModifyCategory : ScriptableObject
    {
        public ModifyType ModifyType;

        public abstract void OnActivateModify(BattleManager battleManager, Entity entity);
    }
}
