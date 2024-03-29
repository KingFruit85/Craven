﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowTurret : MonoBehaviour
{
    public float shotDelay = 2f;
    public float shotCooldown = -9999;
    public GameObject arrow;
    public GameObject rightSpawn;
    public GameObject leftSpawn;
    public GameObject topSpawn;
    public GameObject bottomSpawn;

    public bool isActive = true;

    public void RotateTurret()
    {
        transform.Rotate(0f, 0f, 45f);
    }

    void Update()
    {
        if (Time.time > shotCooldown + shotDelay && isActive)
        {
            // RotateTurret();

            GameObject rightArrow = Instantiate(arrow,
                                       rightSpawn.transform.position,
                                       Quaternion.identity,
                                       transform)
                                       as GameObject;

            rightArrow.GetComponent<TrapArrow>().Direction = "right";

            GameObject leftArrow = Instantiate(arrow,
                                       leftSpawn.transform.position,
                                       Quaternion.identity,
                                       transform)
                                       as GameObject;

            rightArrow.GetComponent<TrapArrow>().Direction = "left";

            GameObject upArrow = Instantiate(arrow,
                                       topSpawn.transform.position,
                                       Quaternion.identity,
                                       transform)
                                       as GameObject;

            rightArrow.GetComponent<TrapArrow>().Direction = "up";

            GameObject downArrow = Instantiate(arrow,
                                       bottomSpawn.transform.position,
                                       Quaternion.identity,
                                       transform)
                                       as GameObject;

            rightArrow.GetComponent<TrapArrow>().Direction = "down";

            shotCooldown = Time.time;
        }
    }
}
