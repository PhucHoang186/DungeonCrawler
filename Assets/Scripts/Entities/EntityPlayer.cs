using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Map;
using UnityEngine;

namespace EntityObject
{
    public class EntityPlayer : CharacterEntity
    {
        [HideInInspector] public bool IsMainPlayer;
    }
}
