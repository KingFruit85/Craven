using UnityEngine;

public class EndRoom : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.name += " END ROOM";

        GameObject mb = Instantiate(Resources.Load("GhostMiniBoss"),
                                    gameObject.transform.transform.position,
                                    Quaternion.identity,
                                    transform) as GameObject;
        mb.name = "GhostMiniBoss";
        GetComponent<SimpleRoom>().SpawnExitTile();
    }

}
