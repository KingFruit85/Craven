using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUpdater : MonoBehaviour
{
    public GameObject player;
    float playerHealth;
    public GameManager gameManager;
    int attackDamage;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void GetAttackDamage(string currentHost)
    {
        switch (currentHost)
        {
            default: attackDamage = 80085; break;
            case "Human":

                if (!player.GetComponent<Human>()) return;

                if (player.GetComponent<Human>().SwordEquipped)
                {
                    attackDamage = player.GetComponent<Human>().swordDamage + gameManager.meleeAttackBonus;
                }
                else if (player.GetComponent<Human>().BowEquipped)
                {
                    attackDamage = player.GetComponent<Human>().arrowDamage + gameManager.rangedAttackBonus;
                }
                break;

            case "Ghost":
                if (!player.GetComponent<Ghost>()) return;
                // attackDamage = player.GetComponent<Ghost>().ghostBoltDamage + gameManager.rangedAttackBonus;
                break;

            case "Worm":
                if (!player.GetComponent<Worm>()) return;
                attackDamage = player.GetComponent<Worm>().attackDamage + gameManager.meleeAttackBonus;
                break;

        }
    }


    void Update()
    {

        if (gameManager == null)
        {
            Debug.LogError("failed to find GameManager object, disabling HudUpdater script");
            this.enabled = false;
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerHealth = player.GetComponent<Health>().currentHealth;
            GetAttackDamage(gameManager.currentHost);
        }


        GetComponent<Text>().text = "Attack Damage: " + attackDamage
                                    + "\nXP: " + gameManager.XP
                                    + "\nCoins: " + gameManager.coinCount
                                    + "\nArrows: " + gameManager.arrowCount
                                    + "\nMap Level: " + gameManager.currentGameLevel
                                    + "\nHealth: " + playerHealth
                                    + "\nCurrent Host: " + gameManager.currentHost
                                    + "\n"
                                    + "\n"
                                    + "PlayerPrefs"
                                    + "\n---------"
                                    + "\nArrowcount: " + PlayerPrefs.GetInt("arrowCount")
                                    + "\ncoinCount: " + PlayerPrefs.GetInt("coinCount")
                                    + "\ncurrentGameLevel: " + PlayerPrefs.GetInt("currentGameLevel")
                                    + "\ncurrentHost: " + PlayerPrefs.GetString("currentHost")
                                    + "\nhealthBonus: " + PlayerPrefs.GetInt("healthBonus")
                                    + "\nmeleeAttackBonus: " + PlayerPrefs.GetInt("meleeAttackBonus")
                                    + "\nrangedAttackBonus: " + PlayerPrefs.GetInt("rangedAttackBonus")
                                    + "\nrangedWeaponEquipped: " + PlayerPrefs.GetInt("rangedWeaponEquipped");



        // Need to add in player prefs here, need to understand what isn't wiping after player death and what is.
        //


    }

}

