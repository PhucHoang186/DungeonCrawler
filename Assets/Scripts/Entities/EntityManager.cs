using System.Collections;
using System.Collections.Generic;
using Data;
using EntityObject;
using Map;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] EntityPlayer playerPrefab;
    [SerializeField] EntityEnemy enemyPrefab;
    [Header("Test")]
    [SerializeField] List<Vector3> enemyPos;
    private List<EntityPlayer> players = new();
    private List<EntityEnemy> enemies = new();
    [SerializeField] List<CharacterData> playerDatas;
    [SerializeField] List<CharacterData> enemyDatas;

    public void Init(GridManager gridManager)
    {
        GenerateEntities(gridManager);
    }

    public void GenerateEntities(GridManager gridManager)
    {
        GeneratePlayers(gridManager);
        GenerateEnemies(gridManager);
    }

    public (List<EntityPlayer> players, List<EntityEnemy> enemies) GetAllEntities()
    {
        return (players, enemies);
    }

    private void GeneratePlayers(GridManager gridManager)
    {
        for (int i = 0; i < playerDatas.Count; i++)
        {
            EntityPlayer player = Instantiate(playerPrefab, transform);
            Vector3 nodeIndex = player.transform.position;
            Node node = gridManager.GetNodeByIndex((int)nodeIndex.x, (int)nodeIndex.z);
            player.Init(node, playerDatas[i]);
            player.IsMainPlayer = true;
            players.Add(player);
        }
    }

    private void GenerateEnemies(GridManager gridManager)
    {
        for (int i = 0; i < enemyDatas.Count; i++)
        {
            EntityEnemy enemy = Instantiate(enemyPrefab, transform);
            enemy.transform.position = enemyPos[i];
            Vector3 nodeIndex = enemy.transform.position;
            Node node = gridManager.GetNodeByIndex((int)nodeIndex.x, (int)nodeIndex.z);
            enemy.Init(node, enemyDatas[i]);
            enemies.Add(enemy);
        }
    }
}