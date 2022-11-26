using UnityEngine;

public class PrizeRoom : MonoBehaviour
{
    public Helper helper;
    public SimpleRoom room;
    public DoorController doorController;
    public EnemySpawner enemySpawner;

    void Start()
    {
        helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        room = GetComponent<SimpleRoom>();
        gameObject.AddComponent<EnemySpawner>();
        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.SetEnemyCount(UnityEngine.Random.Range(3, 5));

        doorController = gameObject.transform.Find("DoorController").GetComponent<DoorController>();
        doorController.openCondition = DoorController.OpenCondition.MobDeath;

        // Get the random floor tile to spawn a chest on
        var chestSpawnLocation = room.SpawnableFloorTiles[UnityEngine.Random.Range(0, room.SpawnableFloorTiles.Length)].transform;

        // Spawn a chest on a random tile
        GameObject barrier = Instantiate(
            Resources.Load<GameObject>("chestBarrier"),
            chestSpawnLocation.position,
            Quaternion.identity);

        barrier.transform.parent = transform.Find("Tiles");

        GameObject chest = Instantiate(
            helper.chest,
            chestSpawnLocation.position,
            Quaternion.identity);

        chest.transform.parent = transform.Find("Tiles");

        //Add to room contents array
        room.AddItemToRoomContents(chest.transform.localPosition, 'C');
    }
}
