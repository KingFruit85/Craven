using UnityEngine;

public class PrizeRoom : MonoBehaviour
{
    public SimpleRoom room;
    public DoorController doorController;
    public EnemySpawner enemySpawner;

    void Start()
    {
        room = GetComponent<SimpleRoom>();
        gameObject.AddComponent<EnemySpawner>();
        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.SetEnemyCount(UnityEngine.Random.Range(3, 5));

        doorController = gameObject.transform.Find("DoorController").GetComponent<DoorController>();
        doorController.openCondition = DoorController.OpenCondition.MobDeath;

        // Get the random floor tile to spawn a chest on
        var chestSpawnLocation = room.SpawnableFloorTiles[UnityEngine.Random.Range(0, room.SpawnableFloorTiles.Length)].transform;

        var tilesTransform = gameObject.transform.Find("Tiles");
        // Spawn a chest on a random tile
        GameObject bars = Instantiate(
            Resources.Load<GameObject>("chestBarrier"),
            chestSpawnLocation.position,
            Quaternion.identity,
            tilesTransform) as GameObject;

        GameObject chest = Instantiate(
            Resources.Load<GameObject>("chest"),
            chestSpawnLocation.position,
            Quaternion.identity,
            tilesTransform) as GameObject;

        //Add to room contents array
        room.AddItemToRoomContents(chest.transform.localPosition, 'C');
    }
}
