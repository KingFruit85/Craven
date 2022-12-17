using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBox : MonoBehaviour
{
    public static CameraBox instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

}
