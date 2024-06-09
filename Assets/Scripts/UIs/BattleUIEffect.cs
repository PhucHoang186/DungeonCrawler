using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class BattleUIEffect : MonoBehaviour
    {
        [SerializeField] ModifyValueUIDisplay modifyDisplayPrefab;
        [SerializeField] float displayDuration;

        public void ShowModifyValue(float value, Vector2 spawnPosition)
        {
            var modifyDisplay = Instantiate(modifyDisplayPrefab, spawnPosition, Quaternion.identity, transform);
            modifyDisplay.ShowModifyValue(value);
            Destroy(modifyDisplay.gameObject, displayDuration);
        }
    }
}
