using System.Collections;
using System.Collections.Generic;
using Data;
using EntityObject;
using GameUI;
using Map;
using UnityEngine;

namespace Managers
{

    public class SpellManager : MonoBehaviour
    {
        public static SpellManager Instance;
        public List<SpellData> CurrentEquippedSpells = new();
        private Node currentNodeOn;
        private SpellData currentUsedSpell;
        private EntityPlayer currentPlayerSelected;
        private bool isExecutable;
        public bool IsExecutable => isExecutable;
        public Node CurrentNodeOn => currentNodeOn;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public List<SpellData> GetSpellsForTurn(int numberOfSpell, CharacterData characterData)
        {
            CurrentEquippedSpells.Clear();
            CurrentEquippedSpells.AddRange(characterData.EquippedSpellList);
            while (CurrentEquippedSpells.Count < numberOfSpell)
            {
                CurrentEquippedSpells.AddRange(characterData.EquippedSpellList);
            }

            //temp
            // will need logic to pick spell
            CurrentEquippedSpells.Shuffle();
            int removeNumber = CurrentEquippedSpells.Count - numberOfSpell;
            CurrentEquippedSpells.RemoveRange(numberOfSpell, removeNumber);
            return CurrentEquippedSpells;
        }

        public void GetNodeMouseOn(Node node)
        {
            currentNodeOn = node;
        }

        public void GetCurrentSpellSelected(SpellData spellData)
        {
            currentUsedSpell = spellData;
        }

        public void GetCurrentPlayerSelected(EntityPlayer entityPlayer)
        {
            currentPlayerSelected = entityPlayer;
        }

        public List<Node> GetSpellRange(AttackSpellData activeSpell, Node startNode, int range)
        {
            if (activeSpell.CastPattern == CastPattern.Around)
            {
                return GridManager.Instance.GetNeighborNodesWithStep(startNode, range, getEmptyOnly: false);
            }
            else
            {
                return GridManager.Instance.GetStraightNodesWithStep(currentNodeOn, range, getEmptyOnly: false);
            }
        }

        public (List<Node> castNodes, List<Node> modifyNodes, Node startNode, Node endNode) OnCastingSpell()
        {
            if (currentUsedSpell == null)
                return (null, null, null, null);

            AttackSpellData activeSpell = currentUsedSpell as AttackSpellData;
            isExecutable = activeSpell.CheckCastable(currentNodeOn);

            // get cast range
            var castNodes = GetSpellRange(activeSpell, currentPlayerSelected.currentNode, activeSpell.CastRange);

            // get modify range
            var modifyNodes = GetSpellRange(activeSpell, currentNodeOn, activeSpell.ModifyRange);

            return (castNodes, modifyNodes, currentPlayerSelected.currentNode, currentNodeOn);
        }

        public void OnExecuteSpell(BattleManager battleManager)
        {
            currentUsedSpell.ExcuteSpell(battleManager);
            currentUsedSpell = null;
        }
    }
}
