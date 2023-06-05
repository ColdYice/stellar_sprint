using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerLife playerLife;
    [SerializeField] private Text fuelsText, fuelsCurrentText;
    [SerializeField] private AudioSource pickupSound;

    public string collectibleTag;
    public int fuelsCollectedCurrentLevel;
    private int fuelsTotal;

    private void Awake()
    {
        fuelsTotal = PlayerPrefs.GetInt("fuels");
        fuelsText.text = fuelsTotal.ToString() + "|+" + fuelsCollectedCurrentLevel;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collectibleTag = other.gameObject.tag;

        switch (collectibleTag)
        {
            //case "Fuel":
            //    pickupSound.Play();
            //    fuelsCollectedCurrentLevel++;
            //    fuelsText.text = fuelsTotal.ToString() + "|+" + fuelsCollectedCurrentLevel.ToString();
            //    fuelsCurrentText.text = fuelsCollectedCurrentLevel.ToString();
            //    Destroy(other.gameObject);
            //    break;
            case "Jetpack":
                pickupSound.Play();
                playerMovement.hasJetpack = true;
                Destroy(other.gameObject);
                break;
            case "HealthHeal":
                pickupSound.Play();
                playerLife.health++;
                Destroy(other.gameObject);
                break;
            case "HealthBooster":
                pickupSound.Play();
                playerLife.maxHealth++;
                playerLife.health = playerLife.maxHealth;
                Destroy(other.gameObject);
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Fuel")
        {
            pickupSound.Play();
            fuelsCollectedCurrentLevel++;
            fuelsText.text = fuelsTotal.ToString() + "|+" + fuelsCollectedCurrentLevel.ToString();
            fuelsCurrentText.text = fuelsCollectedCurrentLevel.ToString();
            Destroy(other.gameObject);
        }
    }
}
