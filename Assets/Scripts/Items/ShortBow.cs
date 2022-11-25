using UnityEngine;

public class ShortBow : MonoBehaviour
{
    private GameObject arrow;
    private GameObject player;
    private GameManager gameManager;

    void Start()
    {
        arrow = Resources.Load("arrow") as GameObject;
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void ShootBow(Vector3 mousePosition)
    {
        if (gameManager.arrowCount > 0)
        {

            var playerPos = new Vector3(player.transform.position.x,
                                        player.transform.position.y,
                                        player.transform.position.z);
            // Spawn arrow on top of player
            GameObject a = Instantiate(
                                        arrow,
                                        playerPos,
                                        player.transform.rotation,
                                        player.transform);

            arrow.GetComponent<Arrow>().clickPoint = mousePosition;

            GameObject.Find("GameManager").GetComponent<GameManager>().arrowCount--;

        }
    }
}
