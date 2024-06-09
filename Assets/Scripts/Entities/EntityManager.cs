using System.Collections;
using System.Collections.Generic;
using EntityObject;
using Map;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] EntityPlayer playerPrefab;
    [SerializeField] EntityEnemy enemyPrefab;
    [Header("Test")]
    [SerializeField] Vector3 enemyPos;
    private List<EntityPlayer> players = new();
    private List<EntityEnemy> enemies = new();

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
        EntityPlayer player = Instantiate(playerPrefab, transform);
        Vector3 nodeIndex = player.transform.position;
        Node node = gridManager.GetNodeByIndex((int)nodeIndex.x, (int)nodeIndex.z);
        player.Init(node);
        player.IsMainPlayer = true;
        players.Add(player);
    }

    private void GenerateEnemies(GridManager gridManager)
    {
        EntityEnemy enemy = Instantiate(enemyPrefab, transform);
        enemy.transform.position = enemyPos;
        Vector3 nodeIndex = enemy.transform.position;
        Node node = gridManager.GetNodeByIndex((int)nodeIndex.x, (int)nodeIndex.z);
        enemy.Init(node);
        enemies.Add(enemy);
    }
}