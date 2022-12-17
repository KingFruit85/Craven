using UnityEngine;

public class ShortBow : MonoBehaviour
{
    private Helper Helper;
    public GameObject Arrow;
    private GameObject Player;
    private GameManager GameManager;

    void Start()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();
        Player = Helper.Player;
        GameManager = Helper.GameManager;
    }

    public void ShootBow(Vector3 mousePosition)
    {
        if (GameManager.arrowCount <= 0)
        {
            Helper.AudioManager.PlayAudioClip("SwordMiss");
        }

        if (GameManager.arrowCount > 0)
        {
            GameObject a = Instantiate(
                                        Arrow,
                                        Helper.PlayerPosition,
                                        Player.transform.rotation,
                                        Player.transform);

            Arrow.GetComponent<Arrow>().clickPoint = mousePosition;
            GameManager.arrowCount--;
        }
    }
}
