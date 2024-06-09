using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Map;
using UnityEngine;

namespace EntityObject
{
    public class EntityPlayer : Entity
    {
        [HideInInspector] public bool IsMainPlayer;
    }
}
