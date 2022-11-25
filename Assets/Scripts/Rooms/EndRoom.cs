using UnityEngine;

public class EndRoom : MonoBehaviour
{
    public SimpleRoom room;

    void Start()
    {
        room = GetComponent<SimpleRoom>();

        gameObject.transform.name += " END ROOM";

        GameObject mb = Instantiate(Resources.Load("GhostMiniBoss"),
                                    gameObject.transform.transform.position,
                                    Quaternion.identity,
                                    transform) as GameObject;
        room.SpawnExitTile();
    }

}
