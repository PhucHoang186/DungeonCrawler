using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ActionTypeColorConfig")]
public class ActionTypeColorConfig : ScriptableObject
{
    public List<ActionColor> ActionColors;

    public ActionColor GetMatchActionColorData(ModifyType modifyType)
    {
        for (int i = 0; i < ActionColors.Count; i++)
        {
            if (ActionColors[i].ModifyType == modifyType)
                return ActionColors[i];
        }
        return null;
    }
}

[Serializable]
public class ActionColor
{
    public ModifyType ModifyType;
    public Color MainColor;
    public Sprite Icon;
}
