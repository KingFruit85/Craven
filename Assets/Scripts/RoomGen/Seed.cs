using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{

    public string GameSeed = "testing";
    public int CurrentSeed = 0;
    //comment to commit
    private void Awake()
    {
        CurrentSeed = GameSeed.GetHashCode();
        Random.InitState(CurrentSeed);
    }
}
