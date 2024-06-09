using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class ModifyValueUIDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text modifyText;

        public void ShowModifyValue(float value)
        {
            value = Mathf.Abs(value);
            modifyText.text = value.ToString();
        }
    }
}
