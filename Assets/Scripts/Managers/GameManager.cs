using System.Collections;
using System.Collections.Generic;
using EntityObject;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public MovementCombatManager movementCombatManager;
        public GridManager GridManager;
        public EntityManager EntityManager;


        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        void Start()
        {
            EntityManager.Init(GridManager);
            // temp
            movementCombatManager.Init(GridManager, EntityManager);
        }
    }
}
