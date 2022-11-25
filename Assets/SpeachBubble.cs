using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeachBubble : MonoBehaviour
{
    public TextMeshPro textMesh;
    public Vector3 initialPOS;

    public Vector3 offSet = new Vector3(1f,1f,0f);
    private bool isStatic;
    private bool dissappears;
    private float disappearTimer;
    private Color textColor;

    private string labFirstText = "Ughh...Where the hell am I?";
    private string secondText = "Last thing I remember is checking on the test subject";
    private string thirdText = "Woah, what's going on?";

    private string tempWinScreen = "Well done! But that's the end for now. \n Thanks for playing!";



    void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshPro>();
        textMesh.fontSize = 3;
        textColor = textMesh.color;

        if (SceneManager.GetActiveScene().name == "Lab")
        {
            textMesh.text = labFirstText;
            initialPOS = transform.position;
            isStatic = false;
            dissappears = true;
            disappearTimer = 3.0f;
        }

        if (SceneManager.GetActiveScene().name == "PlaceholderWinScreen")
        {
            textMesh.text = tempWinScreen;
            initialPOS = transform.position;
            isStatic = true;
            dissappears = false;
            initialPOS = GameObject.FindGameObjectWithTag("Table").transform.position;

        }

    }
   

    void Update()
    {

        if (!isStatic)
        {
            // Track player
            transform.position = GameObject.Find("Player").GetComponent<Transform>().position + offSet;

            if (dissappears)
            {
                disappearTimer -= Time.deltaTime;

                if(disappearTimer < 0)
                {
                    // Start disappearing
                    float disappearSpeed = 2f;
                    textColor.a -= disappearSpeed * Time.deltaTime;
                    textMesh.color = textColor;
                    if (textColor.a < 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
        else if (isStatic && !dissappears)
        {
            transform.position = GameObject.Find("Table").GetComponent<Transform>().position + new Vector3(0f,-1.3f,0f);
        }

    }
}
