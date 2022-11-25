using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTerminalWindowButton : MonoBehaviour
{
    public void CloseWindow()
    {
        Destroy(gameObject.transform.parent.parent.gameObject);
    }

}
