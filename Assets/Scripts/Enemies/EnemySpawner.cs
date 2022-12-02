using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Helper Helper;
    public List<string> Enemies = new() { "Worm", "Ghost" };
    public int EnemyCount;
    public SimpleRoom ThisRoom;
    public GameManager GameManager;
    public DoorController DoorController;

    public List<Vector2> _availbleTiles;
    public List<Vector2> availbleTiles;

    void Start()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ThisRoom = GetComponent<SimpleRoom>();
        DoorController = transform.Find("DoorController").GetComponent<DoorController>();

        for (int i = 0; i <= EnemyCount; i++)
        {
            var spawnTile = ThisRoom.SpawnableFloorTiles[Random.Range(0, (ThisRoom.SpawnableFloorTiles.Length))].transform.position;
            var randomEnemy = Enemies[Random.Range(0, (Enemies.Count))];
            Instantiate(Resources.Load<GameObject>(randomEnemy), spawnTile, Quaternion.identity, transform);
        }
    }

    public void SetEnemyCount(int noOfEnemiesToSpawn)
    {
        EnemyCount = noOfEnemiesToSpawn;
    }

    public List<string> GetEnemies(int count = 1)
    {
        var results = new List<string>();
        for (int i = 0; i < count; i++)
        {
            var r = Random.Range(0, Enemies.Count);
            results.Add(Enemies[r]);
        }
        return results;
    }
}
