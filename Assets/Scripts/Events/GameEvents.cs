using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Map;
using UnityEngine;

public class GameEvents
{
    public static Action<Node> ON_MOUSE_OVER_NODE;
    public static Action ON_RESET_NODE;
    public static Action<SpellData> ON_SELECT_ACTION;
}
