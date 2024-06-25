using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace EditorCustom
{
    public class SpellCreationMenu : EditorWindow
    {
        private string savePath = "Assets/Datas/Spell Datas/";
        private string spellName = "Something Cool";
        private static SpellCreationMenu myWindow;
        private bool toggleModifyPath;
        private int castRange;
        private int ModifyRange;
        private ModifyData modifyData;
        private float modifyValue;
        private ModifyData costData;
        private float costValue;
        private EffectModifier effectModifier;

        // data
        private Sprite spellIcon;
        private string spellDescription = "Some Useful Info";
        private bool needTarget;


        [MenuItem("Window/Spell Creation Menu")]
        public static void ShowWindow()
        {
            myWindow = GetWindow<SpellCreationMenu>();
            myWindow.titleContent = new GUIContent("Creation Spell");
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            CreatSpellAsset();
            EditorGUILayout.EndVertical();
        }

        private void CreatSpellAsset()
        {
            EditorGUILayout.Space(10);
            toggleModifyPath = EditorGUILayout.BeginToggleGroup("Allow Modify Path", toggleModifyPath);
            savePath = EditorGUILayout.TextField("Save Folder Path:", savePath);
            EditorGUILayout.EndToggleGroup();

            GetSpellDatas();
            if (GUILayout.Button("Create Spell!"))
            {
                string finalPath = string.Format("{0}{1}.asset", savePath, spellName);
                SpellData spellData = ScriptableObject.CreateInstance<AttackSpellData>();
                SetSpellDatas(spellData);
                AssetDatabase.CreateAsset(spellData, finalPath);
                AssetDatabase.SaveAssets();
            }
        }

        private void GetSpellDatas()
        {   
            GUILayout.Label("Infomations");
            spellName = EditorGUILayout.TextField("Spell Name:", spellName);
            spellIcon = EditorGUILayout.ObjectField("Spell Icon", spellIcon, typeof(Sprite), false) as Sprite;
            spellDescription = EditorGUILayout.TextField("Spell Description:", spellDescription);

            GUILayout.Label("Values");
            needTarget = EditorGUILayout.Toggle("Need Target:", needTarget);
            
            castRange = EditorGUILayout.IntField("Cast Range:", castRange);
            ModifyRange = EditorGUILayout.IntField("Modify Range:", ModifyRange);

            EditorGUILayout.Space(10);
            GUILayout.Label("Spell Type");
            modifyData = EditorGUILayout.ObjectField("Modify Data: ", modifyData, typeof(ModifyData), false) as ModifyData;
            modifyValue = EditorGUILayout.FloatField("Modify Value:", modifyValue);

            EditorGUILayout.Space(10);
            GUILayout.Label("Cost Type");
            costData = EditorGUILayout.ObjectField("Cost Data: ", costData, typeof(ModifyData), false) as ModifyData;
            costValue = EditorGUILayout.FloatField("Modify Value:", costValue);
        }

        private void SetSpellDatas(SpellData spellData)
        {
            spellData.SpellIcon = spellIcon;
            spellData.SpellDescription = spellDescription;

            spellData.CastRange = castRange;
            spellData.ModifyRange = ModifyRange;

            spellData.ModifierData = new ModifierData();
            spellData.ModifierData.ModifyData = modifyData;
            spellData.ModifierData.ModifyValue = modifyValue;

            spellData.CostData = new ModifierData();
            spellData.CostData.ModifyData = costData;
            spellData.CostData.ModifyValue = costValue;
        }
    }
}
