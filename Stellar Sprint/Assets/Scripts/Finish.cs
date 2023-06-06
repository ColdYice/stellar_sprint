using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private AudioSource finishSound;
    private ItemCollector itemCollector;
    private PlayerLife playerLife;
    [SerializeField] private GameObject levelEndPanel;

    void Start()
    {
        finishSound = GetComponent<AudioSource>();
        playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>();
        itemCollector = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemCollector>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            finishSound.Play();
            Time.timeScale = 0f; 

            int fuels = PlayerPrefs.GetInt("fuels");
            PlayerPrefs.SetInt("fuels", fuels + itemCollector.fuelsCollectedCurrentLevel);

            int maxHealth = PlayerPrefs.GetInt("maxHealth");
            PlayerPrefs.SetInt("maxHealth", playerLife.maxHealth);

            levelEndPanel.gameObject.SetActive(true);
        }
    }
}
