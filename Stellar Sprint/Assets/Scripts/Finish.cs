using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private AudioSource finishSound;
    [SerializeField] private ItemCollector itemCollector;
    [SerializeField] private GameObject levelEndPanel;
    //private bool levelCompleted = false;

    void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            finishSound.Play();
            Time.timeScale = 0f; 
            int fuels = PlayerPrefs.GetInt("fuels");
            PlayerPrefs.SetInt("fuels", fuels + itemCollector.fuelsCollectedCurrentLevel);
            levelEndPanel.gameObject.SetActive(true);
        }
    }
}
