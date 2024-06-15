using EntityObject;
using Managers;
using UnityEngine;

namespace Data
{
    public abstract class ModifyData : ScriptableObject
    {
        public ModifyType ModifyType;

        public abstract void OnActivateModify(BattleManager battleManager, Entity entity);
    }
}
