using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;

    public Helper Helper { get; private set; }

    void Awake()
    {
        Helper = GameObject.FindGameObjectWithTag("Helper").GetComponent<Helper>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (SceneManager.GetActiveScene().name == "shop")
        {
            Helper.Camera.transform.position = Vector3.zero;
        }
    }

    public void LoadNextLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
